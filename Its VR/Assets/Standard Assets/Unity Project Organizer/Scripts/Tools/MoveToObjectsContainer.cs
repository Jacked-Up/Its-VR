using System.Collections.Generic;
using System.Linq;
using JackedUp.Core;
using UnityEngine;

namespace JackedUp.Tools {
    /// <summary>
    /// Moves the referenced game objects into the object containers requested.
    /// This component runs at start and destroys itself when it is finished.
    /// </summary>
    /// <para>Author: Jack Randolph</para>
    [DisallowMultipleComponent]
    public class MoveToObjectsContainer : MonoBehaviour {
        #region Variables
        
        /// <summary>
        /// The game object(s) to move into the object container(s).
        /// The game object(s) are moved on start.
        /// </summary>
        [Tooltip("The game object(s) to move into the object container(s).")]
        public List<ContainerObject> gameObjectsToMove = new List<ContainerObject>();

        #endregion

        private void Start() {
            if (gameObjectsToMove.Count == 0 || gameObjectsToMove == null) {
                Destroy(gameObject);
                return;
            }

            foreach (var gameObjectToMove in gameObjectsToMove.Where(gameObjectToContain => gameObjectToContain.containerObject != null)) 
                ObjectsContainer.MoveGameObjectToContainerFolder(gameObjectToMove);
            
            Destroy(gameObject);
        }
    }
}