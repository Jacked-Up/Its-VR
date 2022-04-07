using UnityEditor;
using UnityEngine;

namespace JackedUp.Core {
    /// <summary>
    /// Tool for creating and deleting folders.
    /// </summary>
    /// <para>Author: Jack Randolph</para>
    public static class FolderTool {
        #region Variables

        /// <summary>
        /// The name of the projects root folder. (Should be 'Assets')
        /// </summary>
        public const string ROOT_FOLDER = "Assets";

        /// <summary>
        /// Checks if the folder exists at the path.
        /// </summary>
        /// <param name="parentFolder">The root parent folder.</param>
        /// <param name="subfolderName">Optional name of the subfolder located within the root parent folder.</param>
        /// <returns>True if the folder exists at the path specified.</returns>
        public static bool FolderExists(ParentFolders parentFolder, string subfolderName = null) 
            => AssetDatabase.IsValidFolder($"{ROOT_FOLDER}/{parentFolder}" + (string.IsNullOrEmpty(subfolderName) ? string.Empty : $"/{subfolderName}"));

        #endregion

        /// <summary>
        /// Creates a folder at the specified path.
        /// </summary>
        /// <param name="parentFolder">The root parent folder.</param>
        /// <param name="subfolderName">Optional name of the subfolder to create. LEAVE THIS BLANK IF YOU'D LIKE TO
        /// CREATE THE PARENT FOLDER ONLY.</param>
        public static void CreateFolder(ParentFolders parentFolder, string subfolderName = null) {
            if (Application.isPlaying) {
#if UNITY_EDITOR
                Debug.LogError("You cannot create folders while in play mode.");
#endif
                return;
            }

            if (!FolderExists(parentFolder)) 
                AssetDatabase.GUIDToAssetPath(AssetDatabase.CreateFolder(ROOT_FOLDER, parentFolder.ToString()));

            if (string.IsNullOrEmpty(subfolderName) || FolderExists(parentFolder, subfolderName))
                return;

            AssetDatabase.GUIDToAssetPath(AssetDatabase.CreateFolder($"{ROOT_FOLDER}/{parentFolder}", subfolderName));
        }

        /// <summary>
        /// Deletes the folder at the path specified.
        /// </summary>
        /// <param name="parentFolder">The root parent folder.</param>
        /// <param name="subfolderName">Optional name of the subfolder to delete. LEAVE THIS BLANK IF YOU'D LIKE TO
        /// DELETE THE PARENT FOLDER ONLY.</param>
        public static void DeleteFolder(ParentFolders parentFolder, string subfolderName = null) {
            if (Application.isPlaying) {
#if UNITY_EDITOR
                Debug.LogError("You cannot delete folders while in play mode.");
#endif
                return;
            }
            
            AssetDatabase.DeleteAsset($"{ROOT_FOLDER}/{parentFolder}" + (!string.IsNullOrEmpty(subfolderName) ? $"/{subfolderName}" : string.Empty));
        }
    }

    /// <summary>
    /// A list of all of the parent folders. All assets should fit into at least one of these categories.
    /// </summary>
    public enum ParentFolders {
        Animations,
        Audio,
        Materials,
        Models,
        Prefabs,
        Resources,
        Scenes,
        Scripts,
        Textures,
        Shaders,
        Fonts,
        Physics,
        Editor,
        Settings,
        Plugins,
        Extensions,
        Presets
    }
}
