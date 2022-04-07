// This script was updated on 03/20/2022 by Jack Randolph.

using UnityEngine;

namespace ItsVR.Miscellaneous {
    /// <summary>
    /// Communicates with other scripts when certain loop event functions are invoked via a callback.
    /// </summary>
    public class ItsVRMonoBehaviorCallback : MonoBehaviour {
        #region Variables

        /// <summary>
        /// Invoked when the update event function is fired.
        /// </summary>
        public static event UpdateTimeCallback UpdateCallback;
        
        /// <summary>
        /// Invoked when the late update event function is fired.
        /// </summary>
        public static event UpdateTimeCallback LateUpdateCallback;
        
        /// <summary>
        /// Invoked when the fixed update event function is fired.
        /// </summary>
        public static event UpdateTimeCallback FixedUpdateCallback;
        
        public delegate void UpdateTimeCallback();

        #endregion

        private void Update() => UpdateCallback?.Invoke();

        private void LateUpdate() => LateUpdateCallback?.Invoke();

        private void FixedUpdate() => FixedUpdateCallback?.Invoke();
    }
}
