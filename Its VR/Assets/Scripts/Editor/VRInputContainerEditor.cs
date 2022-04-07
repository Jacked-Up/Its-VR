using ItsVR.Scriptables;
using UnityEditor;
using UnityEngine;

namespace ItsVR.Editor {
    /// <summary>
    /// Custom inspector UI VR input container.
    /// </summary>
    /// <para>Author: Jack Randolph</para>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(VRInputContainer))]
    public class VRInputContainerEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            if (Application.isPlaying) {
                var instance = (VRInputContainer)target;
            
                GUILayout.Space(5);
                GUILayout.BeginHorizontal();

                    if (GUILayout.Button(instance.AllInputsWereRegistered ? "Unregister All Inputs" : "Register All Inputs")) {
                        if (instance.AllInputsWereRegistered)
                            instance.UnregisterAllInputs();
                        else
                            instance.RegisterAllInputs();
                    }
                        
                    if (GUILayout.Button("Open In Input Debugger"))
                        ItsVRInputDebuggerEditor.OpenWindowWithInputContainer(instance);

                GUILayout.EndHorizontal();
            }
            
            GUILayout.Space(5);
            EditorGUILayout.HelpBox("Remember to always save your VR input container to a preset!", MessageType.Warning);
        }
    }
}