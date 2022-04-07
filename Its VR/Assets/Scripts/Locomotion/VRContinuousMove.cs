// This script was updated on 03/19/2022 by Jack Randolph.

using ItsVR.Editor.Tools;
using ItsVR.Player;
using UnityEngine;

namespace ItsVR.Locomotion {
    /// <summary>
    /// Continuous move locomotion component.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    [AddComponentMenu("It's VR/Locomotion/Continuous Move")]
    public class VRContinuousMove : VRBaseLocomotion, IItsVRProblemDebugable {
        #region Variables

        /// <summary>
        /// Defines the forward direction of the player.
        /// </summary>
        [Tooltip("Defines the forward direction of the player.")]
        public ForwardDefinitions forwardDefinition;

        public enum ForwardDefinitions { Head, Hand }
        
        /// <summary>
        /// The maximum speed the player can move while walking.
        /// </summary>
        [Range(1, 10)] [Tooltip("The maximum speed the player can move while walking.")]
        public float walkingSpeed = 1.5f;
        
        /// <summary>
        /// The maximum speed the player can move while sprinting.
        /// </summary>
        [Range(1, 10)] [Tooltip("The maximum speed the player can move while sprinting.")]
        public float sprintingSpeed = 2.25f;

        /// <summary>
        /// All the layers the locomotion controller can interact with.
        /// </summary>
        [Tooltip("All the layers the locomotion controller can interact with.")]
        public LayerMask physicsLayerMask;
        
        /// <summary>
        /// The player velocity.
        /// </summary>
        [HideInInspector]
        public Vector3 playerVelocity;

        /// <summary>
        /// True if the player is touching the ground.
        /// </summary>
        public bool IsOnGround => Physics.CheckSphere(GetComponent<VRRig>().GetEstimatedFeetPosition(), GetComponent<VRRig>().Width, physicsLayerMask);
        
        private CharacterController _characterController;

        #endregion

        private void OnEnable() {
            _characterController = GetComponent<CharacterController>();
            ItsSystems.BeforeUpdateAndOnUpdate += BeforeUpdateAndOnUpdateCallback;

#if UNITY_EDITOR
            RefreshProblems();
#endif
        }

        private void OnDisable() => ItsSystems.BeforeUpdateAndOnUpdate -= BeforeUpdateAndOnUpdateCallback;

        private void BeforeUpdateAndOnUpdateCallback(UpdateTime arg) {
            if (inputController == null || !_characterController.enabled) 
                return;
            
            var joystickPosition = inputController.inputContainer.universal.JoystickPosition;
            
            
            var definedForward = forwardDefinition == ForwardDefinitions.Head 
                ? GetComponent<VRRig>().head 
                : inputController.transform;
            
            var movementSpeed = inputController.inputContainer.universal.JoystickPressed 
                ? sprintingSpeed 
                : walkingSpeed;
            
            var movementDirection = definedForward.right * joystickPosition.x + definedForward.forward * joystickPosition.y;
            movementDirection.y = 0;
            
            _characterController.height = GetComponent<VRRig>().Height;
            _characterController.radius = GetComponent<VRRig>().Width;
            _characterController.center = GetComponent<VRRig>().GetEstimatedHipPosition(false);

            _characterController.Move(movementDirection * (movementSpeed * Time.deltaTime));
            
            if (IsOnGround) 
                playerVelocity = new Vector3(0, -2, 0);
            else 
                playerVelocity.y += Physics.gravity.y * Time.deltaTime;

            _characterController.Move(playerVelocity * Time.deltaTime);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos() {
            var rig = GetComponent<VRRig>();
            var characterController = GetComponent<CharacterController>();
            
            if (rig == null || characterController == null)
                return;
            
            characterController.height = rig.Height;
            characterController.radius = rig.Width;
            characterController.center = rig.GetEstimatedHipPosition(false);
        }

        private void OnValidate() => RefreshProblems();

        public void RefreshProblems() {
            if (inputController == null)
                ItsVRProblemDebuggerEditor.SubmitProblem("An input VR controller must be referenced for the VR continuous move component to receive input.", Editor.Tools.ItsVRProblemDebuggerEditor.ProblemLevels.Error, transform);
        }
#endif
    }
}
