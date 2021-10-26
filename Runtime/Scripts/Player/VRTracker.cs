// This script was updated on 10/26/2021 by Jack Randolph.
// Documentation: https://jackedupstudios.com/vr-tracker

using UnityEngine;
using UnityEngine.InputSystem;

namespace ItsVR.Player {
    [DisallowMultipleComponent]
    [AddComponentMenu("It's VR/Player/VR Tracker")]
    public class VRTracker : MonoBehaviour {
        #region Variables

        /// <summary>
        /// Tracker reference which contains the position and rotation bindings of the tracker.
        /// </summary>
        [Tooltip("Tracker reference which contains the position and rotation bindings of the tracker.")]
        public Scriptables.VRTrackerReferences trackerReference;

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
        public bool IsTracking => trackerReference != null && trackerReference.IsTracking;

        /// <summary>
        /// If the tracker is paused.
        /// </summary>
        public bool IsPaused { get; private set; }

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

        #endregion
        
        private void OnEnable() {
            if (trackerReference == null) 
                Debug.LogError("[VR Tracker] No tracker reference was referenced.", this);

            if (updateMode == UpdateModes.BeforeRender || updateMode == UpdateModes.UpdateAndBeforeRender) 
                InputSystem.onAfterUpdate += Track;
        }

        private void OnDisable() {
            InputSystem.onAfterUpdate -= Track;
            
            if (trackerReference != null)
                trackerReference.DisableInputs();
        }

        private void Update() {
            if (updateMode == UpdateModes.Update || updateMode == UpdateModes.UpdateAndBeforeRender)
                Track();
        }

        /// <summary>
        /// Syncs the objects position and rotation with the trackers position and rotation.
        /// </summary>
        public void Track() {
            if (trackerReference == null || !IsTracking || IsPaused) return;
            
            var self = transform;
            
            if (trackingConstraint == TrackingConstraints.PositionOnly || trackingConstraint == TrackingConstraints.PositionAndRotation)
                self.localPosition = trackerReference.TrackerPosition + positionOffset;
            
            // Warning! Doesn't apply the rotation to the object! (Unimplemented)
            // Need to use 'rotationOffset'.
            if (trackingConstraint == TrackingConstraints.RotationOnly || trackingConstraint == TrackingConstraints.PositionAndRotation)
               self.localRotation = trackerReference.TrackerRotation;
            
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
