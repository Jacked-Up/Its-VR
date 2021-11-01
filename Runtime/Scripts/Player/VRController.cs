// This script was updated on 10/30/2021 by Jack Randolph.

using System;
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
        /// The hand side which this controller is. This can be automatically set by naming the object either 'left hand' or 'right hand'.
        /// </summary>
        [Tooltip("The hand side which this controller is. This can be automatically set by naming the object either 'left hand' or 'right hand'.")]
        public HandSides handSide;

        public enum HandSides { Left, Right }
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
        /// <param name="amplitude"></param>
        /// <param name="duration"></param>
        public void Vibrate(float amplitude, float duration) {
            inputReference.universalInputs?.SendImpulse(amplitude, duration);
        }

        /// <summary>
        /// The speed of the controller in local space.
        /// </summary>
        public float Speed => _tracker.LocalSpeed;

        /// <summary>
        /// The velocity of the controller in local space.
        /// </summary>
        public Vector3 Velocity => _tracker.LocalVelocity;

        /// <summary>
        /// The angular velocity of the controller in local space.
        /// </summary>
        public Vector3 AngularVelocity => _tracker.LocalAngularVelocity;
        
        /// <summary>
        /// (Incomplete) Returns the pointer direction of the controller.
        /// </summary>
        public Vector3 PointerDirection {
            get {
                return -transform.up;
            }
        }

        #region Editor

        private void OnValidate() {
            if (name.Contains("Left") || name.Contains("left")) 
                handSide = HandSides.Left;
            else if (name.Contains("Right") || name.Contains("right")) 
                handSide = HandSides.Right;
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
            
            transform.localPosition = Vector3.Scale(new Vector3(handSide == HandSides.Right ? 0.5f : -0.5f, heightOffset / 2, 0.75f), new Vector3(1, 1, 1));
            
            // Draw arm.
            Gizmos.color = handSide == HandSides.Right ? Color.green : Color.blue;
            Gizmos.DrawLine(selfPosition, Vector3.Scale(new Vector3(0, heightOffset / 1.45f, 0), selfScale) + rigPosition);
            
            if (GetComponentInChildren<MeshRenderer>() || GetComponentInChildren<MeshFilter>()) return;
            
            // Draw cube hand.
            Gizmos.color = Color.cyan;
            Gizmos.DrawCube(selfPosition, Vector3.Scale(new Vector3(0.075f, 0.075f, 0.075f), selfScale));
        }

        #endregion
    }
}
