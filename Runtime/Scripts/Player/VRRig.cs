// This script was updated on 11/10/2021 by Jack Randolph.

using UnityEngine;

namespace ItsVR.Player {
    /// <summary>
    /// Central VR player controller.
    /// </summary>
    [DisallowMultipleComponent]
    [HelpURL("https://jackedupstudios.com/vr-rig")]
    [AddComponentMenu("It's VR/Player/VR Rig")]
    public class VRRig : MonoBehaviour {
        #region Variables

        /// <summary>
        /// The players head.
        /// </summary>
        [Tooltip("The players head.")]
        public Transform head;

        /// <summary>
        /// The players body. NOTE: Used to offset the head/camera and the hands/controllers.
        /// </summary>
        [Tooltip("The players body. NOTE: Used to offset the head/camera and the hands/controllers.")]
        public Transform body;

        /// <summary>
        /// How wide the player is.
        /// </summary>
        [SerializeField] [Tooltip("How wide the player is.")]
        private float width = 0.3f;
        
        /// <summary>
        /// Offsets the players height when the height mode is set to float.
        /// </summary>
        [Tooltip("Offsets the players height when the height mode is set to float.")]
        public float heightOffset = 1.75f;
        
        /// <summary>
        /// How the rig detects height.
        /// </summary>
        [Tooltip("How the rig detects height. Device height mode uses the players physical height. Float offsets the players height.")]
        public HeightModes heightMode = HeightModes.Device;

        /// <summary>
        /// How tall the player is.
        /// </summary>
        public float Height {
            get {
                var height = 0f;

                if (heightMode == HeightModes.Device && head != null)
                    height = head.localPosition.y;
                
                return height;
            }
        }
        
        /// <summary>
        /// How wide the player is.
        /// </summary>
        public float Width {
            get => width;
            set => width = value;
        }
        
        /// <summary>
        /// The heads world position. Equivalent of 'head.position'.
        /// </summary>
        public Vector3 HeadPosition => head == null ? Vector3.zero : head.position;

        /// <summary>
        /// The heads local position. Equivalent of 'head.localPosition'.
        /// </summary>
        public Vector3 HeadLocalPosition => head == null ? Vector3.zero : head.localPosition;

        /// <summary>
        /// The estimated world position of the hip. This is the point directly in the middle of the head and the bottom of the bounding box.
        /// </summary>
        public Vector3 HipPosition => head == null ? Vector3.zero : new Vector3(HeadPosition.x, transform.position.y + Height / 2, HeadPosition.z);

        /// <summary>
        /// The estimated local position of the hip. This is the point directly in the middle of the head and the bottom of the bounding box.
        /// </summary>
        public Vector3 HipLocalPosition => head == null ? Vector3.zero : new Vector3(HeadLocalPosition.x, Height / 2, HeadLocalPosition.z);
        
        /// <summary>
        /// The estimated world position of the feet. (This is the position directly below the head to the bottom of the rigs bounding box).
        /// </summary>
        public Vector3 FeetPosition => head == null ? Vector3.zero : new Vector3(HeadPosition.x, transform.position.y, HeadPosition.z);
        
        /// <summary>
        /// The estimated local position of the feet. (This is the position directly below the head to the bottom of the rigs bounding box).
        /// </summary>
        public Vector3 FeetLocalPosition => head == null ? Vector3.zero : new Vector3(HeadLocalPosition.x, 0f, HeadLocalPosition.z);

        /// <summary>
        /// Invoked when the rig is rotated.
        /// </summary>
        public event VRRigEvent RigTransformed;
        
        /// <summary>
        /// Invoked when the rig is rotated.
        /// </summary>
        public event VRRigEvent RigRotated;
        
        public enum HeightModes { Device, Float }
        public delegate void VRRigEvent();
        private VRTracker _headTracker;

        #endregion

        private void OnEnable() {
            if (head == null) 
                Debug.LogError("[VR Rig] Head transform was not referenced.", this);
            else {
                if (!head.GetComponent<VRTracker>()) 
                    Debug.LogError("[VR Rig] Head doesn't have a VR Tracker component attached to it. The VR Rig will not be able to apply height offset.", head);
                else 
                    _headTracker = head.GetComponent<VRTracker>();
            }
            
            if (body == null) {
                Debug.LogWarning("[VR Rig] No offset object was referenced. Setting to self.", this);
                body = transform;
            }
            
            if (head != null && !head.GetComponent<Collider>()) 
                Debug.LogWarning("[VR Rig] It is recommended that you add a sphere collider on the players head.", this);
        }

        private void Update() {
            if (_headTracker == null) return;
            _headTracker.positionOffset = new Vector3(0f, heightMode == HeightModes.Float ? heightOffset : 0f, 0f);
        }

        /// <summary>
        /// Moves the rig to the position.
        /// </summary>
        /// <param name="position">The position to move the rig to.</param>>
        public void PositionRig(Vector3 position) {
            if (head == null) {
                Debug.LogError("[VR Rig] Cannot transform rig because no head object was referenced.", this);
                return;
            }
            
            RigTransformed?.Invoke();
            var root = transform.root;
            root.position = position + (root.position - FeetPosition);
        }
        
        /// <summary>
        /// Rotates the rig by using the head as a pivot.
        /// </summary>
        /// <param name="angle">The angle to turn the rig.</param>
        public void RotateRig(float angle) {
            if (head == null) {
                Debug.LogError("[VR Rig] Cannot rotate rig because no head object was referenced.", this);
                return;
            }
            
            if (angle == 0) 
                Debug.LogWarning("[VR Rig] You tried to rotate the rig by zero degrees.", this);

            RigRotated?.Invoke();
            transform.RotateAround(head.position, Vector3.up, angle);
        }

        #region Editor

        private void OnDrawGizmos() {
            var heightYOffset = heightMode == HeightModes.Float ? heightOffset : 1.75f;
            
            var self = transform;
            var selfPosition = self.position;
            var selfScale = self.lossyScale;

            // Head setup.
            if (head != null) {
                if (!Application.isPlaying)
                    head.transform.position = Vector3.Scale(new Vector3(transform.position.x, selfPosition.y + heightYOffset, selfPosition.z), selfScale);

                var headCol = head.GetComponent<SphereCollider>();
                if (headCol != null) 
                    headCol.radius = Mathf.Clamp(width, 0.15f, 0.5f);
            }
            else 
                head = GetComponentInChildren<Camera>()?.transform;

            // Body setup.
            if (body != null) {}
            else if (body == null && head != null) {
                var headParent = head.transform.parent;
                body = !headParent.GetComponent<VRRig>() ? headParent : null;
            }

            // Draw boundary box.
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(Vector3.Scale(new Vector3(selfPosition.x, selfPosition.y + (heightYOffset + 1.2f) / 2, selfPosition.z), selfScale), Vector3.Scale(new Vector3(3f, (heightYOffset + 1.2f), 3f), selfScale));

            // Draw vectors.
            if (!Application.isPlaying) {
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(FeetPosition, transform.forward);   
            }
            
            // Draw body.
            Gizmos.color = Color.red;
            Gizmos.DrawLine(HeadPosition, FeetPosition);
        }

        private void OnDrawGizmosSelected() {
            var heightYOffset = heightMode == HeightModes.Float ? heightOffset : 1.75f;
           
            var self = transform;
            var selfPosition = self.position;
            var selfScale = self.lossyScale;
            
            // Draw boundary box.
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(Vector3.Scale(new Vector3(selfPosition.x, selfPosition.y + (heightYOffset + 1.2f) / 2, selfPosition.z), selfScale), Vector3.Scale(new Vector3(3f, (heightYOffset + 1.2f), 3f), selfScale));
           
            // Draw grid.
            Gizmos.color = Color.green;
            for (var i = 0; i < 4; i++) {
                Gizmos.DrawLine(Vector3.Scale(new Vector3(selfPosition.x + 3, selfPosition.y, selfPosition.z + i), selfScale), Vector3.Scale(new Vector3(selfPosition.x + -3, selfPosition.y, selfPosition.z + i), selfScale));
                Gizmos.DrawLine(Vector3.Scale(new Vector3(selfPosition.x + 3, selfPosition.y, selfPosition.z + -i), selfScale),  Vector3.Scale(new Vector3(selfPosition.x + -3, selfPosition.y, selfPosition.z + -i), selfScale));
            }
            for (var i = 0; i < 4; i++) {
                Gizmos.DrawLine(Vector3.Scale(new Vector3(selfPosition.x + i, selfPosition.y, selfPosition.z + 3), selfScale), Vector3.Scale(new Vector3(selfPosition.x + i, selfPosition.y, selfPosition.z + -3), selfScale));
                Gizmos.DrawLine(Vector3.Scale(new Vector3(selfPosition.x + -i, selfPosition.y, selfPosition.z + 3), selfScale),  Vector3.Scale(new Vector3(selfPosition.x + -i, selfPosition.y, selfPosition.z + -3), selfScale));
            }
        }

        #endregion
    }
}
