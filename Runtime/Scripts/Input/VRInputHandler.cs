// This script was updated on 10/26/2021 by Jack Randolph.
// This script is incomplete.
// Documentation: https://jackedupstudios.com/vr-input-handler

using ItsVR.Scriptables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItsVR.Input {
    [DisallowMultipleComponent]
    [AddComponentMenu("It's VR/Input/VR Input Handler (Incomplete)")]
    public class VRInputHandler : MonoBehaviour {
        #region Variables

        /// <summary>
        /// HMD/headset tracker reference.
        /// </summary>
        [Tooltip("HMD/headset tracker reference.")]
        public VRTrackerReferences headsetTracker;

        /// <summary>
        /// Right hand/controller tracker reference.
        /// </summary>
        [Tooltip("Right hand/controller tracker reference.")]
        public VRTrackerReferences rightHandTracker;
        
        /// <summary>
        /// Left hand/controller tracker reference.
        /// </summary>
        [Tooltip("Left hand/controller tracker reference.")]
        public VRTrackerReferences leftHandTracker;
        
        /// <summary>
        /// Right hand/controller input reference.
        /// </summary>
        [Tooltip("Right hand/controller input reference.")]
        public VRInputReferences rightHandInput;
        
        /// <summary>
        /// Left hand/controller input reference.
        /// </summary>
        [Tooltip("Left hand/controller input reference.")]
        public VRInputReferences leftHandInput;

        /// <summary>
        /// If true, the script will be put into the 'DontDestroyOnLoad' area.
        /// </summary>
        [SerializeField] [Tooltip("If true, the script will be put into the 'DontDestroyOnLoad' area.")]
        private bool dontDestroyOnLoad = true;
        
        /// <summary>
        /// The current position of the HMD/headset in local coordinates.
        /// </summary>
        public static Vector3 HeadsetPosition => _self.headsetTracker.TrackerPosition;

        /// <summary>
        /// The current rotation of the HMD/headset in local coordinates.
        /// </summary>
        public static Quaternion HeadsetRotation => _self.headsetTracker.TrackerRotation;
        
        /// <summary>
        /// The current position of the right hand/controller in local coordinates.
        /// </summary>
        public static Vector3 RightHandPosition => _self.rightHandTracker.TrackerPosition;

        /// <summary>
        /// The current rotation of the right hand/controller in local coordinates.
        /// </summary>
        public static Quaternion RightHandRotation => _self.rightHandTracker.TrackerRotation;
        
        /// <summary>
        /// The current position of the right hand/controller in local coordinates.
        /// </summary>
        public static Vector3 LeftHandPosition => _self.leftHandTracker.TrackerPosition;

        /// <summary>
        /// The current rotation of the right hand/controller in local coordinates.
        /// </summary>
        public static Quaternion LeftHandRotation => _self.leftHandTracker.TrackerRotation;
        
        /// <summary>
        /// The current position of the dominate hand/controller in local coordinates.
        /// </summary>
        public static Vector3 DominateHandPosition => ItsVRManager.DominateHand == ItsVRManager.DominateHands.Right ? _self.rightHandTracker.TrackerPosition : _self.leftHandTracker.TrackerPosition;

        /// <summary>
        /// The current rotation of the dominate hand/controller in local coordinates.
        /// </summary>
        public static Quaternion DominateHandRotation => ItsVRManager.DominateHand == ItsVRManager.DominateHands.Right ? _self.rightHandTracker.TrackerRotation : _self.leftHandTracker.TrackerRotation;
        
        /// <summary>
        /// How far the right controller trigger is depressed. (From 0 to 1)
        /// </summary>
        public static float RightHandTriggerDepress => _self.rightHandInput.TriggerDepress;
        
        /// <summary>
        /// How far the left controller trigger is depressed. (From 0 to 1)
        /// </summary>
        public static float LeftHandTriggerDepress => _self.leftHandInput.TriggerDepress;
        
        /// <summary>
        /// How far the dominate controller trigger is depressed. (From 0 to 1)
        /// </summary>
        public static float DominateHandTriggerDepress => ItsVRManager.DominateHand == ItsVRManager.DominateHands.Right ? _self.rightHandInput.TriggerDepress : _self.leftHandInput.TriggerDepress;
        
        /// <summary>
        /// If the right controllers trigger is pressed.
        /// </summary>
        public static bool RightHandTriggerPressed => _self.rightHandInput.TriggerPressed;
        
        /// <summary>
        /// If the left controllers trigger is pressed.
        /// </summary>
        public static bool LeftHandTriggerPressed => _self.leftHandInput.TriggerPressed;
        
        /// <summary>
        /// If the dominate controllers trigger is pressed.
        /// </summary>
        public static bool DominateHandTriggerPressed => ItsVRManager.DominateHand == ItsVRManager.DominateHands.Right ? _self.rightHandInput.TriggerPressed : _self.leftHandInput.TriggerPressed;
        
        /// <summary>
        /// How far the right controller grip is depressed. (From 0 to 1)
        /// </summary>
        public static float RightHandGripDepress => _self.rightHandInput.GripDepress;
        
        /// <summary>
        /// How far the left controller grip is depressed. (From 0 to 1)
        /// </summary>
        public static float LeftHandGripDepress => _self.leftHandInput.GripDepress;
        
        /// <summary>
        /// How far the dominate controller grip is depressed. (From 0 to 1)
        /// </summary>
        public static float DominateHandGripDepress => ItsVRManager.DominateHand == ItsVRManager.DominateHands.Right ? _self.rightHandInput.GripDepress : _self.leftHandInput.GripDepress;

        /// <summary>
        /// If the right controllers grip is pressed.
        /// </summary>
        public static bool RightHandGripPressed => _self.rightHandInput.GripPressed;
        
        /// <summary>
        /// If the left controllers grip is pressed.
        /// </summary>
        public static bool LeftHandGripPressed => _self.leftHandInput.GripPressed;
        
        /// <summary>
        /// If the dominate controllers grip is pressed.
        /// </summary>
        public static bool DominateHandGripPressed => ItsVRManager.DominateHand == ItsVRManager.DominateHands.Right ? _self.rightHandInput.GripPressed : _self.leftHandInput.GripPressed;
        
        /// <summary>
        /// Position of the joystick on the right controller.
        /// </summary>
        public static Vector2 RightHandJoystickPosition => _self.rightHandInput.JoystickPosition;
        
        /// <summary>
        /// Position of the joystick on the left controller.
        /// </summary>
        public static Vector2 LeftHandJoystickPosition => _self.leftHandInput.JoystickPosition;
        
        /// <summary>
        /// Position of the joystick on the dominate controller.
        /// </summary>
        public static Vector2 DominateHandJoystickPosition => ItsVRManager.DominateHand == ItsVRManager.DominateHands.Right ? _self.rightHandInput.JoystickPosition : _self.leftHandInput.JoystickPosition;
        
        /// <summary>
        /// If the right controllers joystick is pressed.
        /// </summary>
        public static bool RightHandJoystickPressed => _self.rightHandInput.JoystickPressed;
        
        /// <summary>
        /// If the left controllers joystick is pressed.
        /// </summary>
        public static bool LeftHandJoystickPressed => _self.leftHandInput.JoystickPressed;
        
        /// <summary>
        /// If the dominate controllers joystick is pressed.
        /// </summary>
        public static bool DominateHandJoystickPressed => ItsVRManager.DominateHand == ItsVRManager.DominateHands.Right ? _self.rightHandInput.JoystickPressed : _self.leftHandInput.JoystickPressed;
        
        /// <summary>
        /// If the right controllers joystick is touched.
        /// </summary>
        public static bool RightHandJoystickTouched => _self.rightHandInput.JoystickTouched;
        
        /// <summary>
        /// If the left controllers joystick is touched.
        /// </summary>
        public static bool LeftHandJoystickTouched => _self.leftHandInput.JoystickTouched;
        
        /// <summary>
        /// If the dominate controllers joystick is touched.
        /// </summary>
        public static bool DominateHandJoystickTouched => ItsVRManager.DominateHand == ItsVRManager.DominateHands.Right ? _self.rightHandInput.JoystickTouched : _self.leftHandInput.JoystickTouched;

        /// <summary>
        /// If the right controllers primary button is pressed.
        /// </summary>
        public static bool RightHandPrimaryButtonPressed => _self.rightHandInput.PrimaryButtonPressed;
        
        /// <summary>
        /// If the left controllers primary button is pressed.
        /// </summary>
        public static bool LeftHandPrimaryButtonPressed => _self.leftHandInput.PrimaryButtonPressed;
        
        /// <summary>
        /// If the dominate controllers primary button is pressed.
        /// </summary>
        public static bool DominateHandPrimaryButtonPressed => ItsVRManager.DominateHand == ItsVRManager.DominateHands.Right ? _self.rightHandInput.PrimaryButtonPressed : _self.leftHandInput.PrimaryButtonPressed;
        
        /// <summary>
        /// If the right controllers primary button is touched.
        /// </summary>
        public static bool RightHandPrimaryButtonTouched => _self.rightHandInput.PrimaryButtonTouched;
        
        /// <summary>
        /// If the left controllers primary button is touched.
        /// </summary>
        public static bool LeftHandPrimaryButtonTouched => _self.leftHandInput.PrimaryButtonTouched;
        
        /// <summary>
        /// If the dominate controllers primary button is touched.
        /// </summary>
        public static bool DominateHandPrimaryButtonTouched => ItsVRManager.DominateHand == ItsVRManager.DominateHands.Right ? _self.rightHandInput.PrimaryButtonTouched : _self.leftHandInput.PrimaryButtonTouched;

        /// <summary>
        /// If the right controllers secondary button is pressed.
        /// </summary>
        public static bool RightHandSecondaryButtonPressed => _self.rightHandInput.SecondaryButtonPressed;
        
        /// <summary>
        /// If the left controllers secondary button is pressed.
        /// </summary>
        public static bool LeftHandSecondaryButtonPressed => _self.leftHandInput.SecondaryButtonPressed;
        
        /// <summary>
        /// If the dominate controllers secondary button is pressed.
        /// </summary>
        public static bool DominateHandSecondaryButtonPressed => ItsVRManager.DominateHand == ItsVRManager.DominateHands.Right ? _self.rightHandInput.SecondaryButtonPressed : _self.leftHandInput.SecondaryButtonPressed;
        
        /// <summary>
        /// If the right controllers secondary button is touched.
        /// </summary>
        public static bool RightHandSecondaryButtonTouched => _self.rightHandInput.SecondaryButtonTouched;
        
        /// <summary>
        /// If the left controllers secondary button is touched.
        /// </summary>
        public static bool LeftHandSecondaryButtonTouched => _self.leftHandInput.SecondaryButtonTouched;
        
        /// <summary>
        /// If the dominate controllers secondary button is touched.
        /// </summary>
        public static bool DominateHandSecondaryButtonTouched => ItsVRManager.DominateHand == ItsVRManager.DominateHands.Right ? _self.rightHandInput.SecondaryButtonTouched : _self.leftHandInput.SecondaryButtonTouched;
        
        /// <summary>
        /// If the right controllers thumbrest is touched.
        /// </summary>
        public static bool RightHandThumbrestTouched => _self.rightHandInput.ThumbrestTouched;
        
        /// <summary>
        /// If the left controllers thumbrest is touched.
        /// </summary>
        public static bool LeftHandThumbrestTouched => _self.leftHandInput.ThumbrestTouched;
        
        /// <summary>
        /// If the dominate controllers thumbrest is touched.
        /// </summary>
        public static bool DominateHandThumbrestTouched => ItsVRManager.DominateHand == ItsVRManager.DominateHands.Right ? _self.rightHandInput.ThumbrestTouched : _self.leftHandInput.ThumbrestTouched;
        
        public delegate void InputEvent();
        private static VRInputHandler _self;
        private bool _alreadySubscribedToInputEvents;
        
        #endregion

        private void Awake() {
            // First we fina all of the input handlers in the scene (which should only be one)
            // and put the script into 'DontDestroyOnLoad' so that the script is never destroyed.
            // If the script finds more than one input handler, the script will destroy itself.
            var allInputHandlers = FindObjectsOfType<VRInputHandler>();
            if (dontDestroyOnLoad && allInputHandlers.Length == 1) {
                transform.SetParent(null);
                DontDestroyOnLoad(gameObject);
            }
            else {
                Debug.LogWarning("[VR Input Handler] You may only have one input handler in a scene at once. Destroying this input handler in the scene.",this);
                Destroy(this);
            }
        }

        private void OnEnable() {
            _self = this;
            
            // Prevent the script from subscribing to the events more than once. 
            // This just saves some resources as this is a very beefy process.
            if (_alreadySubscribedToInputEvents) return;
            _alreadySubscribedToInputEvents = true;
            
            rightHandInput.triggerPressed.performed += OnRightTriggerPressed;
            rightHandInput.gripPressed.performed += OnRightGripPressed;
            rightHandInput.joystickPressed.performed += OnRightJoystickPressed;
            rightHandInput.joystickTouched.performed += OnRightJoystickTouched;
            rightHandInput.primaryButtonPressed.performed += OnRightPrimaryButtonPressed;
            rightHandInput.primaryButtonTouched.performed += OnRightPrimaryButtonTouched;
            rightHandInput.secondaryButtonPressed.performed += OnRightSecondaryButtonPressed;
            rightHandInput.secondaryButtonTouched.performed += OnRightSecondaryButtonTouched;
            rightHandInput.thumbrestTouched.performed += OnRightThumbrestTouched;

            leftHandInput.triggerPressed.performed += OnLeftTriggerPressed;
            leftHandInput.gripPressed.performed += OnLeftGripPressed;
            leftHandInput.joystickPressed.performed += OnLeftJoystickPressed;
            leftHandInput.joystickTouched.performed += OnLeftJoystickTouched;
            leftHandInput.primaryButtonPressed.performed += OnLeftPrimaryButtonPressed;
            leftHandInput.primaryButtonTouched.performed += OnLeftPrimaryButtonTouched;
            leftHandInput.secondaryButtonPressed.performed += OnLeftSecondaryButtonPressed;
            leftHandInput.secondaryButtonTouched.performed += OnLeftSecondaryButtonTouched;
            leftHandInput.thumbrestTouched.performed += OnLeftThumbrestTouched;
            
            if (ItsVRManager.DominateHand == ItsVRManager.DominateHands.Right) {
                rightHandInput.triggerPressed.performed += OnDominateTriggerPressed;
                rightHandInput.gripPressed.performed += OnDominateGripPressed;
                rightHandInput.joystickPressed.performed += OnDominateJoystickPressed;
                rightHandInput.joystickTouched.performed += OnDominateJoystickTouched;
                rightHandInput.primaryButtonPressed.performed += OnDominatePrimaryButtonPressed;
                rightHandInput.primaryButtonTouched.performed += OnDominatePrimaryButtonTouched;
                rightHandInput.secondaryButtonPressed.performed += OnDominateSecondaryButtonPressed;
                rightHandInput.secondaryButtonTouched.performed += OnDominateSecondaryButtonTouched;
                rightHandInput.thumbrestTouched.performed += OnDominateThumbrestTouched;
            }
            else {
                leftHandInput.triggerPressed.performed += OnDominateTriggerPressed;
                leftHandInput.gripPressed.performed += OnDominateGripPressed;
                leftHandInput.joystickPressed.performed += OnDominateJoystickPressed;
                leftHandInput.joystickTouched.performed += OnDominateJoystickTouched;
                leftHandInput.primaryButtonPressed.performed += OnDominatePrimaryButtonPressed;
                leftHandInput.primaryButtonTouched.performed += OnDominatePrimaryButtonTouched;
                leftHandInput.secondaryButtonPressed.performed += OnDominateSecondaryButtonPressed;
                leftHandInput.secondaryButtonTouched.performed += OnDominateSecondaryButtonTouched;
                leftHandInput.thumbrestTouched.performed += OnDominateThumbrestTouched;
            }
        }

        private void OnDisable() {
            _self = null;
            DisableInputs();
        }

        /// <summary>
        /// Disables all inputs referenced on this script.
        /// </summary>
        public static void DisableInputs() {
            if (_self.headsetTracker != null) 
                _self.headsetTracker.DisableInputs();
            else 
                Debug.LogError("[VR Input Handler] Cannot disable HMD inputs as no HMD tracker was referenced.", _self);

            if (_self.rightHandTracker != null) 
                _self.rightHandTracker.DisableInputs();
            else 
                Debug.LogError("[VR Input Handler] Cannot disable right controller tracking inputs as no right hand tracker was referenced.", _self);

            if (_self.headsetTracker != null) 
                _self.leftHandTracker.DisableInputs();
            else 
                Debug.LogError("[VR Input Handler] Cannot disable left controller tracking inputs as no left hand tracker was referenced.", _self);

            if (_self.rightHandInput != null) 
                _self.rightHandInput.DisableInputs();
            else 
                Debug.LogError("[VR Input Handler] Cannot disable right controller inputs as no right controller inputs are referenced.", _self);

            if (_self.leftHandInput != null) 
                _self.leftHandInput.DisableInputs();
            else 
                Debug.LogError("[VR Input Handler] Cannot disable left controller inputs as no left controller inputs are referenced.", _self);
        }
        
        #region Input Events

        #region Trigger Events

        /// <summary>
        /// Invoked when the right trigger is pressed.
        /// </summary>
        public static event InputEvent RightTriggerPressed;
        private static void OnRightTriggerPressed(InputAction.CallbackContext callbackContext) {
            RightTriggerPressed?.Invoke();
        }
        
        /// <summary>
        /// Invoked when the left trigger is pressed.
        /// </summary>
        public static event InputEvent LeftTriggerPressed;
        private static void OnLeftTriggerPressed(InputAction.CallbackContext callbackContext) {
            LeftTriggerPressed?.Invoke();
        }

        /// <summary>
        /// Invoked when the dominate trigger is pressed.
        /// </summary>
        public static event InputEvent DominateTriggerPressed;
        private static void OnDominateTriggerPressed(InputAction.CallbackContext callbackContext) {
            DominateTriggerPressed?.Invoke();
        }

        #endregion

        #region Grip Events

        /// <summary>
        /// Invoked when the right grip is pressed.
        /// </summary>
        public static event InputEvent RightGripPressed;
        private static void OnRightGripPressed(InputAction.CallbackContext callbackContext) {
            RightGripPressed?.Invoke();
        }
        
        /// <summary>
        /// Invoked when the left grip is pressed.
        /// </summary>
        public static event InputEvent LeftGripPressed;
        private static void OnLeftGripPressed(InputAction.CallbackContext callbackContext) {
            LeftGripPressed?.Invoke();
        }

        /// <summary>
        /// Invoked when the dominate grip is pressed.
        /// </summary>
        public static event InputEvent DominateGripPressed;
        private static void OnDominateGripPressed(InputAction.CallbackContext callbackContext) {
            DominateGripPressed?.Invoke();    
        }

        #endregion
        
        #region Joystick Events

        /// <summary>
        /// Invoked when the right joystick is pressed.
        /// </summary>
        public static event InputEvent RightJoystickPressed;
        private static void OnRightJoystickPressed(InputAction.CallbackContext callbackContext) {
            RightJoystickPressed?.Invoke();
        }
        
        /// <summary>
        /// Invoked when the left joystick is pressed.
        /// </summary>
        public static event InputEvent LeftJoystickPressed;
        private static void OnLeftJoystickPressed(InputAction.CallbackContext callbackContext) {
            LeftJoystickPressed?.Invoke();
        }

        /// <summary>
        /// Invoked when the dominate joystick is pressed.
        /// </summary>
        public static event InputEvent DominateJoystickPressed;
        private static void OnDominateJoystickPressed(InputAction.CallbackContext callbackContext) {
            DominateJoystickPressed?.Invoke();
        }
        
        /// <summary>
        /// Invoked when the right joystick is touched.
        /// </summary>
        public static event InputEvent RightJoystickTouched;
        private static void OnRightJoystickTouched(InputAction.CallbackContext callbackContext) {
            RightJoystickTouched?.Invoke();
        }
        
        /// <summary>
        /// Invoked when the left joystick is touched.
        /// </summary>
        public static event InputEvent LeftJoystickTouched;
        private static void OnLeftJoystickTouched(InputAction.CallbackContext callbackContext) {
            LeftJoystickTouched?.Invoke();
        }

        /// <summary>
        /// Invoked when the dominate joystick is touched.
        /// </summary>
        public static event InputEvent DominateJoystickTouched;
        private static void OnDominateJoystickTouched(InputAction.CallbackContext callbackContext) {
            DominateJoystickTouched?.Invoke();
        }

        #endregion
        
        #region Primary Button Events

        /// <summary>
        /// Invoked when the right primary button is pressed.
        /// </summary>
        public static event InputEvent RightPrimaryButtonPressed;
        private static void OnRightPrimaryButtonPressed(InputAction.CallbackContext callbackContext) {
            RightPrimaryButtonPressed?.Invoke();
        }
        
        /// <summary>
        /// Invoked when the left primary button is pressed.
        /// </summary>
        public static event InputEvent LeftPrimaryButtonPressed;
        private static void OnLeftPrimaryButtonPressed(InputAction.CallbackContext callbackContext) {
            LeftPrimaryButtonPressed?.Invoke();
        }

        /// <summary>
        /// Invoked when the dominate primary button is pressed.
        /// </summary>
        public static event InputEvent DominatePrimaryButtonPressed;
        private static void OnDominatePrimaryButtonPressed(InputAction.CallbackContext callbackContext) {
            DominatePrimaryButtonPressed?.Invoke();
        }
        
        /// <summary>
        /// Invoked when the right primary button is touched.
        /// </summary>
        public static event InputEvent RightPrimaryButtonTouched;
        private static void OnRightPrimaryButtonTouched(InputAction.CallbackContext callbackContext) {
            RightPrimaryButtonTouched?.Invoke();
        }
        
        /// <summary>
        /// Invoked when the left primary button is touched.
        /// </summary>
        public static event InputEvent LeftPrimaryButtonTouched;
        private static void OnLeftPrimaryButtonTouched(InputAction.CallbackContext callbackContext) {
            LeftPrimaryButtonTouched?.Invoke();
        }

        /// <summary>
        /// Invoked when the dominate primary button is touched.
        /// </summary>
        public static event InputEvent DominatePrimaryButtonTouched;
        private static void OnDominatePrimaryButtonTouched(InputAction.CallbackContext callbackContext) {
            DominatePrimaryButtonTouched?.Invoke();
        }

        #endregion
        
        #region Secondary Button Events

        /// <summary>
        /// Invoked when the right secondary button is pressed.
        /// </summary>
        public static event InputEvent RightSecondaryButtonPressed;
        private static void OnRightSecondaryButtonPressed(InputAction.CallbackContext callbackContext) {
            RightSecondaryButtonPressed?.Invoke();
        }
        
        /// <summary>
        /// Invoked when the left secondary button is pressed.
        /// </summary>
        public static event InputEvent LeftSecondaryButtonPressed;
        private static void OnLeftSecondaryButtonPressed(InputAction.CallbackContext callbackContext) {
            LeftSecondaryButtonPressed?.Invoke();
        }

        /// <summary>
        /// Invoked when the dominate secondary button is pressed.
        /// </summary>
        public static event InputEvent DominateSecondaryButtonPressed;
        private static void OnDominateSecondaryButtonPressed(InputAction.CallbackContext callbackContext) {
            DominateSecondaryButtonPressed?.Invoke();
        }
        
        /// <summary>
        /// Invoked when the right secondary button is touched.
        /// </summary>
        public static event InputEvent RightSecondaryButtonTouched;
        private static void OnRightSecondaryButtonTouched(InputAction.CallbackContext callbackContext) {
            RightSecondaryButtonTouched?.Invoke();
        }
        
        /// <summary>
        /// Invoked when the left secondary button is touched.
        /// </summary>
        public static event InputEvent LeftSecondaryButtonTouched;
        private static void OnLeftSecondaryButtonTouched(InputAction.CallbackContext callbackContext) {
            LeftSecondaryButtonTouched?.Invoke();
        }

        /// <summary>
        /// Invoked when the dominate secondary button is touched.
        /// </summary>
        public static event InputEvent DominateSecondaryButtonTouched;
        private static void OnDominateSecondaryButtonTouched(InputAction.CallbackContext callbackContext) {
            DominateSecondaryButtonTouched?.Invoke();
        }

        #endregion
        
        #region Thumbrest Events

        /// <summary>
        /// Invoked when the right thumbrest is touched.
        /// </summary>
        public static event InputEvent RightThumbrestTouched;
        private static void OnRightThumbrestTouched(InputAction.CallbackContext callbackContext) {
            RightThumbrestTouched?.Invoke();
        }
        
        /// <summary>
        /// Invoked when the left thumbrest is touched.
        /// </summary>
        public static event InputEvent LeftThumbrestTouched;
        private static void OnLeftThumbrestTouched(InputAction.CallbackContext callbackContext) {
            LeftThumbrestTouched?.Invoke();
        }

        /// <summary>
        /// Invoked when the dominate thumbrest is touched.
        /// </summary>
        public static event InputEvent DominateThumbrestTouched;
        private static void OnDominateThumbrestTouched(InputAction.CallbackContext callbackContext) {
            DominateThumbrestTouched?.Invoke();
        }

        #endregion

        #endregion

        #region Editor

        private void OnValidate() {
            // Checking to see if multiple VR Input Handlers exist in the
            // scene. Since this script gives developers static references
            // to the inputs, only one of these can exist.
            var check = FindObjectsOfType<VRInputHandler>();
            
            if (check.Length > 1) 
                Debug.LogError("[VR Input Handler] Multiple VR Input Handlers were found. Please only use one VR Input Handler per scene.", this);
        }

        #endregion
    }
}
