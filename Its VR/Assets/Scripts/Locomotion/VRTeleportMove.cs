// This script was updated on 03/19/2022 by Jack Randolph.

using ItsVR.Editor.Tools;
using ItsVR.Player;
using UnityEngine;

namespace ItsVR.Locomotion {
    /// <summary>
    /// Teleport move locomotion component.
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("It's VR/Locomotion/Teleport Move")]
    public class VRTeleportMove : VRBaseLocomotion, IItsVRProblemDebugable {
        #region Variables

        private const float JOYSTICK_FLICK_INITIALIZE_THRESHOLD = 0.75f;
        private const float HAPTICS_AMPLITUDE = 0.15f;
        private const float HAPTICS_DURATION = 0.075f;
        
        /// <summary>
        /// Direction the joystick must be flicked to initialize a teleport.
        /// </summary>
        [Tooltip("Direction the joystick must be flicked to initialize a teleport.")]
        public JoystickFlickDirections joystickFlickDirection = JoystickFlickDirections.Up;
        
        public enum JoystickFlickDirections { Up, Down }
        
        /// <summary>
        /// Layers which the player can teleport on.
        /// </summary>
        [Tooltip("Layers which the player can teleport on.")]
        public LayerMask validTeleportLayers;

        /// <summary>
        /// The maximum distance the player can travel per teleport.
        /// </summary>
        [Tooltip("The maximum distance the player can travel per teleport.")]
        public float maximumTeleportDistance = 12f;
        
        /// <summary>
        /// The maximum angle of the normal which the player can teleport onto.
        /// </summary>
        [Range(0f, 90f)] [Tooltip("The maximum angle of the normal which the player can teleport onto.")]
        public float maximumNormalAngle = 45f;
        
        /// <summary>
        /// The ray drawn to display where the player will teleport.
        /// </summary>
        [Space(5)] [Tooltip("The ray drawn to display where the player will teleport.")]
        public LineRenderer lineRenderer;
        
        /// <summary>
        /// Number of segments the line renderer calculates. More means it looks smoother.
        /// </summary>
        [Range(3, 30)] [Tooltip("Number of segments the line renderer calculates. More means it looks smoother.")]
        public int lineQuality = 15;
        
        /// <summary>
        /// Width of the ray.
        /// </summary>
        [Range(0.01f, 0.25f)] [Tooltip("Width of the ray.")]
        public float lineWidthAtStart = 0.02f;
        
        /// <summary>
        /// Width of the ray.
        /// </summary>
        [Range(0.01f, 0.25f)] [Tooltip("Width of the ray.")]
        public float lineWidthAtEnd = 0.05f;
        
        /// <summary>
        /// The color displayed when the player can teleport.
        /// </summary>
        [Tooltip("The color displayed when the player can teleport.")]
        public Gradient lineValidColor;
        
        /// <summary>
        /// The color displayed when the player cannot teleport.
        /// </summary>
        [Tooltip("The color displayed when the player cannot teleport.")]
        public Gradient lineInvalidColor;

        /// <summary>
        /// Displayed at the end of the teleport ray if the player can teleport to the area.
        /// </summary>
        [Tooltip("Displayed at the end of the teleport ray if the player can teleport to the area.")]
        public GameObject reticle;
        
        /// <summary>
        /// If true, the haptics will engage when the player turns.
        /// </summary>
        [Tooltip("If true, the haptics will engage when the player turns.")]
        public bool useHaptics = true;

        /// <summary>
        /// Invoked everytime the line is rendered.
        /// </summary>
        public event VRTeleportMoveEvent LineRendered;
        
        /// <summary>
        /// Invoked everytime the line is cleaned/cleared.
        /// </summary>
        public event VRTeleportMoveEvent LineCleaned;
        
        public delegate void VRTeleportMoveEvent();

        /// <summary>
        /// Invoked whenever the player is teleported.
        /// </summary>
        public event TeleportCallback DidTeleport;
        
        public delegate void TeleportCallback(Vector3 teleportPosition);

        private VRRig _vrRig;
        private bool _teleportIsReady;
        private bool _teleportIsInvalid;
        private Vector3 _firstLineVector;
        private Vector3 _secondLineVector;
        private Vector3 _thirdLineVector;

        #endregion

        private void OnEnable() {
            _vrRig = GetComponent<VRRig>();
            ItsSystems.BeforeUpdateAndOnUpdate += BeforeUpdateAndOnUpdateCallback;
            
            if (lineRenderer == null)
                Debug.LogError("The VR teleport move component needs a line renderer to display the ray. Add one to the input controller.", this);
        }

        private void OnDisable() {
            ItsSystems.BeforeUpdateAndOnUpdate -= BeforeUpdateAndOnUpdateCallback;
            
            if (lineRenderer != null)
                lineRenderer.enabled = false;
        }

        private void BeforeUpdateAndOnUpdateCallback(UpdateTime arg) {
            if (inputController == null) 
                return;

            var joystickPositionY = inputController.inputContainer.universal.JoystickPosition.y;

            switch (joystickFlickDirection) {
                case JoystickFlickDirections.Down when joystickPositionY < -JOYSTICK_FLICK_INITIALIZE_THRESHOLD:
                    _teleportIsReady = true;
                    RenderRaycastVisualLine();
                    break;
                case JoystickFlickDirections.Up when joystickPositionY > JOYSTICK_FLICK_INITIALIZE_THRESHOLD:
                    _teleportIsReady = true;
                    RenderRaycastVisualLine();
                    break;
                default: {
                    CleanRaycastVisualLine();
                    break;
                }
            }

            if (!_teleportIsReady || !(joystickPositionY > -JOYSTICK_FLICK_INITIALIZE_THRESHOLD) || !(joystickPositionY < JOYSTICK_FLICK_INITIALIZE_THRESHOLD)) 
                return;
            
            TeleportToPosition(_thirdLineVector);
        }
        
        private void TeleportToPosition(Vector3 position) {
            _vrRig.MoveRig(position);
            _teleportIsReady = false;
            
            if (useHaptics && inputController != null)
                inputController.inputContainer.universal.SendHapticPulse(HAPTICS_AMPLITUDE, HAPTICS_DURATION);
            
            DidTeleport?.Invoke(_vrRig.GetEstimatedFeetPosition());
        }
        
        private void RenderRaycastVisualLine() { 
            if (inputController == null || lineRenderer == null) 
                return;
            
            var didHitValidTeleportArea = Physics.Raycast(inputController.transform.position, -inputController.transform.up, out var hit, maximumTeleportDistance, validTeleportLayers);

            if (!didHitValidTeleportArea) {
                CleanRaycastVisualLine();
                return;
            }

            lineRenderer.widthCurve = AnimationCurve.Linear(0f, lineWidthAtStart, 1f, lineWidthAtEnd);
            lineRenderer.enabled = true;
            
            var normalAngle = maximumNormalAngle / 90f;
            if (hit.normal.x < normalAngle && hit.normal.x > -normalAngle && hit.normal.z < normalAngle && hit.normal.z > -normalAngle) {
                lineRenderer.colorGradient = lineValidColor;
                _teleportIsInvalid = false;
            }
            else { 
                lineRenderer.colorGradient = lineInvalidColor;
                _teleportIsInvalid = true;
            }

            if (Physics.Raycast(_thirdLineVector, Vector3.up, _vrRig.Height, validTeleportLayers)) {
                lineRenderer.colorGradient = lineInvalidColor;
                _teleportIsInvalid = true;
            }

            // First point
            _firstLineVector = inputController.transform.position;
            
            // Second point
            var calculatedMiddlePoint = Vector3.Lerp(_firstLineVector, hit.point, 0.5f);
            calculatedMiddlePoint.y += Vector3.Distance(_firstLineVector, hit.point) * 0.5f;
            _secondLineVector = calculatedMiddlePoint;

            // Third point
            _thirdLineVector = hit.point;

            if (reticle != null) {
                reticle.SetActive(!_teleportIsInvalid);
                
                if (reticle.activeSelf) {
                    reticle.transform.position = _thirdLineVector;
                    reticle.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                }
            }

            var rayLinePoints = new System.Collections.Generic.List<Vector3>();
            
            for (float ratio = 0; ratio <= 1; ratio += 1 / (float)lineQuality) {
                var tangent1 = Vector3.Lerp(_firstLineVector, _secondLineVector, ratio);
                var tangent2 = Vector3.Lerp(_secondLineVector, _thirdLineVector, ratio);
                var curve = Vector3.Lerp(tangent1, tangent2, ratio);

                rayLinePoints.Add(curve);
            }

            lineRenderer.positionCount = rayLinePoints.Count;
            lineRenderer.SetPositions(rayLinePoints.ToArray());
            
            LineRendered?.Invoke();
        }
        
        private void CleanRaycastVisualLine() {
            if (lineRenderer != null) 
                lineRenderer.enabled = false;
            
            if (reticle != null) 
                reticle.SetActive(false);
            
            LineCleaned?.Invoke();
        }

#if UNITY_EDITOR
        private void OnValidate() => RefreshProblems();
        
        public void RefreshProblems() {
            if (inputController == null)
                ItsVRProblemDebuggerEditor.SubmitProblem("An input VR controller must be referenced for the VR teleport move component to receive input.", Editor.Tools.ItsVRProblemDebuggerEditor.ProblemLevels.Error, transform);
            
            if (lineRenderer == null)
                ItsVRProblemDebuggerEditor.SubmitProblem("The VR teleport move component requires a line renderer to display the ray visual. The line renderer should be added to the input controller.", Editor.Tools.ItsVRProblemDebuggerEditor.ProblemLevels.Error, transform);
            
            if (reticle == null)
                ItsVRProblemDebuggerEditor.SubmitProblem("Add a reticle to the VR teleport move component.", Editor.Tools.ItsVRProblemDebuggerEditor.ProblemLevels.Recommendation, transform);
        }
#endif
    }
}