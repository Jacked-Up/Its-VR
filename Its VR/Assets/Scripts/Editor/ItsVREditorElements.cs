// This script was updated on 03/23/2022 by Jack Randolph.

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ItsVR.Editor {
    /// <summary>
    /// Contains methods which generate It's VR editor UI elements.
    ///
    /// Contains text style properties which are used to select the GUILabelElement text size.
    ///
    /// GUILabelElement creates a label, with the specified color (taking into account the editor theme), and optional
    /// bold font.
    ///
    /// GUIInfoElement creates a list of elements containing all the information requested.
    /// </summary>
    public static class ItsVREditorElements {
        #region Variables

        /// <summary>
        /// Large text style.
        /// </summary>
        public static readonly GUIStyle TextStyleLarge = new GUIStyle {
            richText = true,
            fontSize = 22,
            wordWrap = false
        };
        
        /// <summary>
        /// Medium text style.
        /// </summary>
        public static readonly GUIStyle TextStyleMedium = new GUIStyle {
            richText = true,
            fontSize = 15,
            wordWrap = false
        };
        
        /// <summary>
        /// Small text style.
        /// </summary>
        public static readonly GUIStyle TextStyleSmall = new GUIStyle {
            richText = true,
            fontSize = 12,
            wordWrap = true
        };
        
        /// <summary>
        /// Extra small text style.
        /// </summary>
        public static readonly GUIStyle TextStyleExtraSmall = new GUIStyle {
            richText = true,
            fontSize = 9,
            wordWrap = true
        };
        
        private static bool _showInfo;
        
        public enum TextColors { Black, White, Grey, Red, Blue, Green, Yellow, Magenta }

        public struct InfoEntry {
            public string name; 
            public string value;
            public int pixelSpace;
        }

        #endregion

        /// <summary>
        /// Draws a UI element label with some style.
        /// Text style MUST have rich text enabled.
        /// </summary>
        /// <param name="textToConvert">Text to convert.</param>
        /// <param name="textColor">Color of the text.</param>
        /// <param name="textStyle">Style to use on the text.</param>
        /// <param name="boldText">If the text should be bold.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void GUILabelElement(string textToConvert, TextColors textColor, GUIStyle textStyle, bool boldText = false) {
            var convertedText = string.Empty;
            
            convertedText += textColor switch {
                TextColors.Black => EditorGUIUtility.isProSkin ? "<color=#adadad>" : "<color=#1a1a1a>",
                TextColors.White => EditorGUIUtility.isProSkin ? "<color=#ffffff>" : "<color=#0a0a0a>",
                TextColors.Grey => EditorGUIUtility.isProSkin ? "<color=#b3b3b3>" : "<color=#404040>",
                TextColors.Red => EditorGUIUtility.isProSkin ? "<color=#ff3030>" : "<color=#ff0000>",
                TextColors.Blue => EditorGUIUtility.isProSkin ? "<color=#0080ff>" : "<color=#3098ff>",
                TextColors.Green => EditorGUIUtility.isProSkin ? "<color=#4dff00>" : "<color=#367318>",
                TextColors.Yellow => EditorGUIUtility.isProSkin ? "<color=#f0c800>" : "<color=#d98900>",
                TextColors.Magenta => EditorGUIUtility.isProSkin ? "<color=#a442ff>" : "<color=#ff0080>",
                _ => throw new ArgumentOutOfRangeException(nameof(textColor), textColor, null)
            };
            
            convertedText += boldText
                ? "<b>" + textToConvert + "</b>"
                : textToConvert;
            
            GUILayout.Label(convertedText + "</color>", textStyle);
        }

        /// <summary>
        /// Draws a UI element containing all of the information supplied.
        /// Text style MUST have rich text enabled.
        /// </summary>
        /// <param name="infoEntries">All of the info to display.</param>
        /// <param name="title">The title of the info list.</param>
        public static void GUIInfoElement(List<InfoEntry> infoEntries, string title = "Info") {
            GUILayout.Space(5);
            _showInfo = EditorGUILayout.Foldout(_showInfo, new GUIContent(title));

            if (!_showInfo)
                return;
            
            foreach (var a in infoEntries) {
                GUILayout.Space(a.pixelSpace);
                GUILabelElement(" " + a.name + ": " + a.value, TextColors.Grey, TextStyleSmall);
            }

            if (Application.isPlaying) 
                return;
            
            GUILayout.Space(2);
            EditorGUILayout.HelpBox("Some info may only refresh in playmode.", MessageType.Warning);
        }
    }
}