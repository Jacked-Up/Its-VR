// This script was updated on 10/31/2021 by Jack Randolph.

using System;
using ItsVR.Interaction;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ItsVR_Samples.Interaction {
    [DisallowMultipleComponent]
    [AddComponentMenu("It's VR/Samples/Interaction/Grabbable (Sample)")]
    [RequireComponent(typeof(Rigidbody))]
    public class VRGrabbable : VRInteractable {
        #region Variables

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
        /// <param name="interactor"></param>
        /// <returns></returns>
        private Vector3 AttachPositionDelta(VRInteractor interactor) {
            return interactor.attachmentPoint.position + interactor.attachmentPoint.rotation * _interactorLocalPosition;
        }

        /// <summary>
        /// Calculates the interactors attachment point rotation delta in world space.
        /// </summary>
        /// <param name="interactor"></param>
        /// <returns></returns>
        private Quaternion AttachRotationDelta(VRInteractor interactor) {
            return interactor.attachmentPoint.rotation * _interactorLocalRotation;
        }

        private Rigidbody _rigidbody;
        private bool _rbOldIsKinematic;
        private CollisionDetectionMode _rbOldCollisionDetectionMode;
        private Vector3 _interactorLocalPosition;
        private Quaternion _interactorLocalRotation;
        private Vector3 _lastWorldPosition;

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

        private void LateUpdate() {
            _lastWorldPosition = transform.position;
        }

        public override void Associate(VRInteractor interactor, Transform interactableAttachmentPoint) {
            if (!allowTwoHandGrab && IsGrabbed) return;

            if (!IsGrabbed) {
                _rbOldIsKinematic = _rigidbody.isKinematic;
                _rbOldCollisionDetectionMode = _rigidbody.collisionDetectionMode;

                _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                _rigidbody.isKinematic = true;

                var colliders = GetComponentsInChildren<Collider>();
                if (colliders.Length == 0) return;
                foreach (var col in colliders) {
                    if (col == null || col.isTrigger) continue;
                    col.enabled = false;
                }
                
                grabbableEvents.grabEntered.Invoke();
            }

            base.Associate(interactor, interactableAttachmentPoint);
            grabbableEvents.grabOccured.Invoke();
        }

        public override void Dissociate(VRInteractor interactor) {
            base.Dissociate(interactor);
            grabbableEvents.releaseOccured.Invoke();
            
            if (IsGrabbed) return;
            
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.isKinematic = _rbOldIsKinematic;
            _rigidbody.collisionDetectionMode = _rbOldCollisionDetectionMode;

            var colliders = GetComponentsInChildren<Collider>();
            if (colliders.Length == 0) return;
            foreach (var col in colliders) {
                if (col == null || col.isTrigger) continue;
                col.enabled = true;
            }
                
            _rigidbody.AddForce(Vector3.Scale(transform.position - _lastWorldPosition, transform.localScale) / Time.deltaTime * 50, ForceMode.Acceleration);
            
            grabbableEvents.grabExited.Invoke();
        }

        /// <summary>
        /// Updates when the tracking is updated.
        /// </summary>
        private void Track() {
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
            
            // Here's where we calculate the position of the grabbable. The math may look complex,
            // but it's actually simple. We are simply calculating the attachment point offset and using
            // it as a pivot.
            // First we check if there is more than one hand holding the grabbable. If there is only
            // one hand, we follow the position of that controller. If there's two hands, we will lerp
            // the position to the middle of the two controllers.
            transform.position = associatedInteractors.Count > 1 
                ? Vector3.Lerp(AttachPositionDelta(MainInteractor), AttachPositionDelta(OtherInteractor(1)), 0.5f) 
                : AttachPositionDelta(MainInteractor);
            
            // Here's where we calculate the rotation of the grabbable.
            transform.rotation = associatedInteractors.Count > 1
                ? Quaternion.LookRotation(MainInteractor.attachmentPoint.position - OtherInteractor(1).attachmentPoint.position, Vector3.Lerp(MainInteractor.attachmentPoint.forward, OtherInteractor(1).attachmentPoint.forward, twistBias))
                : AttachRotationDelta(MainInteractor);
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
