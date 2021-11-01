// This script was updated on 10/30/2021 by Jack Randolph.

using UnityEngine;
using UnityEngine.InputSystem;

namespace ItsVR.Player {
    [DisallowMultipleComponent]
    [HelpURL("https://jackedupstudios.com/vr-tracker")]
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
        /// If true, the tracker will draw a line to display its motion and speed. Note: Only draws in the editor.
        /// </summary>
        [SerializeField] [Tooltip("If true, the tracker will draw a line to display its motion and speed. Note: Only draws in the editor.")]
        private bool drawTrackerMotion;
        
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
        private Vector3 _lastWorldPosition;
        private Vector3 _lastLocalPosition;
        private Quaternion _lastWorldRotation;
        private Quaternion _lastLocalRotation;

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

#if UNITY_EDITOR
            if (!drawTrackerMotion) return;
            
            Color color;
            if (WorldSpeed > 5f) {
                color = Color.red;
            }
            else if (WorldSpeed > 2f) {
                color = Color.yellow;
            }
            else {
                color = Color.green;
            }
            
            Debug.DrawLine(_lastWorldPosition, transform.position, color, 5);
#endif
        }

        private void LateUpdate() {
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
            if (trackerReference == null || !IsTracking || IsPaused) return;
            
            var self = transform;
            
            if (trackingConstraint == TrackingConstraints.PositionOnly || trackingConstraint == TrackingConstraints.PositionAndRotation)
                self.localPosition = trackerReference.TrackerPosition + positionOffset;
            
            if (trackingConstraint == TrackingConstraints.RotationOnly || trackingConstraint == TrackingConstraints.PositionAndRotation)
                self.localRotation = Quaternion.Euler(trackerReference.TrackerRotation.eulerAngles + rotationOffset.eulerAngles);
            
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

        /// <summary>
        /// The speed of the tracker in world space.
        /// </summary>
        public float WorldSpeed => (transform.position - _lastWorldPosition).magnitude / Time.deltaTime;
        
        /// <summary>
        /// The speed of the tracker in local space.
        /// </summary>
        public float LocalSpeed => (transform.localPosition - _lastLocalPosition).magnitude / Time.deltaTime;
        
        /// <summary>
        /// The velocity of the tracker in world space.
        /// </summary>
        public Vector3 WorldVelocity => (transform.position - _lastWorldPosition) / Time.deltaTime;

        /// <summary>
        /// The velocity of the tracker in local space.
        /// </summary>
        public Vector3 LocalVelocity => (transform.localPosition - _lastLocalPosition) / Time.deltaTime;
        
        /// <summary>
        /// The angular velocity of the tracker in world space.
        /// </summary>
        public Vector3 WorldAngularVelocity => (transform.rotation.eulerAngles - _lastWorldRotation.eulerAngles) / Time.deltaTime;

        /// <summary>
        /// The angular velocity of the tracker in local space.
        /// </summary>
        public Vector3 LocalAngularVelocity => (transform.localRotation.eulerAngles - _lastLocalRotation.eulerAngles) / Time.deltaTime;
    }
}
