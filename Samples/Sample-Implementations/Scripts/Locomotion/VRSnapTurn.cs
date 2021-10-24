using ItsVR.Player;
using UnityEngine;

namespace ItsVR_Samples.Locomotion {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(VRRig))]
    [AddComponentMenu("It's VR/Samples/Locomotion/Snap Turn")]
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
                Debug.LogError("[VR Snap Turn] An input controller must be referenced for input to work.", this);

            _vrRig = GetComponent<VRRig>();
        }

        private void Update() {
            if (inputController == null) return;
            
            if (_frameTick > debounceTime) {
                if (inputController.inputReference.JoystickPosition.x > 0.5f) {
                    _vrRig.RotateRig(turnAngle);
                    _frameTick = 0;
                }
                else if (inputController.inputReference.JoystickPosition.x < -0.5f) {
                    _vrRig.RotateRig(-turnAngle);
                    _frameTick = 0;
                }
                else if (inputController.inputReference.JoystickPosition.y < -0.5f && canTurnAround) {
                    _vrRig.RotateRig(180);
                    _frameTick = 0;
                }
            }
            else 
                _frameTick += Time.deltaTime;
        }
    }
}
