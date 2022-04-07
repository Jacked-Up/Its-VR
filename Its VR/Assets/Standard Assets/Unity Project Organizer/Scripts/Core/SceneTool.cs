using UnityEngine;

namespace JackedUp.Core {
    /// <summary>
    /// Tool for instantiating and destroying scene folders.
    /// </summary>
    /// <para>Author: Jack Randolph</para>
    public static class SceneTool {
        #region Variables

        /// <summary>
        /// Checks if the folder exists,
        /// </summary>
        /// <param name="sceneFolder">Scene folder to check.</param>
        /// <returns>True if the folder exists.</returns>
        public static bool FolderExists(SceneFolders sceneFolder) {
            var sceneFolderFound = GameObject.Find(sceneFolder.ToString());
            return sceneFolderFound != null && !sceneFolderFound.transform.IsChildOf(sceneFolderFound.transform.root);
        }

        #endregion

        /// <summary>
        /// Instantiates a scene folder.
        /// </summary>
        /// <param name="sceneFolder">The scene folder to create.</param>
        public static GameObject InstantiateFolder(SceneFolders sceneFolder) {
#if UNITY_EDITOR
            if (FolderExists(sceneFolder))
                Debug.LogWarning($"A scene folder named <b>{sceneFolder}</b> already exists in the scene.");
#endif

            return new GameObject(sceneFolder.ToString());
        }

        /// <summary>
        /// Destroys a scene folder.
        /// </summary>
        /// <param name="sceneFolder">The scene folder to delete.</param>
        /// <param name="destroyStoredObjects">If all of the objects inside of the scene folder should be destroyed.</param>
        public static void DestroyFolder(SceneFolders sceneFolder, bool destroyStoredObjects) {
            if (!FolderExists(sceneFolder)) {
#if UNITY_EDITOR
                Debug.LogError($"Failed to destroy a scene folder named <b>{sceneFolder}</b> because it doesn't exist inside the scene.");
#endif
                return;
            }

            var sceneFolderObject = GameObject.Find(sceneFolder.ToString());
            
            if (!destroyStoredObjects)
                sceneFolderObject.transform.DetachChildren();

            Object.Destroy(sceneFolderObject);
        }

        /// <summary>
        /// Adds the game object to the scene folder specified.
        /// </summary>
        /// <param name="sceneFolder">The scene folder to put the game object into. If the scene folder doesn't exist, it will be created.</param>
        /// <param name="gameObjectToAdd">The game object to add to the scene folder.</param>
        public static void AddToFolder(SceneFolders sceneFolder, GameObject gameObjectToAdd) {
            if (gameObjectToAdd == null) {
#if UNITY_EDITOR
                Debug.LogError("The game object was null and could not be added to the scene folder.");
#endif
                return;
            }
            
            if (!FolderExists(sceneFolder))
                InstantiateFolder(sceneFolder);

            gameObjectToAdd.transform.SetParent(GameObject.Find(sceneFolder.ToString()).transform);
        }
    }

    /// <summary>
    /// A list of all of the scene folders.
    /// </summary>
    public enum SceneFolders {
        Scene,
        Static,
        Dynamic,
        Lighting,
        Logic,
        GUI,
        Miscellaneous,
        Developer
    }
}
