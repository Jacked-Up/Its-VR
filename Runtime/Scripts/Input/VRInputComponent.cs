// This script was updated on 11/13/2021 by Jack Randolph.

using ItsVR.Scriptables;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace ItsVR.Input {
    /// <summary>
    /// Contains unity events for all universal inputs.
    /// </summary>
    [DisallowMultipleComponent]
    [HelpURL("https://jackedupstudios.com/vr-input-component")]
    [AddComponentMenu("It's VR/Input/VR Input Component")]
    public class VRInputComponent : MonoBehaviour {
        #region Variables
        
        /// <summary>
        /// Input reference bindings this script will listen to.
        /// </summary>
        [FormerlySerializedAs("inputReference")] [Tooltip("Input reference bindings this script will listen to.")]
        public VRInputContainer inputContainer;

        /// <summary>
        /// All of the input reference unity events.
        /// </summary>
        [Tooltip("All of the input reference unity events.")]
        public InputEvents inputEvents;

        private bool _joystickEventAlreadyFired;
        private bool _alreadySubscribedToInputEvents;
        
        #endregion

        private void OnEnable() {
            if (inputContainer == null) {
                Debug.LogError("[VR Input Component] No VR input container was referenced.", this);
                return;
            }
            
            // Prevent the script from subscribing to the events more than once. 
            // This just saves some resources.
            if (_alreadySubscribedToInputEvents) return;
            _alreadySubscribedToInputEvents = true;
            
            inputContainer.universalInputs.RegisterEvents();
            inputContainer.universalInputs.OnTriggerPressed += TriggerPressed;
            inputContainer.universalInputs.OnGripPressed += GripPressed;
            inputContainer.universalInputs.OnJoystickPressed += JoystickPressed;
            inputContainer.universalInputs.OnPrimaryButtonPressed += PrimaryButtonPressed;
            inputContainer.universalInputs.OnPrimaryButtonTouched += PrimaryButtonTouched;
            inputContainer.universalInputs.OnSecondaryButtonPressed += SecondaryButtonPressed;
            inputContainer.universalInputs.OnSecondaryButtonTouched += SecondaryButtonTouched;
        }

        private void OnDisable() {
            DisableInputs();
        }

        private void Update() {
            // Bail if developer did not reference an input container.
            if (inputContainer == null) return;

            // Joystick input events logic.
            if (_joystickEventAlreadyFired) {
                if (inputContainer.universalInputs.JoystickPosition.y < 0.75f && inputContainer.universalInputs.JoystickPosition.y > -0.75f && inputContainer.universalInputs.JoystickPosition.x < 0.75f && inputContainer.universalInputs.JoystickPosition.x > -0.75f) 
                    _joystickEventAlreadyFired = false;
                
                return;
            }
            
            if (inputContainer.universalInputs.JoystickPosition.y > 0.75f) {
                inputEvents.joystick.joystickUp?.Invoke();
                _joystickEventAlreadyFired = true;
            }
            else if (inputContainer.universalInputs.JoystickPosition.y < -0.75f) {
                inputEvents.joystick.joystickDown?.Invoke();
                _joystickEventAlreadyFired = true;
            }
            else if (inputContainer.universalInputs.JoystickPosition.x > 0.75f) {
                inputEvents.joystick.joystickRight?.Invoke();
                _joystickEventAlreadyFired = true;
            }
            else if (inputContainer.universalInputs.JoystickPosition.x < -0.75f) {
                inputEvents.joystick.joystickLeft?.Invoke();
                _joystickEventAlreadyFired = true;
            }
        }

        /// <summary>
        /// Disables all inputs referenced on this script.
        /// </summary>
        public void DisableInputs() {
            if (inputContainer != null) 
                inputContainer.DisableAllInputs();
            else 
                Debug.LogError("[VR Input Component] Cannot disable inputs as no input reference was referenced.", this);
        }

        #region Input Events

        private void TriggerPressed() {
            inputEvents.trigger.triggerPressed.Invoke();
        }        
        
        private void GripPressed() {
            inputEvents.grip.gripPressed.Invoke();
        }

        private void JoystickPressed() {
            inputEvents.joystick.joystickPressed.Invoke();
        }

        private void PrimaryButtonPressed() {
            inputEvents.primaryButton.primaryButtonPressed.Invoke();
        }
        
        private void PrimaryButtonTouched() {
            inputEvents.primaryButton.primaryButtonTouched.Invoke();
        }
        
        private void SecondaryButtonPressed() {
            inputEvents.secondaryButton.secondaryButtonPressed.Invoke();
        }
        
        private void SecondaryButtonTouched() {
            inputEvents.secondaryButton.secondaryButtonTouched.Invoke();
        }

        #endregion
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
