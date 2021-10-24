using ItsVR.Interaction;
using ItsVR.Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ItsVR_Samples.Interaction {
    [DisallowMultipleComponent]
    [AddComponentMenu("It's VR/Samples/Interaction/Direct Interactor")]
    public class VRDirectInteractor : VRInteractor {
        #region Variables

        /// <summary>
        /// Controller used for receiving input.
        /// </summary>
        [Tooltip("Controller used for receiving input.")]
        public VRController connectedController;
        
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
        
        #endregion

        private void OnEnable() {
            if (connectedController == null) 
                Debug.LogError("[VR Grabber] No controller was connected to the grabber. This means no input will be received.", this);
            else if (connectedController != null && connectedController.inputReference != null) {
                connectedController.inputReference.gripPressed.Enable();
                connectedController.inputReference.gripPressed.performed += AttemptGrab;
            }
        }

        public override void Associate(VRInteractable interactable) {
            base.Associate(interactable);
            directInteractorEvents.interactionStarted.Invoke();
        }

        public override void Dissociate(VRInteractable interactable) {
            base.Dissociate(interactable);
            directInteractorEvents.interactionEnded.Invoke();
        }

        private void AttemptGrab(InputAction.CallbackContext callbackContext) {
            if (associatedInteractable == null) {
                var overlaps = Physics.OverlapSphere(attachmentPoint.position, castRadius, interactableMask);

                foreach (var overlap in overlaps) {
                    var interactable = overlap.GetComponentInParent<VRInteractable>();
                    if (interactable == null) continue;

                    if (interactable.IsAttachmentPointAssociated(overlap.transform)) continue;
                    
                    interactable.Associate(this, overlap.transform);
                    associatedInteractable = interactable;
                    break;
                }
            }
            else if (associatedInteractable != null) {
                associatedInteractable.Dissociate(this);
                associatedInteractable = null;
            }
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
