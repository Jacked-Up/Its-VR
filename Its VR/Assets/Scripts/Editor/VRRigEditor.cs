using System.Collections.Generic;
using System.Globalization;
using ItsVR.Player;
using UnityEditor;
using UnityEngine;

namespace ItsVR.Editor {
    /// <summary>
    /// It's VR VR Rigs component editor.
    /// </summary>
    /// <para>Author: Jack Randolph</para>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(VRRig))]
    public class VRRigEditor : UnityEditor.Editor {
        #region Variables

        private SerializedProperty _headTransform;
        private SerializedProperty _bodyOffsetTransform;

        #endregion

        private void OnEnable() {
            _headTransform = serializedObject.FindProperty("head");
            _bodyOffsetTransform = serializedObject.FindProperty("bodyOffset");
        }

        public override void OnInspectorGUI() {
            var instance = (VRRig)target;

            if (instance.transform.localScale.x != instance.transform.localScale.y || instance.transform.localScale.x != instance.transform.localScale.z || instance.transform.localScale.y != instance.transform.localScale.z || instance.bodyOffset.localScale != Vector3.one) {
                GUILayout.BeginHorizontal();
                
                    EditorGUILayout.HelpBox("Scaling the VR rig can cause unexpected behaviors. Reset the scale to repair scaling.", MessageType.Warning);

                    if (GUILayout.Button("Reset scaling", GUILayout.Height(38))) {
                        instance.transform.localScale = Vector3.one;
                        
                        if (instance.bodyOffset != null)
                            instance.bodyOffset.localScale = Vector3.one;
                    }
                
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            }

            serializedObject.Update();
            
            EditorGUILayout.PropertyField(_headTransform);
            EditorGUILayout.PropertyField(_bodyOffsetTransform);

            serializedObject.ApplyModifiedProperties();

            instance.HeightMethod = (HeightMethodTypes)EditorGUILayout.EnumPopup("Height Method", instance.HeightMethod);
            
            if (instance.HeightMethod == HeightMethodTypes.Offset) {
                instance.OffsetHeight = EditorGUILayout.FloatField(" Height Offset", instance.OffsetHeight);
                GUILayout.Space(2);
            }

            instance.Width = EditorGUILayout.FloatField("Width", instance.Width);

            ItsVREditorElements.GUIInfoElement(new List<ItsVREditorElements.InfoEntry> {
                new ItsVREditorElements.InfoEntry { name = "Height", value = instance.Height.ToString(CultureInfo.InvariantCulture), pixelSpace = 0 },
                new ItsVREditorElements.InfoEntry { name = "Width", value = instance.Width.ToString(CultureInfo.InvariantCulture), pixelSpace = 0 },
                new ItsVREditorElements.InfoEntry { name = "Head Position", value = instance.GetHeadPosition().ToString(), pixelSpace = 5 },
                new ItsVREditorElements.InfoEntry { name = "Estimated Hip Position", value = instance.GetEstimatedHipPosition().ToString(), pixelSpace = 0 },
                new ItsVREditorElements.InfoEntry { name = "Estimated Feet Position", value = instance.GetEstimatedFeetPosition().ToString(), pixelSpace = 0 }
            });
        }
    }
}