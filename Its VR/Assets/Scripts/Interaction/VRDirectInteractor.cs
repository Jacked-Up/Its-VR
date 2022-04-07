// This script was updated on 02/18/2022 by Jack Randolph.

using ItsVR.Player;
using UnityEngine;
using UnityEngine.Events;

namespace ItsVR.Interaction {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(VRController))]
    [AddComponentMenu("It's VR/Interaction/Direct Interactor")]
    public class VRDirectInteractor : VRBaseInteractor {
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
        
        private VRController _vrController;
        
        #endregion

        private void OnEnable() {
            _vrController = GetComponent<VRController>();
        }

        private void Update() {
            // Bail if the controllers input references is null.
            if (_vrController.inputContainer == null) 
                return;

            // Lets grab the interactable if the player depresses the grip and the associated
            // interactable is null.
            if (_vrController.inputContainer.universal.GripDepress > 0.65f && AssociatedInteractable == null) {
                // Lets draw an overlap sphere and fetch all of the colliders inside a sphere.
                // ReSharper disable once Unity.PreferNonAllocApi
                var overlaps = Physics.OverlapSphere(attachmentPoint.position, castRadius, interactableMask);

                // Now we check if the overlaps contained any interactable base class inheritances.
                // If it does contain an interactable, the interactor will associate/interact with
                // the interactable.
                foreach (var overlap in overlaps) {
                    var interactable = overlap.GetComponentInParent<VRBaseInteractable>();
                    
                    if (interactable == null) continue;
                    if (interactable.IsInteractableAttachmentPointAssociatedByAnInteractor(overlap.transform)) continue;
                    
                    interactable.AssociateWithInteractable(this, overlap.transform);
                    AssociateWithInteractor(interactable);
                    break;
                }
            }
            // Lets release the interactable if the player is no longer depressing the grip
            // and the interactable is not null.
            else if (_vrController.inputContainer.universal.GripDepress < 0.65f && AssociatedInteractable != null) {
                AssociatedInteractable.DissociateFromInteractable(this);
                DissociateFromInteractor();
            }
        }

        public override void AssociateWithInteractor(VRBaseInteractable interactableToAssociate) {
            base.AssociateWithInteractor(interactableToAssociate);
            InteractionStarted?.Invoke();
            directInteractorEvents.interactionStarted.Invoke();
        }

        public override void DissociateFromInteractor() {
            base.DissociateFromInteractor();
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
