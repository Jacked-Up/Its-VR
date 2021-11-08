// This script was updated on 10/30/2021 by Jack Randolph.

using UnityEngine;
using UnityEngine.InputSystem;

namespace ItsVR.Scriptables {
    [HelpURL("https://jackedupstudios.com/its-vr-documentation-1#b47abff1-8a0b-4eb6-b03c-3617ba2beb61")]
    [CreateAssetMenu(menuName = "It's VR/Input/VR Tracker Reference", fileName = "My VR Tracker")]
    public class VRTrackerReferences : ScriptableObject {
        #region Variables

        /// <summary>
        /// Input reference for trackers position.
        /// </summary>
        [Tooltip("Input reference for trackers position.")]
        public InputAction trackerPosition;

        /// <summary>
        /// Input reference for trackers rotation.
        /// </summary>
        [Tooltip("Input reference for trackers rotation.")]
        public InputAction trackerRotation;

        /// <summary>
        /// Input reference for receiving the trackers status.
        /// </summary>
        [Tooltip("Input reference for receiving the trackers status.")]
        public InputAction trackingStatus;

        #endregion
        
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
        /// If the tracker is being tracked.
        /// </summary>
        public bool IsTracking {
            get {
                if (!trackingStatus.enabled) trackingStatus?.Enable();
                return (int)trackingStatus?.ReadValue<float>() == 1;
            }
        }
        
        /// <summary>
        /// Disables all inputs referenced on this script.
        /// </summary>
        public void DisableInputs() {
            trackerPosition?.Disable();
            trackerRotation?.Disable();
            trackingStatus?.Disable();
        }
    }
}
