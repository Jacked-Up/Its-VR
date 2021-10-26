// This script was updated on 10/26/2021 by Jack Randolph.

using UnityEngine;
using UnityEngine.InputSystem;

namespace ItsVR.Scriptables {
    [HelpURL("https://jackedupstudios.com/its-vr-documentation-1#b47abff1-8a0b-4eb6-b03c-3617ba2beb61")]
    [CreateAssetMenu(menuName = "It's VR/Input/VR Input Reference", fileName = "My VR Inputs")]
    public class VRInputReferences : ScriptableObject {
        #region Variables

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
        /// Input reference for when the thumbrest is touched.
        /// </summary>
        [Tooltip("Input reference for when the thumbrest is touched.")]
        public InputAction thumbrestTouched;

        #endregion

        /// <summary>
        /// How far the trigger is depressed.
        /// </summary>
        /// <returns></returns>
        public float TriggerDepress {
            get {
                if (!triggerDepress.enabled) triggerDepress.Enable();
                return (float)triggerDepress?.ReadValue<float>();
            }
        }
        
        /// <summary>
        /// If the trigger is pressed or not.
        /// </summary>
        /// <returns></returns>
        public bool TriggerPressed {
            get {
                if (!triggerPressed.enabled) triggerPressed.Enable();
                return (int)triggerPressed.ReadValue<float>() == 1;
            }
        }
        
        /// <summary>
        /// How far the grip is depressed.
        /// </summary>
        /// <returns></returns>
        public float GripDepress {
            get {
                if (!gripDepress.enabled) gripDepress.Enable();
                return (float)gripDepress?.ReadValue<float>();  
            }
        }
        
        /// <summary>
        /// If the grip is pressed or not.
        /// </summary>
        /// <returns></returns>
        public bool GripPressed {
            get {
                if (!gripPressed.enabled) gripPressed.Enable();
                return (int)gripPressed.ReadValue<float>() == 1;
            }
        }
        
        /// <summary>
        /// The position of the joystick.
        /// </summary>
        /// <returns></returns>
        public Vector2 JoystickPosition {
            get {
                if (!joystickPosition.enabled) joystickPosition.Enable();
                return (Vector2)joystickPosition?.ReadValue<Vector2>(); 
            }
        }
        
        /// <summary>
        /// If the joystick is pressed or not.
        /// </summary>
        /// <returns></returns>
        public bool JoystickPressed {
            get {
                if (!joystickPressed.enabled) joystickPressed.Enable();
                return (int)joystickPressed.ReadValue<float>() == 1; 
            }
        }
        
        /// <summary>
        /// If the joystick is touched or not.
        /// </summary>
        /// <returns></returns>
        public bool JoystickTouched {
            get {
                if (!joystickTouched.enabled) joystickTouched.Enable();
                return (int)joystickTouched.ReadValue<float>() == 1; 
            }
        }
        
        /// <summary>
        /// If the primary button is pressed or not.
        /// </summary>
        /// <returns></returns>
        public bool PrimaryButtonPressed {
            get {
                if (!primaryButtonPressed.enabled) primaryButtonPressed.Enable();
                return (int)primaryButtonPressed.ReadValue<float>() == 1; 
            }
        }
        
        /// <summary>
        /// If the primary button is touched or not.
        /// </summary>
        /// <returns></returns>
        public bool PrimaryButtonTouched {
            get {
                if (!primaryButtonTouched.enabled) primaryButtonTouched.Enable();
                return (int)primaryButtonTouched.ReadValue<float>() == 1;
            }
        }
        
        /// <summary>
        /// If the secondary button is pressed or not.
        /// </summary>
        /// <returns></returns>
        public bool SecondaryButtonPressed {
            get {
                if (!secondaryButtonPressed.enabled) secondaryButtonPressed.Enable();
                return (int)secondaryButtonPressed.ReadValue<float>() == 1;  
            }
        }
        
        /// <summary>
        /// If the secondary button is touched or not.
        /// </summary>
        /// <returns></returns>
        public bool SecondaryButtonTouched {
            get {
                if (!secondaryButtonTouched.enabled) secondaryButtonTouched.Enable();
                return (int)secondaryButtonTouched.ReadValue<float>() == 1; 
            }
        }
        
        /// <summary>
        /// If the thumbrest is touched or not.
        /// </summary>
        /// <returns></returns>
        public bool ThumbrestTouched {
            get {
                if (!thumbrestTouched.enabled) thumbrestTouched.Enable();
                return (int)thumbrestTouched.ReadValue<float>() == 1;  
            }
        }
        
        /// <summary>
        /// Disables all inputs referenced on this script.
        /// </summary>
        public void DisableInputs() {
            triggerDepress.Disable();
            triggerPressed.Disable();
            
            gripDepress.Disable();
            gripPressed.Disable();
            
            joystickPosition.Disable();
            joystickPressed.Disable();
            joystickTouched.Disable();
            
            primaryButtonPressed.Disable();
            primaryButtonTouched.Disable();
            
            secondaryButtonPressed.Disable();
            secondaryButtonTouched.Disable();
            
            thumbrestTouched.Disable();
        }
    }
}
