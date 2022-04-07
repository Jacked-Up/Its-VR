// This script was updated on 02/20/2022 by Jack Randolph.

using ItsVR.Player;
using UnityEngine;

namespace ItsVR.Locomotion {
    /// <summary>
    /// Base class for all It's VR locomotion methods.
    /// </summary>
    [RequireComponent(typeof(VRRig))]
    public class VRBaseLocomotion : MonoBehaviour {
        #region Variables

        /// <summary>
        /// The controller which this script reads input from.
        /// </summary>
        [Tooltip("The controller which this script reads input from.")]
        public VRController inputController;

        #endregion
    }
}