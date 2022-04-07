using ItsVR.Core;
using ItsVR.Editor;
using ItsVR.Scriptables;
using UnityEngine;

namespace ItsVR.Player {
    /// <summary>
    /// Converts the real-world position and rotation of the tracker device into Unity.
    /// </summary>
    /// <para>Author: Jack Randolph</para>
    [DisallowMultipleComponent]
    [AddComponentMenu("It's VR/VR Tracker")]
    [HelpURL("https://jackedupstudios.com/vr-tracker")]
    public class VRTracker : MonoBehaviour, IItsVRProblemDebugable {
        #region Variables

        /// <summary>
        /// VR Tracker container which contains the input bindings of the tracker.
        /// </summary>
        [Tooltip("VR Tracker container which contains the input bindings of the tracker.")]
        public VRTrackerContainer trackerContainer;

        /// <summary>
        /// Motion constraints the VR tracker will follow.
        /// </summary>
        [Tooltip("Motion constraints the VR tracker will follow.")]
        public TrackingConstraints trackingConstraint = TrackingConstraints.PositionAndRotation;
        
        public enum TrackingConstraints { PositionAndRotation, RotationOnly, PositionOnly }

        /// <summary>
        /// If the tracker is being tracked. 
        /// </summary>
        public bool IsTracking => trackerContainer != null && trackerContainer.IsTracking;

        /// <summary>
        /// If the tracker is paused or not.
        /// </summary>
        public bool IsPaused {
            get => _isPaused;
            private set {
                if (value) 
                    TrackerPaused?.Invoke();
                else
                    TrackerResumed?.Invoke();
                
                _isPaused = value;
            }
        }
        private bool _isPaused;
        
        /// <summary>
        /// The speed of the tracker.
        /// </summary>
        public float Speed => ItsMath.Velocity(_currentFramePosition, _lastFramePosition).magnitude;

        /// <summary>
        /// The velocity of the tracker.
        /// </summary>
        public Vector3 Velocity => ItsMath.Velocity(_currentFramePosition, _lastFramePosition);

        /// <summary>
        /// The angular velocity of the tracker.
        /// </summary>
        public Vector3 AngularVelocity => ItsMath.AngularVelocity(_currentFrameRotation, _lastFrameRotation);
        
        /// <summary>
        /// Invoked when the tracker is paused.
        /// </summary>
        public event VRTrackerCallback TrackerPaused;
        
        /// <summary>
        /// Invoked when the tracker is resumed.
        /// </summary>
        public event VRTrackerCallback TrackerResumed;

        public delegate void VRTrackerCallback();

        private Vector3 _currentFramePosition;
        private Quaternion _currentFrameRotation;
        private Vector3 _lastFramePosition;
        private Quaternion _lastFrameRotation;

        #endregion

        private void OnEnable() {
            if (trackerContainer != null) {
                trackerContainer.OnPositionChanged += OnPositionChangedCallback;
                trackerContainer.OnRotationChanged += OnRotationChangedCallback;
                
                if (!trackerContainer.AllActionsRegistered)
                    trackerContainer.EnableActions();
            }

#if UNITY_EDITOR
            if (trackerContainer == null) 
                Debug.LogError("The VR Tracker requires a tracker container to be assigned.", this);
#endif
        }

        private void OnDisable() {
            if (trackerContainer != null) {
                trackerContainer.OnPositionChanged -= OnPositionChangedCallback;
                trackerContainer.OnRotationChanged -= OnRotationChangedCallback;
                trackerContainer.DisableActions();
            }
        }

        private void Update() {
            _currentFramePosition = transform.position;
            _currentFrameRotation = transform.rotation;
        }

        private void LateUpdate() {
            _lastFramePosition = transform.position;
            _lastFrameRotation = transform.rotation;
        }

        private void OnPositionChangedCallback(Vector3 trackerPosition) {
            if (!IsPaused && trackingConstraint == TrackingConstraints.PositionOnly || trackingConstraint == TrackingConstraints.PositionAndRotation)
                transform.localPosition = trackerContainer.Position;
        }

        private void OnRotationChangedCallback(Quaternion trackerRotation) {
            if (!IsPaused && trackingConstraint == TrackingConstraints.RotationOnly || trackingConstraint == TrackingConstraints.PositionAndRotation)
                transform.localRotation = trackerContainer.Rotation;
        }

#if UNITY_EDITOR
        public void RefreshProblems() {
            if (trackerContainer == null)
                ItsVRProblemDebuggerEditor.SubmitProblem("This VR Tracker requires a VR tracker container to be assigned.", ItsVRProblemDebuggerEditor.ProblemLevels.Error, transform);
            
            if (trackerContainer != null && trackerContainer.updateFrequency != UpdateFrequencies.BeforeUpdateAndOnUpdate)
                ItsVRProblemDebuggerEditor.SubmitProblem($"{trackerContainer.name}s update frequency should be set to 'BeforeUpdateAndOnUpdate' to prevent tracker lag.", ItsVRProblemDebuggerEditor.ProblemLevels.Recommendation, transform);
            
            if (GetComponentInParent<VRRig>() && GetComponentInParent<VRRig>().bodyOffset != null && !transform.IsChildOf(GetComponentInParent<VRRig>().bodyOffset))
                ItsVRProblemDebuggerEditor.SubmitProblem("This VR tracker is not a child of the VR rigs body offset.", ItsVRProblemDebuggerEditor.ProblemLevels.Issue, transform);
        }
#endif
    }
}
