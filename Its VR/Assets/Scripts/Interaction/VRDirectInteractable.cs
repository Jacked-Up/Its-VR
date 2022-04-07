// This script was updated on 11/10/2021 by Jack Randolph.

using System;
using System.Collections.Generic;
using ItsVR.Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ItsVR.Interaction {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    [AddComponentMenu("It's VR/Interaction/Direct Interactable")]
    public class VRDirectInteractable : VRBaseInteractable {
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
        public InteractableEvents interactableEvents;
        
        /// <summary>
        /// If the grabbable is grabbed.
        /// </summary>
        public bool IsGrabbed => AssociatedInteractors.Count != 0;

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
        private Vector3 _firstWorldPosition;
        private Quaternion _firstWorldRotation;
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
            
            var self = transform;
            _firstWorldPosition = self.position;
            _firstWorldRotation = self.rotation;
        }

        private void LateUpdate() {
            _lastWorldPosition = _firstWorldPosition;
            _lastWorldRotation = _firstWorldRotation;
        }

        public override void AssociateWithInteractable(VRBaseInteractor interactorToAssociate, Transform interactableAttachmentPoint) {
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
                
                interactableEvents.InvokeOnInteractionEntered();
            }
            
            base.AssociateWithInteractable(interactorToAssociate, interactableAttachmentPointToAssociate);
            interactableEvents.InvokeOnAssociationOccured();

            // Bail if the grabbable is held by one hand.
            if (AssociatedInteractors.Count <= 1) return;
            
            // Here's where we determine the two hand hold calculation method.
            if (defaultCalculationMethod == CalculationMethods.AutoDetect) {
                // First we calculate the x and z distances.
                var mainLocalPos = GetInteractableAttachmentPointAtIndex(0).localPosition;
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

        public override void DissociateFromInteractable(VRBaseInteractor interactorToDissociate) {
            base.DissociateFromInteractable(interactorToDissociate);
            interactableEvents.InvokeOnDisassociationOccured();

            // Bail if the grabbable is still held.
            if (IsGrabbed) {
                // Set the main attachment point to the main attachment point.
                //if (baseAttachmentPoint != null)
                    //GetInteractableAttachmentPoint(0) = baseAttachmentPoint;
                
                return;
            }

            // Reenable all of the colliders on the grabbable.
            var colliders = GetComponentsInChildren<Collider>();
            if (colliders.Length != 0) {
                foreach (var col in colliders) {
                    if (col == null || col.isTrigger) 
                        continue;
                
                    col.enabled = true;
                }
            }

            // Set rigidbody properties.
            _rigidbody.isKinematic = _rbOldIsKinematic;
            _rigidbody.collisionDetectionMode = _rbOldCollisionDetectionMode;
            
            //_rigidbody.velocity = Vector3.Scale(ItsMath.Velocity(_firstWorldPosition, _lastWorldPosition), transform.localScale);
            //_rigidbody.angularVelocity = Vector3.Scale(ItsMath.AngularVelocity(_firstWorldRotation, _lastWorldRotation), transform.localScale);
                
            interactableEvents.InvokeOnInteractionExited();
        }

        /// <summary>
        /// Updates when the tracking is updated.
        /// </summary>
        private void Track() {
            // Bail if the grabbable is not grabbed.
            if (!IsGrabbed) return;

            // Here's where we calculate the attachment point offset.
            var primaryAttachPosition = GetInteractableAttachmentPointAtIndex(0).position;
            var secondAttachPosition = AssociatedInteractors.Count > 1 ? GetInteractableAttachmentPointAtIndex(1).position : Vector3.zero;

            var attachOffset = transform.position - (AssociatedInteractors.Count > 1 
                ? Vector3.Lerp(primaryAttachPosition, secondAttachPosition, 0.5f) 
                : primaryAttachPosition);
            
            var localAttachOffset = GetInteractableAttachmentPointAtIndex(0).InverseTransformDirection(attachOffset);

            _interactorLocalPosition = localAttachOffset;
            _interactorLocalRotation = Quaternion.Inverse(Quaternion.Inverse(transform.rotation) * GetInteractableAttachmentPointAtIndex(0).rotation);  
            
            var positionDelta = Vector3.zero;
            var rotationDelta = Quaternion.identity;

            // If the interactable is grabbed by more than one interactor (hands) then we should
            // calculate the position as if the interactable is being held by two hands, otherwise
            // it should calculate the position as if the interactable is being held by one hand.
            if (AssociatedInteractors.Count > 1) {
                // First we create a list of all the grab points.
                var points = new List<Vector3> {
                    GetInteractorAtIndex(0).attachmentPoint.position,
                    GetInteractorAtIndex(1).attachmentPoint.position
                };

                // The position is the middle of all the interactors.
                positionDelta = ItsMath.MiddlePoint(points);

                // Caching interactor positions and attach transforms for a more efficient calculation.
                var aInteractorAttach = GetInteractorAtIndex(0).attachmentPoint;
                var bInteractorAttach = GetInteractorAtIndex(1).attachmentPoint;
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
                positionDelta = GetInteractorAtIndex(0).attachmentPoint.position;
                
                // The rotation is the rotation of the interactor.
                rotationDelta = Quaternion.LookRotation(-GetInteractorAtIndex(0).attachmentPoint.up, GetInteractorAtIndex(0).attachmentPoint.forward);
            }

            // Lastly we apply all of the calculations to the position and the rotation
            // of the interactable.
            transform.position = CalculateAttachPosition(positionDelta, rotationDelta);
            transform.rotation = CalculateAttachRotation(rotationDelta);
        }
    }
}
