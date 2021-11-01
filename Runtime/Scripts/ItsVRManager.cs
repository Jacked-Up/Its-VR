// This script was updated on 10/30/2021 by Jack Randolph.

using ItsVR.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItsVR {
    [HelpURL("https://jackedupstudios.com/its-vr-documentation-1#b47abff1-8a0b-4eb6-b03c-3617ba2beb61")]
    public static class ItsVRManager {
        #region Variables

        /// <summary>
        /// The player VR Rig in the scene.
        /// </summary>
        public static VRRig Rig {
            get {
                var rig = Object.FindObjectsOfType<VRRig>();
                
                if (rig.Length > 1) 
                    Debug.LogWarning("[Its VR Manager] Multiple VR Rigs were found in the scene.");
                if (rig.Length == 0)
                    Debug.LogWarning("[Its VR Manager] No VR Rigs were present in the scene.");
                
                return rig.Length > 0 ? rig[0] : null;
            }
        }
        
        /// <summary>
        /// The users dominate hand. (Defaults as right hand).
        /// </summary>
        public static Hand hand = Hand.Right;

        /// <summary>
        /// Invoked when the player dominate hand is changed.
        /// </summary>
        public static event ItsVRPlayerSettingsEvent DominateHandChanged;

        public delegate void ItsVRPlayerSettingsEvent(Hand hand);

        #endregion
        
        /// <summary>
        /// Sets the dominate hand.
        /// </summary>
        /// <param name="newHand"></param>
        public static void SetDominateHand(Hand newHand) {
            if (newHand == hand) return;
            
            hand = newHand;
            DominateHandChanged?.Invoke(newHand);
        }
    }
    
    /// <summary>
    /// Hands.
    /// </summary>
    public enum Hand { Right, Left }
}
