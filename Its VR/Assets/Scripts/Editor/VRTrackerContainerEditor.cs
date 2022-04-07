using UnityEditor;
using UnityEngine;

namespace ItsVR.Editor {
    /// <summary>
    /// Custom inspector UI VR tracker container.
    /// </summary>
    /// <para>Author: Jack Randolph</para>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ItsVR.Scriptables.VRTrackerContainer))]
    public class VRTrackerContainerEditor : UnityEditor.Editor {
        #region Variables

        private SerializedProperty _trackerPosition;
        private SerializedProperty _trackerRotation;
        private SerializedProperty _trackingStatus;
        private SerializedProperty _updateFrequency;
        private SerializedProperty _enableAllActionsOnLoad;
        private SerializedProperty _preventActionDisable;

        #endregion

        private void OnEnable() {
            _trackerPosition = serializedObject.FindProperty("trackerPosition");
            _trackerRotation = serializedObject.FindProperty("trackerRotation");
            _trackingStatus = serializedObject.FindProperty("trackingStatus");
            _updateFrequency = serializedObject.FindProperty("updateFrequency");
            _enableAllActionsOnLoad = serializedObject.FindProperty("enableAllActionsOnLoad");
            _preventActionDisable = serializedObject.FindProperty("preventActionDisable");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_trackerPosition);
            EditorGUILayout.PropertyField(_trackerRotation);
            EditorGUILayout.PropertyField(_trackingStatus);
            
            GUILayout.Space(5);
            EditorGUILayout.PropertyField(_updateFrequency);
            EditorGUILayout.PropertyField(_enableAllActionsOnLoad);
            EditorGUILayout.PropertyField(_preventActionDisable);
            
            serializedObject.ApplyModifiedProperties();

            // Draws register/unregister button to the inspector while in playmode
            var instance = (ItsVR.Scriptables.VRTrackerContainer)target;
            if (Application.isPlaying && GUILayout.Button(instance.AllActionsRegistered ? "Unregister" : "Register")) {
                if (instance.AllActionsRegistered)
                    instance.DisableActions();
                else
                    instance.EnableActions();
            }
        }
    }   
}