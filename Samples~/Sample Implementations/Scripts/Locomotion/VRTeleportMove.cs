// This script was updated on 10/28/2021 by Jack Randolph.

using System.Collections.Generic;
using UnityEngine;
using ItsVR.Player;

namespace ItsVR_Samples.Locomotion {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(VRRig))]
    [AddComponentMenu("It's VR/Samples/Locomotion/Teleport Move (Sample)")]
    public class VRTeleportMove : MonoBehaviour {
        #region Variables

        /// <summary>
        /// The controller which this script reads input from.
        /// </summary>
        [Tooltip("The controller which this script reads input from.")]
        public VRController inputController;

        /// <summary>
        /// Direction the joystick must be flicked to initialize a teleport.
        /// </summary>
        [Tooltip("Direction the joystick must be flicked to initialize a teleport.")]
        public JoystickFlickInitiations flickDirection = JoystickFlickInitiations.Down;
        
        /// <summary>
        /// Layers which the player can teleport on.
        /// </summary>
        [Tooltip("Layers which the player can teleport on.")]
        public LayerMask validLayers;

        /// <summary>
        /// The ray drawn to display where the player will teleport.
        /// </summary>
        [Tooltip("The ray drawn to display where the player will teleport.")]
        public LineRenderer teleportRay;
        
        /// <summary>
        /// The color displayed when the player can teleport.
        /// </summary>
        [Tooltip("The color displayed when the player can teleport.")]
        public Gradient validTeleport;
        
        /// <summary>
        /// The color displayed when the player cannot teleport.
        /// </summary>
        [Tooltip("The color displayed when the player cannot teleport.")]
        public Gradient invalidTeleport;

        /// <summary>
        /// The maximum angle of the normal which the player can teleport onto.
        /// </summary>
        [Range(0f, 90f)] [Tooltip("The maximum angle of the normal which the player can teleport onto.")]
        public float maxAngle = 45f;

        /*
        
        /// <summary>
        /// The maximum distance the player can travel per teleport.
        /// </summary>
        [Tooltip("The maximum distance the player can travel per teleport.")]
        public float maxTravelDistance = 10f;
        
        */
        
        /// <summary>
        /// Number of segments the line renderer calculates. More means it looks smoother.
        /// </summary>
        [Range(3, 25)] [Tooltip("Number of segments the line renderer calculates. More means it looks smoother.")]
        public float lineQuality = 12;
        
        /// <summary>
        /// Width of the ray.
        /// </summary>
        [Range(0.01f, 0.5f)] [Tooltip("Width of the ray.")]
        public float lineWidth = 0.05f;
        
        /// <summary>
        /// Acts as a target object at the end of the teleport ray.
        /// </summary>
        [Tooltip("Acts as a target object at the end of the teleport ray.")]
        public GameObject reticle;
        
        public enum JoystickFlickInitiations { Up, Down }
        private bool _initialized;
        private bool _invalidPoint;
        private Vector3 _lastHitPoint;
        private VRRig _vrRig;
        
        #endregion

        private void OnEnable() {
            if (inputController == null)
                Debug.LogError("[VR Teleport Move] No input controller was referenced. (I have no way to receive input)", this);

            if (teleportRay == null)
                Debug.LogError("[VR Teleport Move] No line renderer was referenced. Please add one to the input controller.", this);
            
            _vrRig = GetComponent<VRRig>();
        }

        private void Update() {
            if (inputController == null || teleportRay == null) return;

            // First we cache the joysticks Y position.
            var joystickPositionY = inputController.inputReference.universalInputs.JoystickPosition.y;

            // Let check to see if the player has flicked the joystick in the initialization position.
            // If they have, we can initialize the teleport. If on the next frame the player has released,
            // the joystick, the player will teleport to the last point they highlighted.
            switch (flickDirection) {
                case JoystickFlickInitiations.Down when joystickPositionY < -0.75f:
                    _initialized = true;
                    DrawRay();
                    break;
                case JoystickFlickInitiations.Up when joystickPositionY > 0.75f:
                    _initialized = true;
                    DrawRay();
                    break;
                default: {
                    ClearRay();
                    break;
                }
            }
            
            // The script will bail if the player is attempting to teleport onto an invalid teleport point.
            if (_invalidPoint) return;
            
            // If the teleport was initialized (the player has flicked the joystick to the initialization position)
            // and then player has released the joystick, then you are going to teleport the player to the position
            // and uninitialize the teleport.
            if (_initialized && joystickPositionY > -0.75f && joystickPositionY < 0.75f) {
                Teleport(_lastHitPoint);
                _initialized = false;
            }
        }

        /// <summary>
        /// Teleports the player to the position.
        /// </summary>
        /// <param name="toPosition"></param>
        private void Teleport(Vector3 toPosition) {
            transform.position = toPosition;
        }

        /// <summary>
        /// Draws a curved ray using the line renderer.
        /// </summary>
        private void DrawRay() { 
            if (inputController == null || teleportRay == null) return;
            
            // The first step is to raycast from the input controller.
            var controller = inputController.transform;
            var didHit = Physics.Raycast(controller.position, -controller.up, out var hit, 100, validLayers);

            // The script will bail if the raycast didn't hit an object. During the bail,
            // the script will also clear the ray so that the player cannot see it.
            if (!didHit) {
                ClearRay();
                return;
            }

            // Here, we are setting up the teleport ray parameters and activating it.
            teleportRay.widthCurve = AnimationCurve.Constant(0f, 1f, lineWidth);
            teleportRay.enabled = true;

            // Checking that the hit normals direction is valid and inside maximum angle. If the normal
            // angle is to steep, the teleport ray will change colors to show the player that they cannot
            // teleport to that area.
            var maxNormalAngle = maxAngle / 90f;
            if (hit.normal.x < maxNormalAngle && hit.normal.x > -maxNormalAngle && hit.normal.z < maxNormalAngle && hit.normal.z > -maxNormalAngle) {
                teleportRay.colorGradient = validTeleport;
                _invalidPoint = false;
            }
            else { 
                teleportRay.colorGradient = invalidTeleport;
                _invalidPoint = true;
            }

            // Our starting point of the ray is the controllers position.
            var startPoint = controller.position;
            
            // The middle point of the ray should be half way between the starting point and the end
            // point. We will also raise the middle point a bit above the starting point to give it
            // the curve effect.
            var calculatedMiddlePoint = Vector3.Lerp(startPoint, hit.point, 0.5f);
            calculatedMiddlePoint.y += Vector3.Distance(startPoint, hit.point) * 0.5f;
            var middlePoint = calculatedMiddlePoint;

            // The end point will be the raycast hit point.
            var endPoint = hit.point;

            // Draw reticle if one is referenced.
            if (reticle != null) {
                reticle.SetActive(!_invalidPoint);
                reticle.transform.position = endPoint;
            }
            
            // Lets calculate all of the line vertexes.
            var pointList = new List<Vector3>();
            
            for(float ratio = 0; ratio <= 1; ratio += 1 / lineQuality) {
                var tangent1 = Vector3.Lerp(startPoint, middlePoint, ratio);
                var tangent2 = Vector3.Lerp(middlePoint, endPoint, ratio);
                var curve = Vector3.Lerp(tangent1, tangent2, ratio);

                pointList.Add(curve);
            }

            // Here's where we are applying all of the vertexes to the line renderer to create the ray.
            teleportRay.positionCount = pointList.Count;
            teleportRay.SetPositions(pointList.ToArray());
            
            // Lets cache the end point calculated this frame so that we can use it for teleportation.
            _lastHitPoint = endPoint;
        }

        /// <summary>
        /// Clears the ray from the screen.
        /// </summary>
        private void ClearRay() {
            if (teleportRay != null) teleportRay.enabled = false;
            if (reticle != null) reticle.SetActive(false);
        }
    }
}