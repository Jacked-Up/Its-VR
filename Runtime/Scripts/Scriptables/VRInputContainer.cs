// This script was updated on 11/14/2021 by Jack Randolph.

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

namespace ItsVR.Scriptables {
    /// <summary>
    /// The VR input container manages input from VR controllers. It helps to simplify the process of supporting
    /// multiple different VR controllers.
    /// </summary>
    [HelpURL("https://jackedupstudios.com/its-vr-documentation-1#b47abff1-8a0b-4eb6-b03c-3617ba2beb61")]
    [CreateAssetMenu(menuName = "It's VR/Input/VR Input Container", fileName = "My VR Input Container")]
    public class VRInputContainer : ScriptableObject {
        #region Variables

        /// <summary>
        /// All of the universal input bindings.
        /// </summary>
        [Tooltip("All of the universal input bindings.")]
        public UniversalInputs universalInputs;

        /// <summary>
        /// All of the system input bindings.
        /// </summary>
        [Tooltip("All of the system input bindings.")]
        public SystemInputs systemInputs;
        
        /// <summary>
        /// All of the Meta specific input bindings.
        /// </summary>
        [Tooltip("All of the Meta specific input bindings.")]
        public MetaInputs metaInputs;

        /// <summary>
        /// All of the Vive specific input bindings.
        /// </summary>
        [Tooltip("All of the Vive specific input bindings.")]
        public ViveInputs viveInputs;
        
        /// <summary>
        /// All of the Valve specific input bindings.
        /// </summary>
        [Tooltip("All of the Valve specific input bindings.")]
        public ValveInputs valveInputs;

        #endregion

        /// <summary>
        /// Registers all input events references in this container.
        /// </summary>
        public void RegisterAllEvents() {
            universalInputs.RegisterEvents();
            systemInputs.RegisterEvents();
            metaInputs.RegisterEvents();
            viveInputs.RegisterEvents();
            valveInputs.RegisterEvents();
        }
        
        /// <summary>
        /// Disables every input referenced in this container.
        /// </summary>
        public void DisableAllInputs() {
            universalInputs.DisableInputs();
            systemInputs.DisableInputs();
            metaInputs.DisableInputs();
            viveInputs.DisableInputs();
            valveInputs.DisableInputs();
        }
    }

    /// <summary>
    /// Class containing all of the VR universal inputs.
    /// </summary>
    [System.Serializable]
    public class UniversalInputs {
        /// <summary>
        /// Input reference for how far the trigger is depressed.
        /// </summary>
        [SerializeField] [Tooltip("Input reference for how far the trigger is depressed.")]
        private InputAction triggerDepress;
        
        /// <summary>
        /// Input reference for when the trigger is pressed.
        /// </summary>
        [SerializeField] [Tooltip("Input reference for when the trigger is pressed.")]
        private InputAction triggerPressed;

        /// <summary>
        /// Input reference for how far the grip is depressed.
        /// </summary>
        [SerializeField] [Tooltip("Input reference for how far the grip is depressed.")]
        private InputAction gripDepress;
        
        /// <summary>
        /// Input reference for when the grip is pressed.
        /// </summary>
        [SerializeField] [Tooltip("Input reference for when the grip is pressed.")]
        private InputAction gripPressed;

        /// <summary>
        /// Input reference for the joysticks position.
        /// </summary>
        [SerializeField] [Tooltip("Input reference for the joysticks position.")]
        private InputAction joystickPosition;
        
        /// <summary>
        /// Input reference for when the joystick is pressed.
        /// </summary>
        [SerializeField] [Tooltip("Input reference for when the joystick is pressed.")]
        private InputAction joystickPressed;

        /// <summary>
        /// Input reference for when the primary button is pressed.
        /// </summary>
        [SerializeField] [Tooltip("Input reference for when the primary button is pressed.")]
        private InputAction primaryButtonPressed;

        /// <summary>
        /// Input reference for when the primary button is touched.
        /// </summary>
        [SerializeField] [Tooltip("Input reference for when the primary button is touched.")]
        private InputAction primaryButtonTouched;
        
        /// <summary>
        /// Input reference for when the secondary button is pressed.
        /// </summary>
        [SerializeField] [Tooltip("Input reference for when the secondary button is pressed.")]
        private InputAction secondaryButtonPressed;

        /// <summary>
        /// Input reference for when the secondary button is touched.
        /// </summary>
        [SerializeField] [Tooltip("Input reference for when the secondary button is touched.")]
        private InputAction secondaryButtonTouched;
        
        /// <summary>
        /// Input reference for the controllers haptics.
        /// </summary>
        [SerializeField] [Tooltip("Input reference for the controllers haptics.")]
        private InputAction haptics;
        
        /// <summary>
        /// How far the trigger is depressed.
        /// </summary>
        /// <returns></returns>
        public float TriggerDepress {
            get {
                if (!triggerDepress.enabled) triggerDepress?.Enable();
                return (float)triggerDepress?.ReadValue<float>();
            }
        }
        
        /// <summary>
        /// Invoked when the trigger depress/position changes.
        /// </summary>
        public event UniversalInputEvent OnTriggerDepressChanged;
        private void TriggerDepressPerformed(InputAction.CallbackContext a) { OnTriggerDepressChanged?.Invoke(); }
        
        /// <summary>
        /// If the trigger is pressed or not.
        /// </summary>
        /// <returns></returns>
        public bool TriggerPressed {
            get {
                if (!triggerPressed.enabled) triggerPressed?.Enable();
                return (int)triggerPressed?.ReadValue<float>() == 1;
            }
        }
        
        /// <summary>
        /// Invoked when the trigger is pressed.
        /// </summary>
        public event UniversalInputEvent OnTriggerPressed;
        private void TriggerPressedPerformed(InputAction.CallbackContext a) { OnTriggerPressed?.Invoke(); }
        
        /// <summary>
        /// How far the grip is depressed.
        /// </summary>
        /// <returns></returns>
        public float GripDepress {
            get {
                if (!gripDepress.enabled) gripDepress?.Enable();
                return (float)gripDepress?.ReadValue<float>();  
            }
        }
        
        /// <summary>
        /// Invoked when the grips depress/position changes.
        /// </summary>
        public event UniversalInputEvent OnGripDepressChanged;
        private void GripDepressPerformed(InputAction.CallbackContext a) { OnGripDepressChanged?.Invoke(); }
        
        /// <summary>
        /// If the grip is pressed or not.
        /// </summary>
        /// <returns></returns>
        public bool GripPressed {
            get {
                if (!gripPressed.enabled) gripPressed?.Enable();
                return (int)gripPressed?.ReadValue<float>() == 1;
            }
        }
        
        /// <summary>
        /// Invoked when the grip is pressed.
        /// </summary>
        public event UniversalInputEvent OnGripPressed;
        private void GripPressedPerformed(InputAction.CallbackContext a) { OnGripPressed?.Invoke(); }
        
        /// <summary>
        /// The position of the joystick.
        /// </summary>
        /// <returns></returns>
        public Vector2 JoystickPosition {
            get {
                if (!joystickPosition.enabled) joystickPosition?.Enable();
                return (Vector2)joystickPosition?.ReadValue<Vector2>(); 
            }
        }
        
        /// <summary>
        /// Invoked when the joysticks position is changed.
        /// </summary>
        public event UniversalInputEvent OnJoystickPositionChanged;
        private void JoystickPositionPerformed(InputAction.CallbackContext a) { OnJoystickPositionChanged?.Invoke(); }
        
        /// <summary>
        /// If the joystick is pressed or not.
        /// </summary>
        /// <returns></returns>
        public bool JoystickPressed {
            get {
                if (!joystickPressed.enabled) joystickPressed?.Enable();
                return (int)joystickPressed?.ReadValue<float>() == 1; 
            }
        }

        /// <summary>
        /// Invoked when the joystick is pressed.
        /// </summary>
        public event UniversalInputEvent OnJoystickPressed;
        private void JoystickPressedPerformed(InputAction.CallbackContext a) { OnJoystickPressed?.Invoke(); }
        
        /// <summary>
        /// If the primary button is pressed or not.
        /// </summary>
        /// <returns></returns>
        public bool PrimaryButtonPressed {
            get {
                if (!primaryButtonPressed.enabled) primaryButtonPressed?.Enable();
                return (int)primaryButtonPressed?.ReadValue<float>() == 1; 
            }
        }
        
        /// <summary>
        /// Invoked when the primary button is pressed.
        /// </summary>
        public event UniversalInputEvent OnPrimaryButtonPressed;
        private void PrimaryButtonPressedPerformed(InputAction.CallbackContext a) { OnPrimaryButtonPressed?.Invoke(); }
        
        /// <summary>
        /// If the primary button is touched or not.
        /// </summary>
        /// <returns></returns>
        public bool PrimaryButtonTouched {
            get {
                if (!primaryButtonTouched.enabled) primaryButtonTouched?.Enable();
                return (int)primaryButtonTouched?.ReadValue<float>() == 1;
            }
        }
        
        /// <summary>
        /// Invoked when the primary button is touched.
        /// </summary>
        public event UniversalInputEvent OnPrimaryButtonTouched;
        private void PrimaryButtonTouchedPerformed(InputAction.CallbackContext a) { OnPrimaryButtonTouched?.Invoke(); }
        
        /// <summary>
        /// If the secondary button is pressed or not.
        /// </summary>
        /// <returns></returns>
        public bool SecondaryButtonPressed {
            get {
                if (!secondaryButtonPressed.enabled) secondaryButtonPressed?.Enable();
                return (int)secondaryButtonPressed?.ReadValue<float>() == 1;  
            }
        }

        /// <summary>
        /// Invoked when the secondary button is pressed.
        /// </summary>
        public event UniversalInputEvent OnSecondaryButtonPressed;
        private void SecondaryButtonPressedPerformed(InputAction.CallbackContext a) { OnSecondaryButtonPressed?.Invoke(); }
        
        /// <summary>
        /// If the secondary button is touched or not.
        /// </summary>
        /// <returns></returns>
        public bool SecondaryButtonTouched {
            get {
                if (!secondaryButtonTouched.enabled) secondaryButtonTouched?.Enable();
                return (int)secondaryButtonTouched?.ReadValue<float>() == 1; 
            }
        }
        
        /// <summary>
        /// Invoked when the secondary button is touched.
        /// </summary>
        public event UniversalInputEvent OnSecondaryButtonTouched;
        private void SecondaryButtonTouchedPerformed(InputAction.CallbackContext a) { OnSecondaryButtonTouched?.Invoke(); }
        
        /// <summary>
        /// Vibrates the controller at amplitude for duration.
        /// </summary>
        /// <param name="amplitude">The speed of the motor during the vibration. (0-1)</param>
        /// <param name="duration">How long the haptic device vibrates.</param>
        public void SendImpulse(float amplitude, float duration) {
            if (!haptics.enabled) haptics?.Enable();
            if (haptics?.activeControl?.device is XRControllerWithRumble rumbleController) 
                rumbleController.SendImpulse(amplitude, duration);
        }

        /// <summary>
        /// Registers with all universal specific input events.
        /// </summary>
        public void RegisterEvents() {
            triggerDepress?.Enable();
            if (triggerDepress != null) triggerDepress.performed += TriggerDepressPerformed;
            triggerPressed?.Enable();
            if (triggerPressed != null) triggerPressed.performed += TriggerPressedPerformed;

            gripDepress?.Enable();
            if (gripDepress != null) gripDepress.performed += GripDepressPerformed;
            gripPressed?.Enable();
            if (gripPressed != null) gripPressed.performed += GripPressedPerformed;

            joystickPosition?.Enable();
            if (joystickPosition != null) joystickPosition.performed += JoystickPositionPerformed;
            joystickPressed?.Enable();
            if (joystickPressed != null) joystickPressed.performed += JoystickPressedPerformed;

            primaryButtonPressed?.Enable();
            if (primaryButtonPressed != null) primaryButtonPressed.performed += PrimaryButtonPressedPerformed;
            primaryButtonTouched?.Enable();
            if (primaryButtonTouched != null) primaryButtonTouched.performed += PrimaryButtonTouchedPerformed;

            secondaryButtonPressed?.Enable();
            if (secondaryButtonPressed != null) secondaryButtonPressed.performed += SecondaryButtonPressedPerformed;
            secondaryButtonTouched?.Enable();
            if (secondaryButtonTouched != null) secondaryButtonTouched.performed += SecondaryButtonTouchedPerformed;
        }
        
        /// <summary>
        /// Disables all universal specific inputs.
        /// </summary>
        public void DisableInputs() {
            triggerDepress?.Disable();
            if (triggerDepress != null) triggerDepress.performed -= TriggerDepressPerformed;
            triggerPressed?.Disable();
            if (triggerPressed != null) triggerPressed.performed -= TriggerPressedPerformed;

            gripDepress?.Disable();
            if (gripDepress != null) gripDepress.performed -= GripDepressPerformed;
            gripPressed?.Disable();
            if (gripPressed != null) gripPressed.performed -= GripPressedPerformed;

            joystickPosition?.Disable();
            if (joystickPosition != null) joystickPosition.performed -= JoystickPositionPerformed;
            joystickPressed?.Disable();
            if (joystickPressed != null) joystickPressed.performed -= JoystickPressedPerformed;

            primaryButtonPressed?.Disable();
            if (primaryButtonPressed != null) primaryButtonPressed.performed -= PrimaryButtonPressedPerformed;
            primaryButtonTouched?.Disable();
            if (primaryButtonTouched != null) primaryButtonTouched.performed -= PrimaryButtonTouchedPerformed;

            secondaryButtonPressed?.Disable();
            if (secondaryButtonPressed != null) secondaryButtonPressed.performed -= SecondaryButtonPressedPerformed;
            secondaryButtonTouched?.Disable();
            if (secondaryButtonTouched != null) secondaryButtonTouched.performed -= SecondaryButtonTouchedPerformed;

            haptics?.Disable();
        }

        public delegate void UniversalInputEvent();
    }

    /// <summary>
    /// Class containing all system specific inputs.
    /// </summary>
    [System.Serializable]
    public class SystemInputs {
        /// <summary>
        /// Input reference for when the system button is touched.
        /// </summary>
        [SerializeField] [Tooltip("Input reference for when the system button is touched.")]
        private InputAction systemPressed;

        /// <summary>
        /// If the system button is pressed or not.
        /// </summary>
        /// <returns></returns>
        public bool SystemPressed {
            get {
                if (!systemPressed.enabled) systemPressed?.Enable();
                return (int)systemPressed?.ReadValue<float>() == 1;  
            }
        }
        
        /// <summary>
        /// Invoked when the system is pressed.
        /// </summary>
        public event SystemInputEvent OnSystemPressed;
        private void SystemPressedPerformed(InputAction.CallbackContext a) { OnSystemPressed?.Invoke(); }
        
        /// <summary>
        /// Registers with all system specific input events.
        /// </summary>
        public void RegisterEvents() {
            systemPressed?.Enable();
            if (systemPressed != null) systemPressed.performed += SystemPressedPerformed;
        }
        
        /// <summary>
        /// Disables all system specific inputs.
        /// </summary>
        public void DisableInputs() {
            systemPressed?.Disable();
            if (systemPressed != null) systemPressed.performed -= SystemPressedPerformed;
        }

        public delegate void SystemInputEvent();
    }

    /// <summary>
    /// Class containing all Meta specific inputs.
    /// </summary>
    [System.Serializable]
    public class MetaInputs {
        /// <summary>
        /// Input reference for when the trigger is touched.
        /// </summary>
        [SerializeField] [Tooltip("Input reference for when the trigger is touched.")]
        private InputAction triggerTouched;

        /// <summary>
        /// Input reference for when the joystick is touched.
        /// </summary>
        [SerializeField] [Tooltip("Input reference for when the joystick is touched.")]
        private InputAction joystickTouched;
        
        /// <summary>
        /// If the trigger is touched or not.
        /// </summary>
        /// <returns></returns>
        public bool TriggerTouched {
            get {
                if (!triggerTouched.enabled) triggerTouched?.Enable();
                return (int)triggerTouched?.ReadValue<float>() == 1;  
            }
        }
        
        /// <summary>
        /// Invoked when the trigger is touched.
        /// </summary>
        public event MetaInputEvent OnTriggerTouched;
        private void TriggerTouchedPerformed(InputAction.CallbackContext a) { OnTriggerTouched?.Invoke(); }
        
        /// <summary>
        /// If the joystick is touched or not.
        /// </summary>
        /// <returns></returns>
        public bool JoystickTouched {
            get {
                if (!joystickTouched.enabled) joystickTouched?.Enable();
                return (int)joystickTouched?.ReadValue<float>() == 1; 
            }
        }
        
        /// <summary>
        /// Invoked when the joystick is touched.
        /// </summary>
        public event MetaInputEvent OnJoystickTouched;
        private void JoystickTouchedPerformed(InputAction.CallbackContext a) { OnJoystickTouched?.Invoke(); }
        
        /// <summary>
        /// Registers with all Meta specific input events.
        /// </summary>
        public void RegisterEvents() {
            triggerTouched?.Enable();
            if (triggerTouched != null) triggerTouched.performed += TriggerTouchedPerformed;
           
            joystickTouched?.Enable();
            if (joystickTouched != null) joystickTouched.performed += JoystickTouchedPerformed;
        }
        
        /// <summary>
        /// Disables all Meta specific inputs.
        /// </summary>
        public void DisableInputs() {
            triggerTouched?.Disable();
            if (triggerTouched != null) triggerTouched.performed -= TriggerTouchedPerformed;
            
            joystickTouched?.Disable();
            if (joystickTouched != null) joystickTouched.performed -= JoystickTouchedPerformed;
        }

        public delegate void MetaInputEvent();
    }
    
    /// <summary>
    /// Class containing all Vive specific inputs.
    /// </summary>
    [System.Serializable]
    public class ViveInputs {
        /// <summary>
        /// Input reference for the fingers position on the trackpad.
        /// </summary>
        [SerializeField] [Tooltip("Input reference for the fingers position on the trackpad.")]
        private InputAction trackpadPosition;
        
        /// <summary>
        /// Input reference for when the trackpad is pressed.
        /// </summary>
        [SerializeField] [Tooltip("Input reference for when the trackpad is pressed.")]
        private InputAction trackpadPressed;
        
        /// <summary>
        /// Input reference for when the trackpad is touched.
        /// </summary>
        [SerializeField] [Tooltip("Input reference for when the trackpad is touched.")]
        private InputAction trackpadTouched;

        /// <summary>
        /// The position of the finger on the trackpad.
        /// </summary>
        /// <returns></returns>
        public Vector2 TrackpadPosition {
            get {
                if (!trackpadPosition.enabled) trackpadPosition?.Enable();
                return (Vector2)trackpadPosition?.ReadValue<Vector2>();  
            }
        }
        
        /// <summary>
        /// Invoked when the trackpad position is changed.
        /// </summary>
        public event ViveInputEvent OnTrackpadPositionChanged;
        private void TrackpadPositionPerformed(InputAction.CallbackContext a) { OnTrackpadPositionChanged?.Invoke(); }
        
        /// <summary>
        /// If the trackpad is pressed or not.
        /// </summary>
        /// <returns></returns>
        public bool TrackpadPressed {
            get {
                if (!trackpadPressed.enabled) trackpadPressed?.Enable();
                return (int)trackpadPressed?.ReadValue<float>() == 1;  
            }
        }
        
        /// <summary>
        /// Invoked when the trackpad is touched.
        /// </summary>
        public event ViveInputEvent OnTrackpadPressed;
        private void TrackpadPressedPerformed(InputAction.CallbackContext a) { OnTrackpadPressed?.Invoke(); }
        
        /// <summary>
        /// If the trackpad is touched or not.
        /// </summary>
        /// <returns></returns>
        public bool TrackpadTouched {
            get {
                if (!trackpadTouched.enabled) trackpadTouched?.Enable();
                return (int)trackpadTouched?.ReadValue<float>() == 1;  
            }
        }
        
        /// <summary>
        /// Invoked when the trackpad is touched.
        /// </summary>
        public event ViveInputEvent OnTrackpadTouched;
        private void TrackpadTouchedPerformed(InputAction.CallbackContext a) { OnTrackpadTouched?.Invoke(); }
        
        /// <summary>
        /// Registers with all Vive specific input events.
        /// </summary>
        public void RegisterEvents() {
            trackpadPosition?.Enable();
            if (trackpadPosition != null) trackpadPosition.performed += TrackpadPositionPerformed;
            trackpadPressed?.Enable();
            if (trackpadPressed != null) trackpadPressed.performed += TrackpadPressedPerformed;
            trackpadTouched?.Enable();
            if (trackpadTouched != null) trackpadTouched.performed += TrackpadTouchedPerformed;
        }
        
        /// <summary>
        /// Disables all Vive specific inputs.
        /// </summary>
        public void DisableInputs() {
            trackpadPosition?.Disable();
            if (trackpadPosition != null) trackpadPosition.performed -= TrackpadPositionPerformed;
            trackpadPressed?.Disable();
            if (trackpadPressed != null) trackpadPressed.performed -= TrackpadPressedPerformed;
            trackpadTouched?.Disable();
            if (trackpadTouched != null) trackpadTouched.performed -= TrackpadTouchedPerformed;
        }

        public delegate void ViveInputEvent();
    }
    
    /// <summary>
    /// Class containing all Valve specific inputs.
    /// </summary>
    [System.Serializable]
    public class ValveInputs {
        /// <summary>
        /// Input reference for how tightly the grip is squeezed.
        /// </summary>
        [SerializeField] [Tooltip("Input reference for how tightly the grip is squeezed.")]
        private InputAction gripForce;
    
        /// <summary>
        /// Input reference for the fingers position on the trackpad.
        /// </summary>
        [SerializeField] [Tooltip("Input reference for the fingers position on the trackpad.")]
        private InputAction trackpadPosition;
        
        /// <summary>
        /// Input reference for when the trackpad is touched.
        /// </summary>
        [SerializeField] [Tooltip("Input reference for when the trackpad is touched.")]
        private InputAction trackpadTouched;
        
        /// <summary>
        /// Input reference for how tightly the trackpad is squeezed.
        /// </summary>
        [SerializeField] [Tooltip("Input reference for how tightly the trackpad is squeezed.")]
        private InputAction trackpadForce;
        
        /// <summary>
        /// Input reference for when the trigger is touched.
        /// </summary>
        [SerializeField] [Tooltip("Input reference for when the trigger is touched.")]
        private InputAction triggerTouched;

        /// <summary>
        /// Input reference for when the joystick is touched.
        /// </summary>
        [SerializeField] [Tooltip("Input reference for when the joystick is touched.")]
        private InputAction joystickTouched;
        
        /// <summary>
        /// How tightly the player is squeezing the grip.
        /// </summary>
        /// <returns></returns>
        public float GripForce {
            get {
                if (!gripForce.enabled) gripForce?.Enable();
                return (float)gripForce?.ReadValue<float>();  
            }
        }
        
        /// <summary>
        /// Invoked when the grip force is changed.
        /// </summary>
        public event ValveInputEvent OnGripForceChanged;
        private void GripForcePerformed(InputAction.CallbackContext a) { OnGripForceChanged?.Invoke(); }
        
        /// <summary>
        /// The position of the finger on the trackpad.
        /// </summary>
        /// <returns></returns>
        public Vector2 TrackpadPosition {
            get {
                if (!trackpadPosition.enabled) trackpadPosition?.Enable();
                return (Vector2)trackpadPosition?.ReadValue<Vector2>();  
            }
        }
        
        /// <summary>
        /// Invoked when the trackpad position is changed.
        /// </summary>
        public event ValveInputEvent OnTrackpadPositionChanged;
        private void TrackpadPositionPerformed(InputAction.CallbackContext a) { OnTrackpadPositionChanged?.Invoke(); }
        
        /// <summary>
        /// If the trackpad is clicked or not.
        /// </summary>
        /// <returns></returns>
        public bool TrackpadTouched {
            get {
                if (!trackpadTouched.enabled) trackpadTouched?.Enable();
                return (int)trackpadTouched?.ReadValue<float>() == 1;  
            }
        }

        /// <summary>
        /// Invoked when the trackpad is touched.
        /// </summary>
        public event ValveInputEvent OnTrackpadTouched;
        private void TrackpadTouchedPerformed(InputAction.CallbackContext a) { OnTrackpadTouched?.Invoke(); }
        
        /// <summary>
        /// How tightly the player is squeezing the trackpad.
        /// </summary>
        /// <returns></returns>
        public float TrackpadForce {
            get {
                if (!trackpadForce.enabled) trackpadForce?.Enable();
                return (float)trackpadForce?.ReadValue<float>();  
            }
        }
        
        /// <summary>
        /// Invoked when the trackpad force is changed.
        /// </summary>
        public event ValveInputEvent OnTrackpadForceChanged;
        private void TrackpadForcePerformed(InputAction.CallbackContext a) { OnTrackpadForceChanged?.Invoke(); }
        
        /// <summary>
        /// If the trigger is touched or not.
        /// </summary>
        /// <returns></returns>
        public bool TriggerTouched {
            get {
                if (!triggerTouched.enabled) triggerTouched?.Enable();
                return (int)triggerTouched?.ReadValue<float>() == 1;  
            }
        }
        
        /// <summary>
        /// Invoked when the trigger is touched.
        /// </summary>
        public event ValveInputEvent OnTriggerTouched;
        private void TriggerTouchedPerformed(InputAction.CallbackContext a) { OnTriggerTouched?.Invoke(); }
        
        /// <summary>
        /// If the joystick is touched or not.
        /// </summary>
        /// <returns></returns>
        public bool JoystickTouched {
            get {
                if (!joystickTouched.enabled) joystickTouched?.Enable();
                return (int)joystickTouched?.ReadValue<float>() == 1; 
            }
        }
        
        /// <summary>
        /// Invoked when the joystick is touched.
        /// </summary>
        public event ValveInputEvent OnJoystickTouched;
        private void JoystickTouchedPerformed(InputAction.CallbackContext a) { OnJoystickTouched?.Invoke(); }
        
        /// <summary>
        /// Registers with all Valve specific input events.
        /// </summary>
        public void RegisterEvents() {
            gripForce?.Enable();
            if (gripForce != null) gripForce.performed += GripForcePerformed;
            
            trackpadPosition?.Enable();
            if (trackpadPosition != null) trackpadPosition.performed += TrackpadPositionPerformed;
            trackpadTouched?.Enable();
            if (trackpadTouched != null) trackpadTouched.performed += TrackpadTouchedPerformed;
            trackpadForce?.Enable();
            if (trackpadForce != null) trackpadForce.performed += TrackpadForcePerformed;

            triggerTouched?.Enable();
            if (triggerTouched != null) triggerTouched.performed += TriggerTouchedPerformed;
            
            joystickTouched?.Enable();
            if (joystickTouched != null) joystickTouched.performed += JoystickTouchedPerformed;
        }
        
        /// <summary>
        /// Disables all Valve specific inputs.
        /// </summary>
        public void DisableInputs() {
            gripForce?.Disable();
            if (gripForce != null) gripForce.performed -= GripForcePerformed;
            
            trackpadPosition?.Disable();
            if (trackpadPosition != null) trackpadPosition.performed -= TrackpadPositionPerformed;
            trackpadTouched?.Disable();
            if (trackpadTouched != null) trackpadTouched.performed -= TrackpadTouchedPerformed;
            trackpadForce?.Disable();
            if (trackpadForce != null) trackpadForce.performed -= TrackpadForcePerformed;
            
            triggerTouched?.Disable();
            if (triggerTouched != null) triggerTouched.performed -= TriggerTouchedPerformed;
            
            joystickTouched?.Disable();
            if (joystickTouched != null) joystickTouched.performed -= JoystickTouchedPerformed;
        }

        public delegate void ValveInputEvent();
    }
}
