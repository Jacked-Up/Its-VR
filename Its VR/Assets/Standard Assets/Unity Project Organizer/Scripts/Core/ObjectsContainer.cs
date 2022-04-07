using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JackedUp.Core {
    /// <summary>
    /// A system for storing game objects in scene folders.
    /// </summary>
    /// <para>Author: Jack Randolph</para>
    public static class ObjectsContainer {
        #region Variables 

        /// <summary>
        /// The root object container.
        /// </summary>
        public static Transform RootObjectContainer { get; private set; }
        
        /// <summary>
        /// A list of all of the object containers created.
        /// </summary>
        public static List<ObjectContainer> ObjectContainers { get; private set; } = new List<ObjectContainer>();

        /// <summary>
        /// Fetches the object container by name.
        /// </summary>
        /// <param name="name">The name of the object container to return.</param>
        /// <returns>The object container.</returns>
        public static ObjectContainer GetContainerFolderByName(string name) {
            foreach (var containerFolder in ObjectContainers.Where(containerFolder => containerFolder.objectContainerName == name))
                return containerFolder;

            return new ObjectContainer();
        }
        
        private static List<ContainerObject> _nonPersistentContainerObjects = new List<ContainerObject>();

        #endregion

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoadCallback() {
            SceneManager.sceneUnloaded += SceneUnloadedCallback;
            
            RootObjectContainer = new GameObject("Objects Container").transform;
            UnityEngine.Object.DontDestroyOnLoad(RootObjectContainer);
        }

        /// <summary>
        /// Moves the game object in to the scene container folder.
        /// If the container folder doesn't exist, the container folder will be created.
        /// </summary>
        /// <param name="newContainerObject">The object to be put into the object container.</param>
        public static void MoveGameObjectToContainerFolder(ContainerObject newContainerObject) {
            if (newContainerObject.containerObject == null) {
#if UNITY_EDITOR
                Debug.LogError("The game object was null and could not be added to a container folder.");
#endif
                return;
            }

            // Create object container if one does not exist
            if (!ObjectContainers.Contains(GetContainerFolderByName(newContainerObject.objectContainerName))) {
                var newObjectContainer = new GameObject(newContainerObject.objectContainerName).transform;
                UnityEngine.Object.DontDestroyOnLoad(newObjectContainer);
                newObjectContainer.transform.SetParent(RootObjectContainer);

                var newContainerFolder = new ObjectContainer {
                    objectContainerName = newContainerObject.objectContainerName,
                    containerRoot = newObjectContainer
                };
                ObjectContainers ??= new List<ObjectContainer>();
                ObjectContainers.Add(newContainerFolder);
            }

            UnityEngine.Object.DontDestroyOnLoad(newContainerObject.containerObject);
            newContainerObject.containerObject.SetParent(GetContainerFolderByName(newContainerObject.objectContainerName).containerRoot);
            
            if (newContainerObject.isPersistent)
                return;

            _nonPersistentContainerObjects ??= new List<ContainerObject>();
            _nonPersistentContainerObjects.Add(newContainerObject);
        }
        
        private static void SceneUnloadedCallback(Scene scene) {
            if (RootObjectContainer == null) {
#if UNITY_EDITOR
                Debug.LogWarning("The root container folder seems to have been destroyed. Repairing...");
#endif
                
                RootObjectContainer = new GameObject("Object Container (Repair)").transform;
                UnityEngine.Object.DontDestroyOnLoad(RootObjectContainer);
            }
            
            foreach (var containerObject in _nonPersistentContainerObjects.Where(containerObject => containerObject.containerObject != null)) 
                UnityEngine.Object.Destroy(containerObject.containerObject);

            if (ObjectContainers != null) {
                foreach (var containerFolder in ObjectContainers.Where(containerFolder => containerFolder.containerRoot.childCount == 0)) {
                    ObjectContainers.Remove(containerFolder);
                    UnityEngine.Object.Destroy(containerFolder.containerRoot);
                }
            }
        }
    }
    
    /// <summary>
    /// Information about an object container.
    /// </summary>
    [Serializable]
    public struct ObjectContainer {
        public string objectContainerName;
        public Transform containerRoot;
    }
    
    /// <summary>
    /// Information about an object inside an object container.
    /// </summary>
    [Serializable]
    public struct ContainerObject {
        public string objectContainerName;
        public Transform containerObject;
        public bool isPersistent;
    }
}
