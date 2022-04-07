// This script was updated on 03/23/2022 by Jack Randolph.

using System;
using ItsVR.Core;
using ItsVR.Editor;
using ItsVR.Scriptables;
using UnityEngine;
using UnityEngine.Events;

namespace ItsVR.Player {
    /// <summary>
    /// The VR controller is a system for communicating and controlling the real-world VR controller.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(VRTracker))]
    [AddComponentMenu("It's VR/VR Controller")]
    [HelpURL("https://jackedupstudios.com/vr-controller")]
    public class VRController : MonoBehaviour, IItsVRProblemDebugable {
        #region Variables
        
        /// <summary>
        /// The VR input container which contains all the input bindings for this VR controller.
        /// </summary>
        [Tooltip("The VR input container which contains all the input bindings for this VR controller.")]
        public VRInputContainer inputContainer;

        /// <summary>
        /// The hand which this controller is defined as.
        /// </summary>
        [Tooltip("The hand which this controller is defined as.")]
        public HandDefinitions handDefinition;

        /// <summary>
        /// The VR tracker which tracks this VR controller.
        /// </summary>
        [HideInInspector]
        public VRTracker vrTracker;
        
        /// <summary>
        /// Unity event which is invoked when the trigger is pressed.
        /// </summary>
        [Tooltip("Unity event which is invoked when the trigger is pressed.")]
        public UnityEvent onTriggerPressed;
        
        /// <summary>
        /// Unity event which is invoked when the trigger depress changes.
        /// </summary>
        [Tooltip("Unity event which is invoked when the trigger depress changes.")]
        public UnityFloatEvent onTriggerDepressChanged;
        
        /// <summary>
        /// Unity event which is invoked when the grip is pressed.
        /// </summary>
        [Tooltip("Unity event which is invoked when the grip is pressed.")]
        public UnityEvent onGripPressed;
        
        /// <summary>
        /// Unity event which is invoked when the grip depress changes.
        /// </summary>
        [Tooltip("Unity event which is invoked when the grip depress changes.")]
        public UnityFloatEvent onGripDepressChanged;
        
        /// <summary>
        /// Unity event which is invoked when the primary button is pressed.
        /// </summary>
        [Tooltip("Unity event which is invoked when the primary button is pressed.")]
        public UnityEvent onPrimaryButtonPressed;
        
        /// <summary>
        /// Unity event which is invoked when the secondary button is pressed.
        /// </summary>
        [Tooltip("Unity event which is invoked when the secondary button is pressed.")]
        public UnityEvent onSecondaryButtonPressed;
        
        /// <summary>
        /// Unity event which is invoked when the joystick is pressed.
        /// </summary>
        [Tooltip("Unity event which is invoked when the joystick is pressed.")]
        public UnityEvent onJoystickPressed;
        
        /// <summary>
        /// Unity event which is invoked when the joystick position changes.
        /// </summary>
        [Tooltip("Unity event which is invoked when the joystick position changes.")]
        public UnityVectorTwoEvent onJoystickPositionChanged;

        #endregion

        private void OnEnable() {
            vrTracker = GetComponent<VRTracker>();
            
            transform.localPosition = Vector3.zero;
            
            if (inputContainer != null) {
                inputContainer.universal.OnTriggerPressed += OnTriggerPressedCallback;
                inputContainer.universal.OnTriggerDepressChanged += TriggerDepressChangedCallback;
                inputContainer.universal.OnGripPressed += OnGripPressedCallback;
                inputContainer.universal.OnGripDepressChanged += GripDepressChangedCallback;
                inputContainer.universal.OnPrimaryButtonPressed += OnPrimaryButtonPressedCallback;
                inputContainer.universal.OnSecondaryButtonPressed += OnSecondaryButtonPressedCallback;
                inputContainer.universal.OnJoystickPressed += OnJoystickPressedCallback;
                inputContainer.universal.OnJoystickPositionChanged += JoystickPositionChangedCallback;
            }
            
#if UNITY_EDITOR
            if (inputContainer == null)
                Debug.LogError("A VR input container is not assigned to the VR controller.", this);
            
            if (vrTracker == null)
                Debug.LogError("Add a VR tracker component to the same object as this VR controller.", this);
#endif
        }

        private void OnDisable() {
            transform.localPosition = Vector3.zero;
            
            if (inputContainer != null) {
                inputContainer.universal.OnTriggerPressed -= OnTriggerPressedCallback;
                inputContainer.universal.OnTriggerDepressChanged -= TriggerDepressChangedCallback;
                inputContainer.universal.OnGripPressed -= OnGripPressedCallback;
                inputContainer.universal.OnGripDepressChanged -= GripDepressChangedCallback;
                inputContainer.universal.OnPrimaryButtonPressed -= OnPrimaryButtonPressedCallback;
                inputContainer.universal.OnSecondaryButtonPressed -= OnSecondaryButtonPressedCallback;
                inputContainer.universal.OnJoystickPressed -= OnJoystickPressedCallback;
                inputContainer.universal.OnJoystickPositionChanged -= JoystickPositionChangedCallback;
            }
        }

        private void OnTriggerPressedCallback() => onTriggerPressed.Invoke();
        
        private void TriggerDepressChangedCallback() => onTriggerDepressChanged.Invoke(inputContainer.universal.TriggerDepress);
        
        private void OnGripPressedCallback() => onGripPressed.Invoke();

        private void GripDepressChangedCallback() => onGripDepressChanged.Invoke(inputContainer.universal.GripDepress);
        
        private void OnPrimaryButtonPressedCallback() => onPrimaryButtonPressed.Invoke();
        
        private void OnSecondaryButtonPressedCallback() => onSecondaryButtonPressed.Invoke();
       
        private void OnJoystickPressedCallback() => onJoystickPressed.Invoke();

        private void JoystickPositionChangedCallback() => onJoystickPositionChanged.Invoke(inputContainer.universal.JoystickPosition);

        /// <summary>
        /// Switches the VR controllers VR input controller to the new VR input container.
        /// </summary>
        /// <param name="newVRInputContainer">The VR input container to switch to.</param>
        public void SwitchVRInputContainer(VRInputContainer newVRInputContainer) {
            if (newVRInputContainer == null) {
#if UNITY_EDITOR
                Debug.LogError("You attempted to switch the VR input container to a null VR input container.");
#endif
                return;
            }
            
            if (inputContainer != null) {
                inputContainer.universal.OnTriggerPressed -= OnTriggerPressedCallback;
                inputContainer.universal.OnTriggerDepressChanged -= TriggerDepressChangedCallback;
                inputContainer.universal.OnGripPressed -= OnGripPressedCallback;
                inputContainer.universal.OnGripDepressChanged -= GripDepressChangedCallback;
                inputContainer.universal.OnPrimaryButtonPressed -= OnPrimaryButtonPressedCallback;
                inputContainer.universal.OnSecondaryButtonPressed -= OnSecondaryButtonPressedCallback;
                inputContainer.universal.OnJoystickPressed -= OnJoystickPressedCallback;
                inputContainer.universal.OnJoystickPositionChanged -= JoystickPositionChangedCallback;
            }
            
            newVRInputContainer.universal.OnTriggerPressed += OnTriggerPressedCallback;
            newVRInputContainer.universal.OnTriggerDepressChanged += TriggerDepressChangedCallback;
            newVRInputContainer.universal.OnGripPressed += OnGripPressedCallback;
            newVRInputContainer.universal.OnGripDepressChanged += GripDepressChangedCallback;
            newVRInputContainer.universal.OnPrimaryButtonPressed += OnPrimaryButtonPressedCallback;
            newVRInputContainer.universal.OnSecondaryButtonPressed += OnSecondaryButtonPressedCallback;
            newVRInputContainer.universal.OnJoystickPressed += OnJoystickPressedCallback;
            newVRInputContainer.universal.OnJoystickPositionChanged += JoystickPositionChangedCallback;
            inputContainer = newVRInputContainer;
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos() {
            if (Application.isPlaying || GetComponentInParent<VRRig>() == null)
                return;
            
            transform.localPosition = Vector3.Scale(new Vector3(handDefinition == HandDefinitions.Right ? 0.5f : -0.5f, -0.5f, 0.5f), transform.localScale);
            
            Gizmos.color = handDefinition == HandDefinitions.Right 
                ? Color.green 
                : Color.red;
            
            Gizmos.DrawLine(transform.position, GetComponentInParent<VRRig>().GetEstimatedHipPosition());
            
            if (GetComponentInChildren<MeshRenderer>() == null && GetComponentInChildren<SkinnedMeshRenderer>() == null)
                Gizmos.DrawWireCube(transform.position, Vector3.Scale(Vector3.one / 6f, transform.lossyScale));
        }

        public void RefreshProblems() {
            if (inputContainer == null)
                ItsVRProblemDebuggerEditor.SubmitProblem("A VR input container is not assigned to this VR controller.", ItsVRProblemDebuggerEditor.ProblemLevels.Error, transform);

            if (Application.isPlaying && vrTracker == null)
                ItsVRProblemDebuggerEditor.SubmitProblem("Add a VR tracker component to the same object as this VR controller.", ItsVRProblemDebuggerEditor.ProblemLevels.Error, transform);

            if (name.IndexOf("right", StringComparison.CurrentCultureIgnoreCase) >= 0 && handDefinition != HandDefinitions.Right)
                ItsVRProblemDebuggerEditor.SubmitProblem("The controller is named 'Right' but is not defined as a right controller.", ItsVRProblemDebuggerEditor.ProblemLevels.Recommendation, transform);
            else if (name.IndexOf("left", StringComparison.CurrentCultureIgnoreCase) >= 0 && handDefinition != HandDefinitions.Left)
                ItsVRProblemDebuggerEditor.SubmitProblem("The controller is named 'Left' but is not defined as a left controller.", ItsVRProblemDebuggerEditor.ProblemLevels.Recommendation, transform);
            
            if (!GetComponent<Collider>())
                ItsVRProblemDebuggerEditor.SubmitProblem("Add a collider the VR controller with 'Is Trigger' enabled.", ItsVRProblemDebuggerEditor.ProblemLevels.Recommendation, transform);
            
            if (transform == transform.root)
                ItsVRProblemDebuggerEditor.SubmitProblem("This VR controller is detected on a root transform. VR controllers should never be on the root transform. Was this intentional?", ItsVRProblemDebuggerEditor.ProblemLevels.Recommendation, transform);
        }
#endif
    }
    
    /// <summary>
    /// Unity event with a float parameter.
    /// </summary>
    [Serializable]
    public class UnityFloatEvent : UnityEvent<float> {}
    
    /// <summary>
    /// Unity event with a Vector2 parameter.
    /// </summary>
    [Serializable]
    public class UnityVectorTwoEvent : UnityEvent<Vector2> {}
}
