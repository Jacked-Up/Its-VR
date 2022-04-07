// This script was updated on 03/20/2022 by Jack Randolph.

using System.Linq;
using ItsVR.Editor.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace ItsVR.Interaction {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    [AddComponentMenu("It's VR/Interaction/Button")]
    public class VRButtonInteractable : VRBaseInteractable, IItsVRProblemDebugable {
        #region Variables

        private const float POINT_CHECK_TOLERANCE = 0.025f;
        
        /// <summary>
        /// Layer(s) which the script can check for interactor components.
        /// </summary>
        [Tooltip("Layer(s) which the script can check for interactor components.")]
        public LayerMask interactorMask;
        
        /// <summary>
        /// The overlap detection distance.
        /// </summary>
        [Tooltip("The overlap detection distance.")]
        public float interactorDetectionRadius = 0.1f;

        /// <summary>
        /// The speed the button returns to its starting position when the interaction is ended.
        /// </summary>
        [Tooltip("The speed the button returns to its starting position when the interaction is ended.")]
        public float returnSpeedMultiplier = 5f;

        /// <summary>
        /// Audio clip played when the button returns to its up position.
        /// </summary>
        [Tooltip("Audio clip played when the button returns to its up position.")]
        public AudioClip sfxButtonUp;

        /// <summary>
        /// Audio clip played when the button reaches its down position.
        /// </summary>
        [Tooltip("Audio clip played when the button reaches its down position.")]
        public AudioClip sfxButtonDown;

        [Space(5)] [SerializeField] [Tooltip("Invoked when the button is in the up position.")]
        private UnityEvent onButtonUp;
        
        [SerializeField] [Tooltip("Invoked every frame when the button is in the up position.")]
        private UnityEvent whileButtonUp;
        
        [SerializeField] [Tooltip("Invoked when the button is in the down position.")]
        private UnityEvent onButtonDown;
        
        [SerializeField] [Tooltip("Invoked every frame when the button is in the down position.")]
        private UnityEvent whileButtonDown;
        
        private AudioSource _audioSource;
        private Collider _buttonCollider;
        private VRBaseInteractor _interactor;
        private Vector3 _buttonStartingPosition;
        private float _maximumPositionY;
        private float _minimumPositionY;
        private float _interactorHeight;
        private bool _wasPreviouslyInDownPosition;
        private bool _wasPreviouslyInUpPosition;

        #endregion

        private void OnEnable() {
            _audioSource = GetComponent<AudioSource>();
            _buttonCollider = GetComponent<Collider>();
            
            var buttonPosition = transform.localPosition;
            _maximumPositionY = buttonPosition.y;
            _minimumPositionY = buttonPosition.y - _buttonCollider.bounds.size.y / 2f;
            _buttonStartingPosition = buttonPosition;
            
            ItsSystems.OnUpdate += OnUpdateCallback;
        }

        private void OnDisable() => ItsSystems.OnUpdate -= OnUpdateCallback;
        
        private void OnUpdateCallback(UpdateTime updateTime) {
            _interactor = Physics.OverlapSphere(transform.position, interactorDetectionRadius, interactorMask).Select(overlap => overlap.GetComponent<VRBaseInteractor>()).FirstOrDefault(overlapInteractor => overlapInteractor != null);

            if (_interactor != null) {
                var currentInteractorHeight = transform.root.InverseTransformPoint(_interactor.attachmentPoint.position).y;
                var interactorHeightDifference = _interactorHeight - currentInteractorHeight;
                _interactorHeight = currentInteractorHeight;

                SetButtonLocalYPosition(transform.localPosition.y - interactorHeightDifference);
            }
            else 
                SetButtonLocalYPosition(Mathf.Lerp(transform.localPosition.y, _buttonStartingPosition.y, Time.deltaTime * returnSpeedMultiplier));
            
            ProcessEvents();
        }

        private void SetButtonLocalYPosition(float interactorYPosition) {
            var newButtonPosition = transform.localPosition;
            newButtonPosition.y = Mathf.Clamp(interactorYPosition, _minimumPositionY, _maximumPositionY);
            transform.localPosition = newButtonPosition;
        }

        private void ProcessEvents() {
            var selfLocalPos = transform.localPosition;
            var isInUpPosition = Mathf.Abs(selfLocalPos.y - _maximumPositionY) < POINT_CHECK_TOLERANCE;
            var isInDownPosition = Mathf.Abs(selfLocalPos.y - _minimumPositionY) < POINT_CHECK_TOLERANCE;

            if (isInUpPosition)
                whileButtonUp.Invoke();
            
            if (isInDownPosition)
                whileButtonDown.Invoke();
            
            if (isInUpPosition && !_wasPreviouslyInUpPosition) {
                if (sfxButtonUp != null) 
                    _audioSource.PlayOneShot(sfxButtonUp);
                
                onButtonUp.Invoke();
            }

            if (isInDownPosition && !_wasPreviouslyInDownPosition) {
                if (sfxButtonDown != null) 
                    _audioSource.PlayOneShot(sfxButtonDown);
                
                onButtonDown.Invoke();
            }

            _wasPreviouslyInUpPosition = isInUpPosition;
            _wasPreviouslyInDownPosition = isInDownPosition;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, interactorDetectionRadius);
        }

        public void RefreshProblems() {
            if (interactorMask.value == 0)
                ItsVRProblemDebuggerEditor.SubmitProblem("Interactor mask is set to nothing. Set the mask to 'Everything' or assign a specific layer.", ItsVRProblemDebuggerEditor.ProblemLevels.Issue, transform);
            
            if (sfxButtonDown == null || sfxButtonUp == null)
                ItsVRProblemDebuggerEditor.SubmitProblem("Assign sound effects to the button interactable.", ItsVRProblemDebuggerEditor.ProblemLevels.Recommendation, transform);
        }
#endif
    }
}
