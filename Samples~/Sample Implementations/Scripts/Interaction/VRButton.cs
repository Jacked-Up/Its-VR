// This script was updated on 11/8/2021 by Jack Randolph.

using System;
using System.Linq;
using ItsVR.Player;
using UnityEngine;
using UnityEngine.Events;

namespace ItsVR_Samples.Interaction {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    [AddComponentMenu("It's VR/Samples/Interaction/Button (Sample)")]
    public class VRButton : MonoBehaviour {
        #region Variables

        /// <summary>
        /// Layer which the controller is on.
        /// </summary>
        [Tooltip("Layer which the controller is on.")]
        public LayerMask controllerMask;
        
        /// <summary>
        /// The overlap detection distance.
        /// </summary>
        [Tooltip("The overlap detection distance.")]
        public float handDetectionRadius = 0.2f;
        
        /// <summary>
        /// The point check tolerance for the up position and the down position.
        /// </summary>
        [Tooltip("The point check tolerance for the up position and the down position.")]
        public float pointCheckTolerance = 0.025f;
        
        /// <summary>
        /// The speed the button returns to its starting position when the interaction is ended.
        /// </summary>
        [Tooltip("The speed the button returns to its starting position when the interaction is ended.")]
        public float returnSpeed = 5f;

        /// <summary>
        /// Audio clip played when the button returns to its up position.
        /// </summary>
        [Tooltip("Audio clip played when the button returns to its up position.")]
        public AudioClip fxButtonUp;
        
        /// <summary>
        /// Audio clip played when the button reaches its down position.
        /// </summary>
        [Tooltip("Audio clip played when the button reaches its down position.")]
        public AudioClip fxButtonDown;
        
        /// <summary>
        /// All of the buttons unity events.
        /// </summary>
        [Tooltip("All of the buttons unity events.")]
        public ButtonEvents buttonEvents;

        private AudioSource _audioSource;
        private VRController _interactingController;
        private Vector3 _startingPosition;
        private float _maximumY;
        private float _minimumY;
        private float _previousHandHeight;
        private bool _previouslyInDownPosition;
        private bool _previouslyInUpPosition;
        private bool _handPresentLastFrame;
        private bool _handPresentThisFrame;
        
        #endregion

        private void Start() {
            // Calculates the bounds of the button using the collider component on the button.
            var col = GetComponent<Collider>();
            var selfPos = transform.localPosition;
            _maximumY = selfPos.y;
            _minimumY = selfPos.y - col.bounds.size.y * 0.5f;
            
            _startingPosition = selfPos;
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update() {
            // First we fetch all of the overlapping colliders in the hand detection radius.
            var overlaps = Physics.OverlapSphere(transform.position, handDetectionRadius, controllerMask);
            
            // Next we check to see if any controllers are on any of the overlapping objects.
            var overlappingController = overlaps.Select(overlap => overlap.GetComponent<VRController>()).FirstOrDefault(overlapController => overlapController != null);
            _handPresentThisFrame = overlappingController != null;
            
            switch (_handPresentThisFrame) {
                // If the hand is present this frame and was not last frame, then we start the
                // press.
                case true when !_handPresentLastFrame:
                    StartPress(overlappingController);
                    break;
                // If the hand is not present this frame and was last frame, then we end the
                // press.
                case false when _handPresentLastFrame:
                    EndPress();
                    break;
            }
            
            // Cache if the hand was present this frame.
            _handPresentLastFrame = _handPresentThisFrame;
            
            if (_interactingController != null) {
                // First we calculate the new hand height. This is the interacting controllers
                // world space y position converted to local space.
                var newHandHeight = GetLocalYPos(_interactingController.transform.position);
                
                // Next we calculate the height difference from last frame.
                var handDifference = _previousHandHeight - newHandHeight;
                
                // Cache the new hand height for next frame.
                _previousHandHeight = newHandHeight;

                // Lastly we sync the y position of the button with the interacting controllers
                // y position.
                var newPosition = transform.localPosition.y - handDifference;
                SyncYPosition(newPosition);
            }
            else {
                // If the player is no longer pressing the button, the button will return to its
                // original position over time.
                SyncYPosition(Mathf.Lerp(transform.localPosition.y, _startingPosition.y, Time.deltaTime * returnSpeed));
            }
            
            // Process the button unity events.
            ProcessEvents();
        }

        /// <summary>
        /// Sets up button for the interaction.
        /// </summary>
        /// <param name="controller">The controller which is interacting with the button.</param>
        private void StartPress(VRController controller) {
            _interactingController = controller;
            _previousHandHeight = GetLocalYPos(_interactingController.transform.position);
        }

        /// <summary>
        /// Ends the button and controllers interaction.
        /// </summary>
        private void EndPress() {
            _interactingController = null;
            _previousHandHeight = 0f;
        }

        /// <summary>
        /// Coverts the world space y position into local space y position.
        /// </summary>
        /// <param name="position">The world space position to convert.</param>
        /// <returns>Converted world space y position into local space y position.</returns>
        private float GetLocalYPos(Vector3 position) {
            var localPosition = transform.root.InverseTransformPoint(position);
            return localPosition.y;
        }

        /// <summary>
        /// Syncs the buttons position with the interacting controllers position.
        /// </summary>
        /// <param name="position"></param>
        private void SyncYPosition(float position) {
            var newPos = transform.localPosition;
            newPos.y = Mathf.Clamp(position, _minimumY, _maximumY);
            transform.localPosition = newPos;
        }

        /// <summary>
        /// Processes all of the buttons unity events.
        /// </summary>
        private void ProcessEvents() {
            // Check that the button is inside the point tolerance of either the
            // up position or the down position.
            var selfLocalPos = transform.localPosition;
            var inUpPosition = Math.Abs(selfLocalPos.y - _maximumY) < pointCheckTolerance;
            var inDownPosition = Math.Abs(selfLocalPos.y - _minimumY) < pointCheckTolerance;

            // Button is in the up position this frame but was not last frame.
            if (inUpPosition && !_previouslyInUpPosition) {
                if (fxButtonUp != null) _audioSource.PlayOneShot(fxButtonUp);
                buttonEvents.onButtonUp.Invoke();
            }
             
            // Button is in the up position.
            if (inUpPosition)
                buttonEvents.whileButtonUp.Invoke();
            
            // Button is in the down position this frame but was not last frame.
            if (inDownPosition && !_previouslyInDownPosition) {
                if (fxButtonDown != null) _audioSource.PlayOneShot(fxButtonDown);
                buttonEvents.onButtonDown.Invoke();
            }

            // Button is in the down position.
            if (inDownPosition)
                buttonEvents.whileButtonDown.Invoke();
            
            // Cache button position states.
            _previouslyInUpPosition = inUpPosition;
            _previouslyInDownPosition = inDownPosition;
        }

        #region Editor

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, handDetectionRadius);
        }

        #endregion
    }

    [Serializable]
    public class ButtonEvents {
        /// <summary>
        /// Invoked when the button is in the up position.
        /// </summary>
        [Tooltip("Invoked when the button is in the up position.")]
        public UnityEvent onButtonUp;
        
        /// <summary>
        /// Invoked every frame when the button is in the up position.
        /// </summary>
        [Tooltip("Invoked every frame when the button is in the up position.")]
        public UnityEvent whileButtonUp;
        
        /// <summary>
        /// Invoked when the button is in the down position.
        /// </summary>
        [Tooltip("Invoked when the button is in the down position.")]
        public UnityEvent onButtonDown;
        
        /// <summary>
        /// Invoked every frame when the button is in the down position.
        /// </summary>
        [Tooltip("Invoked every frame when the button is in the down position.")]
        public UnityEvent whileButtonDown;
    }
}
