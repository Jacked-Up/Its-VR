// This script was updated on 11/12/2021 by Jack Randolph.

using UnityEngine;
using UnityEngine.InputSystem;

namespace ItsVR.Scriptables {
    /// <summary>
    /// The VR tracker container contains references to tracker position, rotation, and tracking state. This class
    /// also automatically enables the bindings and has properties for each binding. (Ex: Vector3 - Position)
    /// </summary>
    [HelpURL("https://jackedupstudios.com/its-vr-documentation-1#b47abff1-8a0b-4eb6-b03c-3617ba2beb61")]
    [CreateAssetMenu(menuName = "It's VR/Input/VR Tracker Container", fileName = "My VR Tracker Container")]
    public class VRTrackerContainer : ScriptableObject {
        /// <summary>
        /// Input reference for trackers position.
        /// </summary>
        [SerializeField] [Tooltip("Input reference for trackers position.")]
        private InputAction trackerPosition;

        /// <summary>
        /// Input reference for trackers rotation.
        /// </summary>
        [SerializeField] [Tooltip("Input reference for trackers rotation.")]
        private InputAction trackerRotation;

        /// <summary>
        /// Input reference for receiving the trackers status.
        /// </summary>
        [SerializeField] [Tooltip("Input reference for receiving the trackers status.")]
        private InputAction trackingStatus;

        /// <summary>
        /// The position of the tracker.
        /// </summary>
        /// <returns></returns>
        public Vector3 TrackerPosition {
            get {
                if (!trackerPosition.enabled) trackerPosition?.Enable();
                return (Vector3)trackerPosition?.ReadValue<Vector3>();
            }
        }

        /// <summary>
        /// Invoked when the tracker position changes.
        /// </summary>
        public event TrackerEvent OnTrackerPositionChanged;
        private void TrackerPositionPerformed(InputAction.CallbackContext a) { OnTrackerPositionChanged?.Invoke(); }
        
        /// <summary>
        /// The rotation of the tracker.
        /// </summary>
        /// <returns></returns>
        public Quaternion TrackerRotation {
            get {
                if (!trackerRotation.enabled) trackerRotation?.Enable();
                return (Quaternion)trackerRotation?.ReadValue<Quaternion>();
            }
        }

        /// <summary>
        /// Invoked when the tracker rotation changes.
        /// </summary>
        public event TrackerEvent OnTrackerRotationChanged;
        private void TrackerRotationPerformed(InputAction.CallbackContext a) { OnTrackerRotationChanged?.Invoke(); }
        
        /// <summary>
        /// If the tracker is being tracked.
        /// </summary>
        public bool IsTracking {
            get {
                if (!trackingStatus.enabled) trackingStatus?.Enable();
                return (int)trackingStatus?.ReadValue<float>() == 1;
            }
        }
        
        /// <summary>
        /// Invoked when the tracking status changes.
        /// </summary>
        public event TrackerEvent OnTrackerStatusChanged;
        private void TrackerStatusPerformed(InputAction.CallbackContext a) { OnTrackerStatusChanged?.Invoke(); }

        /// <summary>
        /// Registers all input events references in this container.
        /// </summary>
        public void RegisterEvents() {
            trackerPosition?.Enable();
            if (trackerPosition != null) trackerPosition.performed += TrackerPositionPerformed;
            trackerRotation?.Enable();
            if (trackerRotation != null) trackerRotation.performed += TrackerRotationPerformed;

            trackingStatus?.Enable();
            if (trackingStatus != null) trackingStatus.performed += TrackerStatusPerformed;
        }
        
        /// <summary>
        /// Disables every input referenced in this container.
        /// </summary>
        public void DisableInputs() {
            trackerPosition?.Disable();
            if (trackerPosition != null) trackerPosition.performed -= TrackerPositionPerformed;
            trackerRotation?.Disable();
            if (trackerRotation != null) trackerRotation.performed -= TrackerRotationPerformed;

            trackingStatus?.Disable();
            if (trackingStatus != null) trackingStatus.performed -= TrackerStatusPerformed;
        }

        public delegate void TrackerEvent();
    }
}
