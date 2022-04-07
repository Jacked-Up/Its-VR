using JackedUp.Core;
using UnityEditor;
using UnityEngine;

namespace JackedUp.Editor.Windows {
    /// <summary>
    /// Editor tool responsible for creating and setting up the scene structure.
    /// </summary>
    /// <para>Author: Jack Randolph</para>
    public class SceneStructuringToolEditor : EditorWindow {
        #region Variables

        private bool _generateSceneFolder;
        private bool _generateStaticFolder;
        private bool _generateDynamicFolder;
        private bool _generateLightingFolder;
        private bool _generateLogicFolder;
        private bool _generateGUIFolder;
        private bool _generateMiscellaneousFolder;
        private bool _generateDeveloperFolder;
        private bool _drawEntryBackground = true;
        private Vector2 _scrollPosition;
        
        #endregion

        private void OnEnable() {
            _generateSceneFolder = !SceneTool.FolderExists(SceneFolders.Scene);
            _generateStaticFolder = !SceneTool.FolderExists(SceneFolders.Static);
            _generateDynamicFolder = !SceneTool.FolderExists(SceneFolders.Dynamic);
            _generateLightingFolder = !SceneTool.FolderExists(SceneFolders.Lighting);
            _generateLogicFolder = !SceneTool.FolderExists(SceneFolders.Logic);
            _generateGUIFolder = !SceneTool.FolderExists(SceneFolders.GUI);
        }

        private void OnGUI() {
            if (Application.isPlaying) {
                EditorGUILayout.HelpBox("You cannot set up the scene structure while in playmode.", MessageType.Info);
                
                if (GUILayout.Button("Exit playmode"))
                    EditorApplication.ExitPlaymode();
                
                return;
            }

            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            GUILayout.BeginVertical();
            
            var content = new GUIContent {
                text = "Select all the scene folders you would like to have setup in your scene."
            };
            GUILayout.Box(content);
            
            GUILayout.BeginHorizontal();
            
                if (GUILayout.Button("?", GUILayout.Width(25)))
                    Application.OpenURL("https://github.com/Jacked-Up/Unity-Project-Organizer/wiki/Using-the-Scene-Structuring-Tool");
                    
                if (GUILayout.Button("Select all", GUILayout.Width(100)))
                    SelectAll();

            GUILayout.EndHorizontal();
            
            GUILayout.Space(5);
            JackedUpGUILayout.Label("Selection", JackedUpGUILayout.TextColors.Blue, JackedUpGUILayout.MediumTextStyle, true);
            JackedUpGUILayout.DividerLine(5);
            GUILayout.Space(2);

            _drawEntryBackground = false;
            
            // Scene
            DrawEntry("Scene", _generateSceneFolder, out _generateSceneFolder);
            
            // Static
            DrawEntry("Static", _generateStaticFolder, out _generateStaticFolder);
            
            // Dynamic
            DrawEntry("Dynamic", _generateDynamicFolder, out _generateDynamicFolder);
            
            // Lighting
            DrawEntry("Lighting", _generateLightingFolder, out _generateLightingFolder);
            
            // Logic
            DrawEntry("Logic", _generateLogicFolder, out _generateLogicFolder);
            
            // GUI
            DrawEntry("GUI", _generateGUIFolder, out _generateGUIFolder);
            
            GUILayout.Space(15);
            JackedUpGUILayout.Label("More", JackedUpGUILayout.TextColors.Blue, JackedUpGUILayout.MediumTextStyle, true);
            JackedUpGUILayout.DividerLine(5);
            GUILayout.Space(2);

            _drawEntryBackground = false;
            
            // Miscellaneous
            DrawEntry("Miscellaneous", _generateMiscellaneousFolder, out _generateMiscellaneousFolder);
            
            // Developer
            DrawEntry("Developer", _generateDeveloperFolder, out _generateDeveloperFolder);
            
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            GUILayout.FlexibleSpace();

            EditorGUILayout.HelpBox("The scene structuring tool is nondestructive.", MessageType.Info);

            if (GUILayout.Button("Setup structure"))
                SetupStructure();
        }
        
        [MenuItem("Window/Project Organizer/Setup/Scene Structure")]
        private static void OpenWindow() => GetWindow(typeof(SceneStructuringToolEditor), false, "Scene Structuring Tool");

        private void DrawEntry(string name, bool value, out bool outValue) {
            GUILayout.BeginVertical(new GUIStyle {normal = new GUIStyleState {background = _drawEntryBackground ? Texture2D.grayTexture : null}});
            
                outValue = EditorGUILayout.Toggle(name, value);
            
            GUILayout.EndVertical();

            _drawEntryBackground = !_drawEntryBackground;
        }

        private void SelectAll() {
            _generateSceneFolder = true;
            _generateStaticFolder = true;
            _generateDynamicFolder = true;
            _generateLightingFolder = true;
            _generateLogicFolder = true;
            _generateGUIFolder = true;
            _generateMiscellaneousFolder = true;
            _generateDeveloperFolder = true;
            
            Repaint();
        }
        
        private void SetupStructure() {
            // Scene
            if (_generateSceneFolder)
                SceneTool.InstantiateFolder(SceneFolders.Scene);
            
            // Static
            if (_generateStaticFolder)
                SceneTool.InstantiateFolder(SceneFolders.Static);
            
            // Dynamic
            if (_generateDynamicFolder)
                SceneTool.InstantiateFolder(SceneFolders.Dynamic);
            
            // Lighting
            if (_generateLightingFolder)
                SceneTool.InstantiateFolder(SceneFolders.Lighting);
            
            // Logic
            if (_generateLogicFolder)
                SceneTool.InstantiateFolder(SceneFolders.Logic);
            
            // GUI
            if (_generateGUIFolder)
                SceneTool.InstantiateFolder(SceneFolders.GUI);
            
            // Miscellaneous
            if (_generateMiscellaneousFolder)
                SceneTool.InstantiateFolder(SceneFolders.Miscellaneous);
            
            // Developer
            if (_generateDeveloperFolder)
                SceneTool.InstantiateFolder(SceneFolders.Developer);
            
            Debug.Log("<color=green><b>Selected scene folder structures were set up successfully.</b></color>");
            Close();
        }
    }
}
