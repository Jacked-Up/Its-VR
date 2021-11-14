// This script was updated on 11/12/2021 by Jack Randolph.

using System;
using ItsVR.Scriptables;
using UnityEngine;

namespace ItsVR.Input {
    /// <summary>
    /// Contains static properties for all universal inputs.
    /// </summary>
    [DisallowMultipleComponent]
    [HelpURL("https://jackedupstudios.com/vr-input-suite")]
    [AddComponentMenu("It's VR/Input/VR Input Suite")]
    public class VRInputSuite : MonoBehaviour {
        #region Variables

        /// <summary>
        /// Right hand/controller input reference.
        /// </summary>
        [Tooltip("Right hand/controller input reference.")]
        public VRInputContainer rightHandInput;
        
        /// <summary>
        /// Left hand/controller input reference.
        /// </summary>
        [Tooltip("Left hand/controller input reference.")]
        public VRInputContainer leftHandInput;

        /// <summary>
        /// If true, the script will be put into the 'DontDestroyOnLoad' area.
        /// </summary>
        [SerializeField] [Tooltip("If true, the script will be put into the 'DontDestroyOnLoad' area.")]
        private bool dontDestroyOnLoad = true;
        
        /// <summary>
        /// How far the right controller trigger is depressed. (From 0 to 1)
        /// </summary>
        public static float RightHandTriggerDepress => _self.rightHandInput.universalInputs.TriggerDepress;
        
        /// <summary>
        /// How far the left controller trigger is depressed. (From 0 to 1)
        /// </summary>
        public static float LeftHandTriggerDepress => _self.leftHandInput.universalInputs.TriggerDepress;
        
        /// <summary>
        /// How far the dominate controller trigger is depressed. (From 0 to 1)
        /// </summary>
        public static float DominateHandTriggerDepress => ItsSystems.DominateHand == Hand.Right ? _self.rightHandInput.universalInputs.TriggerDepress : _self.leftHandInput.universalInputs.TriggerDepress;
        
        /// <summary>
        /// If the right controllers trigger is pressed.
        /// </summary>
        public static bool RightHandTriggerPressed => _self.rightHandInput.universalInputs.TriggerPressed;
        
        /// <summary>
        /// If the left controllers trigger is pressed.
        /// </summary>
        public static bool LeftHandTriggerPressed => _self.leftHandInput.universalInputs.TriggerPressed;
        
        /// <summary>
        /// If the dominate controllers trigger is pressed.
        /// </summary>
        public static bool DominateHandTriggerPressed => ItsSystems.DominateHand == Hand.Right ? _self.rightHandInput.universalInputs.TriggerPressed : _self.leftHandInput.universalInputs.TriggerPressed;
        
        /// <summary>
        /// How far the right controller grip is depressed. (From 0 to 1)
        /// </summary>
        public static float RightHandGripDepress => _self.rightHandInput.universalInputs.GripDepress;
        
        /// <summary>
        /// How far the left controller grip is depressed. (From 0 to 1)
        /// </summary>
        public static float LeftHandGripDepress => _self.leftHandInput.universalInputs.GripDepress;
        
        /// <summary>
        /// How far the dominate controller grip is depressed. (From 0 to 1)
        /// </summary>
        public static float DominateHandGripDepress => ItsSystems.DominateHand == Hand.Right ? _self.rightHandInput.universalInputs.GripDepress : _self.leftHandInput.universalInputs.GripDepress;

        /// <summary>
        /// If the right controllers grip is pressed.
        /// </summary>
        public static bool RightHandGripPressed => _self.rightHandInput.universalInputs.GripPressed;
        
        /// <summary>
        /// If the left controllers grip is pressed.
        /// </summary>
        public static bool LeftHandGripPressed => _self.leftHandInput.universalInputs.GripPressed;
        
        /// <summary>
        /// If the dominate controllers grip is pressed.
        /// </summary>
        public static bool DominateHandGripPressed => ItsSystems.DominateHand == Hand.Right ? _self.rightHandInput.universalInputs.GripPressed : _self.leftHandInput.universalInputs.GripPressed;
        
        /// <summary>
        /// Position of the joystick on the right controller.
        /// </summary>
        public static Vector2 RightHandJoystickPosition => _self.rightHandInput.universalInputs.JoystickPosition;
        
        /// <summary>
        /// Position of the joystick on the left controller.
        /// </summary>
        public static Vector2 LeftHandJoystickPosition => _self.leftHandInput.universalInputs.JoystickPosition;
        
        /// <summary>
        /// Position of the joystick on the dominate controller.
        /// </summary>
        public static Vector2 DominateHandJoystickPosition => ItsSystems.DominateHand == Hand.Right ? _self.rightHandInput.universalInputs.JoystickPosition : _self.leftHandInput.universalInputs.JoystickPosition;
        
        /// <summary>
        /// If the right controllers joystick is pressed.
        /// </summary>
        public static bool RightHandJoystickPressed => _self.rightHandInput.universalInputs.JoystickPressed;
        
        /// <summary>
        /// If the left controllers joystick is pressed.
        /// </summary>
        public static bool LeftHandJoystickPressed => _self.leftHandInput.universalInputs.JoystickPressed;
        
        /// <summary>
        /// If the dominate controllers joystick is pressed.
        /// </summary>
        public static bool DominateHandJoystickPressed => ItsSystems.DominateHand == Hand.Right ? _self.rightHandInput.universalInputs.JoystickPressed : _self.leftHandInput.universalInputs.JoystickPressed;
        
        /// <summary>
        /// If the right controllers primary button is pressed.
        /// </summary>
        public static bool RightHandPrimaryButtonPressed => _self.rightHandInput.universalInputs.PrimaryButtonPressed;
        
        /// <summary>
        /// If the left controllers primary button is pressed.
        /// </summary>
        public static bool LeftHandPrimaryButtonPressed => _self.leftHandInput.universalInputs.PrimaryButtonPressed;
        
        /// <summary>
        /// If the dominate controllers primary button is pressed.
        /// </summary>
        public static bool DominateHandPrimaryButtonPressed => ItsSystems.DominateHand == Hand.Right ? _self.rightHandInput.universalInputs.PrimaryButtonPressed : _self.leftHandInput.universalInputs.PrimaryButtonPressed;
        
        /// <summary>
        /// If the right controllers primary button is touched.
        /// </summary>
        public static bool RightHandPrimaryButtonTouched => _self.rightHandInput.universalInputs.PrimaryButtonTouched;
        
        /// <summary>
        /// If the left controllers primary button is touched.
        /// </summary>
        public static bool LeftHandPrimaryButtonTouched => _self.leftHandInput.universalInputs.PrimaryButtonTouched;
        
        /// <summary>
        /// If the dominate controllers primary button is touched.
        /// </summary>
        public static bool DominateHandPrimaryButtonTouched => ItsSystems.DominateHand == Hand.Right ? _self.rightHandInput.universalInputs.PrimaryButtonTouched : _self.leftHandInput.universalInputs.PrimaryButtonTouched;

        /// <summary>
        /// If the right controllers secondary button is pressed.
        /// </summary>
        public static bool RightHandSecondaryButtonPressed => _self.rightHandInput.universalInputs.SecondaryButtonPressed;
        
        /// <summary>
        /// If the left controllers secondary button is pressed.
        /// </summary>
        public static bool LeftHandSecondaryButtonPressed => _self.leftHandInput.universalInputs.SecondaryButtonPressed;
        
        /// <summary>
        /// If the dominate controllers secondary button is pressed.
        /// </summary>
        public static bool DominateHandSecondaryButtonPressed => ItsSystems.DominateHand == Hand.Right ? _self.rightHandInput.universalInputs.SecondaryButtonPressed : _self.leftHandInput.universalInputs.SecondaryButtonPressed;
        
        /// <summary>
        /// If the right controllers secondary button is touched.
        /// </summary>
        public static bool RightHandSecondaryButtonTouched => _self.rightHandInput.universalInputs.SecondaryButtonTouched;
        
        /// <summary>
        /// If the left controllers secondary button is touched.
        /// </summary>
        public static bool LeftHandSecondaryButtonTouched => _self.leftHandInput.universalInputs.SecondaryButtonTouched;
        
        /// <summary>
        /// If the dominate controllers secondary button is touched.
        /// </summary>
        public static bool DominateHandSecondaryButtonTouched => ItsSystems.DominateHand == Hand.Right ? _self.rightHandInput.universalInputs.SecondaryButtonTouched : _self.leftHandInput.universalInputs.SecondaryButtonTouched;
        
        public delegate void InputEvent();
        private static VRInputSuite _self;
        private bool _alreadySubscribedToInputEvents;
        
        #endregion

        private void Awake() {
            // First we find all of the input suites in the scene (which should only be one)
            // and put the script into 'DontDestroyOnLoad' so that the script is never destroyed.
            // If the script finds more than one input suite, the script will destroy itself.
            var allInputSuites = FindObjectsOfType<VRInputSuite>();
            if (dontDestroyOnLoad && allInputSuites.Length == 1) {
                transform.SetParent(null);
                DontDestroyOnLoad(gameObject);
            }
            else {
                Debug.LogWarning("[VR Input Suite] You may only have one input suite in a scene at once. Destroying this input suite in the scene.",this);
                Destroy(this);
            }
        }

        private void OnEnable() {
            _self = this;
            
            // Prevent the script from subscribing to the events more than once. 
            // This just saves some resources as this is a very beefy process.
            if (_alreadySubscribedToInputEvents) return;
            _alreadySubscribedToInputEvents = true;

            if (rightHandInput != null) {
                rightHandInput.universalInputs.RegisterEvents();
                rightHandInput.universalInputs.OnTriggerPressed += OnRightTriggerPressed;
                rightHandInput.universalInputs.OnGripPressed += OnRightGripPressed;
                rightHandInput.universalInputs.OnJoystickPressed += OnRightJoystickPressed;
                rightHandInput.universalInputs.OnPrimaryButtonPressed += OnRightPrimaryButtonPressed;
                rightHandInput.universalInputs.OnPrimaryButtonTouched += OnRightPrimaryButtonTouched;
                rightHandInput.universalInputs.OnSecondaryButtonPressed += OnRightSecondaryButtonPressed;
                rightHandInput.universalInputs.OnSecondaryButtonTouched += OnRightSecondaryButtonTouched;   
            }
            else Debug.LogError("[VR Input Suite] Cannot register events with right controller because it is null.", this);

            if (leftHandInput != null) {
                leftHandInput.universalInputs.RegisterEvents();
                leftHandInput.universalInputs.OnTriggerPressed += OnLeftTriggerPressed;
                leftHandInput.universalInputs.OnGripPressed += OnLeftGripPressed;
                leftHandInput.universalInputs.OnJoystickPressed += OnLeftJoystickPressed;
                leftHandInput.universalInputs.OnPrimaryButtonPressed += OnLeftPrimaryButtonPressed;
                leftHandInput.universalInputs.OnPrimaryButtonTouched += OnLeftPrimaryButtonTouched;
                leftHandInput.universalInputs.OnSecondaryButtonPressed += OnLeftSecondaryButtonPressed;
                leftHandInput.universalInputs.OnSecondaryButtonTouched += OnLeftSecondaryButtonTouched;
            }
            else Debug.LogError("[VR Input Suite] Cannot register events with left controller because it is null.", this);
            
            switch (ItsSystems.DominateHand) {
                case Hand.Right:
                    if (rightHandInput == null) return;
                    
                    rightHandInput.universalInputs.OnTriggerPressed += OnDominateTriggerPressed;
                    rightHandInput.universalInputs.OnGripPressed += OnDominateGripPressed;
                    rightHandInput.universalInputs.OnJoystickPressed += OnDominateJoystickPressed;
                    rightHandInput.universalInputs.OnPrimaryButtonPressed += OnDominatePrimaryButtonPressed;
                    rightHandInput.universalInputs.OnPrimaryButtonTouched += OnDominatePrimaryButtonTouched;
                    rightHandInput.universalInputs.OnSecondaryButtonPressed += OnDominateSecondaryButtonPressed;
                    rightHandInput.universalInputs.OnSecondaryButtonTouched += OnDominateSecondaryButtonTouched;
                    break;
                case Hand.Left:
                    if (leftHandInput == null) return;
                    
                    leftHandInput.universalInputs.OnTriggerPressed += OnDominateTriggerPressed;
                    leftHandInput.universalInputs.OnGripPressed += OnDominateGripPressed;
                    leftHandInput.universalInputs.OnJoystickPressed += OnDominateJoystickPressed;
                    leftHandInput.universalInputs.OnPrimaryButtonPressed += OnDominatePrimaryButtonPressed;
                    leftHandInput.universalInputs.OnPrimaryButtonTouched += OnDominatePrimaryButtonTouched;
                    leftHandInput.universalInputs.OnSecondaryButtonPressed += OnDominateSecondaryButtonPressed;
                    leftHandInput.universalInputs.OnSecondaryButtonTouched += OnDominateSecondaryButtonTouched;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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
            if (_self.rightHandInput != null) 
                _self.rightHandInput.DisableAllInputs();
            else 
                Debug.LogError("[VR Input Suite] Cannot disable right controller inputs as no right controller inputs are referenced.", _self);

            if (_self.leftHandInput != null) 
                _self.leftHandInput.DisableAllInputs();
            else 
                Debug.LogError("[VR Input Suite] Cannot disable left controller inputs as no left controller inputs are referenced.", _self);
        }
        
        #region Input Events

        #region Trigger Events

        /// <summary>
        /// Invoked when the right trigger is pressed.
        /// </summary>
        public static event InputEvent RightTriggerPressed;
        private static void OnRightTriggerPressed() {
            RightTriggerPressed?.Invoke();
        }
        
        /// <summary>
        /// Invoked when the left trigger is pressed.
        /// </summary>
        public static event InputEvent LeftTriggerPressed;
        private static void OnLeftTriggerPressed() {
            LeftTriggerPressed?.Invoke();
        }

        /// <summary>
        /// Invoked when the dominate trigger is pressed.
        /// </summary>
        public static event InputEvent DominateTriggerPressed;
        private static void OnDominateTriggerPressed() {
            DominateTriggerPressed?.Invoke();
        }

        #endregion

        #region Grip Events

        /// <summary>
        /// Invoked when the right grip is pressed.
        /// </summary>
        public static event InputEvent RightGripPressed;
        private static void OnRightGripPressed() {
            RightGripPressed?.Invoke();
        }
        
        /// <summary>
        /// Invoked when the left grip is pressed.
        /// </summary>
        public static event InputEvent LeftGripPressed;
        private static void OnLeftGripPressed() {
            LeftGripPressed?.Invoke();
        }

        /// <summary>
        /// Invoked when the dominate grip is pressed.
        /// </summary>
        public static event InputEvent DominateGripPressed;
        private static void OnDominateGripPressed() {
            DominateGripPressed?.Invoke();    
        }

        #endregion
        
        #region Joystick Events

        /// <summary>
        /// Invoked when the right joystick is pressed.
        /// </summary>
        public static event InputEvent RightJoystickPressed;
        private static void OnRightJoystickPressed() {
            RightJoystickPressed?.Invoke();
        }
        
        /// <summary>
        /// Invoked when the left joystick is pressed.
        /// </summary>
        public static event InputEvent LeftJoystickPressed;
        private static void OnLeftJoystickPressed() {
            LeftJoystickPressed?.Invoke();
        }

        /// <summary>
        /// Invoked when the dominate joystick is pressed.
        /// </summary>
        public static event InputEvent DominateJoystickPressed;
        private static void OnDominateJoystickPressed() {
            DominateJoystickPressed?.Invoke();
        }

        #endregion
        
        #region Primary Button Events

        /// <summary>
        /// Invoked when the right primary button is pressed.
        /// </summary>
        public static event InputEvent RightPrimaryButtonPressed;
        private static void OnRightPrimaryButtonPressed() {
            RightPrimaryButtonPressed?.Invoke();
        }
        
        /// <summary>
        /// Invoked when the left primary button is pressed.
        /// </summary>
        public static event InputEvent LeftPrimaryButtonPressed;
        private static void OnLeftPrimaryButtonPressed() {
            LeftPrimaryButtonPressed?.Invoke();
        }

        /// <summary>
        /// Invoked when the dominate primary button is pressed.
        /// </summary>
        public static event InputEvent DominatePrimaryButtonPressed;
        private static void OnDominatePrimaryButtonPressed() {
            DominatePrimaryButtonPressed?.Invoke();
        }
        
        /// <summary>
        /// Invoked when the right primary button is touched.
        /// </summary>
        public static event InputEvent RightPrimaryButtonTouched;
        private static void OnRightPrimaryButtonTouched() {
            RightPrimaryButtonTouched?.Invoke();
        }
        
        /// <summary>
        /// Invoked when the left primary button is touched.
        /// </summary>
        public static event InputEvent LeftPrimaryButtonTouched;
        private static void OnLeftPrimaryButtonTouched() {
            LeftPrimaryButtonTouched?.Invoke();
        }

        /// <summary>
        /// Invoked when the dominate primary button is touched.
        /// </summary>
        public static event InputEvent DominatePrimaryButtonTouched;
        private static void OnDominatePrimaryButtonTouched() {
            DominatePrimaryButtonTouched?.Invoke();
        }

        #endregion
        
        #region Secondary Button Events

        /// <summary>
        /// Invoked when the right secondary button is pressed.
        /// </summary>
        public static event InputEvent RightSecondaryButtonPressed;
        private static void OnRightSecondaryButtonPressed() {
            RightSecondaryButtonPressed?.Invoke();
        }
        
        /// <summary>
        /// Invoked when the left secondary button is pressed.
        /// </summary>
        public static event InputEvent LeftSecondaryButtonPressed;
        private static void OnLeftSecondaryButtonPressed() {
            LeftSecondaryButtonPressed?.Invoke();
        }

        /// <summary>
        /// Invoked when the dominate secondary button is pressed.
        /// </summary>
        public static event InputEvent DominateSecondaryButtonPressed;
        private static void OnDominateSecondaryButtonPressed() {
            DominateSecondaryButtonPressed?.Invoke();
        }
        
        /// <summary>
        /// Invoked when the right secondary button is touched.
        /// </summary>
        public static event InputEvent RightSecondaryButtonTouched;
        private static void OnRightSecondaryButtonTouched() {
            RightSecondaryButtonTouched?.Invoke();
        }
        
        /// <summary>
        /// Invoked when the left secondary button is touched.
        /// </summary>
        public static event InputEvent LeftSecondaryButtonTouched;
        private static void OnLeftSecondaryButtonTouched() {
            LeftSecondaryButtonTouched?.Invoke();
        }

        /// <summary>
        /// Invoked when the dominate secondary button is touched.
        /// </summary>
        public static event InputEvent DominateSecondaryButtonTouched;
        private static void OnDominateSecondaryButtonTouched() {
            DominateSecondaryButtonTouched?.Invoke();
        }

        #endregion

        #endregion

        #region Editor

        private void OnValidate() {
            // Checking to see if multiple VR Input Suites exist in the
            // scene. Since this script gives developers static references
            // to the inputs, only one of these can exist.
            var check = FindObjectsOfType<VRInputSuite>();
            
            if (check.Length > 1) 
                Debug.LogError("[VR Input Suite] Multiple VR Input Suites were found. Please only use one VR Input Suite per scene.", this);
        }

        #endregion
    }
}
