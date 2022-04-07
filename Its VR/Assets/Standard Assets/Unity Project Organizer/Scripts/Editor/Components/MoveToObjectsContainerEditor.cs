using JackedUp.Tools;
using UnityEditor;

namespace JackedUp.Editor.Components {
    /// <summary>
    /// Editor for the 'MoveToContainerFolder' component.
    /// </summary>
    /// <para>Author: Jack Randolph</para>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MoveToObjectsContainer))]
    public class MoveToObjectsContainerEditor : UnityEditor.Editor {
        #region Variables

        private SerializedProperty _gameObjectsToMoveProperty;

        #endregion

        private void OnEnable() => _gameObjectsToMoveProperty = serializedObject.FindProperty("gameObjectsToMove");

        public override void OnInspectorGUI() {
            serializedObject.Update();
            EditorGUILayout.PropertyField(_gameObjectsToMoveProperty);
            serializedObject.ApplyModifiedProperties();
            
            // Check for game objects referenced multiple times
            var instance = (MoveToObjectsContainer)target;
            for (var y = 0; y < instance.gameObjectsToMove.Count; y++) {
                var objectToCheck = instance.gameObjectsToMove[y].containerObject;
                var detectedMultipleReferences = false;
                
                if (objectToCheck == null)
                    continue;
                
                for (var i = 0; i < instance.gameObjectsToMove.Count; i++) {
                    if (i == y)
                        continue;
                    
                    if (objectToCheck != instance.gameObjectsToMove[i].containerObject) 
                        continue;

                    detectedMultipleReferences = true;
                    break;
                }

                if (!detectedMultipleReferences) 
                    continue;
                
                EditorGUILayout.HelpBox("Detected multiple references of the same game object(s).", MessageType.Error);
                break;
            }
        }
    }
}