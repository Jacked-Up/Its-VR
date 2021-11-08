// This script was updated on 11/8/2021 by Jack Randolph.

using UnityEngine;
using UnityEngine.InputSystem;
using ItsVR.Player;

namespace ItsVR_Samples.Locomotion {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(VRRig))]
    [RequireComponent(typeof(CharacterController))]
    [AddComponentMenu("It's VR/Samples/Locomotion/Continuous Move (Sample)")]
    public class VRContinuousMove : MonoBehaviour {
        #region Variables

        /// <summary>
        /// The controller which this script reads input from.
        /// </summary>
        [Tooltip("The controller which this script reads input from.")]
        public VRController inputController;
        
        /// <summary>
        /// Defines the forward direction of the rig. NOTE: If hand is selected, the script will use the input controller as the forward hand.
        /// </summary>
        [Tooltip("Defines the forward direction of the rig. NOTE: If hand is selected, the script will use the input controller as the forward hand.")]
        public MoveVectors moveVector;

        /// <summary>
        /// The maximum speed the rig can move while walking.
        /// </summary>
        [Range(1, 10)] [Tooltip("The maximum speed the rig can move while walking.")]
        public float walkingSpeed = 1.5f;
        
        /// <summary>
        /// The maximum speed the rig can move while sprinting.
        /// </summary>
        [Range(1, 10)] [Tooltip("The maximum speed the rig can move while sprinting.")]
        public float sprintSpeed = 2.25f;

        /// <summary>
        /// All the layers the locomotion controller can interact with.
        /// </summary>
        [Tooltip("All the layers the locomotion controller can interact with.")]
        public LayerMask layerMask;
        
        /// <summary>
        /// The rigs velocity.
        /// </summary>
        [HideInInspector]
        public Vector3 velocity;

        /// <summary>
        /// If the player is touching the ground or not.
        /// </summary>
        public bool IsGrounded => Physics.CheckSphere(_vrRig.FeetPosition, _vrRig.Width, layerMask);
        
        private VRRig _vrRig;
        private CharacterController _characterController;
        public enum MoveVectors { Head, Hand }

        #endregion

        private void OnEnable() {
            if (inputController == null)
                Debug.LogError("[VR Continuous Move] No input controller was referenced. (I have no way to receive input)", this);
            
            if (_characterController == null)
                _characterController = GetComponent<CharacterController>();
            
            if (_vrRig == null) 
                _vrRig = GetComponent<VRRig>();
            
            // This class contains an event which fires right after the Update()
            // method is invoked. Subscribing to the event will ensure that the
            // continuous move script stays in sync with the VR Trackers.
            InputSystem.onAfterUpdate += MoveUpdate;
        }

        private void OnDisable() {
            _characterController.enabled = false;

            InputSystem.onAfterUpdate -= MoveUpdate;
        }

        private void Update() {
            MoveUpdate();
        }

        /// <summary>
        /// Runs the movement behavior.
        /// </summary>
        private void MoveUpdate() {
            // Bail if the input controller is null.
            if (inputController == null) return;

            // Enable the character controller is it is disabled.
            if (!_characterController.enabled)
                _characterController.enabled = true;
            
            // Here, we are sizing and centering the character controller with the
            // players height and position.
            _characterController.height = _vrRig.Height;
            _characterController.radius = _vrRig.Width;
            
            // The hip position is not exactly where the players actual hips are. The hip position
            // is the position between the head and the feet.
            _characterController.center = _vrRig.HipLocalPosition;

            // First, we fetch the position of the joystick.
            var joystickInputPosition = inputController.inputReference.universalInputs.JoystickPosition;
            
            // Secondly, we get a transform which will define what is forward, backwards, left, and right for the player. 
            var motionVectorsReference = moveVector == MoveVectors.Head ? _vrRig.head : inputController.transform;
            
            // (Optional) A speed to move the player at. Here, we are seeing if the joystick is pressed in or not.
            // If the joystick is pushed in, we are making the player sprint. If the player is not pushing the
            // joystick, we are making the player walk.
            var movementSpeed = inputController.inputReference.universalInputs.JoystickPressed ? sprintSpeed : walkingSpeed;
            
            // Thirdly, we will combine all of the input and the defined direction vectors into one vector3.
            var moveDirection = motionVectorsReference.right * joystickInputPosition.x + motionVectorsReference.forward * joystickInputPosition.y;
            moveDirection.y = 0;
            
            // Lastly, lets apply all of the movement calculations to the player.
            _characterController.Move(moveDirection * (movementSpeed * Time.deltaTime));

            // First we check if the player is on the ground or not.
            // If the player is not touching the ground, we are gradually adding velocity downward
            // to make the player fall.
            // If the player is touching the ground, we are applying a small downward force in order
            // to keep the player clamped to the ground.
            if (!IsGrounded) {
                velocity.y += Physics.gravity.y * Time.deltaTime;
            }
            else {
                velocity = new Vector3(0, -2, 0);
            }

            // Lastly, we just apply the physics to the player.
            _characterController.Move(velocity * Time.deltaTime);
        }
        
        #region Editor

        private void OnDrawGizmos() {
            var vrRig = GetComponent<VRRig>();
            var characterController = GetComponent<CharacterController>();
            
            if (vrRig == null || characterController == null) return;
            
            characterController.height = vrRig.Height;
            characterController.radius = vrRig.Width;
            characterController.center = new Vector3(vrRig.FeetLocalPosition.x, vrRig.Height / 2, vrRig.FeetLocalPosition.z);
        }

        #endregion
    }
}
