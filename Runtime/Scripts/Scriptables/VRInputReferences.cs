// This script was updated on 10/31/2021 by Jack Randolph.

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

namespace ItsVR.Scriptables {
    [HelpURL("https://jackedupstudios.com/its-vr-documentation-1#b47abff1-8a0b-4eb6-b03c-3617ba2beb61")]
    [CreateAssetMenu(menuName = "It's VR/Input/VR Input Reference", fileName = "My VR Inputs")]
    public class VRInputReferences : ScriptableObject {
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
        /// All of the Oculus specific input bindings.
        /// </summary>
        [Tooltip("All of the Oculus specific input bindings.")]
        public OculusInputs oculusInputs;

        /// <summary>
        /// All of the Index specific input bindings.
        /// </summary>
        [Tooltip("All of the Index specific input bindings.")]
        public IndexInputs indexInputs;
        
        /// <summary>
        /// All of the Vive specific input bindings.
        /// </summary>
        [Tooltip("All of the Vive specific input bindings.")]
        public ViveInputs viveInputs;

        #endregion

        /// <summary>
        /// Disables every input referenced on this reference.
        /// </summary>
        public void DisableAllInputs() {
            universalInputs.DisableInputs();
            oculusInputs.DisableInputs();
            indexInputs.DisableInputs();
            viveInputs.DisableInputs();
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
        [Tooltip("Input reference for how far the trigger is depressed.")]
        public InputAction triggerDepress;
        
        /// <summary>
        /// Input reference for when the trigger is pressed.
        /// </summary>
        [Tooltip("Input reference for when the trigger is pressed.")]
        public InputAction triggerPressed;

        /// <summary>
        /// Input reference for how far the grip is depressed.
        /// </summary>
        [Tooltip("Input reference for how far the grip is depressed.")]
        public InputAction gripDepress;
        
        /// <summary>
        /// Input reference for when the grip is pressed.
        /// </summary>
        [Tooltip("Input reference for when the grip is pressed.")]
        public InputAction gripPressed;

        /// <summary>
        /// Input reference for the joysticks position.
        /// </summary>
        [Tooltip("Input reference for the joysticks position.")]
        public InputAction joystickPosition;
        
        /// <summary>
        /// Input reference for when the joystick is pressed.
        /// </summary>
        [Tooltip("Input reference for when the joystick is pressed.")]
        public InputAction joystickPressed;

        /// <summary>
        /// Input reference for when the joystick is touched.
        /// </summary>
        [Tooltip("Input reference for when the joystick is touched.")]
        public InputAction joystickTouched;
        
        /// <summary>
        /// Input reference for when the primary button is pressed.
        /// </summary>
        [Tooltip("Input reference for when the primary button is pressed.")]
        public InputAction primaryButtonPressed;

        /// <summary>
        /// Input reference for when the primary button is touched.
        /// </summary>
        [Tooltip("Input reference for when the primary button is touched.")]
        public InputAction primaryButtonTouched;
        
        /// <summary>
        /// Input reference for when the secondary button is pressed.
        /// </summary>
        [Tooltip("Input reference for when the secondary button is pressed.")]
        public InputAction secondaryButtonPressed;

        /// <summary>
        /// Input reference for when the secondary button is touched.
        /// </summary>
        [Tooltip("Input reference for when the secondary button is touched.")]
        public InputAction secondaryButtonTouched;
        
        /// <summary>
        /// Input reference for the controllers haptics.
        /// </summary>
        [Tooltip("Input reference for the controllers haptics.")]
        public InputAction haptics;
        
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
        /// Vibrates the controller at amplitude for duration.
        /// </summary>
        /// <param name="amplitude"></param>
        /// <param name="duration"></param>
        public void SendImpulse(float amplitude, float duration) {
            if (!haptics.enabled) haptics?.Enable();
            if (haptics?.activeControl?.device is XRControllerWithRumble rumbleController) 
                rumbleController.SendImpulse(amplitude, duration);
        }
        
        /// <summary>
        /// Disables all universal inputs.
        /// </summary>
        public void DisableInputs() {
            triggerDepress?.Disable();
            triggerPressed?.Disable();
            
            gripDepress?.Disable();
            gripPressed?.Disable();
            
            joystickPosition?.Disable();
            joystickPressed?.Disable();
            joystickTouched?.Disable();
            
            primaryButtonPressed?.Disable();
            primaryButtonTouched?.Disable();
            
            secondaryButtonPressed?.Disable();
            secondaryButtonTouched?.Disable();
            
            haptics?.Disable();
        }
    }

    /// <summary>
    /// Class containing all system specific inputs.
    /// </summary>
    [System.Serializable]
    public class SystemInputs {
        /// <summary>
        /// Input reference for when the system button is touched.
        /// </summary>
        [Tooltip("Input reference for when the system button is touched.")]
        public InputAction systemPressed;

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
        /// Disables all system specific inputs.
        /// </summary>
        public void DisableInputs() {
            systemPressed?.Disable();
        }
    }

    /// <summary>
    /// Class containing all oculus specific inputs.
    /// </summary>
    [System.Serializable]
    public class OculusInputs {
        /// <summary>
        /// Input reference for when the trigger is touched.
        /// </summary>
        [Tooltip("Input reference for when the trigger is touched.")]
        public InputAction triggerTouched;

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
        /// Disables all Oculus specific inputs.
        /// </summary>
        public void DisableInputs() {
            triggerTouched?.Disable();
        }
    }
    
    /// <summary>
    /// Class containing all vive specific inputs.
    /// </summary>
    [System.Serializable]
    public class ViveInputs {
        /// <summary>
        /// Input reference for the fingers position on the trackpad.
        /// </summary>
        [Tooltip("Input reference for the fingers position on the trackpad.")]
        public InputAction trackpadPosition;
        
        /// <summary>
        /// Input reference for when the trackpad is clicked.
        /// </summary>
        [Tooltip("Input reference for when the trackpad is clicked.")]
        public InputAction trackpadClicked;
        
        /// <summary>
        /// Input reference for when the trackpad is touched.
        /// </summary>
        [Tooltip("Input reference for when the trackpad is touched.")]
        public InputAction trackpadTouched;

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
        /// If the trackpad is clicked or not.
        /// </summary>
        /// <returns></returns>
        public bool TrackpadClicked {
            get {
                if (!trackpadClicked.enabled) trackpadClicked?.Enable();
                return (int)trackpadClicked?.ReadValue<float>() == 1;  
            }
        }
        
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
        /// Disables all Vive specific inputs.
        /// </summary>
        public void DisableInputs() {
            trackpadPosition?.Disable();
            trackpadClicked?.Disable();
            trackpadTouched?.Disable();
        }
    }
    
    /// <summary>
    /// Class containing all index specific inputs.
    /// </summary>
    [System.Serializable]
    public class IndexInputs {
        /// <summary>
        /// Input reference for how tightly the grip is squeezed.
        /// </summary>
        [Tooltip("Input reference for how tightly the grip is squeezed.")]
        public InputAction gripForce;
    
        /// <summary>
        /// Input reference for the fingers position on the trackpad.
        /// </summary>
        [Tooltip("Input reference for the fingers position on the trackpad.")]
        public InputAction trackpadPosition;
        
        /// <summary>
        /// Input reference for when the trackpad is touched.
        /// </summary>
        [Tooltip("Input reference for when the trackpad is touched.")]
        public InputAction trackpadTouched;
        
        /// <summary>
        /// Input reference for how tightly the trackpad is squeezed.
        /// </summary>
        [Tooltip("Input reference for how tightly the trackpad is squeezed.")]
        public InputAction trackpadForce;
        
        /// <summary>
        /// Input reference for when the trigger is touched.
        /// </summary>
        [Tooltip("Input reference for when the trigger is touched.")]
        public InputAction triggerTouched;

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
        /// Disables all Index specific inputs.
        /// </summary>
        public void DisableInputs() {
            gripForce?.Disable();
            trackpadPosition?.Disable();
            trackpadTouched?.Disable();
            trackpadForce?.Disable();
            triggerTouched?.Disable();
        }
    }
}
