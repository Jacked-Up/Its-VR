// This script was updated on 03/22/2022 by Jack Randolph.

using UnityEditor;
using UnityEngine;

namespace ItsVR.Editor {
    /// <summary>
    /// A GUI window containing all of It's VR links and important files.
    ///
    /// Contains: Documentation, repository, official YouTube tutorials, official Discord server, Patreon, privacy
    /// policy, license, and terms and conditions.
    ///
    /// All resource links are navigated to using the GUI button elements.
    ///
    /// Also contains a method for opening the online documentation in the browser.
    /// </summary>
    public class ItsVRResourcesEditor : EditorWindow {
        #region Variables

        private static Vector2 _scrollPosition;

        #endregion

        private void OnGUI() {
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();

                ItsVREditorElements.GUILabelElement("  It's VR", ItsVREditorElements.TextColors.White, ItsVREditorElements.TextStyleLarge, true);
                ItsVREditorElements.GUILabelElement("Your VR development buddy", ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleMedium);
            
            GUILayout.EndHorizontal();
            GUILayout.BeginVertical();
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            
                GUILayout.Space(5);
                ItsVREditorElements.GUILabelElement("Resources", ItsVREditorElements.TextColors.White, ItsVREditorElements.TextStyleMedium, true);
                
                GUILayout.Space(5);
                ItsVREditorElements.GUILabelElement("It's VR documentation and examples.", ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);
                if (GUILayout.Button("Navigate"))
                    ItsVRRequestEditor.CreateAndShowRequest("https://jackedupstudios.com/its-vr-documentation-1", ItsVRRequestEditor.RequestTypes.NavigateToWebsite);
                
                ItsVREditorElements.GUILabelElement("It's VR Github repository.", ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);
                if (GUILayout.Button("Navigate"))
                    ItsVRRequestEditor.CreateAndShowRequest("https://github.com/Jacked-Up/It-s-VR", ItsVRRequestEditor.RequestTypes.NavigateToWebsite);
                
                ItsVREditorElements.GUILabelElement("It's VR Youtube tutorials.", ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);
                if (GUILayout.Button("Navigate"))
                    ItsVRRequestEditor.CreateAndShowRequest("https://www.youtube.com/channel/UC3RAAagF1VBFfDSLAC47qkA", ItsVRRequestEditor.RequestTypes.NavigateToWebsite);
                
                ItsVREditorElements.GUILabelElement("Join the official It's VR Discord server.", ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);
                if (GUILayout.Button("Navigate"))
                    ItsVRRequestEditor.CreateAndShowRequest("https://discord.gg/pSUnvtPB7H", ItsVRRequestEditor.RequestTypes.NavigateToWebsite);
                
                ItsVREditorElements.GUILabelElement("Support It's VR on Patreon. <b><3</b>", ItsVREditorElements.TextColors.Magenta, ItsVREditorElements.TextStyleSmall);
                if (GUILayout.Button("Navigate"))
                    ItsVRRequestEditor.CreateAndShowRequest("https://www.patreon.com/JackRandolph", ItsVRRequestEditor.RequestTypes.NavigateToWebsite);
                
                GUILayout.Space(5);
                ItsVREditorElements.GUILabelElement("Credits and Legal", ItsVREditorElements.TextColors.White, ItsVREditorElements.TextStyleMedium, true);
                
                GUILayout.Space(5);
                ItsVREditorElements.GUILabelElement("Developed by Jacked Up Studios in the United States.\nProgrammed and designed by Jack Randolph.", ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);

                GUILayout.Space(5);
                if (GUILayout.Button("Privacy Policy"))
                    ItsVRRequestEditor.CreateAndShowRequest("https://jackedupstudios.com/privacy-policy", ItsVRRequestEditor.RequestTypes.NavigateToWebsite);
                
                if (GUILayout.Button("Licensing"))
                    ItsVRRequestEditor.CreateAndShowRequest("Packages", ItsVRRequestEditor.RequestTypes.OpenAFile);

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            
                ItsVREditorElements.GUILabelElement("Copyright © 2021 - 2022 Jacked Up Studios LLC - All Rights Reserved", ItsVREditorElements.TextColors.White, ItsVREditorElements.TextStyleExtraSmall, true);
        }
        
        /// <summary>
        /// Opens the resources window.
        /// </summary>
        [MenuItem("It's VR/Resources")]
        public static void OpenResourcesWindow() => GetWindow<ItsVRResourcesEditor>("It's VR Resources");

        /// <summary>
        /// Opens the It's VR online documentation.
        /// </summary>
        [MenuItem("It's VR/Online Documentation")]
        public static void OpenOnlineDocumentation() => ItsVRRequestEditor.CreateAndShowRequest("https://jackedupstudios.com/its-vr-documentation-1", ItsVRRequestEditor.RequestTypes.NavigateToWebsite);
    }
}
