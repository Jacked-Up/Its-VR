using System;
using ItsVR.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

namespace ItsVR.Scriptables {
    /// <summary>
    /// The VR input container manages the input from a virtual reality controller. The container uses the Jacked
    /// Up Studios input translator to make supporting input from multiple different VR controllers simpler and
    /// intuitive.
    /// </summary>
    /// <para>Author: Jack Randolph</para>
    [HelpURL("https://jackedupstudios.com/its-vr-documentation-1#b47abff1-8a0b-4eb6-b03c-3617ba2beb61")]
    [CreateAssetMenu(menuName = "It's VR/Input/VR Input Container", fileName = "New Input Container")]
    public class VRInputContainer : ScriptableObject {
        #region Variables

        /// <summary>
        /// Universal input bindings.
        /// </summary>
        [Tooltip("Universal input bindings.")]
        public UniversalActionBindings universal;

        /// <summary>
        /// System input bindings.
        /// </summary>
        [Tooltip("System input bindings.")]
        public SystemActionBindings system;
        
        /// <summary>
        /// Meta specific input bindings.
        /// </summary>
        [Tooltip("Meta specific input bindings.")]
        public MetaActionBindings meta;

        /// <summary>
        /// Vive specific input bindings.
        /// </summary>
        [Tooltip("Vive specific input bindings.")]
        public ViveActionBindings vive;
        
        /// <summary>
        /// Valve specific input bindings.
        /// </summary>
        [Tooltip("Valve specific input bindings.")]
        public ValveActionBindings valve;
        
        /// <summary>
        /// Struct which manages and contains all universal specific input actions.
        /// </summary>
        [Serializable]
        public struct UniversalActionBindings {
            #region Bindings

            [SerializeField] [Tooltip("Input reference for how far the trigger is depressed.")]
            private InputAction triggerDepress;
            
            [SerializeField] [Tooltip("Input reference for when the trigger is pressed.")]
            private InputAction triggerPressed;
            
            [SerializeField] [Tooltip("Input reference for how far the grip is depressed.")]
            private InputAction gripDepress;
            
            [SerializeField] [Tooltip("Input reference for when the grip is pressed.")]
            private InputAction gripPressed;
            
            [SerializeField] [Tooltip("Input reference for the joysticks position.")]
            private InputAction joystickPosition;
            
            [SerializeField] [Tooltip("Input reference for when the joystick is pressed.")]
            private InputAction joystickPressed;
            
            [SerializeField] [Tooltip("Input reference for when the primary button is pressed.")]
            private InputAction primaryButtonPressed;
            
            [SerializeField] [Tooltip("Input reference for when the primary button is touched.")]
            private InputAction primaryButtonTouched;
            
            [SerializeField] [Tooltip("Input reference for when the secondary button is pressed.")]
            private InputAction secondaryButtonPressed;
            
            [SerializeField] [Tooltip("Input reference for when the secondary button is touched.")]
            private InputAction secondaryButtonTouched;

            [SerializeField] [Tooltip("Input reference for the controllers haptics.")]
            private InputAction haptics;

            #endregion

            /// <summary>
            /// The amount the trigger is depressed.
            /// </summary>
            public float TriggerDepress {
                get {
                    if (!triggerDepress.enabled) 
                        triggerDepress?.Enable();

                    var value = (float) triggerDepress?.ReadValue<float>();
                    return value > 0.01 ? value : 0f;
                }
            }
            
            /// <summary>
            /// Invoked when the trigger depress/position changes.
            /// </summary>
            public event UniversalInputCallback OnTriggerDepressChanged;
            private void TriggerDepressPerformed(InputAction.CallbackContext a) => OnTriggerDepressChanged?.Invoke(); 
            
            /// <summary>
            /// If the trigger is pressed or not.
            /// </summary>
            public bool TriggerPressed {
                get {
                    if (!triggerPressed.enabled) 
                        triggerPressed?.Enable();
                    
                    return (int)triggerPressed?.ReadValue<float>() == 1;
                }
            }
            
            /// <summary>
            /// Invoked when the trigger is pressed.
            /// </summary>
            public event UniversalInputCallback OnTriggerPressed;
            private void TriggerPressedPerformed(InputAction.CallbackContext a) => OnTriggerPressed?.Invoke(); 
            
            /// <summary>
            /// The amount the grip is depressed.
            /// </summary>
            public float GripDepress {
                get {
                    if (!gripDepress.enabled) 
                        gripDepress?.Enable();
                    
                    var value = (float)gripDepress?.ReadValue<float>();
                    return value > 0.01f ? value : 0f;
                }
            }
            
            /// <summary>
            /// Invoked when the grips depress/position changes.
            /// </summary>
            public event UniversalInputCallback OnGripDepressChanged;
            private void GripDepressPerformed(InputAction.CallbackContext a) => OnGripDepressChanged?.Invoke(); 
            
            /// <summary>
            /// If the grip is pressed or not.
            /// </summary>
            public bool GripPressed {
                get {
                    if (!gripPressed.enabled) 
                        gripPressed?.Enable();
                    
                    return (int)gripPressed?.ReadValue<float>() == 1;
                }
            }
            
            /// <summary>
            /// Invoked when the grip is pressed.
            /// </summary>
            public event UniversalInputCallback OnGripPressed;
            private void GripPressedPerformed(InputAction.CallbackContext a) => OnGripPressed?.Invoke(); 
            
            /// <summary>
            /// The position of the joystick.
            /// </summary>
            public Vector2 JoystickPosition {
                get {
                    if (!joystickPosition.enabled) 
                        joystickPosition?.Enable();
                    
                    return (Vector2)joystickPosition?.ReadValue<Vector2>(); 
                }
            }
            
            /// <summary>
            /// Invoked when the joysticks position is changed.
            /// </summary>
            public event UniversalInputCallback OnJoystickPositionChanged;
            private void JoystickPositionPerformed(InputAction.CallbackContext a) => OnJoystickPositionChanged?.Invoke(); 
            
            /// <summary>
            /// If the joystick is pressed or not.
            /// </summary>
            public bool JoystickPressed {
                get {
                    if (!joystickPressed.enabled) 
                        joystickPressed?.Enable();
                    
                    return (int)joystickPressed?.ReadValue<float>() == 1; 
                }
            }

            /// <summary>
            /// Invoked when the joystick is pressed.
            /// </summary>
            public event UniversalInputCallback OnJoystickPressed;
            private void JoystickPressedPerformed(InputAction.CallbackContext a) => OnJoystickPressed?.Invoke(); 
            
            /// <summary>
            /// If the primary button is pressed or not.
            /// </summary>
            public bool PrimaryButtonPressed {
                get {
                    if (!primaryButtonPressed.enabled) 
                        primaryButtonPressed?.Enable();
                    
                    return (int)primaryButtonPressed?.ReadValue<float>() == 1; 
                }
            }
            
            /// <summary>
            /// Invoked when the primary button is pressed.
            /// </summary>
            public event UniversalInputCallback OnPrimaryButtonPressed;
            private void PrimaryButtonPressedPerformed(InputAction.CallbackContext a) => OnPrimaryButtonPressed?.Invoke(); 
            
            /// <summary>
            /// If the primary button is touched or not.
            /// </summary>
            public bool PrimaryButtonTouched {
                get {
                    if (!primaryButtonTouched.enabled)
                        primaryButtonTouched?.Enable();
                    
                    return (int)primaryButtonTouched?.ReadValue<float>() == 1;
                }
            }
            
            /// <summary>
            /// Invoked when the primary button is touched.
            /// </summary>
            public event UniversalInputCallback OnPrimaryButtonTouched;
            private void PrimaryButtonTouchedPerformed(InputAction.CallbackContext a) => OnPrimaryButtonTouched?.Invoke(); 
            
            /// <summary>
            /// If the secondary button is pressed or not.
            /// </summary>
            public bool SecondaryButtonPressed {
                get {
                    if (!secondaryButtonPressed.enabled)
                        secondaryButtonPressed?.Enable();
                    
                    return (int)secondaryButtonPressed?.ReadValue<float>() == 1;  
                }
            }

            /// <summary>
            /// Invoked when the secondary button is pressed.
            /// </summary>
            public event UniversalInputCallback OnSecondaryButtonPressed;
            private void SecondaryButtonPressedPerformed(InputAction.CallbackContext a) => OnSecondaryButtonPressed?.Invoke(); 
            
            /// <summary>
            /// If the secondary button is touched or not.
            /// </summary>
            public bool SecondaryButtonTouched {
                get {
                    if (!secondaryButtonTouched.enabled) 
                        secondaryButtonTouched?.Enable();
                    
                    return (int)secondaryButtonTouched?.ReadValue<float>() == 1; 
                }
            }
            
            /// <summary>
            /// Invoked when the secondary button is touched.
            /// </summary>
            public event UniversalInputCallback OnSecondaryButtonTouched;
            private void SecondaryButtonTouchedPerformed(InputAction.CallbackContext a) => OnSecondaryButtonTouched?.Invoke(); 
            
            /// <summary>
            /// Engages the haptics on the controller at the amplitude for duration.
            /// </summary>
            /// <param name="amplitude">The amount of power supplied to the haptics. Maximum of 1 and minimum of 0.</param>
            /// <param name="duration">How long the haptic device is engaged.</param>
            public void SendHapticPulse(float amplitude, float duration) {
                if (amplitude <= 0f || duration <= 0f)
                    return;
                
                if (!haptics.enabled) 
                    haptics?.Enable();

                if (amplitude > 1f)
                    amplitude = 1f;

                if (haptics?.activeControl?.device is XRControllerWithRumble rumbleController) 
                    rumbleController.SendImpulse(amplitude, duration);
            }

            /// <summary>
            /// Registers all input(s).
            /// </summary>
            public void RegisterInputs() {
                if (triggerDepress != null) {
                    triggerDepress.Enable();
                    triggerDepress.performed += TriggerDepressPerformed;
                }
                
                if (triggerPressed != null) {
                    triggerPressed.Enable();
                    triggerPressed.performed += TriggerPressedPerformed;
                }
                
                if (gripDepress != null) {
                    gripDepress.Enable();
                    gripDepress.performed += GripDepressPerformed;
                }
                
                if (gripPressed != null) {
                    gripPressed.Enable();
                    gripPressed.performed += GripPressedPerformed;
                }
                
                if (joystickPosition != null) {
                    joystickPosition.Enable();
                    joystickPosition.performed += JoystickPositionPerformed;
                }
                
                if (joystickPressed != null) {
                    joystickPressed.Enable();
                    joystickPressed.performed += JoystickPressedPerformed;
                }
                
                if (primaryButtonPressed != null) {
                    primaryButtonPressed.Enable();
                    primaryButtonPressed.performed += PrimaryButtonPressedPerformed;
                }
                
                if (primaryButtonTouched != null) {
                    primaryButtonTouched.Enable();
                    primaryButtonTouched.performed += PrimaryButtonTouchedPerformed;
                }
                
                if (secondaryButtonPressed != null) {
                    secondaryButtonPressed.Enable();
                    secondaryButtonPressed.performed += SecondaryButtonPressedPerformed;
                }
                
                if (secondaryButtonTouched != null) {
                    secondaryButtonTouched.Enable();
                    secondaryButtonTouched.performed += SecondaryButtonTouchedPerformed;
                }
                
                haptics?.Enable();
            }
            
            /// <summary>
            /// Unregisters all input(s).
            /// </summary>
            public void UnregisterInputs() {
                if (triggerDepress != null) {
                    triggerDepress.Disable();
                    triggerDepress.performed -= TriggerDepressPerformed;
                }
                
                if (triggerPressed != null) {
                    triggerPressed.Disable();
                    triggerPressed.performed -= TriggerPressedPerformed;
                }
                
                if (gripDepress != null) {
                    gripDepress.Disable();
                    gripDepress.performed -= GripDepressPerformed;
                }
                
                if (gripPressed != null) {
                    gripPressed.Disable();
                    gripPressed.performed -= GripPressedPerformed;
                }
                
                if (joystickPosition != null) {
                    joystickPosition.Disable();
                    joystickPosition.performed -= JoystickPositionPerformed;
                }
                
                if (joystickPressed != null) {
                    joystickPressed.Disable();
                    joystickPressed.performed -= JoystickPressedPerformed;
                }
                
                if (primaryButtonPressed != null) {
                    primaryButtonPressed.Disable();
                    primaryButtonPressed.performed -= PrimaryButtonPressedPerformed;
                }
                
                if (primaryButtonTouched != null) {
                    primaryButtonTouched.Disable();
                    primaryButtonTouched.performed -= PrimaryButtonTouchedPerformed;
                }
                
                if (secondaryButtonPressed != null) {
                    secondaryButtonPressed.Disable();
                    secondaryButtonPressed.performed -= SecondaryButtonPressedPerformed;
                }
                
                if (secondaryButtonTouched != null) {
                    secondaryButtonTouched.Disable();
                    secondaryButtonTouched.performed -= SecondaryButtonTouchedPerformed;
                }

                haptics?.Disable();
            }

            public delegate void UniversalInputCallback();
        }

        /// <summary>
        /// Struct which manages and contains all system specific input actions.
        /// </summary>
        [Serializable]
        public struct SystemActionBindings {
            #region Bindings

            [SerializeField] [Tooltip("Input reference for when the system button is touched.")]
            private InputAction systemPressed;

            #endregion

            /// <summary>
            /// If the system button is pressed or not.
            /// </summary>
            public bool SystemPressed {
                get {
                    if (!systemPressed.enabled) 
                        systemPressed?.Enable();
                    
                    return (int)systemPressed?.ReadValue<float>() == 1;  
                }
            }
            
            /// <summary>
            /// Invoked when the system is pressed.
            /// </summary>
            public event SystemInputCallback OnSystemPressed;
            private void SystemPressedPerformed(InputAction.CallbackContext a) => OnSystemPressed?.Invoke(); 
            
            /// <summary>
            /// Registers all input(s).
            /// </summary>
            public void RegisterInputs() {
                if (systemPressed != null) {
                    systemPressed.Enable();
                    systemPressed.performed += SystemPressedPerformed;
                }
            }
            
            /// <summary>
            /// Unregisters all input(s).
            /// </summary>
            public void UnregisterInputs() {
                if (systemPressed != null) {
                    systemPressed.Disable();
                    systemPressed.performed -= SystemPressedPerformed;
                }
            }

            public delegate void SystemInputCallback();
        }

        /// <summary>
        /// Struct which manages and contains all Meta specific input actions.
        /// </summary>
        [Serializable]
        public struct MetaActionBindings {
            #region Bindings

            [SerializeField] [Tooltip("Input reference for when the trigger is touched.")]
            private InputAction triggerTouched;
            
            [SerializeField] [Tooltip("Input reference for when the joystick is touched.")]
            private InputAction joystickTouched;

            #endregion

            /// <summary>
            /// If the trigger is touched or not.
            /// </summary>
            public bool TriggerTouched {
                get {
                    if (!triggerTouched.enabled) 
                        triggerTouched?.Enable();
                    
                    return (int)triggerTouched?.ReadValue<float>() == 1;  
                }
            }
            
            /// <summary>
            /// Invoked when the trigger is touched.
            /// </summary>
            public event MetaInputCallback OnTriggerTouched;
            private void TriggerTouchedPerformed(InputAction.CallbackContext a) => OnTriggerTouched?.Invoke(); 
            
            /// <summary>
            /// If the joystick is touched or not.
            /// </summary>
            public bool JoystickTouched {
                get {
                    if (!joystickTouched.enabled)
                        joystickTouched?.Enable();
                    
                    return (int)joystickTouched?.ReadValue<float>() == 1; 
                }
            }
            
            /// <summary>
            /// Invoked when the joystick is touched.
            /// </summary>
            public event MetaInputCallback OnJoystickTouched;
            private void JoystickTouchedPerformed(InputAction.CallbackContext a) => OnJoystickTouched?.Invoke(); 
            
            /// <summary>
            /// Registers all input(s).
            /// </summary>
            public void RegisterInputs() {
                if (triggerTouched != null) {
                    triggerTouched.Enable();
                    triggerTouched.performed += TriggerTouchedPerformed;
                }
               
                if (joystickTouched != null) {
                    joystickTouched.Enable();
                    joystickTouched.performed += JoystickTouchedPerformed;
                }
            }
            
            /// <summary>
            /// Unregisters all input(s).
            /// </summary>
            public void UnregisterInputs() {
                if (triggerTouched != null) {
                    triggerTouched.Disable();
                    triggerTouched.performed -= TriggerTouchedPerformed;
                }

                if (joystickTouched != null) {
                    joystickTouched.Disable();
                    joystickTouched.performed -= JoystickTouchedPerformed;
                }
            }

            public delegate void MetaInputCallback();
        }
    
        /// <summary>
        /// Struct which manages and contains all Vive specific input actions.
        /// </summary>
        [Serializable]
        public struct ViveActionBindings {
            #region Bindings

            [SerializeField] [Tooltip("Input reference for the fingers position on the trackpad.")]
            private InputAction trackpadPosition;

            [SerializeField] [Tooltip("Input reference for when the trackpad is pressed.")]
            private InputAction trackpadPressed;

            [SerializeField] [Tooltip("Input reference for when the trackpad is touched.")]
            private InputAction trackpadTouched;

            #endregion

            /// <summary>
            /// The position of the finger on the trackpad.
            /// </summary>
            public Vector2 TrackpadPosition {
                get {
                    if (!trackpadPosition.enabled)
                        trackpadPosition?.Enable();
                    
                    return (Vector2)trackpadPosition?.ReadValue<Vector2>();  
                }
            }
            
            /// <summary>
            /// Invoked when the trackpad position is changed.
            /// </summary>
            public event ViveInputCallback OnTrackpadPositionChanged;
            private void TrackpadPositionPerformed(InputAction.CallbackContext a) => OnTrackpadPositionChanged?.Invoke(); 
            
            /// <summary>
            /// If the trackpad is pressed or not.
            /// </summary>
            public bool TrackpadPressed {
                get {
                    if (!trackpadPressed.enabled)
                        trackpadPressed?.Enable();
                    
                    return (int)trackpadPressed?.ReadValue<float>() == 1;  
                }
            }
            
            /// <summary>
            /// Invoked when the trackpad is touched.
            /// </summary>
            public event ViveInputCallback OnTrackpadPressed;
            private void TrackpadPressedPerformed(InputAction.CallbackContext a) => OnTrackpadPressed?.Invoke(); 
            
            /// <summary>
            /// If the trackpad is touched or not.
            /// </summary>
            public bool TrackpadTouched {
                get {
                    if (!trackpadTouched.enabled)
                        trackpadTouched?.Enable();
                    
                    return (int)trackpadTouched?.ReadValue<float>() == 1;  
                }
            }
            
            /// <summary>
            /// Invoked when the trackpad is touched.
            /// </summary>
            public event ViveInputCallback OnTrackpadTouched;
            private void TrackpadTouchedPerformed(InputAction.CallbackContext a) => OnTrackpadTouched?.Invoke(); 
            
            /// <summary>
            /// Registers all input(s).
            /// </summary>
            public void RegisterInputs() {
                if (trackpadPosition != null) {
                    trackpadPosition.Enable();
                    trackpadPosition.performed += TrackpadPositionPerformed;
                }
                
                if (trackpadPressed != null) {
                    trackpadPressed.Enable();
                    trackpadPressed.performed += TrackpadPressedPerformed;
                }
                
                if (trackpadTouched != null) {
                    trackpadTouched.Enable();
                    trackpadTouched.performed += TrackpadTouchedPerformed;
                }
            }
            
            /// <summary>
            /// Unregisters all input(s).
            /// </summary>
            public void UnregisterInputs() {
                if (trackpadPosition != null) {
                    trackpadPosition.Disable();
                    trackpadPosition.performed -= TrackpadPositionPerformed;
                }
                
                if (trackpadPressed != null) {
                    trackpadPressed.Disable();
                    trackpadPressed.performed -= TrackpadPressedPerformed;
                }
                
                if (trackpadTouched != null) {
                    trackpadTouched.Disable();
                    trackpadTouched.performed -= TrackpadTouchedPerformed;
                }
            }

            public delegate void ViveInputCallback();
        }
    
        /// <summary>
        /// Struct which manages and contains all Valve specific input actions.
        /// </summary>
        [Serializable]
        public struct ValveActionBindings {
            [SerializeField] [Tooltip("Input reference for how tightly the grip is squeezed.")]
            private InputAction gripForce;
            
            [SerializeField] [Tooltip("Input reference for the fingers position on the trackpad.")]
            private InputAction trackpadPosition;
            
            [SerializeField] [Tooltip("Input reference for when the trackpad is touched.")]
            private InputAction trackpadTouched;
            
            [SerializeField] [Tooltip("Input reference for how tightly the trackpad is squeezed.")]
            private InputAction trackpadForce;
            
            [SerializeField] [Tooltip("Input reference for when the trigger is touched.")]
            private InputAction triggerTouched;
            
            [SerializeField] [Tooltip("Input reference for when the joystick is touched.")]
            private InputAction joystickTouched;
            
            /// <summary>
            /// How tightly the player is squeezing the grip.
            /// </summary>
            public float GripForce {
                get {
                    if (!gripForce.enabled)
                        gripForce?.Enable();
                    
                    var value = (float)gripForce?.ReadValue<float>();
                    return value > 0.01f ? value : 0f;
                }
            }
            
            /// <summary>
            /// Invoked when the grip force is changed.
            /// </summary>
            public event ValveInputCallback OnGripForceChanged;
            private void GripForcePerformed(InputAction.CallbackContext a) => OnGripForceChanged?.Invoke(); 
            
            /// <summary>
            /// The position of the finger on the trackpad.
            /// </summary>
            public Vector2 TrackpadPosition {
                get {
                    if (!trackpadPosition.enabled)
                        trackpadPosition?.Enable();
                    
                    return (Vector2)trackpadPosition?.ReadValue<Vector2>();  
                }
            }
            
            /// <summary>
            /// Invoked when the trackpad position is changed.
            /// </summary>
            public event ValveInputCallback OnTrackpadPositionChanged;
            private void TrackpadPositionPerformed(InputAction.CallbackContext a) => OnTrackpadPositionChanged?.Invoke(); 
            
            /// <summary>
            /// If the trackpad is clicked or not.
            /// </summary>
            public bool TrackpadTouched {
                get {
                    if (!trackpadTouched.enabled) 
                        trackpadTouched?.Enable();
                    
                    return (int)trackpadTouched?.ReadValue<float>() == 1;  
                }
            }

            /// <summary>
            /// Invoked when the trackpad is touched.
            /// </summary>
            public event ValveInputCallback OnTrackpadTouched;
            private void TrackpadTouchedPerformed(InputAction.CallbackContext a) => OnTrackpadTouched?.Invoke(); 
            
            /// <summary>
            /// How tightly the player is squeezing the trackpad.
            /// </summary>
            public float TrackpadForce {
                get {
                    if (!trackpadForce.enabled)
                        trackpadForce?.Enable();
                    
                    var value = (float)trackpadForce?.ReadValue<float>();  
                    return value > 0.01f ? value : 0f;
                }
            }
            
            /// <summary>
            /// Invoked when the trackpad force is changed.
            /// </summary>
            public event ValveInputCallback OnTrackpadForceChanged;
            private void TrackpadForcePerformed(InputAction.CallbackContext a) => OnTrackpadForceChanged?.Invoke(); 
            
            /// <summary>
            /// If the trigger is touched or not.
            /// </summary>
            public bool TriggerTouched {
                get {
                    if (!triggerTouched.enabled)
                        triggerTouched?.Enable();
                    
                    return (int)triggerTouched?.ReadValue<float>() == 1;  
                }
            }
            
            /// <summary>
            /// Invoked when the trigger is touched.
            /// </summary>
            public event ValveInputCallback OnTriggerTouched;
            private void TriggerTouchedPerformed(InputAction.CallbackContext a) => OnTriggerTouched?.Invoke(); 
            
            /// <summary>
            /// If the joystick is touched or not.
            /// </summary>
            public bool JoystickTouched {
                get {
                    if (!joystickTouched.enabled)
                        joystickTouched?.Enable();
                    
                    return (int)joystickTouched?.ReadValue<float>() == 1; 
                }
            }
            
            /// <summary>
            /// Invoked when the joystick is touched.
            /// </summary>
            public event ValveInputCallback OnJoystickTouched;
            private void JoystickTouchedPerformed(InputAction.CallbackContext a) => OnJoystickTouched?.Invoke(); 
            
            /// <summary>
            /// Registers all input(s).
            /// </summary>
            public void RegisterInputs() {
                if (gripForce != null) {
                    gripForce.Enable();
                    gripForce.performed += GripForcePerformed;
                }

                if (trackpadPosition != null) {
                    trackpadPosition.Enable();
                    trackpadPosition.performed += TrackpadPositionPerformed;
                }
                
                if (trackpadTouched != null) {
                    trackpadTouched.Enable();
                    trackpadTouched.performed += TrackpadTouchedPerformed;
                }
                
                if (trackpadForce != null) {
                    trackpadForce.Enable();
                    trackpadForce.performed += TrackpadForcePerformed;
                }
                
                if (triggerTouched != null) {
                    triggerTouched.Enable();
                    triggerTouched.performed += TriggerTouchedPerformed;
                }
                
                if (joystickTouched != null) {
                    joystickTouched.Enable();
                    joystickTouched.performed += JoystickTouchedPerformed;
                }
            }
            
            /// <summary>
            /// Unregisters all input(s).
            /// </summary>
            public void UnregisterInputs() {
                if (gripForce != null) {
                    gripForce.Disable();
                    gripForce.performed -= GripForcePerformed;
                }
                
                if (trackpadPosition != null) {
                    trackpadPosition.Disable();
                    trackpadPosition.performed -= TrackpadPositionPerformed;
                }
                
                if (trackpadTouched != null) {
                    trackpadTouched.Disable();
                    trackpadTouched.performed -= TrackpadTouchedPerformed;
                }
                
                if (trackpadForce != null) {
                    trackpadForce.Disable();
                    trackpadForce.performed -= TrackpadForcePerformed;
                }
                
                if (triggerTouched != null) {
                    triggerTouched.Disable();
                    triggerTouched.performed -= TriggerTouchedPerformed;
                }
                
                if (joystickTouched != null) {
                    joystickTouched.Disable();
                    joystickTouched.performed -= JoystickTouchedPerformed;
                }
            }

            public delegate void ValveInputCallback();
        }
        
        /// <summary>
        /// The frequency at which the OnTrackerPositionChanged and OnTrackerRotationChanged events invoke at.
        /// </summary>
        [Space(5)] [Tooltip("The frequency at which the TrackerPosition and TrackerRotation events invoke.")]
        public UpdateFrequencies updateFrequency = UpdateFrequencies.OnUpdate;
        
        [Space(5)] [SerializeField] [Tooltip("If the container should register when the container loads.")]
        private bool enableActionsOnLoad = true;

        [SerializeField] [Tooltip("If the container should ignore unregister requests. This is not recommended!")]
        private bool preventUnregisters;
        
        /// <summary>
        /// If true, all of the action events have been registered and the inputs are enabled.
        /// </summary>
        public bool AllInputsWereRegistered { get; private set; }

        /// <summary>
        /// Invoked when all of the events are registered.
        /// </summary>
        public event VRInputContainerEvent RegisteredAllInputs;
        
        /// <summary>
        /// Invoked when all of the inputs are disabled.
        /// </summary>
        public event VRInputContainerEvent UnRegisteredAllInputs;
        
        public delegate void VRInputContainerEvent();
        
        #endregion

        private void OnEnable() {
            if (enableActionsOnLoad)
                RegisterAllInputs();
        }

        /// <summary>
        /// Registers all of the input bindings events AND enables the actions.
        /// </summary>
        public void RegisterAllInputs() {
            universal.RegisterInputs();
            system.RegisterInputs();
            meta.RegisterInputs();
            vive.RegisterInputs();
            valve.RegisterInputs();

            RegisteredAllInputs?.Invoke();
            AllInputsWereRegistered = true;
        }
        
        /// <summary>
        /// Unregisters all of the input bindings events AND disables the actions.
        /// </summary>
        public void UnregisterAllInputs() {
            if (preventUnregisters)
                return;
            
            universal.UnregisterInputs();
            system.UnregisterInputs();
            meta.UnregisterInputs();
            vive.UnregisterInputs();
            valve.UnregisterInputs();

            UnRegisteredAllInputs?.Invoke();
            AllInputsWereRegistered = false;
        }
    }
}
