// This script was updated on 11/7/2021 by Jack Randolph.

using System.Collections.Generic;
using ItsVR.Player;
using UnityEngine;

namespace ItsVR {
    [HelpURL("https://jackedupstudios.com/its-vr-documentation-1#b47abff1-8a0b-4eb6-b03c-3617ba2beb61")]
    public static class ItsVR {
        #region Variables

        /// <summary>
        /// The player VR Rig in the scene.
        /// </summary>
        public static VRRig Rig {
            get {
                var rig = Object.FindObjectsOfType<VRRig>();
                
                if (rig.Length > 1) 
                    Debug.LogError("[It's VR Manager] Multiple VR Rigs were found in the scene.");
                if (rig.Length == 0)
                    Debug.LogError("[It's VR Manager] No VR Rigs were present in the scene.");
                
                return rig.Length > 0 ? rig[0] : null;
            }
        }

        /// <summary>
        /// The players right hand controller.
        /// </summary>
        public static VRController RightController {
            get {
                var controllers = Object.FindObjectsOfType<VRController>();

                if (controllers.Length == 0) {
                    Debug.LogError("[It's VR Manager] No controllers were found in the scene.");
                    return null;
                }

                var rightControllers = new List<VRController>();
                
                foreach (var controller in controllers) {
                    if (controller.handSide != Hand.Right) continue;
                    rightControllers.Add(controller);
                    break;
                }

                if (rightControllers.Count == 0) {
                    Debug.LogError("[It's VR Manager] No right hand controller was found in the scene.");
                    return null;
                }
                
                if (rightControllers.Count > 1)
                    Debug.LogError("[It's VR Manager] Multiple right hand controllers were found in the scene. Returning the first right hand controller found.", controllers[0]);

                return controllers[0];
            }
        }
        
        /// <summary>
        /// The players left hand controller.
        /// </summary>
        public static VRController LeftController {
            get {
                var controllers = Object.FindObjectsOfType<VRController>();

                if (controllers.Length == 0) {
                    Debug.LogError("[It's VR Manager] No controllers were found in the scene.");
                    return null;
                }

                var leftControllers = new List<VRController>();
                
                foreach (var controller in controllers) {
                    if (controller.handSide != Hand.Left) continue;
                    leftControllers.Add(controller);
                    break;
                }

                if (leftControllers.Count == 0) {
                    Debug.LogError("[It's VR Manager] No left hand controller was found in the scene.");
                    return null;
                }
                
                if (leftControllers.Count > 1)
                    Debug.LogError("[It's VR Manager] Multiple left hand controllers were found in the scene. Returning the first left hand controller found.", controllers[0]);

                return controllers[0];
            }
        }
        
        /// <summary>
        /// The players dominate hand controller.
        /// </summary>
        public static VRController DominateController {
            get {
                var controllers = Object.FindObjectsOfType<VRController>();

                if (controllers.Length == 0) {
                    Debug.LogError("[It's VR Manager] No controllers were found in the scene.");
                    return null;
                }

                var dominateControllers = new List<VRController>();
                
                foreach (var controller in controllers) {
                    if (controller.handSide != dominateHand) continue;
                    dominateControllers.Add(controller);
                    break;
                }

                if (dominateControllers.Count == 0) {
                    Debug.LogError("[It's VR Manager] No dominate hand controller was found in the scene.");
                    return null;
                }
                
                if (dominateControllers.Count > 1)
                    Debug.LogError("[It's VR Manager] Multiple dominate hand controllers were found in the scene. Returning the first dominate hand controller found.", controllers[0]);

                return controllers[0];
            }
        }
        
        /// <summary>
        /// The users dominate hand. (Defaults as right hand).
        /// </summary>
        public static Hand dominateHand = Hand.Right;

        /// <summary>
        /// Invoked when the player dominate hand is changed.
        /// </summary>
        public static event ItsVRPlayerSettingsEvent DominateHandChanged;

        public delegate void ItsVRPlayerSettingsEvent(Hand hand);

        #endregion
        
        /// <summary>
        /// Sets the dominate hand.
        /// </summary>
        /// <param name="newHand">Hand side to set as the dominate hand.</param>
        public static void SetDominateHand(Hand newHand) {
            if (newHand == dominateHand) return;
            
            dominateHand = newHand;
            DominateHandChanged?.Invoke(newHand);
        }
    }
    
    /// <summary>
    /// Hand sides.
    /// </summary>
    public enum Hand { Right, Left }
}
