using System.Collections.Generic;
using System.Globalization;
using ItsVR.Player;
using UnityEditor;

namespace ItsVR.Editor {
    /// <summary>
    /// It's VR VR Tracker component editor.
    /// </summary>
    /// <para>Author: Jack Randolph</para>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(VRTracker))]
    public class VRTrackerEditor : UnityEditor.Editor {
        #region Variables

        private SerializedProperty _trackerContainer;
        private SerializedProperty _trackingConstraint;

        #endregion

        private void OnEnable() {
            _trackerContainer = serializedObject.FindProperty("trackerContainer");
            _trackingConstraint = serializedObject.FindProperty("trackingConstraint");
        }

        public override void OnInspectorGUI() {
            var instance = (VRTracker)target;

            serializedObject.Update();

            EditorGUILayout.PropertyField(_trackerContainer);

            if (instance.trackerContainer == null) {
                EditorGUILayout.HelpBox("Assign a VR tracker container.", MessageType.Info);
                serializedObject.ApplyModifiedProperties();
            }
            else {
                EditorGUILayout.PropertyField(_trackingConstraint);
                instance.trackerContainer.updateFrequency = (UpdateFrequencies)EditorGUILayout.EnumPopup("Update Frequency", instance.trackerContainer.updateFrequency);
                
                serializedObject.ApplyModifiedProperties();
                
                ItsVREditorElements.GUIInfoElement(new List<ItsVREditorElements.InfoEntry> {
                    new ItsVREditorElements.InfoEntry { name = "Tracking", value = instance.IsTracking.ToString(), pixelSpace = 0 },
                    new ItsVREditorElements.InfoEntry { name = "Paused", value = instance.IsPaused.ToString(), pixelSpace = 0 },
                    new ItsVREditorElements.InfoEntry { name = "Speed", value = instance.Speed.ToString(CultureInfo.InvariantCulture), pixelSpace = 5 },
                    new ItsVREditorElements.InfoEntry { name = "Velocity", value = instance.Velocity.ToString(), pixelSpace = 0 },
                    new ItsVREditorElements.InfoEntry { name = "Angular Velocity", value = instance.AngularVelocity.ToString(), pixelSpace = 0 }
                });
            }
        }
    }
}