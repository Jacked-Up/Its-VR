using ItsVR.Core;
using ItsVR.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItsVR.Scriptables {
    /// <summary>
    /// The VR tracker container manages the input from a virtual reality tracking device. The container will
    /// automatically manage the enable states of the input actions.
    /// </summary>
    /// <para>Author: Jack Randolph</para>
    [HelpURL("https://jackedupstudios.com/its-vr-documentation-1#b47abff1-8a0b-4eb6-b03c-3617ba2beb61")]
    [CreateAssetMenu(menuName = "It's VR/Input/VR Tracker Container", fileName = "New Tracker Container")]
    public class VRTrackerContainer : ScriptableObject {
        #region Variables

        [SerializeField] [Tooltip("Input action reference for trackers position.")]
        private InputAction trackerPosition;
        
        [SerializeField] [Tooltip("Input action reference for trackers rotation.")]
        private InputAction trackerRotation;

        [SerializeField] [Tooltip("Input action reference for receiving the trackers status.")]
        private InputAction trackingStatus;

        /// <summary>
        /// The frequency at which the OnTrackerPositionChanged and OnTrackerRotationChanged events invoke at.
        /// </summary>
        [Space(5)] [Tooltip("The frequency at which the TrackerPosition and TrackerRotation events invoke.")]
        public UpdateFrequencies updateFrequency = UpdateFrequencies.BeforeUpdateAndOnUpdate;
        
        [SerializeField] [Tooltip("If the container should enable all actions when the container loads.")]
        private bool enableActionsOnLoad = true;

        /// <summary>
        /// True if all of the actions have been enabled.
        /// </summary>
        public bool AllActionsRegistered { get; private set; }

        /// <summary>
        /// Invoked when all actions are enabled.
        /// </summary>
        public event VRTrackerContainerCallback EnabledActions;
        
        /// <summary>
        /// Invoked when all actions are disabled.
        /// </summary>
        public event VRTrackerContainerCallback DisabledActions;
        
        public delegate void VRTrackerContainerCallback();
        
        /// <summary>
        /// The position of the tracker.
        /// </summary>
        /// <returns>The trackers real-world position in a Vector3.</returns>
        public Vector3 Position {
            get {
                if (!trackerPosition.enabled)
                    trackerPosition?.Enable();
                
                return (Vector3)trackerPosition?.ReadValue<Vector3>();
            }
        }

        /// <summary>
        /// Invoked when the tracker position changes.
        /// </summary>
        public event TrackerPositionCallback OnPositionChanged;

        public delegate void TrackerPositionCallback(Vector3 trackerPosition);

        /// <summary>
        /// The rotation of the tracker.
        /// </summary>
        /// <returns>The trackers real-world rotation in a quaternion.</returns>
        public Quaternion Rotation {
            get {
                if (!trackerRotation.enabled) 
                    trackerRotation?.Enable();
                
                return (Quaternion)trackerRotation?.ReadValue<Quaternion>();
            }
        }

        /// <summary>
        /// Invoked when the tracker rotation changes.
        /// </summary>
        public event TrackerRotationCallback OnRotationChanged;

        public delegate void TrackerRotationCallback(Quaternion trackerRotation);

        /// <summary>
        /// If the tracker is being tracked.
        /// </summary>
        /// <returns>If the tracker is tracking.</returns>
        public bool IsTracking {
            get {
                if (!trackingStatus.enabled) 
                    trackingStatus?.Enable();
                
                return (int)trackingStatus?.ReadValue<float>() == 1;
            }
        }
        
        /// <summary>
        /// Invoked when the tracking status changes.
        /// </summary>
        public event TrackingStatusCallback OnTrackingStatusChanged;
        
        public delegate void TrackingStatusCallback();
        
        private void TrackerStatusPerformed(InputAction.CallbackContext callbackContext) => OnTrackingStatusChanged?.Invoke();

        private Vector3 _previousTrackerPosition;
        private Quaternion _previousTrackerRotation;
        
        #endregion

        private void OnEnable() {
            if (enableActionsOnLoad)
                EnableActions();
        }

        private void OnDisable() => DisableActions();

        private void BeforeUpdateAndOnUpdateCallback(UpdateTime updateTime) {
            if (updateFrequency != UpdateFrequencies.BeforeUpdateAndOnUpdate && (updateTime == UpdateTime.BeforeUpdate && updateFrequency == UpdateFrequencies.OnUpdate) || (updateTime == UpdateTime.OnUpdate && updateFrequency == UpdateFrequencies.BeforeUpdate))
                return;
            
            if (Position != _previousTrackerPosition) {
                _previousTrackerPosition = Position;
                OnPositionChanged?.Invoke(Position);
            }
            
            if (Rotation != _previousTrackerRotation) {
                _previousTrackerRotation = Rotation;
                OnRotationChanged?.Invoke(Rotation);
            }
        }
        
        /// <summary>
        /// Enables all of the actions.
        /// </summary>
        public void EnableActions() {
            trackerPosition?.Enable();

            trackerRotation?.Enable();

            if (trackerPosition != null || trackerRotation != null)
                ItsSystems.BeforeUpdateAndOnUpdate += BeforeUpdateAndOnUpdateCallback;
            
            if (trackingStatus != null) {
                trackingStatus.Enable();
                trackingStatus.performed += TrackerStatusPerformed;
            }

            AllActionsRegistered = true;
            EnabledActions?.Invoke();
        }
        
        /// <summary>
        /// Disabled all of the actions.
        /// </summary>
        public void DisableActions() {
            trackerPosition?.Disable();

            trackerRotation?.Disable();

            if (trackerPosition != null || trackerRotation != null)
                ItsSystems.BeforeUpdateAndOnUpdate -= BeforeUpdateAndOnUpdateCallback;
            
            if (trackingStatus != null) {
                trackingStatus.Disable();
                trackingStatus.performed -= TrackerStatusPerformed;
            }

            AllActionsRegistered = false;
            DisabledActions?.Invoke();
        }
    }
}
