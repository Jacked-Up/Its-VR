// This script was updated on 11/4/2021 by Jack Randolph.

using ItsVR.Scriptables;
using UnityEngine;

namespace ItsVR.Player {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(VRTracker))]
    [HelpURL("https://jackedupstudios.com/vr-controller")]
    [AddComponentMenu("It's VR/Player/VR Controller")]
    public class VRController : MonoBehaviour {
        #region Variables
        
        /// <summary>
        /// The input reference which contains all the VR input bindings for the controller.
        /// </summary>
        [Tooltip("The input reference which contains all the VR input bindings for the controller.")]
        public VRInputReferences inputReference;

        /// <summary>
        /// The hand side this controller is. This can be automatically set by naming the object either 'left hand' or 'right hand'.
        /// </summary>
        [Tooltip("The hand side this controller is. This can be automatically set by naming the object either 'left hand' or 'right hand'.")]
        public Hand handSide;
        
        private VRTracker _tracker;
        
        #endregion

        private void OnEnable() {
            if (inputReference == null)
                Debug.LogWarning("[VR Controller] Input reference bindings were not referenced. It is not required but is recommended.", this);
            
            if (_tracker == null)
                _tracker = GetComponent<VRTracker>();
        }

        /// <summary>
        /// Vibrates this controller at the amplitude for the duration.
        /// </summary>
        /// <param name="amplitude">The power setting of the haptic motor. Amplitude value is between 0-1.</param>
        /// <param name="duration">How long the haptic device vibrates.</param>
        public void Vibrate(float amplitude, float duration) {
            if (amplitude <= 0f) {
                Debug.LogWarning("[VR Controller] You attempted to vibrated the controller with an amplitude of zero.");
                return;
            }
            
            if (amplitude > 1f)
                amplitude = 1f;

            inputReference.universalInputs?.SendImpulse(amplitude, duration);
        }

        #region Editor

        private void OnValidate() {
            if (name.Contains("Left") || name.Contains("left")) 
                handSide = Hand.Left;
            else if (name.Contains("Right") || name.Contains("right")) 
                handSide = Hand.Right;
        }

        private void OnDrawGizmos() {
            if (Application.isPlaying) return;
            
            var rig = GetComponentInParent<VRRig>();
            var rigPosition = rig != null ? rig.transform.localPosition : Vector3.zero;
            var heightOffset = rig != null ? rig.heightMode == VRRig.HeightModes.Float ? rig.heightOffset : 1.75f : 1.75f;

            var self = transform;
            var selfScale = self.lossyScale;
            var selfPosition = self.position;
            
            if (rig == null) return;
            
            transform.localPosition = Vector3.Scale(new Vector3(handSide == Hand.Right ? 0.5f : -0.5f, heightOffset / 2, 0.75f), new Vector3(1, 1, 1));
            
            // Draw arm.
            Gizmos.color = handSide == Hand.Right ? Color.green : Color.blue;
            Gizmos.DrawLine(selfPosition, Vector3.Scale(new Vector3(0, heightOffset / 1.45f, 0), selfScale) + rigPosition);
            
            if (GetComponentInChildren<MeshRenderer>() || GetComponentInChildren<MeshFilter>()) return;
            
            // Draw cube hand.
            Gizmos.color = Color.cyan;
            Gizmos.DrawCube(selfPosition, Vector3.Scale(new Vector3(0.075f, 0.075f, 0.075f), selfScale));
        }

        #endregion
    }
}
