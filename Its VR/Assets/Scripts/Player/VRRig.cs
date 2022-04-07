using ItsVR.Editor.Tools;
using UnityEngine;

namespace ItsVR.Player {
    /// <summary>
    /// Central VR player controller system. Provides details about VR rig to developer through properties and methods.
    /// Calculates the height offset depending on the calculation method chosen.
    /// </summary>
    /// <para>Author: Jack Randolph</para>
    [DisallowMultipleComponent]
    [AddComponentMenu("It's VR/VR Rig")]
    [HelpURL("https://jackedupstudios.com/vr-rig")]
    public class VRRig : MonoBehaviour, IItsVRProblemDebugable {
        #region Variables

        private const HeightMethodTypes DEFAULT_HEIGHT_METHOD = HeightMethodTypes.Offset;
        private const float RIG_OFFSET_HEIGHT_DEFAULT = 1.6f;
        private const float RIG_WIDTH_DEFAULT = 0.3f;
        
        /// <summary>
        /// The VR rigs head object transform.
        /// </summary>
        [HideInInspector] [Tooltip("The VR rigs head transform.")]
        public Transform head;

        /// <summary>
        /// The VR rigs body offset object transform.
        /// </summary>
        [HideInInspector] [Tooltip("The VR rigs body offset transform.")]
        public Transform bodyOffset;

        /// <summary>
        /// Height method used to calculate the players height.
        /// </summary>
        public HeightMethodTypes HeightMethod {
            get => _heightMethod;
            set {
                if (_heightMethod == value)
                    return;
                
                _heightMethod = value;
                RecalculateOffset();
            }
        }
        private HeightMethodTypes _heightMethod = DEFAULT_HEIGHT_METHOD;

        /// <summary>
        /// The amount of Y offset to apply to the body offset.
        /// </summary>
        public float OffsetHeight {
            get => _offsetHeight;
            set {
                if (_offsetHeight == value)
                    return;

                _offsetHeight = value;
                RecalculateOffset();
            }
        }
        private float _offsetHeight= RIG_OFFSET_HEIGHT_DEFAULT;
        
        /// <summary>
        /// The height of the VR rig.
        /// </summary>
        public float Height => GetHeadPosition().y - transform.position.y;

        /// <summary>
        /// The width of the vr rig.
        /// </summary>
        public float Width {
            get => _width;
            set {
                if (value < 0) {
                    _width = 0f;
                    return;
                }

                _width = value;
            }
        }
        private float _width = RIG_WIDTH_DEFAULT;

        /// <summary>
        /// The position of the players head.
        /// </summary>
        /// <param name="inWorldSpace">If the position should be calculated in world space.</param>
        /// <returns>The position of the players head.</returns>
        public Vector3 GetHeadPosition(bool inWorldSpace = true) {
            if (head == null)
                return Vector3.zero;

            return !inWorldSpace
                ? head.localPosition
                : head.position;
        }
        
        /// <summary>
        /// The *estimated* position of the players hips. The position returned is the point between the players head
        /// and the floor of the player bounds and the local offset of the head.
        /// </summary>
        /// <param name="inWorldSpace">If the position should be calculated in world space.</param>
        /// <returns>The estimated position of the players hips.</returns>
        public Vector3 GetEstimatedHipPosition(bool inWorldSpace = true) {
            if (head == null)
                return Vector3.zero;
            
            return !inWorldSpace
                ? new Vector3(head.localPosition.x, Height / 2f, head.localPosition.z)
                : new Vector3(head.position.x, transform.position.y + Height / 2f, head.position.z);
        }
        
        /// <summary>
        /// The *estimated* position of the players feet. The position returned is the point on the floor of the player
        /// bounds and the local offset of the head.
        /// </summary>
        /// <param name="inWorldSpace">If the position should be calculated in world space.</param>
        /// <returns>The estimated position of the players feet.</returns>
        public Vector3 GetEstimatedFeetPosition(bool inWorldSpace = true) {
            if (head == null)
                return Vector3.zero;

            return !inWorldSpace
                ? new Vector3(head.localPosition.x, 0f, head.localPosition.z)
                : new Vector3(head.position.x, transform.position.y, head.position.z);
        }

        /// <summary>
        /// Invoked when the VR rig is moved.
        /// </summary>
        public event MovedRigCallback MovedRig;
        
        public delegate void MovedRigCallback(Vector3 position);
        
        /// <summary>
        /// Invoked when the VR rig is rotated.
        /// </summary>
        public event RotatedRigCallback RotatedRig;
        
        public delegate void RotatedRigCallback(float degrees, Vector3 axis);

        #endregion

        private void OnEnable() {
            RecalculateOffset();

#if UNITY_EDITOR
            if (head == null)
                Debug.LogError("The VR rig requires a head transform to be assigned.", this);
            
            if (bodyOffset == null)
                Debug.LogError("The VR rig requires a body offset transform to be assigned.", this);
            
            if (head != null && (!head.IsChildOf(transform) || head.transform == transform))
                Debug.LogError("The VR rigs head must be a child of the VR rigs transform.", this);

            if (bodyOffset != null && (!bodyOffset.IsChildOf(transform) || bodyOffset.transform == transform))
                Debug.LogError("The VR rigs body offset must be a child of the VR rigs transform.", this);
            
            if (head != null && bodyOffset != null && (!head.IsChildOf(bodyOffset) || head.transform == bodyOffset))
                Debug.LogError("The VR rigs head must also be a child of the VR rigs body offset.", this);

            if (transform.localScale.x != transform.localScale.y || transform.localScale.x != transform.localScale.z || transform.localScale.y != transform.localScale.z)
                Debug.LogError("Not scaling the VR rig uniformly can cause unexpected behaviors.", this);
            
            if (bodyOffset.localScale != Vector3.one)
                Debug.LogError("You should not scale the body offset transform.", this);
#endif
        }
        
        /// <summary>
        /// Moves the VR rig to the position accounting for the head offset position.
        /// </summary>
        /// <param name="position">The position to move the VR rig to.</param>
        public void MoveRig(Vector3 position) {
            transform.position = position + (transform.position - GetEstimatedFeetPosition());
            MovedRig?.Invoke(position);
        }

        /// <summary>
        /// Rotates the VR rig around the head.
        /// </summary>
        /// <param name="angle">The angle to turn the VR rig.</param>
        /// <param name="axis">The axis to turn the VR rig around.</param>
        public void RotateRig(float angle, Vector3 axis) {
            if (angle == 0) {
#if UNITY_EDITOR
                Debug.LogWarning("You attempted to rotate the VR rig by zero degrees.", this);
#endif
                return;
            }

            if (axis == Vector3.zero) {
#if UNITY_EDITOR
                Debug.LogError("You attempted to rotate the VR rig on a nil axis.", this);
#endif
                return;
            }
            
            transform.RotateAround(GetEstimatedHipPosition(), axis, angle);
            RotatedRig?.Invoke(angle, axis);
        }

        private void RecalculateOffset() {
            switch (HeightMethod) {
                case HeightMethodTypes.Offset when bodyOffset != null:
                    bodyOffset.localPosition = new Vector3(head.localPosition.x, OffsetHeight, head.localPosition.z);
                    head.localPosition = Vector3.zero;
                    break;
                case HeightMethodTypes.Device when bodyOffset != null:
                    bodyOffset.localPosition = Application.isPlaying ? Vector3.zero : new Vector3(GetHeadPosition().x, transform.localPosition.y, GetHeadPosition().z);
                    head.localPosition = Vector3.zero;
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException();
            }
        }
        
#if UNITY_EDITOR
        private const float BOUNDING_BOX_SIZE = 3f;
        private const float PLAYER_BOX_SIZE = 0.15f;
        private const float ADDITION_BOUNDING_BOX_HEIGHT = 1.5f;
        private const int GRID_LINE_COUNT = 4;
        
        private void OnDrawGizmos() {
            if (head != null && bodyOffset != null) {
                Gizmos.color = transform.localScale.x != transform.localScale.y || transform.localScale.x != transform.localScale.z || transform.localScale.y != transform.localScale.z || bodyOffset.localScale != Vector3.one ? Color.red : Color.green;
                Gizmos.DrawWireCube(new Vector3(GetEstimatedHipPosition().x, GetEstimatedHipPosition().y + ADDITION_BOUNDING_BOX_HEIGHT / 2f * transform.localScale.y, GetEstimatedHipPosition().z), new Vector3(BOUNDING_BOX_SIZE * transform.localScale.x, Height + ADDITION_BOUNDING_BOX_HEIGHT * transform.localScale.y, BOUNDING_BOX_SIZE * transform.localScale.z));
            }
            
            if (transform.rotation.x == 0f && transform.rotation.z == 0) {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(GetEstimatedHipPosition(), new Vector3(PLAYER_BOX_SIZE * transform.localScale.x, Height, PLAYER_BOX_SIZE * transform.localScale.z));
            }
        }

        private void OnDrawGizmosSelected() {
            var transformPosition = transform.position;
            var transformScale = transform.lossyScale;
            
            Gizmos.color = Color.grey;
            for (var i = 0; i < GRID_LINE_COUNT; i++) {
                Gizmos.DrawLine(Vector3.Scale(new Vector3(transformPosition.x + GRID_LINE_COUNT - 1, transformPosition.y, transformPosition.z + i), transformScale), Vector3.Scale(new Vector3(transformPosition.x + -(GRID_LINE_COUNT - 1), transformPosition.y, transformPosition.z + i), transformScale));
                Gizmos.DrawLine(Vector3.Scale(new Vector3(transformPosition.x + GRID_LINE_COUNT - 1, transformPosition.y, transformPosition.z + -i), transformScale),  Vector3.Scale(new Vector3(transformPosition.x + -(GRID_LINE_COUNT - 1), transformPosition.y, transformPosition.z + -i), transformScale));
            }
            for (var i = 0; i < GRID_LINE_COUNT; i++) {
                Gizmos.DrawLine(Vector3.Scale(new Vector3(transformPosition.x + i, transformPosition.y, transformPosition.z + GRID_LINE_COUNT - 1), transformScale), Vector3.Scale(new Vector3(transformPosition.x + i, transformPosition.y, transformPosition.z + -(GRID_LINE_COUNT - 1)), transformScale));
                Gizmos.DrawLine(Vector3.Scale(new Vector3(transformPosition.x + -i, transformPosition.y, transformPosition.z + GRID_LINE_COUNT - 1), transformScale),  Vector3.Scale(new Vector3(transformPosition.x + -i, transformPosition.y, transformPosition.z + -(GRID_LINE_COUNT - 1)), transformScale));
            }
        }
        
        private void OnValidate() {
            if (head == null && GetComponentInChildren<Camera>())
                head = GetComponentInChildren<Camera>().transform;

            if (bodyOffset == null && head != null)
                bodyOffset = head.parent;
        }

        public void RefreshProblems() {
            // ERRORS --------------------------------------------------------------------------------------------------
            if (head == null)
                ItsVRProblemDebuggerEditor.SubmitProblem("The VR rig requires a head transform to be assigned.", ItsVRProblemDebuggerEditor.ProblemLevels.Error, transform);
            
            if (bodyOffset == null)
                ItsVRProblemDebuggerEditor.SubmitProblem("The VR rig requires a body offset transform to be assigned.", ItsVRProblemDebuggerEditor.ProblemLevels.Error, transform);
            
            if (head != null && (!head.IsChildOf(transform) || head.transform == transform))
                ItsVRProblemDebuggerEditor.SubmitProblem("The VR rigs head must be a child of the VR rigs transform.", ItsVRProblemDebuggerEditor.ProblemLevels.Error, transform);

            if (bodyOffset != null && (!bodyOffset.IsChildOf(transform) || bodyOffset.transform == transform))
                ItsVRProblemDebuggerEditor.SubmitProblem("The VR rigs body offset must be a child of the VR rigs transform.", ItsVRProblemDebuggerEditor.ProblemLevels.Error, transform);
            
            if (head != null && bodyOffset != null && (!head.IsChildOf(bodyOffset) || head.transform == bodyOffset))
                ItsVRProblemDebuggerEditor.SubmitProblem("The VR rigs head must also be a child of the VR rigs body offset.", ItsVRProblemDebuggerEditor.ProblemLevels.Error, transform);

            if (transform.localScale.x != transform.localScale.y || transform.localScale.x != transform.localScale.z || transform.localScale.y != transform.localScale.z)
                ItsVRProblemDebuggerEditor.SubmitProblem("Not scaling the VR rig uniformly can cause unexpected behaviors.", ItsVRProblemDebuggerEditor.ProblemLevels.Error, transform);
            
            if (bodyOffset.localScale != Vector3.one)
                ItsVRProblemDebuggerEditor.SubmitProblem("You should not scale the body offset transform.", ItsVRProblemDebuggerEditor.ProblemLevels.Error, transform);
            
            // ISSUES --------------------------------------------------------------------------------------------------
            
            if (!GetComponentInChildren<Camera>())
                ItsVRProblemDebuggerEditor.SubmitProblem("A camera couldn't be detected on the VR rig. Please add one to the head transform.", ItsVRProblemDebuggerEditor.ProblemLevels.Issue, transform);
            
            if (transform.rotation.x != 0f || transform.rotation.z != 0)
                ItsVRProblemDebuggerEditor.SubmitProblem("You should not rotate the VR rig on the X axis or Z axis.", ItsVRProblemDebuggerEditor.ProblemLevels.Issue, transform);
            
            // RECOMMENDATIONS -----------------------------------------------------------------------------------------

            if (GetComponentInChildren<Camera>() != null && GetComponentInChildren<Camera>().nearClipPlane > 0.01f)
                ItsVRProblemDebuggerEditor.SubmitProblem("Set the cameras near clipping plane to 0.01. This will prevent your camera from clipping through close up game objects.", ItsVRProblemDebuggerEditor.ProblemLevels.Recommendation, transform);
            
            if (Height > 3f)
                ItsVRProblemDebuggerEditor.SubmitProblem("The VR rig is extremely tall. Is this intentional?", ItsVRProblemDebuggerEditor.ProblemLevels.Recommendation, transform);

            if (HeightMethod == HeightMethodTypes.Device)
                ItsVRProblemDebuggerEditor.SubmitProblem("We recommend you use the 'float' height method instead of the 'device' height method. This is because the 'device' height method will not work on some platforms.", ItsVRProblemDebuggerEditor.ProblemLevels.Recommendation, transform);
        }
#endif
    }
}
