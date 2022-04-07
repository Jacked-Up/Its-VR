using ItsVR.Miscellaneous;
using UnityEngine;

namespace ItsVR.Core {
    /// <summary>
    /// The central main class for It's VR.
    /// 
    /// Contains very important update timing events which synchronizes all It's VR components.
    /// 
    /// Also contains a property for defining the dominate hand. The 'PlayersDominateHand' property will save to
    /// the PlayerPrefs class when set.
    /// </summary>
    /// <para>Author: Jack Randolph</para>
    public static class ItsSystems {
        #region Variables

        /// <summary>
        /// The defined dominate hand.
        /// </summary>
        public static HandDefinitions PlayersDominateHand {
            get => PlayerPrefs.GetInt(DOMINATE_HAND_SAVE_KEY, DOMINATE_HAND_DEFAULT) == 0 ? HandDefinitions.Right : HandDefinitions.Left;
            set {
                PlayerPrefs.SetInt(DOMINATE_HAND_SAVE_KEY, value == HandDefinitions.Right ? 0 : 1);
                DominateHandSet?.Invoke(value);
            }
        }

        private const string DOMINATE_HAND_SAVE_KEY = "ITS_VR_DOMINATE_HAND";
        private const int DOMINATE_HAND_DEFAULT = 0;

        /// <summary>
        /// Invoked before update occurs.
        /// </summary>
        public static event UpdateEvent BeforeUpdate;
        
        /// <summary>
        /// Invoked when update occurs.
        /// </summary>
        public static event UpdateEvent OnUpdate;
        
        /// <summary>
        /// Invoked before and when an update occurs.
        /// </summary>
        public static event UpdateEvent BeforeUpdateAndOnUpdate;
        
        public delegate void UpdateEvent(UpdateTime updateTime);

        /// <summary>
        /// Invoked when the dominate hand is set.
        /// </summary>
        public static event DominateHandCallback DominateHandSet;
        
        public delegate void DominateHandCallback(HandDefinitions handDefinition);

        #endregion

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AfterSceneLoadCallback() {
            UnityEngine.InputSystem.InputSystem.onBeforeUpdate += BeforeUpdateCallback;
            ItsVRMonoBehaviorCallback.UpdateCallback += OnUpdateCallback;
        }

        private static void BeforeUpdateCallback() {
            BeforeUpdate?.Invoke(UpdateTime.BeforeUpdate);
            BeforeUpdateAndOnUpdate?.Invoke(UpdateTime.BeforeUpdate);
        }
        
        private static void OnUpdateCallback() {
            OnUpdate?.Invoke(UpdateTime.OnUpdate);
            BeforeUpdateAndOnUpdate?.Invoke(UpdateTime.OnUpdate);
        }
    }

    /// <summary>
    /// Hand definition sides.
    /// </summary>
    public enum HandDefinitions { Right, Left }
    
    /// <summary>
    /// Update times in the update loop.
    /// </summary>
    public enum UpdateTime { BeforeUpdate, OnUpdate }
}