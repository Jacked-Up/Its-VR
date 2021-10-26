// This script was updated on 10/26/2021 by Jack Randolph.
// Documentation: https://jackedupstudios.com/vr-interactor

using UnityEngine;

namespace ItsVR.Interaction {
    public class VRInteractor : MonoBehaviour {
        #region Variables

        /// <summary>
        /// The interactors attachment point.
        /// </summary>
        [Tooltip("The interactors attachment point.")]
        public Transform attachmentPoint;

        /// <summary>
        /// The interactable currently associated with the interactor.
        /// </summary>
        [HideInInspector]
        public VRInteractable associatedInteractable;

        /// <summary>
        /// Invoked when the interactor is associated.
        /// </summary>
        public event VRInteractorEvent Associated;
        
        /// <summary>
        /// Invoked when the interactor is dissociated.
        /// </summary>
        public event VRInteractorEvent Dissociated;
        
        public delegate void VRInteractorEvent();
        
        #endregion

        /// <summary>
        /// Associates the interactor with the interactable.
        /// </summary>
        /// <param name="interactable"></param>
        public virtual void Associate(VRInteractable interactable) {
            // We must dissociate the associated interactor before we
            // can associate a new interactor.
            if (associatedInteractable != null) 
                Dissociate(associatedInteractable);

            associatedInteractable = interactable;
            Associated?.Invoke();
        }

        /// <summary>
        /// Disassociates the interactor from the interactable.
        /// </summary>
        /// <param name="interactable"></param>
        public virtual void Dissociate(VRInteractable interactable) {
            // If the interactable is not associated with the interactor,
            // the developer attempted to dissociate an interactable which
            // currently is not associated.
            if (interactable != associatedInteractable) {
                Debug.LogError("[VR Interactor] This interactable couldn't be dissociated because it wasn't associated with the interactor.", interactable);
                return;
            }
            
            associatedInteractable = null;
            Dissociated?.Invoke();
        }
    }
}
