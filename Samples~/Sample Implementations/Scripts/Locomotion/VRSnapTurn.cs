// This script was updated on 10/30/2021 by Jack Randolph.

using ItsVR.Player;
using UnityEngine;

namespace ItsVR_Samples.Locomotion {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(VRRig))]
    [AddComponentMenu("It's VR/Samples/Locomotion/Snap Turn (Sample)")]
    public class VRSnapTurn : MonoBehaviour {
        #region Variables

        /// <summary>
        /// The controller which this script reads input from.
        /// </summary>
        [Tooltip("The controller which this script reads input from.")]
        public VRController inputController;
        
        /// <summary>
        /// How far the player turns.
        /// </summary>
        [Range(1, 360)] [Tooltip("How far the player turns.")]
        public int turnAngle = 45;

        /// <summary>
        /// How long the player must wait until they can turn again.
        /// </summary>
        [Range(0.05f, 1f)] [Tooltip("How long the player must wait until they can turn again.")]
        public float debounceTime = 0.22f;

        /// <summary>
        /// If the player can turn completely around by flicking the joystick downward.
        /// </summary>
        [Tooltip("If the player can turn completely around by flicking the joystick downward.")]
        public bool canTurnAround;
        
        private float _frameTick;
        private VRRig _vrRig;
        
        #endregion

        private void OnEnable() {
            if (inputController == null)
                Debug.LogError("[VR Continuous Move] No input controller was referenced. (I have no way to receive input)", this);

            _vrRig = GetComponent<VRRig>();
        }

        private void Update() {
            if (inputController == null) return;
            
            // The debounce feature allows the player to hold the joystick to one side
            // and snap spin over some time. This makes it easier to spin a lot without
            // flicking the joystick a bunch of times (which can be annoying and tiring).
            if (_frameTick > debounceTime) {
                if (inputController.inputReference.universalInputs.JoystickPosition.x > 0.5f) {
                    _vrRig.RotateRig(turnAngle);
                    inputController.Vibrate(0.15f, 0.075f);
                    _frameTick = 0;
                }
                else if (inputController.inputReference.universalInputs.JoystickPosition.x < -0.5f) {
                    _vrRig.RotateRig(-turnAngle);
                    inputController.Vibrate(0.15f, 0.075f);
                    _frameTick = 0;
                }
                else if (inputController.inputReference.universalInputs.JoystickPosition.y < -0.5f && canTurnAround) {
                    _vrRig.RotateRig(180);
                    inputController.Vibrate(0.15f, 0.075f);
                    _frameTick = 0;
                }
            }
            else {
                // If the frame tick counter if below the debounce time, we should count
                // up the frame tick until it is greater than the debounce time. This
                // feature prevents the player from spinning every single frame and causing
                // motion sickness.
                _frameTick += Time.deltaTime;
            }
        }
    }
}
