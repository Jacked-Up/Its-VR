using System;
using ItsVR.Interaction;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ItsVR_Samples.Interaction {
    [DisallowMultipleComponent]
    [AddComponentMenu("It's VR/Samples/Interaction/Grabbable (Sample) (Incomplete)")]
    [RequireComponent(typeof(Rigidbody))]
    public class VRGrabbable : VRInteractable {
        #region Variables

        /// <summary>
        /// Objects twist bias while held by two interactors. The closer to zero, the closer the grabbable will rotate with the primary interactor. The closer to one, the closer the grabbable will rotate with the secondary interactor.
        /// </summary>
        [Range(0f, 1f)] [Tooltip("Objects twist bias while held by two interactors. Offsets the rotation relative to the two interactors. The closer to zero, the closer the grabbable will rotate with the primary interactor. The closer to one, the closer the grabbable will rotate with the secondary interactor.")]
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
        /// Returns the interactor position in relation to the world coordinates.
        /// </summary>
        /// <param name="interactor"></param>
        /// <returns></returns>
        private Vector3 GetInteractorAttachPosition(VRInteractor interactor) {
            return interactor.attachmentPoint.position + interactor.attachmentPoint.rotation * _interactorLocalPosition;
        }

        /// <summary>
        /// Returns the interactor rotation in relation to the world coordinates.
        /// </summary>
        /// <param name="interactor"></param>
        /// <returns></returns>
        private Quaternion GetInteractorAttachRotation(VRInteractor interactor) {
            return interactor.attachmentPoint.rotation * _interactorLocalRotation;
        }
        
        /// <summary>
        /// Returns the position of the grabbable in relation to the interactor(s).
        /// </summary>
        /// <returns></returns>
        private Vector3 CalculateLookPosition() {
            return associatedInteractors.Count > 1 
                ? Vector3.Lerp(GetInteractorAttachPosition(MainInteractor), GetInteractorAttachPosition(OtherInteractor(1)), 0.5f) 
                : GetInteractorAttachPosition(MainInteractor);
        }
        
        /// <summary>
        /// Returns the forward direction of the grabbable in relation to the interactor(s).
        /// </summary>
        /// <returns></returns>
        private Quaternion CalculateLookRotation() {
            return associatedInteractors.Count > 1 
                ? Quaternion.LookRotation(GetInteractorAttachPosition(OtherInteractor(1)) - GetInteractorAttachPosition(MainInteractor), Vector3.Lerp(MainInteractor.attachmentPoint.forward, OtherInteractor(1).attachmentPoint.forward, twistBias))
                : GetInteractorAttachRotation(MainInteractor);
        }
        
        private Vector3 _interactorLocalPosition;
        private Quaternion _interactorLocalRotation;
        private Rigidbody _rigidbody;
        private bool _rbOldIsKinematic;
        private CollisionDetectionMode _rbOldCollisionDetectionMode;

        #endregion

        private void OnEnable() {
            InputSystem.onAfterUpdate += EarlyUpdate;
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnDisable() {
            InputSystem.onAfterUpdate -= EarlyUpdate;
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
                
            grabbableEvents.grabExited.Invoke();
        }
        
        private void EarlyUpdate() {
            UpdateTracking();
        }
        
        private void Update() {
            UpdateTracking();
        }

        /// <summary>
        /// Updates when the tracking is updated.
        /// </summary>
        private void UpdateTracking() {
            if (!IsGrabbed) return;

            CalculateOffset();
            transform.position = CalculateLookPosition();
            transform.rotation = CalculateLookRotation();
        }

        /// <summary>
        /// Calculates the offset between the interactor(s) and attach point(s).
        /// </summary>
        private void CalculateOffset() {
            var primaryAttachPosition = MainAttachmentPoint.position;
            var secondAttachPosition = associatedInteractors.Count > 1 ? OtherAttachmentPoint(1).position : Vector3.zero;
            
            var attachOffset = transform.position - (associatedInteractors.Count > 1 ? Vector3.Lerp(primaryAttachPosition, secondAttachPosition, 0.5f) : primaryAttachPosition);
            var localAttachOffset = MainAttachmentPoint.InverseTransformDirection(attachOffset);

            _interactorLocalPosition = localAttachOffset;
            _interactorLocalRotation = Quaternion.Inverse(Quaternion.Inverse(transform.rotation) * MainAttachmentPoint.rotation);  
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
