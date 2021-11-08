// This script was updated on 11/4/2021 by Jack Randolph.

using ItsVR.Scriptables;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ItsVR.Input {
    [DisallowMultipleComponent]
    [HelpURL("https://jackedupstudios.com/vr-input-component")]
    [AddComponentMenu("It's VR/Input/VR Input Component")]
    public class VRInputComponent : MonoBehaviour {
        #region Variables
        
        /// <summary>
        /// Input reference bindings this script will listen to.
        /// </summary>
        [Tooltip("Input reference bindings this script will listen to.")]
        public VRInputReferences inputReference;

        /// <summary>
        /// All of the input reference unity events.
        /// </summary>
        [Tooltip("All of the input reference unity events.")]
        public InputEvents inputEvents;

        private bool _joystickEventAlreadyFired;
        private bool _alreadySubscribedToInputEvents;
        
        #endregion

        private void OnEnable() {
            if (inputReference == null) 
                Debug.LogError("[VR Input Component] No VR Input Reference was referenced.", this);
            
            // Prevent the script from subscribing to the events more than once. 
            // This just saves some resources.
            if (_alreadySubscribedToInputEvents) return;
            _alreadySubscribedToInputEvents = true;
            
            inputReference.universalInputs.triggerPressed.performed += TriggerPressed;
            inputReference.universalInputs.gripPressed.performed += GripPressed;
            inputReference.universalInputs.joystickPressed.performed += JoystickPressed;
            inputReference.universalInputs.joystickTouched.performed += JoystickTouched;
            inputReference.universalInputs.primaryButtonPressed.performed += PrimaryButtonPressed;
            inputReference.universalInputs.primaryButtonTouched.performed += PrimaryButtonTouched;
            inputReference.universalInputs.secondaryButtonPressed.performed += SecondaryButtonPressed;
            inputReference.universalInputs.secondaryButtonPressed.performed += SecondaryButtonTouched;
        }

        private void OnDisable() {
            DisableInputs();
        }

        private void Update() {
            // Bail if developer did not reference an input reference.
            if (inputReference == null) return;

            // Joystick input events logic.
            if (_joystickEventAlreadyFired) {
                if (inputReference.universalInputs.JoystickPosition.y < 0.75f && inputReference.universalInputs.JoystickPosition.y > -0.75f && inputReference.universalInputs.JoystickPosition.x < 0.75f && inputReference.universalInputs.JoystickPosition.x > -0.75f) 
                    _joystickEventAlreadyFired = false;
                
                return;
            }
            
            if (inputReference.universalInputs.JoystickPosition.y > 0.75f) {
                inputEvents.joystick.joystickUp?.Invoke();
                _joystickEventAlreadyFired = true;
            }
            else if (inputReference.universalInputs.JoystickPosition.y < -0.75f) {
                inputEvents.joystick.joystickDown?.Invoke();
                _joystickEventAlreadyFired = true;
            }
            else if (inputReference.universalInputs.JoystickPosition.x > 0.75f) {
                inputEvents.joystick.joystickRight?.Invoke();
                _joystickEventAlreadyFired = true;
            }
            else if (inputReference.universalInputs.JoystickPosition.x < -0.75f) {
                inputEvents.joystick.joystickLeft?.Invoke();
                _joystickEventAlreadyFired = true;
            }
        }

        /// <summary>
        /// Disables all inputs referenced on this script.
        /// </summary>
        public void DisableInputs() {
            if (inputReference != null) 
                inputReference.DisableAllInputs();
            else 
                Debug.LogError("[VR Input Component] Cannot disable inputs as no input reference was referenced.", this);
        }
        
        private void TriggerPressed(InputAction.CallbackContext callbackContext) {
            inputEvents.trigger.triggerPressed.Invoke();
        }        
        
        private void GripPressed(InputAction.CallbackContext callbackContext) {
            inputEvents.grip.gripPressed.Invoke();
        }

        private void JoystickPressed(InputAction.CallbackContext callbackContext) {
            inputEvents.joystick.joystickPressed.Invoke();
        }
        
        private void JoystickTouched(InputAction.CallbackContext callbackContext) {
            inputEvents.joystick.joystickTouched.Invoke();
        }
        
        private void PrimaryButtonPressed(InputAction.CallbackContext callbackContext) {
            inputEvents.primaryButton.primaryButtonPressed.Invoke();
        }
        
        private void PrimaryButtonTouched(InputAction.CallbackContext callbackContext) {
            inputEvents.primaryButton.primaryButtonTouched.Invoke();
        }
        
        private void SecondaryButtonPressed(InputAction.CallbackContext callbackContext) {
            inputEvents.secondaryButton.secondaryButtonPressed.Invoke();
        }
        
        private void SecondaryButtonTouched(InputAction.CallbackContext callbackContext) {
            inputEvents.secondaryButton.secondaryButtonTouched.Invoke();
        }
    }
    
    [System.Serializable]
    public class InputEvents {
        /// <summary>
        /// All the trigger button events.
        /// </summary>
        [Tooltip("All the trigger button events.")]
        public TriggerInputEvents trigger;

        /// <summary>
        /// All the grip button events.
        /// </summary>
        [Tooltip("All the grip button events.")]
        public GripInputEvents grip;

        /// <summary>
        /// All the joystick button events.
        /// </summary>
        [Tooltip("All joystick events.")]
        public JoystickInputEvents joystick;

        /// <summary>
        /// All the primary button events.
        /// </summary>
        [Tooltip("All the primary button events.")]
        public PrimaryButtonInputEvents primaryButton;

        /// <summary>
        /// All the secondary button events.
        /// </summary>
        [Tooltip("All the secondary button events.")]
        public SecondaryButtonInputEvents secondaryButton;
    }

    [System.Serializable]
    public class TriggerInputEvents {
        /// <summary>
        /// Invoked when the trigger is pressed down.
        /// </summary>
        [Tooltip("Invoked when the trigger is pressed down.")]
        public UnityEvent triggerPressed;
    }
    
    [System.Serializable]
    public class GripInputEvents {
        /// <summary>
        /// Invoked when the grip is pressed down.
        /// </summary>
        [Tooltip("Invoked when the grip is pressed down.")]
        public UnityEvent gripPressed;
    }
    
    [System.Serializable]
    public class JoystickInputEvents {
        /// <summary>
        /// Invoked when the joystick is pressed down.
        /// </summary>
        [Tooltip("Invoked when the joystick is pressed down.")]
        public UnityEvent joystickPressed;
        
        /// <summary>
        /// Invoked when the joystick is touched.
        /// </summary>
        [Tooltip("Invoked when the joystick is touched.")]
        public UnityEvent joystickTouched;
        
        /// <summary>
        /// Invoked when the joystick is pushed up.
        /// </summary>
        [Tooltip("Invoked when the joystick is pushed up.")]
        public UnityEvent joystickUp;
        
        /// <summary>
        /// Invoked when the joystick is pushed down.
        /// </summary>
        [Tooltip("Invoked when the joystick is pushed down.")]
        public UnityEvent joystickDown;
        
        /// <summary>
        /// Invoked when the joystick is pushed right.
        /// </summary>
        [Tooltip("Invoked when the joystick is pushed right.")]
        public UnityEvent joystickRight;
        
        /// <summary>
        /// Invoked when the joystick is pushed left.
        /// </summary>
        [Tooltip("Invoked when the joystick is pushed left.")]
        public UnityEvent joystickLeft;
    }
    
    [System.Serializable]
    public class PrimaryButtonInputEvents {
        /// <summary>
        /// Invoked when the primary button is pressed down.
        /// </summary>
        [Tooltip("Invoked when the primary button is pressed down.")]
        public UnityEvent primaryButtonPressed;
        
        /// <summary>
        /// Invoked when the primary button is touched.
        /// </summary>
        [Tooltip("Invoked when the primary button is touched.")]
        public UnityEvent primaryButtonTouched;
    }
    
    [System.Serializable]
    public class SecondaryButtonInputEvents {
        /// <summary>
        /// Invoked when the secondary button is pressed down.
        /// </summary>
        [Tooltip("Invoked when the secondary button is pressed down.")]
        public UnityEvent secondaryButtonPressed;
        
        /// <summary>
        /// Invoked when the secondary button is touched.
        /// </summary>
        [Tooltip("Invoked when the secondary button is touched.")]
        public UnityEvent secondaryButtonTouched;
    }
}
