// This script was updated on 03/20/2022 by Jack Randolph.

using ItsVR.Editor.Tools;
using ItsVR.Player;
using UnityEngine;

namespace ItsVR.Locomotion {
    /// <summary>
    /// Snap turn locomotion component.
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("It's VR/Locomotion/Snap Turn")]
    public class VRSnapTurn : VRBaseLocomotion, IItsVRProblemDebugable {
        #region Variables

        private const float JOYSTICK_FLICK_INITIALIZE_THRESHOLD = 0.4f;
        private const float HAPTICS_AMPLITUDE = 0.15f;
        private const float HAPTICS_DURATION = 0.075f;
        
        /// <summary>
        /// How far the player turns.
        /// </summary>
        [Range(1, 360)] [Tooltip("How far the player turns.")]
        public int turnAngle = 45;

        /// <summary>
        /// How long the player must wait until they can turn again.
        /// </summary>
        [Range(0.05f, 1f)] [Tooltip("How long the player must wait until they can turn again.")]
        public float inputTimeoutLength = 0.22f;

        /// <summary>
        /// If the player can turn completely around by flicking the joystick downward.
        /// </summary>
        [Tooltip("If the player can turn completely around by flicking the joystick downward.")]
        public bool canTurnAround;

        /// <summary>
        /// If true, the haptics will engage when the player turns.
        /// </summary>
        [Tooltip("If true, the haptics will engage when the player turns.")]
        public bool useHaptics = true;

        private VRRig _vrRig;
        private float _debounceTime;

        #endregion

        private void OnEnable() {
            _vrRig = GetComponent<VRRig>();
            ItsSystems.OnUpdate += OnUpdateCallback;
        }

        private void OnDisable() => ItsSystems.OnUpdate -= OnUpdateCallback;
            
        private void OnUpdateCallback(UpdateTime arg) {
            if (inputController == null)
                return;

            if (_debounceTime > inputTimeoutLength) {
                if (inputController.inputContainer.universal.JoystickPosition.x > JOYSTICK_FLICK_INITIALIZE_THRESHOLD) {
                    _debounceTime = 0;
                    _vrRig.RotateRig(turnAngle, Vector3.up);
                    
                    if (useHaptics)
                        inputController.inputContainer.universal.SendHapticPulse(HAPTICS_AMPLITUDE, HAPTICS_DURATION);
                }
                else if (inputController.inputContainer.universal.JoystickPosition.x < -JOYSTICK_FLICK_INITIALIZE_THRESHOLD) {
                    _debounceTime = 0;
                    _vrRig.RotateRig(-turnAngle, Vector3.up);
                    
                    if (useHaptics) 
                        inputController.inputContainer.universal.SendHapticPulse(HAPTICS_AMPLITUDE, HAPTICS_DURATION);
                }
                else if (inputController.inputContainer.universal.JoystickPosition.y < -JOYSTICK_FLICK_INITIALIZE_THRESHOLD && canTurnAround) {
                    _debounceTime = 0;
                    _vrRig.RotateRig(180, Vector3.up);
                    
                    if (useHaptics)
                        inputController.inputContainer.universal.SendHapticPulse(HAPTICS_AMPLITUDE, HAPTICS_DURATION);
                }
            }
            else 
                _debounceTime += Time.deltaTime;
        }
        
#if UNITY_EDITOR
        private void OnValidate() => RefreshProblems();
        
        public void RefreshProblems() {
            if (inputController == null)
                Editor.Tools.ItsVRProblemDebuggerEditor.SubmitProblem("An input VR controller must be referenced for the VR snap turn component to receive input.", Editor.Tools.ItsVRProblemDebuggerEditor.ProblemLevels.Error, transform);
        }
#endif
    }
}
