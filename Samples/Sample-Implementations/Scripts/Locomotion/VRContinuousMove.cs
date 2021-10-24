using ItsVR.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItsVR_Samples.Locomotion {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(VRRig))]
    [RequireComponent(typeof(CharacterController))]
    [AddComponentMenu("It's VR/Samples/Locomotion/Continuous Move")]
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
        /// The maximum speed the rig can move which walking.
        /// </summary>
        [Range(1, 10)] [Tooltip("The maximum speed the rig can move which walking.")]
        public float walkingSpeed = 1.5f;
        
        /// <summary>
        /// The maximum speed the rig can move which sprinting.
        /// </summary>
        [Range(1, 10)] [Tooltip("The maximum speed the rig can move which sprinting.")]
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
        
        private VRRig _vrRig;
        private CharacterController _characterController;
        public enum MoveVectors { Head, Hand }

        #endregion

        private void OnEnable() {
            if (inputController == null)
                Debug.LogError("[VR Continuous Move] No input controller was referenced. (I have to way to receive input)", this);
            
            _characterController = GetComponent<CharacterController>();
            _vrRig = GetComponent<VRRig>();
            
            InputSystem.onAfterUpdate += Move;
        }

        private void OnDisable() {
            _characterController = null;
            _vrRig = null;
            
            InputSystem.onAfterUpdate -= Move;
        }

        private void Update() {
            Move();
        }

        private void Move() {
            if (inputController == null) return;

            // Scaling behavior.
            _characterController.height = _vrRig.Height;
            _characterController.radius = _vrRig.Width;
            _characterController.center = new Vector3(_vrRig.FeetLocalPosition.x, _vrRig.Height / 2, _vrRig.FeetLocalPosition.z);
            
            // Movement behavior.
            var joystickPosition = inputController.inputReference.JoystickPosition;
            var motionVector = moveVector == MoveVectors.Head ? _vrRig.head : inputController.transform;
            var moveSpeed = inputController.inputReference.JoystickPressed ? sprintSpeed : walkingSpeed;
            var moveDir = motionVector.right * joystickPosition.x + motionVector.forward * joystickPosition.y;
            moveDir.y = 0;
            
            _characterController.Move(moveDir * (moveSpeed * Time.deltaTime));
            
            // 'Physics' behavior.
            if (Physics.CheckSphere(_vrRig.FeetPosition, _vrRig.Width, layerMask)) {
                velocity.y += Physics.gravity.y * Time.deltaTime;
            }
            else {
                if (!(velocity.y > 0)) 
                  velocity = new Vector3(0, -2, 0);
            }

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
