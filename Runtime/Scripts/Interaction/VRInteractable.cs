using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ItsVR.Interaction {
    public class VRInteractable : MonoBehaviour {
        #region Variables

        /// <summary>
        /// All interactors currently interacting with the interactable.
        /// </summary>
        [HideInInspector]
        public List<AssociatedInteractor> associatedInteractors = new List<AssociatedInteractor>();

        /// <summary>
        /// Returns the main interactor in the associated interactors list. Returns null if no main interactor exists.
        /// </summary>
        /// <returns></returns>
        public VRInteractor MainInteractor => associatedInteractors.Count != 0 ? associatedInteractors[0].interactor : null;

        /// <summary>
        /// Returns the interactor from the associated interactors list at the index. Returns null if the interactor doesn't exist.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public VRInteractor OtherInteractor(int index) {
            return associatedInteractors.Count >= index ? associatedInteractors[index].interactor : null;
        }
        
        /// <summary>
        /// Returns the main interactable attachment point (Not to be confused with the interactor attachment point) in the associated interactors list. Returns null if no main interactable attachment point exists.
        /// </summary>
        /// <returns></returns>
        public Transform MainAttachmentPoint => associatedInteractors.Count != 0 ? associatedInteractors[0].attachmentPoint != null ? associatedInteractors[0].attachmentPoint : associatedInteractors[0].interactor.transform : null;

        /// <summary>
        /// Returns the interactable attachment point (Not to be confused with the interactor attachment point) from the associated interactors list at the index. Returns null if the interactable attachment point doesn't exist.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Transform OtherAttachmentPoint(int index) {
            return associatedInteractors.Count >= index ? associatedInteractors[index].attachmentPoint != null ? associatedInteractors[index].attachmentPoint : associatedInteractors[index].interactor.transform : null;
        }
        
        /// <summary>
        /// Returns true if the interactor is associated with the interactable.
        /// </summary>
        /// <param name="interactor"></param>
        /// <returns></returns>
        public bool IsInteractorAssociated(VRInteractor interactor) {
            return associatedInteractors.Any(associatedInteractor => associatedInteractor.interactor == interactor);
        }

        /// <summary>
        /// Returns true if the interactable attachment point (Not to be confused with the interactor attachment point) is associated with the interactable.
        /// </summary>
        /// <param name="attachmentPoint"></param>
        /// <returns></returns>
        public bool IsAttachmentPointAssociated(Transform attachmentPoint) {
            return associatedInteractors.Any(associatedInteractor => associatedInteractor.attachmentPoint == attachmentPoint);
        }

        /// <summary>
        /// Invoked when the interactable is associated.
        /// </summary>
        public event VRInteractableEvent Associated;
        
        /// <summary>
        /// Invoked when the interactable is dissociated.
        /// </summary>
        public event VRInteractableEvent Dissociated;
        
        public delegate void VRInteractableEvent();
        
        #endregion

        /// <summary>
        /// Associates the interactor and interactable attachment point with the interactable.
        /// </summary>
        /// <param name="interactor"></param>
        /// <param name="interactableAttachmentPoint"></param>
        public virtual void Associate(VRInteractor interactor, Transform interactableAttachmentPoint) {
            if (IsInteractorAssociated(interactor)) 
                Debug.LogError("[VR Interactable] This interactor is already associated with the interactable.", interactor);

            if (IsAttachmentPointAssociated(interactableAttachmentPoint)) 
                Debug.LogError("[VR Interactable] This interactable attachment point is already associated with the interactable.", interactableAttachmentPoint);
            
            var addingInteractor = new AssociatedInteractor {
                interactor = interactor,
                attachmentPoint = interactableAttachmentPoint
            };
            
            associatedInteractors.Add(addingInteractor);
            Associated?.Invoke();
        }

        /// <summary>
        /// Dissociates the interactor and interactable attachment point from the interactable.
        /// </summary>
        public virtual void Dissociate(VRInteractor interactor) {
            if (!IsInteractorAssociated(interactor)) {
                Debug.LogError("[VR Interactable] This interactor was not associated with the interactable.", interactor);
                return;
            }

            var removingInteractor = associatedInteractors.FirstOrDefault(associatedInteractor => associatedInteractor.interactor == interactor);
            associatedInteractors.Remove(removingInteractor);
            Dissociated?.Invoke();
        }
    }

    [System.Serializable]
    public class AssociatedInteractor {
        /// <summary>
        /// An associated interactor.
        /// </summary>
        public VRInteractor interactor;
        
        /// <summary>
        /// An attachment point on the interactable.
        /// </summary>
        public Transform attachmentPoint;
    }
}
