// This script was updated on 11/8/2021 by Jack Randolph.

using ItsVR.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItsVR_Samples.Locomotion {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(VRRig))]
    [AddComponentMenu("It's VR/Samples/Locomotion/Continuous Turn (Sample)")]
    public class VRContinuousTurn : MonoBehaviour {
        #region Variables

        /// <summary>
        /// The controller which this script reads input from.
        /// </summary>
        [Tooltip("The controller which this script reads input from.")]
        public VRController inputController;
        
        /// <summary>
        /// The speed at which the player turns.
        /// </summary>
        [Range(1f, 250f)] [Tooltip("The speed at which the player turns.")]
        public float turnSpeed = 50f;

        private VRRig _vrRig;
        
        #endregion

        private void OnEnable() {
            if (inputController == null)
                Debug.LogError("[VR Continuous Move] No input controller was referenced. (I have no way to receive input)", this);

            _vrRig = GetComponent<VRRig>();
            InputSystem.onAfterUpdate += RotateUpdate;
        }

        private void OnDisable() {
            InputSystem.onAfterUpdate -= RotateUpdate;
        }

        private void Update() {
            RotateUpdate();
        }

        /// <summary>
        /// Runs the rotation behavior.
        /// </summary>
        private void RotateUpdate() {
            // Bail if the input controller is null.
            if (inputController == null) return;

            // Cache the joystick x position.
            var joystickPositionX = inputController.inputReference.universalInputs.JoystickPosition.x;
            
            // This break only ensures that the player is intentionally flicking the joystick.
            if (joystickPositionX < 0.2f && joystickPositionX > -0.2f) return;
            
            // Here, we apply the rotation and pivot around the players head.
            _vrRig.RotateRig(turnSpeed * (Time.deltaTime * joystickPositionX));
        }
    }
}