using System;
using UnityEditor;
using UnityEngine;

namespace JackedUp.Editor {
    public static class JackedUpGUILayout {
        #region Variables

        /// <summary>
        /// Large text style.
        /// </summary>
        public static readonly GUIStyle LargeTextStyle = new GUIStyle {
            richText = true,
            fontSize = 22,
            wordWrap = false
        };
        
        /// <summary>
        /// Medium text style.
        /// </summary>
        public static readonly GUIStyle MediumTextStyle = new GUIStyle {
            richText = true,
            fontSize = 15,
            wordWrap = false
        };
        
        /// <summary>
        /// Small text style.
        /// </summary>
        public static readonly GUIStyle SmallTextStyle = new GUIStyle {
            richText = true,
            fontSize = 12,
            wordWrap = true
        };
        
        /// <summary>
        /// Extra small text style.
        /// </summary>
        public static readonly GUIStyle ExtraSmallTextStyle = new GUIStyle {
            richText = true,
            fontSize = 9,
            wordWrap = true
        };
        
        public enum TextColors { Black, White, Grey, Red, Blue, Green, Yellow, Magenta }

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
        public static void Label(string textToConvert, TextColors textColor, GUIStyle textStyle, bool boldText = false) {
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
        
        public static void DividerLine(int height) {
            GUILayout.BeginVertical(new GUIStyle {normal = new GUIStyleState {background = Texture2D.grayTexture}});
            GUILayout.Space(height);
            GUILayout.EndVertical();
        }
    }
}