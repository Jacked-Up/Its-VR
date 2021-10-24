using Codice.Client.Common;
using UnityEngine;

namespace ItsVR.Player {
    [DisallowMultipleComponent]
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
        /// The heads position. Equivalent of 'head.position'.
        /// </summary>
        public Vector3 HeadPosition => head == null ? Vector3.zero : head.position;

        /// <summary>
        /// The heads local position. Equivalent of 'head.localPosition'.
        /// </summary>
        public Vector3 HeadLocalPosition => head == null ? Vector3.zero : head.localPosition;

        /// <summary>
        /// The estimated position of the feet. (This is the position directly below the head to the bottom of the rigs bounding box).
        /// </summary>
        public Vector3 FeetPosition => head == null ? Vector3.zero : new Vector3(head.position.x, transform.position.y, head.position.z);
        
        /// <summary>
        /// The estimated position of the feet. (This is the position directly below the head to the bottom of the rigs bounding box).
        /// </summary>
        public Vector3 FeetLocalPosition => head == null ? Vector3.zero : new Vector3(head.localPosition.x, transform.position.y, head.localPosition.z);

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
        /// Rotates the rig by using the head as a pivot.
        /// </summary>
        /// <param name="angle"></param>
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
            var selfTransform = transform;
            var selfPosition = selfTransform.position;
            var selfScale = selfTransform.lossyScale;
            var heightYOffset = heightMode == HeightModes.Float ? heightOffset : 1.75f;
            
            var headPosition = Vector3.Scale(head != null ? head.transform.position : Vector3.zero, selfScale);
            var feetPosition = Vector3.Scale(Application.isPlaying && _headTracker != null ? new Vector3(_headTracker.trackerReference.TrackerPosition.x, selfPosition.y, _headTracker.trackerReference.TrackerPosition.z) : selfPosition, selfScale);

            // Head setup.
            if (head != null) {
                head.transform.position = Vector3.Scale(new Vector3(selfPosition.x, selfPosition.y + heightYOffset, selfPosition.z), transform.lossyScale);

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
                Gizmos.DrawRay(selfPosition, selfTransform.forward);   
            }
            
            // Draw body.
            Gizmos.color = Color.red;
            Gizmos.DrawLine(feetPosition, headPosition);
        }

        private void OnDrawGizmosSelected() {
            var selfTransform = transform;
            var selfPosition = selfTransform.position;
            var selfScale = selfTransform.lossyScale;
            var heightYOffset = heightMode == HeightModes.Float ? this.heightOffset : 1.75f;

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
