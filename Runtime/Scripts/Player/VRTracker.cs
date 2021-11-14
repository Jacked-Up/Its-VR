// This script was updated on 11/14/2021 by Jack Randolph.

using UnityEngine;
using UnityEngine.InputSystem;

namespace ItsVR.Player {
    /// <summary>
    /// Copies the real-world trackers position and rotation into the virtual world.
    /// </summary>
    [DisallowMultipleComponent]
    [HelpURL("https://jackedupstudios.com/vr-tracker")]
    [AddComponentMenu("It's VR/Player/VR Tracker")]
    public class VRTracker : MonoBehaviour {
        #region Variables

        /// <summary>
        /// Tracker container which contains the input bindings of the tracker.
        /// </summary>
        [Tooltip("Tracker container which contains the input bindings of the tracker.")]
        public Scriptables.VRTrackerContainer trackerContainer;

        /// <summary>
        /// Movement constraints of the object.
        /// </summary>
        [Tooltip("Movement constraints of the object.")]
        public TrackingConstraints trackingConstraint = TrackingConstraints.PositionAndRotation;
        
        /// <summary>
        /// The frequency at which the tracker updates.
        /// </summary>
        [Tooltip("The frequency at which the tracker updates.")]
        public UpdateModes updateMode = UpdateModes.UpdateAndBeforeRender;

        /// <summary>
        /// The position offset of the tracker.
        /// </summary>
        [HideInInspector]
        public Vector3 positionOffset;

        /// <summary>
        /// The rotation offset of the tracker.
        /// </summary>
        [HideInInspector]
        public Quaternion rotationOffset;

        /// <summary>
        /// If the tracker is being tracked. 
        /// </summary>
        public bool IsTracking => trackerContainer != null && trackerContainer.IsTracking;

        /// <summary>
        /// If the tracker is paused.
        /// </summary>
        public bool IsPaused { get; private set; }

        /// <summary>
        /// The speed of the tracker in world space.
        /// </summary>
        public float WorldSpeed => ItsMath.Velocity(transform.position, _lastWorldPosition).magnitude;
        
        /// <summary>
        /// The speed of the tracker in local space.
        /// </summary>
        public float LocalSpeed => ItsMath.Velocity(transform.localPosition, _lastLocalPosition).magnitude;
        
        /// <summary>
        /// The velocity of the tracker in world space.
        /// </summary>
        public Vector3 WorldVelocity => ItsMath.Velocity(transform.position, _lastWorldPosition);

        /// <summary>
        /// The velocity of the tracker in local space.
        /// </summary>
        public Vector3 LocalVelocity => ItsMath.Velocity(transform.localPosition, _lastLocalPosition);
        
        /// <summary>
        /// The angular velocity of the tracker in world space.
        /// </summary>
        public Vector3 WorldAngularVelocity => ItsMath.AngularVelocity(transform.rotation, _lastWorldRotation);

        /// <summary>
        /// The angular velocity of the tracker in local space.
        /// </summary>
        public Vector3 LocalAngularVelocity => ItsMath.AngularVelocity(transform.localRotation, _lastLocalRotation);
        
        /// <summary>
        /// Invoked when the tracker is paused.
        /// </summary>
        public event VRTrackerEvent TrackerPaused;
        
        /// <summary>
        /// Invoked when the tracker is resumed.
        /// </summary>
        public event VRTrackerEvent TrackerResumed;

        /// <summary>
        /// Invoked everytime the tracker updates/tracks.
        /// </summary>
        public event VRTrackerEvent TrackerUpdated;
        
        public enum UpdateModes { UpdateAndBeforeRender, Update, BeforeRender }
        public enum TrackingConstraints { PositionAndRotation, RotationOnly, PositionOnly }
        public delegate void VRTrackerEvent();
        private Vector3 _lastWorldPosition;
        private Vector3 _lastLocalPosition;
        private Quaternion _lastWorldRotation;
        private Quaternion _lastLocalRotation;

        #endregion
        
        private void OnEnable() {
            if (trackerContainer == null) 
                Debug.LogError("[VR Tracker] No tracker reference was referenced.", this);

            if (updateMode == UpdateModes.BeforeRender || updateMode == UpdateModes.UpdateAndBeforeRender)
                InputSystem.onAfterUpdate += Track;
        }

        private void OnDisable() {
            InputSystem.onAfterUpdate -= Track;
            
            if (trackerContainer != null)
                trackerContainer.DisableInputs();
        }

        private void Update() {
            if (updateMode == UpdateModes.Update || updateMode == UpdateModes.UpdateAndBeforeRender)
                Track();
        }

        private void FixedUpdate() {
            var self = transform;
            _lastWorldPosition = self.position;
            _lastLocalPosition = self.localPosition;
            _lastWorldRotation = self.rotation;
            _lastLocalRotation = self.localRotation;
        }

        /// <summary>
        /// Syncs the objects position and rotation with the trackers position and rotation.
        /// </summary>
        public void Track() {
            if (trackerContainer == null || !IsTracking || IsPaused) return;
            
            var self = transform;
            
            if (trackingConstraint == TrackingConstraints.PositionOnly || trackingConstraint == TrackingConstraints.PositionAndRotation)
                self.localPosition = trackerContainer.TrackerPosition + positionOffset;
            
            if (trackingConstraint == TrackingConstraints.RotationOnly || trackingConstraint == TrackingConstraints.PositionAndRotation)
                self.localRotation = Quaternion.Euler(trackerContainer.TrackerRotation.eulerAngles + rotationOffset.eulerAngles);
            
            TrackerUpdated?.Invoke();
        }

        /// <summary>
        /// Pauses the VR tracker. 
        /// </summary>
        public void Pause() {
            IsPaused = true;
            TrackerPaused?.Invoke();
        }

        /// <summary>
        /// Resumes the VR tracker.
        /// </summary>
        public void Resume() {
            IsPaused = false;
            TrackerResumed?.Invoke();
        }
    }
}
