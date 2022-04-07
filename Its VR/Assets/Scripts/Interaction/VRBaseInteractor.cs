// This script was updated on 02/20/2022 by Jack Randolph.

using System;
using ItsVR.Editor;
using ItsVR.Player;
using UnityEngine;
using UnityEngine.Events;

namespace ItsVR.Interaction {
    /// <summary>
    /// It's VR interactor base class.
    /// </summary>
    /// <para>Author: Jack Randolph</para>
    [RequireComponent(typeof(VRController))]
    [HelpURL("https://jackedupstudios.com/vr-interactor")]
    public class VRBaseInteractor : MonoBehaviour, IItsVRProblemDebugable {
        #region Variables

        /// <summary>
        /// The interactors attachment point.
        /// </summary>
        [Tooltip("The interactors attachment point.")]
        public Transform attachmentPoint;

        /// <summary>
        /// If true, the interactor will override the associated interactable when a new association is requested.
        /// </summary>
        [Tooltip("If true, the interactor will override the associated interactable when a new association is requested.")]
        public bool canOverride = true;
        
        /// <summary>
        /// The interactable which is associated with this interactor.
        /// </summary>
        public VRBaseInteractable AssociatedInteractable { get; private set; }

        /// <summary>
        /// Invoked when an interactable is associated with this interactor.
        /// </summary>
        public event InteractableAssociationCallback InteractableAssociated;
        
        public delegate void InteractableAssociationCallback(VRBaseInteractable interactable);
        
        /// <summary>
        /// Invoked when an interactable is dissociated from this interactor.
        /// </summary>
        public event InteractableDissociationCallback InteractableDissociated;
        
        public delegate void InteractableDissociationCallback();
        
        #endregion

        /// <summary>
        /// Associates the interactable with this interactor.
        /// </summary>
        /// <param name="interactableToAssociate">The interactable to associate.</param>
        public virtual void AssociateWithInteractor(VRBaseInteractable interactableToAssociate) {
            if (!canOverride && AssociatedInteractable != null)
                return;

            if (AssociatedInteractable != null) 
                DissociateFromInteractor();

            AssociatedInteractable = interactableToAssociate;
            
            InteractableAssociated?.Invoke(AssociatedInteractable);
        }

        /// <summary>
        /// Disassociates the interactable from this interactor.
        /// </summary>
        public virtual void DissociateFromInteractor() {
            AssociatedInteractable = null;
            
            InteractableDissociated?.Invoke();
        }

#if UNITY_EDITOR
        public void RefreshProblems() {
            if (attachmentPoint == null)
                ItsVRProblemDebuggerEditor.SubmitProblem("An attachment point was not assigned to this interactor.", ItsVRProblemDebuggerEditor.ProblemLevels.Error, transform);
        }
#endif
    }

    [Serializable]
    public class InteractorEvents {
        [SerializeField] private UnityEvent
            onInteractorAssociated,
            whileInteractorAssociated,
            onInteractorDisassociated,
            whileInteractorDisassociated;

        /// <summary>
        /// Invoked when the CallOnInteractorAssociatedCallback method is fired.
        /// </summary>
        public event InteractorCallback OnInteractorAssociated;

        /// <summary>
        /// Invoked when the CallWhileInteractorAssociatedCallback method is fired.
        /// </summary>
        public event InteractorCallback WhileInteractorAssociated;

        /// <summary>
        /// Invoked when the CallOnInteractorDisassociated method is fired.
        /// </summary>
        public event InteractorCallback OnInteractorDisassociated;

        /// <summary>
        /// Invoked when the CallWhileInteractorDissociated method is fired.
        /// </summary>
        public event InteractorCallback WhileInteractorDisassociated;

        public delegate void InteractorCallback();

        /// <summary>
        /// Invokes both the OnInteractorAssociated event and OnInteractorAssociated Unity event.
        /// </summary>
        public void InvokeOnInteractorAssociatedCallback() {
            OnInteractorAssociated?.Invoke();
            onInteractorAssociated.Invoke();
        }

        /// <summary>
        /// Invokes both the WhileInteractorAssociated event and WhileInteractorAssociated Unity event.
        /// </summary>
        public void InvokeWhileInteractorAssociatedCallback() {
            WhileInteractorAssociated?.Invoke();
            whileInteractorAssociated.Invoke();
        }

        /// <summary>
        /// Invokes both the OnInteractorDisassociated event and OnInteractorDisassociated Unity event.
        /// </summary>
        public void InvokeOnInteractorDisassociatedCallback() {
            OnInteractorDisassociated?.Invoke();
            onInteractorDisassociated.Invoke();
        }

        /// <summary>
        /// Invokes both the WhileInteractorDisassociated event and WhileInteractorDisassociated Unity event.
        /// </summary>
        public void InvokeWhileInteractorDisassociatedCallback() {
            WhileInteractorDisassociated?.Invoke();
            whileInteractorDisassociated.Invoke();
        }
    }
}
