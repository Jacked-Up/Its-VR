// This script was updated on 02/20/2022 by Jack Randolph.

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace ItsVR.Interaction {
    /// <summary>
    /// It's VR interactable base class.
    /// </summary>
    [HelpURL("https://jackedupstudios.com/vr-interactable")]
    public class VRBaseInteractable : MonoBehaviour {
        #region Variables

        /// <summary>
        /// A list containing all interactors currently interacting with the interactable.
        /// </summary>
        public List<AssociationEntry> AssociatedInteractors { get; private set; } = new List<AssociationEntry>();

        [Serializable]
        public struct AssociationEntry {
            /// <summary>
            /// Interactor which has associated itself with this interactable.
            /// </summary>
            public VRBaseInteractor associatedInteractor;
        
            /// <summary>
            /// The attachment point on the interactable which the interactor has interacted with.
            /// </summary>
            public Transform interactableAttachmentPoint;
        }
        
        /// <summary>
        /// Returns the interactor from the associated interactors list at the index. Returns null if the interactor doesn't exist.
        /// </summary>
        /// <param name="index">The index to search.</param>
        /// <returns>The interactor found at the index.</returns>
        public VRBaseInteractor GetInteractorAtIndex(int index) => AssociatedInteractors[index].associatedInteractor;
        
        /// <summary>
        /// Returns the interactable attachment point (Not to be confused with the interactor attachment point) from the associated interactors list at the index. Returns null if the interactable attachment point doesn't exist.
        /// </summary>
        /// <param name="index">The index to search.</param>
        /// <returns>The interactables attachment point found at the index.</returns>
        public Transform GetInteractableAttachmentPointAtIndex(int index) => AssociatedInteractors[index].interactableAttachmentPoint;

        /// <summary>
        /// Returns true if the interactor is associated with the interactable.
        /// </summary>
        /// <param name="interactor">The interactor to check.</param>
        public bool IsInteractorAssociatedWithThisInteractable(VRBaseInteractor interactor) => AssociatedInteractors.Any(associatedInteractor => associatedInteractor.associatedInteractor == interactor);

        /// <summary>
        /// True if the interactables attachment point is associated with an interactor.
        /// </summary>
        /// <param name="interactableAttachmentPoint">The interactable attachment point to check.</param>
        public bool IsInteractableAttachmentPointAssociatedByAnInteractor(Transform interactableAttachmentPoint) => AssociatedInteractors.Any(associatedInteractor => associatedInteractor.interactableAttachmentPoint == interactableAttachmentPoint);

        /// <summary>
        /// Invoked when an interactor is disassociated with this interactable.
        /// </summary>
        public event AssociationAndDissociationCallback InteractorAssociated;
        
        /// <summary>
        /// Invoked when an interactor is associated with this interactable.
        /// </summary>
        public event AssociationAndDissociationCallback InteractorDissociated;
        
        public delegate void AssociationAndDissociationCallback(VRBaseInteractor interactor);

        #endregion

        /// <summary>
        /// Associates the interactor and interactable attachment point with the interactable.
        /// </summary>
        /// <param name="interactorToAssociate">The interactor to associate with.</param>
        /// <param name="interactableAttachmentPoint">The attachment point the interactor attached to.</param>
        public virtual void AssociateWithInteractable(VRBaseInteractor interactorToAssociate, Transform interactableAttachmentPoint) {
#if UNITY_EDITOR
            if (IsInteractorAssociatedWithThisInteractable(interactorToAssociate)) 
                Debug.LogWarning("This interactor is already associated with the interactable.", interactorToAssociate);

            if (IsInteractableAttachmentPointAssociatedByAnInteractor(interactableAttachmentPoint)) 
                Debug.LogWarning("This interactable attachment point is already associated with the interactable.", interactableAttachmentPoint);
#endif
            
            AssociatedInteractors.Add(new AssociationEntry { associatedInteractor = interactorToAssociate, interactableAttachmentPoint = interactableAttachmentPoint });
            interactorToAssociate.AssociateWithInteractor(this);
            
            InteractorAssociated?.Invoke(interactorToAssociate);
        }

        /// <summary>
        /// Dissociates the interactor from this interactable.
        /// </summary>
        /// <param name="interactorToDissociate">The interactor to dissociate from this interactable.</param>>
        public virtual void DissociateFromInteractable(VRBaseInteractor interactorToDissociate) {
            if (!IsInteractorAssociatedWithThisInteractable(interactorToDissociate)) {
#if UNITY_EDITOR
                Debug.LogError("This interactor was not associated with the interactable.", interactorToDissociate);
#endif
                return;
            }
            
            AssociatedInteractors.Remove(AssociatedInteractors.FirstOrDefault(associatedInteractor => associatedInteractor.associatedInteractor == interactorToDissociate));
            interactorToDissociate.DissociateFromInteractor();
            
            InteractorDissociated?.Invoke(interactorToDissociate);
        }
    }

    [Serializable]
    public class InteractableEvents {
        [SerializeField]
        private UnityEvent 
            onInteractionEntered,
            onInteractionExited,
            onAssociationOccured,
            onDisassociationOccured;

        /// <summary>
        /// Invoked when the InvokeOnInteractionEntered method is fired.
        /// </summary>
        public event InteractableCallback OnInteractionEntered;
        
        /// <summary>
        /// Invoked when the InvokeOnInteractionExited method is fired.
        /// </summary>
        public event InteractableCallback OnInteractionExited;
        
        /// <summary>
        /// Invoked when the InvokeOnAssociationOccured method is fired.
        /// </summary>
        public event InteractableCallback OnAssociationOccured;
        
        /// <summary>
        /// Invoked when the InvokeOnDisassociationOccured method is fired.
        /// </summary>
        public event InteractableCallback OnDisassociationOccured;
        
        public delegate void InteractableCallback();

        /// <summary>
        /// Invokes both the OnInteractionEntered event Unity event.
        /// </summary>
        public void InvokeOnInteractionEntered() {
            OnInteractionEntered?.Invoke();
            onInteractionEntered.Invoke();
        }
        
        /// <summary>
        /// Invokes both the OnInteractionExited event Unity event.
        /// </summary>
        public void InvokeOnInteractionExited() {
            OnInteractionExited?.Invoke();
            onInteractionExited.Invoke();
        }
        
        /// <summary>
        /// Invokes both the OnAssociationOccured event Unity event.
        /// </summary>
        public void InvokeOnAssociationOccured() {
            OnAssociationOccured?.Invoke();
            onAssociationOccured.Invoke();
        }
        
        /// <summary>
        /// Invokes both the OnDisassociationOccured event and Unity event.
        /// </summary>
        public void InvokeOnDisassociationOccured() {
            OnDisassociationOccured?.Invoke();
            onDisassociationOccured.Invoke();
        }
    }
}
