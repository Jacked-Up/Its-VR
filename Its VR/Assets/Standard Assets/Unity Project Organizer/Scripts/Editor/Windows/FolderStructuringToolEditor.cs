using JackedUp.Core;
using UnityEditor;
using UnityEngine;

namespace JackedUp.Editor.Windows {
    /// <summary>
    /// Editor tool responsible for creating and setting up the folder structure.
    /// </summary>
    /// <para>Author: Jack Randolph</para>
    public class FolderStructuringToolEditor : EditorWindow {
        #region Variables
        
        // Hello there! Please ignore this monolith of code :)
        private bool _generateAnimationsFolder;
        private bool _generateAudioFolder;
        private bool _generateAudioSfxSubFolder;
        private bool _generateAudioMusicSubFolder;
        private bool _generateMaterialsFolder;
        private bool _generateModelsFolder;
        private bool _generateModelsEnvironmentSubFolder;
        private bool _generateModelsPlayerModelSubFolder;
        private bool _generatePrefabsFolder;
        private bool _generatePrefabsModelsSubFolder;
        private bool _generatePrefabsTilesSubFolder;
        private bool _generatePrefabsParticlesSubFolder;
        private bool _generateResourcesFolder;
        private bool _generateResourcesDependenciesSubFolder;
        private bool _generateResourcesPlayerSubFolder;
        private bool _generateResourcesOtherSubFolder;
        private bool _generateScenesFolder;
        private bool _generateScenesLevelsSubFolder;
        private bool _generateScenesCinematicsSubFolder;
        private bool _generateScenesOtherSceneSubFolder;
        private bool _generateScenesTestingSubFolder;
        private bool _generateScriptsFolder;
        private bool _generateScriptsEditorSubFolder;
        private bool _generateScriptsCoreSubFolder;
        private bool _generateScriptsControllersSubFolder;
        private bool _generateScriptsManagersSubFolder;
        private bool _generateScriptsMechanicsSubFolder;
        private bool _generateScriptsUiSubFolder;
        private bool _generateScriptsNetworkingSubFolder;
        private bool _generateScriptsMiscellaneousSubFolder;
        private bool _generateTexturesFolder;
        private bool _generateTexturesSpritesSubFolder;
        private bool _generateTexturesImagesSubFolder;
        private bool _generateTexturesParticlesSubFolder;
        private bool _generateShadersFolder;
        private bool _generateFontsFolder;
        private bool _generatePhysicsFolder;
        private bool _generateEditorFolder;
        private bool _generateSettingsFolder;
        private bool _generatePluginsFolder;
        private bool _generateExtensionsFolder;
        private bool _generatePresetsFolder;
        private bool _drawEntryBackground = true;
        private Vector2 _scrollPosition;
        
        #endregion

        private void OnEnable() {
            _generateAnimationsFolder = !FolderTool.FolderExists(ParentFolders.Animations);
            _generateAudioFolder = !FolderTool.FolderExists(ParentFolders.Audio);
            _generateMaterialsFolder = !FolderTool.FolderExists(ParentFolders.Materials);
            _generateModelsFolder = !FolderTool.FolderExists(ParentFolders.Models);
            _generatePrefabsFolder = !FolderTool.FolderExists(ParentFolders.Prefabs);
            _generateResourcesFolder = !FolderTool.FolderExists(ParentFolders.Resources);
            _generateScenesFolder = !FolderTool.FolderExists(ParentFolders.Scenes);
            _generateScriptsFolder = !FolderTool.FolderExists(ParentFolders.Scripts);
            _generateTexturesFolder = !FolderTool.FolderExists(ParentFolders.Textures);
        }

        private void OnGUI() {
            if (Application.isPlaying) {
                EditorGUILayout.HelpBox("You cannot set up the folder structure while in playmode.", MessageType.Info);
                
                if (GUILayout.Button("Exit playmode"))
                    EditorApplication.ExitPlaymode();
                
                return;
            }
            
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            GUILayout.BeginVertical();
            
            var content = new GUIContent {
                text = "Select all the parent and sub folders you would like to setup in your project."
            };
            GUILayout.Box(content);

            GUILayout.BeginHorizontal();
            
                if (GUILayout.Button("?", GUILayout.Width(25)))
                    Application.OpenURL("https://github.com/Jacked-Up/Unity-Project-Organizer/wiki/Using-the-Folder-Structuring-Tool");
                
                if (GUILayout.Button("Select all", GUILayout.Width(100)))
                    SelectAll();

            GUILayout.EndHorizontal();
            
            GUILayout.Space(5);
            JackedUpGUILayout.Label("Selection", JackedUpGUILayout.TextColors.Blue, JackedUpGUILayout.MediumTextStyle, true);
            JackedUpGUILayout.DividerLine(5);
            GUILayout.Space(2);

            _drawEntryBackground = false;
            
            // Animation
            DrawEntry("Animations Folder", _generateAnimationsFolder, out _generateAnimationsFolder);
            
            // Audio
            DrawEntry("Audio Folder", _generateAudioFolder, out _generateAudioFolder);
            if (_generateAudioFolder) {
                DrawSubEntry("Music", _generateAudioMusicSubFolder, out _generateAudioMusicSubFolder);
                DrawSubEntry("SFX", _generateAudioSfxSubFolder, out _generateAudioSfxSubFolder);
            }

            // Materials
            DrawEntry("Materials Folder", _generateMaterialsFolder, out _generateMaterialsFolder);

            // Models
            DrawEntry("Models Folder", _generateModelsFolder, out _generateModelsFolder);
            if (_generateModelsFolder) {
                DrawSubEntry("Environment", _generateModelsEnvironmentSubFolder, out _generateModelsEnvironmentSubFolder);
                DrawSubEntry("Player", _generateModelsPlayerModelSubFolder, out _generateModelsPlayerModelSubFolder);
            }
            
            // Prefabs
            DrawEntry("Prefabs Folder", _generatePrefabsFolder, out _generatePrefabsFolder);
            if (_generatePrefabsFolder) {
                DrawSubEntry("Models", _generatePrefabsModelsSubFolder, out _generatePrefabsModelsSubFolder);
                DrawSubEntry("Tiles", _generatePrefabsTilesSubFolder, out _generatePrefabsTilesSubFolder);
                DrawSubEntry("Particles", _generatePrefabsParticlesSubFolder, out _generatePrefabsParticlesSubFolder);
            }
            
            // Resources
            DrawEntry("Resources Folder", _generateResourcesFolder, out _generateResourcesFolder);
            if (_generateResourcesFolder) {
                DrawSubEntry("Dependencies", _generateResourcesDependenciesSubFolder, out _generateResourcesDependenciesSubFolder);
                DrawSubEntry("Player", _generateResourcesPlayerSubFolder, out _generateResourcesPlayerSubFolder);
                DrawSubEntry("Other", _generateResourcesOtherSubFolder, out _generateResourcesOtherSubFolder);
            }
            
            // Scenes
            DrawEntry("Scenes Folder", _generateScenesFolder, out _generateScenesFolder);
            if (_generateScenesFolder) {
                DrawSubEntry("Levels", _generateScenesLevelsSubFolder, out _generateScenesLevelsSubFolder);
                DrawSubEntry("Cinematics", _generateScenesCinematicsSubFolder, out _generateScenesCinematicsSubFolder);
                DrawSubEntry("Other", _generateScenesOtherSceneSubFolder, out _generateScenesOtherSceneSubFolder);
                DrawSubEntry("Testing", _generateScenesTestingSubFolder, out _generateScenesTestingSubFolder);
            }
            
            // Scripts
            DrawEntry("Scripts Folder", _generateScriptsFolder, out _generateScriptsFolder);
            if (_generateScriptsFolder) {
                DrawSubEntry("Editor", _generateScriptsEditorSubFolder, out _generateScriptsEditorSubFolder);
                DrawSubEntry("Core", _generateScriptsCoreSubFolder, out _generateScriptsCoreSubFolder);
                DrawSubEntry("Controllers", _generateScriptsControllersSubFolder, out _generateScriptsControllersSubFolder);
                DrawSubEntry("Managers", _generateScriptsManagersSubFolder, out _generateScriptsManagersSubFolder);
                DrawSubEntry("Mechanics", _generateScriptsMechanicsSubFolder, out _generateScriptsMechanicsSubFolder);
                DrawSubEntry("UI", _generateScriptsUiSubFolder, out _generateScriptsUiSubFolder);
                DrawSubEntry("Networking", _generateScriptsNetworkingSubFolder, out _generateScriptsNetworkingSubFolder);
                DrawSubEntry("Miscellaneous", _generateScriptsMiscellaneousSubFolder, out _generateScriptsMiscellaneousSubFolder);
            }

            // Textures
            DrawEntry("Textures Folder", _generateTexturesFolder, out _generateTexturesFolder);
            if (_generateTexturesFolder) {
                DrawSubEntry("Sprites", _generateTexturesSpritesSubFolder, out _generateTexturesSpritesSubFolder);
                DrawSubEntry("Images", _generateTexturesImagesSubFolder, out _generateTexturesImagesSubFolder);
                DrawSubEntry("Particles", _generateTexturesParticlesSubFolder, out _generateTexturesParticlesSubFolder);
            }
            
            GUILayout.Space(15);
            JackedUpGUILayout.Label("More", JackedUpGUILayout.TextColors.Blue, JackedUpGUILayout.MediumTextStyle, true);
            JackedUpGUILayout.DividerLine(5);
            GUILayout.Space(2);

            _drawEntryBackground = false;
            
            // Shaders
            DrawEntry("Shaders Folder", _generateShadersFolder, out _generateShadersFolder);
            
            // Fonts
            DrawEntry("Fonts Folder", _generateFontsFolder, out _generateFontsFolder);
            
            // Physics
            DrawEntry("Physics Folder", _generatePhysicsFolder, out _generatePhysicsFolder);
            
            // Editor
            DrawEntry("Editor Folder", _generateEditorFolder, out _generateEditorFolder);
            
            // Settings
            DrawEntry("Settings Folder", _generateSettingsFolder, out _generateSettingsFolder);
            
            // Plugins 
            DrawEntry("Plugins Folder", _generatePluginsFolder, out _generatePluginsFolder);
            
            // Extensions
            DrawEntry("Extensions Folder", _generateExtensionsFolder, out _generateExtensionsFolder);
            
            // Presets
            DrawEntry("Presets Folder", _generatePresetsFolder, out _generatePresetsFolder);

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            GUILayout.FlexibleSpace();

            EditorGUILayout.HelpBox("The folder structuring tool is nondestructive.", MessageType.Info);
            
            if (GUILayout.Button("Setup structure"))
                SetupStructure();
        }

        [MenuItem("Window/Project Organizer/Setup/Folder Structure")]
        private static void OpenWindow() => GetWindow(typeof(FolderStructuringToolEditor), false, "Folder Structuring Tool");

        private void DrawEntry(string name, bool value, out bool outValue) {
            GUILayout.BeginVertical(new GUIStyle {normal = new GUIStyleState {background = _drawEntryBackground ? Texture2D.grayTexture : null}});
            
                outValue = EditorGUILayout.Toggle(name, value);
            
            GUILayout.EndVertical();

            _drawEntryBackground = !_drawEntryBackground;
        }
        
        private void DrawSubEntry(string name, bool value, out bool outValue) {
            GUILayout.BeginVertical(new GUIStyle {normal = new GUIStyleState {background = !_drawEntryBackground ? Texture2D.grayTexture : null}});
            
                outValue = EditorGUILayout.Toggle($"| {name}", value);
            
            GUILayout.EndVertical();
        }

        private void SelectAll() {
            _generateAnimationsFolder = true;
            _generateAudioFolder = true;
            _generateAudioSfxSubFolder = true;
            _generateAudioMusicSubFolder = true;
            _generateMaterialsFolder = true;
            _generateModelsFolder = true;
            _generateModelsEnvironmentSubFolder = true;
            _generateModelsPlayerModelSubFolder = true;
            _generatePrefabsFolder = true;
            _generatePrefabsModelsSubFolder = true;
            _generatePrefabsTilesSubFolder = true;
            _generatePrefabsParticlesSubFolder = true;
            _generateResourcesFolder = true;
            _generateResourcesDependenciesSubFolder = true;
            _generateResourcesPlayerSubFolder = true;
            _generateResourcesOtherSubFolder = true;
            _generateScenesFolder = true;
            _generateScenesLevelsSubFolder = true;
            _generateScenesCinematicsSubFolder = true;
            _generateScenesOtherSceneSubFolder = true;
            _generateScenesTestingSubFolder = true;
            _generateScriptsFolder = true;
            _generateScriptsEditorSubFolder = true;
            _generateScriptsCoreSubFolder = true;
            _generateScriptsControllersSubFolder = true;
            _generateScriptsManagersSubFolder = true;
            _generateScriptsMechanicsSubFolder = true;
            _generateScriptsUiSubFolder = true;
            _generateScriptsNetworkingSubFolder = true;
            _generateScriptsMiscellaneousSubFolder = true;
            _generateTexturesFolder = true;
            _generateTexturesSpritesSubFolder = true;
            _generateTexturesImagesSubFolder = true;
            _generateTexturesParticlesSubFolder = true;
            _generateShadersFolder = true;
            _generateFontsFolder = true;
            _generatePhysicsFolder  = true;
            _generateEditorFolder = true;
            _generateSettingsFolder = true;
            _generatePluginsFolder = true;
            _generateExtensionsFolder = true;
            _generatePresetsFolder = true;
            
            Repaint();
        }
        
        private void SetupStructure() {
            // Animation
            if (_generateAnimationsFolder)
                FolderTool.CreateFolder(ParentFolders.Animations);
            
            // Audio
            if (_generateAudioFolder)
                FolderTool.CreateFolder(ParentFolders.Audio);
            
            if (_generateAudioMusicSubFolder)
                FolderTool.CreateFolder(ParentFolders.Audio, "Music");
            
            if (_generateAudioSfxSubFolder)
                FolderTool.CreateFolder(ParentFolders.Audio, "SFX");
            
            // Materials
            if (_generateMaterialsFolder)
                FolderTool.CreateFolder(ParentFolders.Materials);
            
            // Models
            if (_generateModelsFolder)
                FolderTool.CreateFolder(ParentFolders.Models);

            if (_generateModelsEnvironmentSubFolder)
                FolderTool.CreateFolder(ParentFolders.Models, "Environment");
            
            if (_generateModelsPlayerModelSubFolder)
                FolderTool.CreateFolder(ParentFolders.Models, "Player");

            // Prefabs
            if (_generatePrefabsFolder)
                FolderTool.CreateFolder(ParentFolders.Prefabs);
            
            if (_generatePrefabsModelsSubFolder)
                FolderTool.CreateFolder(ParentFolders.Prefabs, "Models");

            if (_generatePrefabsTilesSubFolder)
                FolderTool.CreateFolder(ParentFolders.Prefabs, "Tiles");
            
            if (_generatePrefabsParticlesSubFolder)
                FolderTool.CreateFolder(ParentFolders.Prefabs, "Particles");
            
            // Resources
            if (_generateResourcesFolder)
                FolderTool.CreateFolder(ParentFolders.Resources);
            
            if (_generateResourcesDependenciesSubFolder)
                FolderTool.CreateFolder(ParentFolders.Resources, "Dependencies");
            
            if (_generateResourcesPlayerSubFolder)
                FolderTool.CreateFolder(ParentFolders.Resources, "Player");
            
            if (_generateResourcesOtherSubFolder)
                FolderTool.CreateFolder(ParentFolders.Resources, "Other");
            
            // Scenes
            if (_generateScenesFolder)
                FolderTool.CreateFolder(ParentFolders.Scenes);
            
            if (_generateScenesLevelsSubFolder)
                FolderTool.CreateFolder(ParentFolders.Scenes, "Levels");
            
            if (_generateScenesCinematicsSubFolder)
                FolderTool.CreateFolder(ParentFolders.Scenes, "Cinematics");
            
            if (_generateScenesOtherSceneSubFolder)
                FolderTool.CreateFolder(ParentFolders.Scenes, "Other");
            
            if (_generateScenesTestingSubFolder)
                FolderTool.CreateFolder(ParentFolders.Scenes, "Testing");
            
            // Scripts
            if (_generateScriptsFolder)
                FolderTool.CreateFolder(ParentFolders.Scripts);
            
            if (_generateScriptsEditorSubFolder)
                FolderTool.CreateFolder(ParentFolders.Scripts, "Editor");
            
            if (_generateScriptsCoreSubFolder)
                FolderTool.CreateFolder(ParentFolders.Scripts, "Core");
            
            if (_generateScriptsControllersSubFolder)
                FolderTool.CreateFolder(ParentFolders.Scripts, "Controllers");
            
            if (_generateScriptsManagersSubFolder)
                FolderTool.CreateFolder(ParentFolders.Scripts, "Managers");
            
            if (_generateScriptsMechanicsSubFolder)
                FolderTool.CreateFolder(ParentFolders.Scripts, "Mechanics");
            
            if (_generateScriptsUiSubFolder)
                FolderTool.CreateFolder(ParentFolders.Scripts, "UI");
            
            if (_generateScriptsNetworkingSubFolder)
                FolderTool.CreateFolder(ParentFolders.Scripts, "Networking");
            
            if (_generateScriptsMiscellaneousSubFolder)
                FolderTool.CreateFolder(ParentFolders.Scripts, "Miscellaneous");
            
            // Textures
            if (_generateTexturesFolder)
                FolderTool.CreateFolder(ParentFolders.Textures);
            
            if (_generateTexturesSpritesSubFolder)
                FolderTool.CreateFolder(ParentFolders.Textures, "Sprites");
            
            if (_generateTexturesImagesSubFolder)
                FolderTool.CreateFolder(ParentFolders.Textures, "Images");
            
            if (_generateTexturesParticlesSubFolder)
                FolderTool.CreateFolder(ParentFolders.Textures, "Particles");
            
            // Shaders
            if (_generateShadersFolder)
                FolderTool.CreateFolder(ParentFolders.Shaders);
            
            // Fonts
            if (_generateFontsFolder)
                FolderTool.CreateFolder(ParentFolders.Fonts);
            
            // Physics
            if (_generatePhysicsFolder)
                FolderTool.CreateFolder(ParentFolders.Physics);
            
            // Editor
            if (_generateEditorFolder)
                FolderTool.CreateFolder(ParentFolders.Editor);
            
            // Settings
            if (_generateSettingsFolder)
                FolderTool.CreateFolder(ParentFolders.Settings);
            
            // Plugins
            if (_generatePluginsFolder)
                FolderTool.CreateFolder(ParentFolders.Plugins);
            
            // Extensions
            if (_generateExtensionsFolder)
                FolderTool.CreateFolder(ParentFolders.Extensions);
            
            // Presets
            if (_generatePresetsFolder)
                FolderTool.CreateFolder(ParentFolders.Presets);

            Debug.Log("<color=green><b>Selected folder structures were set up successfully.</b></color>");
            Close();
        }
    }
}