using ItsVR.Core;
using ItsVR.Player;
using ItsVR.Scriptables;
using UnityEditor;
using UnityEngine;

namespace ItsVR.Editor {
    /// <summary>
    /// It's VR VR controller component editor.
    /// </summary>
    /// <para>Author: Jack Randolph</para>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(VRController))]
    public class VRControllerEditor : UnityEditor.Editor {
        #region Variables
        
        private SerializedProperty _onTriggerPressedEventProperty;
        private SerializedProperty _onTriggerDepressChangedEventProperty;
        private SerializedProperty _onGripPressedEventProperty;
        private SerializedProperty _onGripDepressChangedEventProperty;
        private SerializedProperty _onPrimaryButtonPressedEventProperty;
        private SerializedProperty _onSecondaryButtonPressedEventProperty;
        private SerializedProperty _onJoystickPressedEventProperty;
        private SerializedProperty _onJoystickPositionChangedEventProperty;
        private bool _showInputEvents;
        private bool _showTriggerInputEvents;
        private bool _showGripInputEvents;
        private bool _showPrimaryButtonInputEvents;
        private bool _showSecondaryButtonInputEvents;
        private bool _showJoystickInputEvents;
        
        #endregion

        private void OnEnable() {
            _onTriggerPressedEventProperty = serializedObject.FindProperty("onTriggerPressed");
            _onTriggerDepressChangedEventProperty = serializedObject.FindProperty("onTriggerDepressChanged");
            _onGripPressedEventProperty = serializedObject.FindProperty("onGripPressed");
            _onGripDepressChangedEventProperty = serializedObject.FindProperty("onGripDepressChanged");
            _onPrimaryButtonPressedEventProperty = serializedObject.FindProperty("onPrimaryButtonPressed");
            _onSecondaryButtonPressedEventProperty = serializedObject.FindProperty("onSecondaryButtonPressed");
            _onJoystickPressedEventProperty = serializedObject.FindProperty("onJoystickPressed");
            _onJoystickPositionChangedEventProperty = serializedObject.FindProperty("onJoystickPositionChanged");
        }

        public override void OnInspectorGUI() {
            var instance = (VRController)target;

#pragma warning disable CS0618
            instance.inputContainer = (VRInputContainer)EditorGUILayout.ObjectField("Input Container", instance.inputContainer, typeof(VRInputContainer));
#pragma warning restore CS0618
            
            if (instance.inputContainer == null) {
                EditorGUILayout.HelpBox("Assign a VR input container.", MessageType.Info);
                return;
            }

            instance.handDefinition = (HandDefinitions)EditorGUILayout.EnumPopup("Hand Definition", instance.handDefinition);

            _showInputEvents = EditorGUILayout.Foldout(_showInputEvents, new GUIContent("Input Events"));
            
            GUILayout.Space(5);
            if (_showInputEvents) {
                serializedObject.Update();
            
                _showTriggerInputEvents = EditorGUILayout.Foldout(_showTriggerInputEvents, new GUIContent("Trigger"));

                if (_showTriggerInputEvents) {
                    EditorGUILayout.PropertyField(_onTriggerPressedEventProperty);
                    EditorGUILayout.PropertyField(_onTriggerDepressChangedEventProperty);
                }

                _showGripInputEvents = EditorGUILayout.Foldout(_showGripInputEvents, new GUIContent("Grip"));
                
                if (_showGripInputEvents) {
                    EditorGUILayout.PropertyField(_onGripPressedEventProperty);
                    EditorGUILayout.PropertyField(_onGripDepressChangedEventProperty);
                }

                _showPrimaryButtonInputEvents = EditorGUILayout.Foldout(_showPrimaryButtonInputEvents, new GUIContent("Primary Button"));
                
                if (_showPrimaryButtonInputEvents) {
                    EditorGUILayout.PropertyField(_onPrimaryButtonPressedEventProperty);
                }

                _showSecondaryButtonInputEvents = EditorGUILayout.Foldout(_showSecondaryButtonInputEvents, new GUIContent("Secondary Button"));
                
                if (_showSecondaryButtonInputEvents) {
                    EditorGUILayout.PropertyField(_onSecondaryButtonPressedEventProperty);
                }

                _showJoystickInputEvents = EditorGUILayout.Foldout(_showJoystickInputEvents, new GUIContent("Joystick"));
                
                if (_showJoystickInputEvents) {
                    EditorGUILayout.PropertyField(_onJoystickPressedEventProperty);
                    EditorGUILayout.PropertyField(_onJoystickPositionChangedEventProperty);
                }

                serializedObject.ApplyModifiedProperties();
            }
            
            if (Application.isPlaying && instance.inputContainer != null) {
                GUILayout.Space(5);
                if (GUILayout.Button("Open Input Debugger"))
                    ItsVRInputDebuggerEditor.OpenWindowWithInputContainer(instance.inputContainer);
            }
        }
    }
}