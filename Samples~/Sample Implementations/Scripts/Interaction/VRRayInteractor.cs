// This script was updated on 11/8/2021 by Jack Randolph.

using System;
using System.Collections.Generic;
using ItsVR.Interaction;
using ItsVR.Player;
using UnityEngine;

namespace ItsVR_Samples.Interaction {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(VRController))]
    [RequireComponent(typeof(LineRenderer))]
    [AddComponentMenu("It's VR/Samples/Interaction/Ray Interactor (Sample)")]
    public class VRRayInteractor : VRInteractor {
        #region Variables

        /// <summary>
        /// The width of the line renderer.
        /// </summary>
        [Tooltip("The width of the line renderer.")]
        public float rayWidth = 0.05f;

        /// <summary>
        /// The maximum cast distance the interactor can sphere cast.
        /// </summary>
        [Tooltip("The maximum cast distance the interactor can sphere cast.")]
        public float castDistance = 12f;
        
        /// <summary>
        /// Physics layer(s) which interactables are on.
        /// </summary>
        [Tooltip("Physics layer(s) which interactables are on.")]
        public LayerMask interactableMask;

        /// <summary>
        /// Physics layer(s) which interactables are on.
        /// </summary>
        [Tooltip("Physics layer(s) which interactables are on.")]
        public LayerMask rayMask;
        
        /// <summary>
        /// The color of the ray while looking at a valid interactable.
        /// </summary>
        [Tooltip("The color of the ray while looking at a valid interactable.")]
        public Gradient validColor;
        
        /// <summary>
        /// The color of the ray while looking at an invalid interactable.
        /// </summary>
        [Tooltip("The color of the ray while looking at an invalid interactable.")]
        public Gradient invalidColor;
        
        /// <summary>
        /// Direct grabber events.
        /// </summary>
        [Tooltip("Direct grabber events.")]
        public DirectInteractorEvents rayInteractorEvents;

        /// <summary>
        /// Invoked when an interaction was started.
        /// </summary>
        public event VRRayInteractorEvent InteractionStarted;
        
        /// <summary>
        /// Invoked when an interaction has ended.
        /// </summary>
        public event VRRayInteractorEvent InteractionEnded;
        
        public delegate void VRRayInteractorEvent();
        private VRController _controller;
        private LineRenderer _lineRenderer;

        #endregion
        
        private void OnEnable() {
            _controller = GetComponent<VRController>();
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void OnDisable() {
            if (_lineRenderer != null)
                _lineRenderer.enabled = false;
        }

        private void Update() {
            _lineRenderer.enabled = false;
            
            var self = transform;
            var direction = -self.up;
            var selfPosition = self.position;

            var castA = false;
            var castB = false;
            var worldHit = new RaycastHit();
            var grabPointIsAlreadyAssociated = false;
            Transform hitTransform = null;
            VRInteractable interactable = null;
            
            if (associatedInteractable == null) {
                castA = Physics.Raycast(selfPosition, direction, out var interactableHit, castDistance, interactableMask);
                castB = Physics.Raycast(selfPosition, direction, out worldHit, castDistance, rayMask);

                if (castA) {
                    hitTransform = interactableHit.collider.transform;
                    interactable = hitTransform.GetComponentInParent<VRInteractable>();

                    if (interactable != null && interactable.IsAttachmentPointAssociated(hitTransform)) {
                        grabPointIsAlreadyAssociated = true;
                        interactable = null;
                    }
                }
            }
            
            if (interactable != null && associatedInteractable == null && _controller.inputContainer.universalInputs.GripDepress > 0.65f) {
                interactable.Associate(this, hitTransform);
                associatedInteractable = interactable;
            }
            else if (associatedInteractable != null && _controller.inputContainer.universalInputs.GripDepress < 0.65f) {
                associatedInteractable.Dissociate(this);
                associatedInteractable = null;
            }

            if (associatedInteractable != null) return;
            
            _lineRenderer.enabled = castA || castB;
            _lineRenderer.colorGradient = castA ? !grabPointIsAlreadyAssociated ? validColor : invalidColor : invalidColor;
            _lineRenderer.widthCurve = AnimationCurve.Linear(0f, rayWidth / 2f, 1f, rayWidth);
            
            var positions = new List<Vector3> {
                selfPosition,
                castA ? hitTransform.position : castB ? worldHit.point : direction
            };

            _lineRenderer.SetPositions(positions.ToArray());
        }
        
        public override void Associate(VRInteractable interactable) {
            base.Associate(interactable);
            InteractionStarted?.Invoke();
            rayInteractorEvents.interactionStarted.Invoke();
        }

        public override void Dissociate(VRInteractable interactable) {
            base.Dissociate(interactable);
            InteractionEnded?.Invoke();
            rayInteractorEvents.interactionEnded.Invoke();
        }
    }
}
