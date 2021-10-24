using ItsVR.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItsVR_Samples.Locomotion {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(VRRig))]
    [AddComponentMenu("It's VR/Samples/Locomotion/Continuous Turn")]
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
                Debug.LogError("[VR Continuous Turn] An input controller must be referenced for input to work.", this);
            
            _vrRig = GetComponent<VRRig>();
            InputSystem.onAfterUpdate += Rotate;
        }

        private void OnDisable() {
            InputSystem.onAfterUpdate -= Rotate;
        }

        private void Update() {
            Rotate();
        }

        private void Rotate() {
            if (inputController == null) return;

            var joystickPositionX = inputController.inputReference.JoystickPosition.x;
            
            // Only rotates the player when they have the joystick flicked for than
            // position X is greater than or less than 0.2.
            if (joystickPositionX < 0.2f && joystickPositionX > -0.2f) return;
            
            _vrRig.RotateRig(turnSpeed * (Time.deltaTime * joystickPositionX));
        }
    }
}