// This script was updated on 11/10/2021 by Jack Randolph.

using System;
using System.Collections.Generic;
using ItsVR;
using ItsVR.Interaction;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ItsVR_Samples.Interaction {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    [AddComponentMenu("It's VR/Samples/Interaction/Grabbable (Sample)")]
    public class VRGrabbable : VRInteractable {
        #region Variables

        /// <summary>
        /// |Optional| If referenced, this will always be the primary attachment point of the object.
        /// </summary>
        [Tooltip("|Optional| If referenced, this will always be the primary attachment point of the object.")]
        public Transform baseAttachmentPoint;
        
        /// <summary>
        /// The calculation method used while holding the object with two hands.
        /// </summary>
        [Tooltip("The calculation method used while holding the object with two hands.")]
        public CalculationMethods defaultCalculationMethod = CalculationMethods.AutoDetect;
        
        /// <summary>
        /// Objects twist bias while held by two interactors. 
        /// </summary>
        [Range(0f, 1f)] [Tooltip("Objects twist bias while held by two interactors.")]
        public float twistBias = 0.5f;
        
        /// <summary>
        /// If the grabbable can be held with two interactors.
        /// </summary>
        [Tooltip("If the grabbable can be held with two interactors.")]
        public bool allowTwoHandGrab;

        /// <summary>
        /// Grabbable events.
        /// </summary>
        [Tooltip("Grabbable events.")]
        public GrabbableEvents grabbableEvents;
        
        /// <summary>
        /// If the grabbable is grabbed.
        /// </summary>
        public bool IsGrabbed => associatedInteractors.Count != 0;

        /// <summary>
        /// Calculates the interactors attachment point position delta in world space.
        /// </summary>
        /// <param name="attachmentPosition">Attachment point position.</param>
        /// <param name="attachmentRotation">Attachment point rotation.</param>
        /// <returns></returns>
        private Vector3 CalculateAttachPosition(Vector3 attachmentPosition, Quaternion attachmentRotation) {
            return attachmentPosition + attachmentRotation * _interactorLocalPosition;
        }

        /// <summary>
        /// Calculates the interactors attachment point rotation delta in world space.
        /// </summary>
        /// <param name="attachmentRotation">Attachment point position.</param>
        /// <returns></returns>
        private Quaternion CalculateAttachRotation(Quaternion attachmentRotation) {
            return attachmentRotation * _interactorLocalRotation;
        }

        private Rigidbody _rigidbody;
        private bool _rbOldIsKinematic;
        private CollisionDetectionMode _rbOldCollisionDetectionMode;
        private Vector3 _interactorLocalPosition;
        private Quaternion _interactorLocalRotation;
        private Vector3 _lastWorldPosition;
        private Quaternion _lastWorldRotation;
        private CalculationMethods _currentCalculationMethod;
        public enum CalculationMethods { AutoDetect, FrontToBack, RightToLeft }
        
        #endregion

        private void OnEnable() {
            InputSystem.onAfterUpdate += Track;
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnDisable() {
            InputSystem.onAfterUpdate -= Track;
        }
        
        private void Update() {
            Track();
        }

        private void FixedUpdate() {
            var self = transform;
            _lastWorldPosition = self.position;
            _lastWorldRotation = self.rotation;
        }

        public override void Associate(VRInteractor interactor, Transform interactableAttachmentPoint) {
            // Bail if the grabbable is grabbed and the object cannot be held by two hands.
            if (!allowTwoHandGrab && IsGrabbed) return;

            // This is the interactable attachment point to associate with the interactable.
            var interactableAttachmentPointToAssociate = interactableAttachmentPoint;
            
            if (!IsGrabbed) {
                // Set the interactable attachment point to associate to the base attachment
                // point if one is referenced.
                if (baseAttachmentPoint != null) 
                    interactableAttachmentPointToAssociate = baseAttachmentPoint;

                // Cache rigidbody properties.
                _rbOldIsKinematic = _rigidbody.isKinematic;
                _rbOldCollisionDetectionMode = _rigidbody.collisionDetectionMode;

                // Set rigidbody properties.
                _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                _rigidbody.isKinematic = true;

                // Disable all colliders on the grabbable.
                var colliders = GetComponentsInChildren<Collider>();
                if (colliders.Length == 0) return;
                foreach (var col in colliders) {
                    if (col == null || col.isTrigger) continue;
                    col.enabled = false;
                }
                
                grabbableEvents.grabEntered.Invoke();
            }
            
            base.Associate(interactor, interactableAttachmentPointToAssociate);
            grabbableEvents.grabOccured.Invoke();

            // Bail if the grabbable is held by one hand.
            if (associatedInteractors.Count <= 1) return;
            
            // Here's where we determine the two hand hold calculation method.
            if (defaultCalculationMethod == CalculationMethods.AutoDetect) {
                // First we calculate the x and z distances.
                var mainLocalPos = MainAttachmentPoint.localPosition;
                var otherLocalPos = interactableAttachmentPoint.localPosition;
                var xDist = Mathf.Abs(mainLocalPos.x - otherLocalPos.x);
                var zDist = Mathf.Abs(mainLocalPos.z - otherLocalPos.z);

                // If the x distance is greater than the z distance, set the calculation
                // method to right to left.
                if (xDist > zDist)
                    _currentCalculationMethod = CalculationMethods.RightToLeft;
                // If the z distance is greater than the x distance, set the calculation
                // method to front to back.
                else if (zDist > xDist)
                    _currentCalculationMethod = CalculationMethods.FrontToBack;
                // If both conditions aren't met, then the calculation method is set to
                // front to back by default.
                else {
                    Debug.LogError("[VR Grabbable] Could not determine a calculation method.", this);
                    _currentCalculationMethod = CalculationMethods.FrontToBack;
                }
            }
            else {
                _currentCalculationMethod = defaultCalculationMethod;
            }
        }

        public override void Dissociate(VRInteractor interactor) {
            base.Dissociate(interactor);
            grabbableEvents.releaseOccured.Invoke();

            // Bail if the grabbable is still held.
            if (IsGrabbed) {
                // Set the main attachment point to the main attachment point.
                if (baseAttachmentPoint != null)
                    MainAttachmentPoint = baseAttachmentPoint;
                
                return;
            }

            // Reenable all of the colliders on the grabbable.
            var colliders = GetComponentsInChildren<Collider>();
            if (colliders.Length == 0) return;
            foreach (var col in colliders) {
                if (col == null || col.isTrigger) continue;
                col.enabled = true;
            }
            
            // Set rigidbody properties.
            var self = transform;
            var selfLocalScale = self.localScale;
            _rigidbody.velocity = Vector3.Scale(ItsMath.Velocity(self.position, _lastWorldPosition), selfLocalScale);
            _rigidbody.angularVelocity = Vector3.Scale(ItsMath.AngularVelocity(self.rotation, _lastWorldRotation), selfLocalScale);
            
            _rigidbody.isKinematic = _rbOldIsKinematic;
            _rigidbody.collisionDetectionMode = _rbOldCollisionDetectionMode;
            
            grabbableEvents.grabExited.Invoke();
        }

        /// <summary>
        /// Updates when the tracking is updated.
        /// </summary>
        private void Track() {
            // Bail if the grabbable is not grabbed.
            if (!IsGrabbed) return;

            // Here's where we calculate the attachment point offset.
            var primaryAttachPosition = MainAttachmentPoint.position;
            var secondAttachPosition = associatedInteractors.Count > 1 ? OtherAttachmentPoint(1).position : Vector3.zero;

            var attachOffset = transform.position - (associatedInteractors.Count > 1 
                ? Vector3.Lerp(primaryAttachPosition, secondAttachPosition, 0.5f) 
                : primaryAttachPosition);
            
            var localAttachOffset = MainAttachmentPoint.InverseTransformDirection(attachOffset);

            _interactorLocalPosition = localAttachOffset;
            _interactorLocalRotation = Quaternion.Inverse(Quaternion.Inverse(transform.rotation) * MainAttachmentPoint.rotation);  
            
            var positionDelta = Vector3.zero;
            var rotationDelta = Quaternion.identity;

            // If the interactable is grabbed by more than one interactor (hands) then we should
            // calculate the position as if the interactable is being held by two hands, otherwise
            // it should calculate the position as if the interactable is being held by one hand.
            if (associatedInteractors.Count > 1) {
                // First we create a list of all the grab points.
                var points = new List<Vector3> {
                    MainInteractor.attachmentPoint.position,
                    OtherInteractor(1).attachmentPoint.position
                };

                // The position is the middle of all the interactors.
                positionDelta = ItsMath.MiddlePoint(points);

                // Caching interactor positions and attach transforms for a more efficient calculation.
                var aInteractorAttach = MainInteractor.attachmentPoint;
                var bInteractorAttach = OtherInteractor(1).attachmentPoint;
                var aInteractorPos = aInteractorAttach.position;
                var bInteractorPos = bInteractorAttach.position;
                
                // Calculate rotation of the interactable using the specified calculation method.
                rotationDelta = _currentCalculationMethod switch {
                    CalculationMethods.FrontToBack => Quaternion.LookRotation((bInteractorPos - aInteractorPos).normalized, Vector3.Lerp(aInteractorAttach.forward, bInteractorAttach.forward, twistBias).normalized),
                    CalculationMethods.RightToLeft => Quaternion.LookRotation(Vector3.Cross(bInteractorPos - aInteractorPos, (aInteractorAttach.forward + bInteractorAttach.forward) / 2).normalized, Vector3.Cross(aInteractorPos - bInteractorPos, (-aInteractorAttach.up + -bInteractorAttach.up) / 2).normalized),
                    _ => rotationDelta
                };
            }
            else {
                // The position is the position of the interactor.
                positionDelta = MainInteractor.attachmentPoint.position;
                
                // The rotation is the rotation of the interactor.
                rotationDelta = Quaternion.LookRotation(-MainInteractor.attachmentPoint.up, MainInteractor.attachmentPoint.forward);
            }

            // Lastly we apply all of the calculations to the position and the rotation
            // of the interactable.
            transform.position = CalculateAttachPosition(positionDelta, rotationDelta);
            transform.rotation = CalculateAttachRotation(rotationDelta);
        }
    }

    [Serializable]
    public class GrabbableEvents {
        /// <summary>
        /// Invoked when the grabbable is first grabbed by an interactor.
        /// </summary>
        [Tooltip("Invoked when the grabbable is first grabbed by an interactor.")]
        public UnityEvent grabEntered;
        
        /// <summary>
        /// Invoked when the grabbable is grabbed by an interactor.
        /// </summary>
        [Tooltip("Invoked when the grabbable is grabbed by an interactor.")]
        public UnityEvent grabOccured;
        
        /// <summary>
        /// Invoked when the grabbable is completely released by an interactor.
        /// </summary>
        [Tooltip("Invoked when the grabbable is completely released by an interactor.")]
        public UnityEvent grabExited;
        
        /// <summary>
        /// Invoked when the grabbable is released by an interactor.
        /// </summary>
        [Tooltip("Invoked when the grabbable is released by an interactor.")]
        public UnityEvent releaseOccured;
    }
}
