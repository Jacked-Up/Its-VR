// This script was updated on 03/22/2022 by Jack Randolph.

using System.Globalization;
using ItsVR.Scriptables;
using UnityEditor;
using UnityEngine;

namespace ItsVR.Editor {
    /// <summary>
    /// Debugs VR input containers.s
    /// </summary>
    public class ItsVRInputDebuggerEditor : EditorWindow {
        #region Variables

        private const string EDITOR_WINDOW_TITLE = "It's VR Input Debugger";
        
        private static VRInputContainer _vrInputContainerToDebug;
        private static Vector2 _scrollPosition;
        private static bool _showUniversalInputs = true;
        private static bool _showMetaInputs;
        private static bool _showViveInputs;
        private static bool _showValveInputs;
        private float _hapticsAmplitude = 1f;
        private float _hapticsDuration = 2f;

        #endregion

        private void Reset() {
            _vrInputContainerToDebug = null;
            _showUniversalInputs = true;
            _showMetaInputs = false;
            _showViveInputs = false;
            _showValveInputs = false;
        }

        private void OnGUI() {
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            
                ItsVREditorElements.GUILabelElement("  It's VR", ItsVREditorElements.TextColors.White, ItsVREditorElements.TextStyleLarge, true);
                ItsVREditorElements.GUILabelElement("Input debugger", ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleMedium);
            
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            if (!Application.isPlaying) {
                EditorGUILayout.HelpBox("Input can only be debugged in play mode.", MessageType.Warning);
                return;
            }

#pragma warning disable CS0618
            _vrInputContainerToDebug = (VRInputContainer)EditorGUILayout.ObjectField(_vrInputContainerToDebug, typeof(VRInputContainer));
#pragma warning restore CS0618

            if (_vrInputContainerToDebug == null) {
                EditorGUILayout.HelpBox("Assign an input container to begin debugging.", MessageType.Info);
                return;
            }

            GUILayout.BeginVertical();
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            
            var container = _vrInputContainerToDebug;
            
            GUILayout.Space(5);
            _showUniversalInputs = EditorGUILayout.Foldout(_showUniversalInputs, new GUIContent("Universal"));

                if (_showUniversalInputs) {
                    ItsVREditorElements.GUILabelElement("Trigger Depress: " + container.universal.TriggerDepress.ToString(CultureInfo.InvariantCulture), ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);

                    GUILayout.BeginHorizontal(new GUIStyle {normal = new GUIStyleState {background = Texture2D.grayTexture}});
                    
                        ItsVREditorElements.GUILabelElement("Trigger Pressed: " + container.universal.TriggerPressed, ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);
                    
                    GUILayout.EndHorizontal();
                    
                    ItsVREditorElements.GUILabelElement("Grip Depress: " + container.universal.GripDepress.ToString(CultureInfo.InvariantCulture), ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);

                    GUILayout.BeginHorizontal(new GUIStyle {normal = new GUIStyleState {background = Texture2D.grayTexture}});
                    
                        ItsVREditorElements.GUILabelElement("Grip Pressed: " + container.universal.TriggerPressed, ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);
                    
                    GUILayout.EndHorizontal();
                    
                    ItsVREditorElements.GUILabelElement("Joystick Position: " + container.universal.JoystickPosition, ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);
                    
                    GUILayout.BeginHorizontal(new GUIStyle {normal = new GUIStyleState {background = Texture2D.grayTexture}});
                    
                        ItsVREditorElements.GUILabelElement("Joystick Pressed: " + container.universal.JoystickPressed, ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);
                    
                    GUILayout.EndHorizontal();
                    
                    ItsVREditorElements.GUILabelElement("Primary Button Pressed: " + container.universal.PrimaryButtonPressed, ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);
                    
                    GUILayout.BeginHorizontal(new GUIStyle {normal = new GUIStyleState {background = Texture2D.grayTexture}});
                    
                        ItsVREditorElements.GUILabelElement("Primary Button Touched: " + container.universal.PrimaryButtonTouched, ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);
                    
                    GUILayout.EndHorizontal();
                    
                    ItsVREditorElements.GUILabelElement("Secondary Button Pressed: " + container.universal.SecondaryButtonPressed, ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);
                    
                    GUILayout.BeginHorizontal(new GUIStyle {normal = new GUIStyleState {background = Texture2D.grayTexture}});
                    
                        ItsVREditorElements.GUILabelElement("Secondary Button Touched: " + container.universal.SecondaryButtonTouched, ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);
                    
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();

                        ItsVREditorElements.GUILabelElement("Amplitude", ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);
                        _hapticsAmplitude = EditorGUILayout.FloatField(_hapticsAmplitude, GUILayout.Width(50));
                        
                        ItsVREditorElements.GUILabelElement("Duration", ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);
                        _hapticsDuration = EditorGUILayout.FloatField(_hapticsDuration, GUILayout.Width(50));
                        
                        if (GUILayout.Button("Test Haptics")) 
                            container.universal.SendHapticPulse(_hapticsAmplitude, _hapticsDuration);
                    
                    GUILayout.EndHorizontal();
                }

                GUILayout.Space(5);
                _showMetaInputs = EditorGUILayout.Foldout(_showMetaInputs, new GUIContent("Meta"));

                if (_showMetaInputs) {
                    ItsVREditorElements.GUILabelElement("Joystick Touched: " + container.meta.JoystickTouched, ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);

                    GUILayout.BeginHorizontal(new GUIStyle {normal = new GUIStyleState {background = Texture2D.grayTexture}});
                    
                        ItsVREditorElements.GUILabelElement("Trigger Touched: " + container.meta.TriggerTouched, ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);
                    
                    GUILayout.EndHorizontal();
                }

                GUILayout.Space(5);
                _showViveInputs = EditorGUILayout.Foldout(_showViveInputs, new GUIContent("Vive"));

                if (_showViveInputs) {
                    ItsVREditorElements.GUILabelElement("Trackpad Position: " + container.vive.TrackpadPosition, ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);
                    
                    GUILayout.BeginHorizontal(new GUIStyle {normal = new GUIStyleState {background = Texture2D.grayTexture}});
                    
                        ItsVREditorElements.GUILabelElement("Trackpad Pressed: " + container.vive.TrackpadPressed, ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);
                    
                    GUILayout.EndHorizontal();
                    
                    ItsVREditorElements.GUILabelElement("Trackpad Touched: " + container.vive.TrackpadTouched, ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);
                }
                
                GUILayout.Space(5);
                _showValveInputs = EditorGUILayout.Foldout(_showValveInputs, new GUIContent("Valve"));

                if (_showValveInputs) {
                    GUILayout.BeginHorizontal(new GUIStyle {normal = new GUIStyleState {background = Texture2D.grayTexture}});
                    
                        ItsVREditorElements.GUILabelElement("Trackpad Position: " + container.valve.TrackpadPosition, ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);
                    
                    GUILayout.EndHorizontal();
                    
                    ItsVREditorElements.GUILabelElement("Trackpad Touched: " + container.valve.TrackpadTouched, ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);

                    GUILayout.BeginHorizontal(new GUIStyle {normal = new GUIStyleState {background = Texture2D.grayTexture}});
                    
                        ItsVREditorElements.GUILabelElement("Trackpad Force: " + container.valve.TrackpadForce, ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);
                    
                    GUILayout.EndHorizontal();
                    
                    ItsVREditorElements.GUILabelElement("Joystick Touched: " + container.valve.JoystickTouched, ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);

                    GUILayout.BeginHorizontal(new GUIStyle {normal = new GUIStyleState {background = Texture2D.grayTexture}});
                    
                        ItsVREditorElements.GUILabelElement("Trigger Touched: " + container.valve.TriggerTouched, ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);
                    
                    GUILayout.EndHorizontal();

                    ItsVREditorElements.GUILabelElement("Grip Force: " + container.valve.GripForce, ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);
                }
            
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            
            Repaint();
        }

        /// <summary>
        /// Open the VR input container input debugger window.
        /// </summary>
        [MenuItem("It's VR/Tools/Input Debugger")]
        public static void OpenWindow() => GetWindow<ItsVRInputDebuggerEditor>(EDITOR_WINDOW_TITLE);

        /// <summary>
        /// Open the window with a VR input container already referenced.
        /// </summary>
        /// <param name="inputContainerToDebug">VR input container to reference.</param>
        public static void OpenWindowWithInputContainer(VRInputContainer inputContainerToDebug) {
            _vrInputContainerToDebug = inputContainerToDebug;
            GetWindow<ItsVRInputDebuggerEditor>(EDITOR_WINDOW_TITLE);  
        }
    }
}
