using ItsVR.Miscellaneous;
using UnityEngine;

namespace ItsVR.Core {
    /// <summary>
    /// Creates all It's VR dependencies when the application is loaded.
    /// </summary>
    /// <para>Author: Jack Randolph</para>
    public static class ItsDependencies {
        #region Variables
        
        private static GameObject DependencyFolderGameObject { get; set; }

        #endregion

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void BeforeSceneLoadCallback() {
            DependencyFolderGameObject = new GameObject("It's VR Dependencies");
            Object.DontDestroyOnLoad(DependencyFolderGameObject);
            
            var monoBehaviorCallbackGameObject = new GameObject("MonoBehaviorCallback");
            monoBehaviorCallbackGameObject.AddComponent<ItsVRMonoBehaviorCallback>();
            AddGameObjectToFolderGameObject(monoBehaviorCallbackGameObject);
        }

        private static void AddGameObjectToFolderGameObject(GameObject gameObject) {
            Object.DontDestroyOnLoad(gameObject);
            gameObject.transform.SetParent(DependencyFolderGameObject.transform);
        }
    }
}