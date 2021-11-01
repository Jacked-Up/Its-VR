// This script was updated on 10/31/2021 by Jack Randolph.

using ItsVR.Interaction;
using ItsVR.Player;
using UnityEngine;
using UnityEngine.Events;

namespace ItsVR_Samples.Interaction {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(VRController))]
    [AddComponentMenu("It's VR/Samples/Interaction/Direct Interactor (Sample)")]
    public class VRDirectInteractor : VRInteractor {
        #region Variables
        
        /// <summary>
        /// Radius of the overlap cast for interactables.
        /// </summary>
        [Tooltip("Radius of the overlap cast for interactables.")]
        public float castRadius = 0.1f;

        /// <summary>
        /// Physics layer(s) which interactables are on.
        /// </summary>
        [Tooltip("Physics layer(s) which interactables are on.")]
        public LayerMask interactableMask;

        /// <summary>
        /// Direct grabber events.
        /// </summary>
        [Tooltip("Direct grabber events.")]
        public DirectInteractorEvents directInteractorEvents;

        /// <summary>
        /// Invoked when an interaction was started.
        /// </summary>
        public event VRDirectInteractorEvent InteractionStarted;
        
        /// <summary>
        /// Invoked when an interaction has ended.
        /// </summary>
        public event VRDirectInteractorEvent InteractionEnded;
        
        public delegate void VRDirectInteractorEvent();
        private VRController _controller;
        
        #endregion

        private void OnEnable() {
            _controller = GetComponent<VRController>();
        }

        private void Update() {
            if (_controller.inputReference == null) return;

            // Lets grab the interactable if the player depresses the grip and the associated
            // interactable is null.
            if (_controller.inputReference.universalInputs.GripDepress > 0.65f && associatedInteractable == null) {
                // Lets draw an overlap sphere and fetch all of the colliders inside a sphere.
                // ReSharper disable once Unity.PreferNonAllocApi
                var overlaps = Physics.OverlapSphere(attachmentPoint.position, castRadius, interactableMask);

                // Now we check if the overlaps contained any interactable base class inheritances.
                // If it does contain an interactable, the interactor will associate/interact with
                // the interactable.
                foreach (var overlap in overlaps) {
                    var interactable = overlap.GetComponentInParent<VRInteractable>();
                    
                    if (interactable == null) continue;
                    if (interactable.IsAttachmentPointAssociated(overlap.transform)) continue;
                    
                    interactable.Associate(this, overlap.transform);
                    associatedInteractable = interactable;
                    break;
                }
            }
            // Lets release the interactable if the player is no longer depressing the grip
            // and the interactable is not null.
            else if (_controller.inputReference.universalInputs.GripDepress < 0.65f && associatedInteractable != null) {
                associatedInteractable.Dissociate(this);
                associatedInteractable = null;
            }
        }

        public override void Associate(VRInteractable interactable) {
            base.Associate(interactable);
            InteractionStarted?.Invoke();
            directInteractorEvents.interactionStarted.Invoke();
        }

        public override void Dissociate(VRInteractable interactable) {
            base.Dissociate(interactable);
            InteractionEnded?.Invoke();
            directInteractorEvents.interactionEnded.Invoke();
        }
        
        #region Editor

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, castRadius);
        }

        #endregion
    }

    [System.Serializable]
    public class DirectInteractorEvents {
        /// <summary>
        /// Invoked when the interactor is associated with the interactable.
        /// </summary>
        [Tooltip("Invoked when the interactor is associated with the interactable.")]
        public UnityEvent interactionStarted;
        
        /// <summary>
        /// Invoked when the interactor is dissociated with the interactable.
        /// </summary>
        [Tooltip("Invoked when the interactor is dissociated with the interactable.")]
        public UnityEvent interactionEnded;
    }
}
