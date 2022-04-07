using UnityEditor;
using UnityEngine;

namespace JackedUp.Editor.Windows {
    /// <summary>
    /// Jacked Up Studios resources window.
    /// </summary>
    /// <para>Author: Jack Randolph</para>
    public class ResourcesEditor : EditorWindow {
        #region Variables

        private Vector2 _scrollPosition;

        #endregion
        
        private void OnGUI() {
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            GUILayout.BeginVertical();
            
            GUILayout.Space(5);
            JackedUpGUILayout.Label("Jacked Up Studios", JackedUpGUILayout.TextColors.White, JackedUpGUILayout.LargeTextStyle, true);

            GUILayout.Space(2);
            JackedUpGUILayout.Label("Thank you so much for using the Unity Project Organizer!", JackedUpGUILayout.TextColors.Grey, JackedUpGUILayout.SmallTextStyle);
            
            GUILayout.Space(5);
            JackedUpGUILayout.DividerLine(2);
            
            GUILayout.Space(5);
            JackedUpGUILayout.Label("If you would like to support us, here's a link to our Patreon <3", JackedUpGUILayout.TextColors.Magenta, JackedUpGUILayout.SmallTextStyle);

            if (GUILayout.Button("Patreon"))
                Application.OpenURL("https://www.patreon.com/JackRandolph");
            
            GUILayout.Space(5);
            JackedUpGUILayout.DividerLine(2);
            
            GUILayout.Space(5);
            JackedUpGUILayout.Label("Need some help or want to request a feature? Join our Discord server.", JackedUpGUILayout.TextColors.Grey, JackedUpGUILayout.SmallTextStyle);

            if (GUILayout.Button("Discord server"))
                Application.OpenURL("https://discord.gg/pSUnvtPB7H");
            
            GUILayout.Space(5);
            JackedUpGUILayout.DividerLine(2);
            
            GUILayout.Space(5);
            JackedUpGUILayout.Label("The Unity Project Organizers GitHub repository.", JackedUpGUILayout.TextColors.Grey, JackedUpGUILayout.SmallTextStyle);

            if (GUILayout.Button("GitHub repository"))
                Application.OpenURL("https://github.com/Jacked-Up/Unity-Project-Organizer");
            
            if (GUILayout.Button("Documentation"))
                Application.OpenURL("https://github.com/Jacked-Up/Unity-Project-Organizer/wiki");
            
            GUILayout.Space(5);
            JackedUpGUILayout.DividerLine(2);
            
            GUILayout.Space(5);
            JackedUpGUILayout.Label("Jacked Up Studios website.", JackedUpGUILayout.TextColors.Grey, JackedUpGUILayout.SmallTextStyle);

            if (GUILayout.Button("Website"))
                Application.OpenURL("https://jackedupstudios.com/");
            
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            GUILayout.FlexibleSpace();
            
            JackedUpGUILayout.Label("Copyright 2022 Jacked Up Studios LLC", JackedUpGUILayout.TextColors.Grey, JackedUpGUILayout.ExtraSmallTextStyle);
        }
        
        [MenuItem("Window/Project Organizer/Resources")]
        private static void ShowWindow() => GetWindow(typeof(ResourcesEditor), false, "Resources");
    }
}