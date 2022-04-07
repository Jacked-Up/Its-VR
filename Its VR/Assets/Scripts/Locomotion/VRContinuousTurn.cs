// This script was updated on 03/19/2022 by Jack Randolph.

using ItsVR.Editor.Tools;
using ItsVR.Player;
using UnityEngine;

namespace ItsVR.Locomotion {
    /// <summary>
    /// Continuous turn locomotion component.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(VRRig))]
    [AddComponentMenu("It's VR/Locomotion/Continuous Turn")]
    public class VRContinuousTurn : VRBaseLocomotion, IItsVRProblemDebugable {
        #region Variables

        private const float JOYSTICK_MINIMUM_THRESHOLD = 0.2f;
        
        /// <summary>
        /// The speed at which the player turns.
        /// </summary>
        [Range(1f, 250f)] [Tooltip("The speed at which the player turns.")]
        public float turnSpeed = 50f;

        private VRRig _vrRig;
        
        #endregion

        private void OnEnable() {
            _vrRig = GetComponent<VRRig>();
            ItsSystems.BeforeUpdateAndOnUpdate += BeforeUpdateAndOnUpdateCallback;
        }

        private void OnDisable() => ItsSystems.BeforeUpdateAndOnUpdate -= BeforeUpdateAndOnUpdateCallback;
            
        private void BeforeUpdateAndOnUpdateCallback(UpdateTime updateType) {
            if (inputController == null) 
                return;

            var joystickPositionX = inputController.inputContainer.universal.JoystickPosition.x;

            if (joystickPositionX < JOYSTICK_MINIMUM_THRESHOLD && joystickPositionX > -JOYSTICK_MINIMUM_THRESHOLD) 
                return;

            _vrRig.RotateRig(turnSpeed * (Time.deltaTime * joystickPositionX), Vector3.up);
        }
        
#if UNITY_EDITOR
        private void OnValidate() => RefreshProblems();
        
        public void RefreshProblems() {
            if (inputController == null)
                Editor.Tools.ItsVRProblemDebuggerEditor.SubmitProblem("An input VR controller must be referenced for the VR continuous turn component to receive input.", Editor.Tools.ItsVRProblemDebuggerEditor.ProblemLevels.Error, transform);
        }
#endif
    }
}