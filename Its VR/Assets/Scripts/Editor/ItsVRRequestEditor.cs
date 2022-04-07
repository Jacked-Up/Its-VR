// This script was updated on 03/22/2022 by Jack Randolph.

using System;
using UnityEditor;
using UnityEngine;

namespace ItsVR.Editor {
    /// <summary>
    /// Displays a request prompt to the developer.
    ///
    /// The purpose of the window is to prevent accidental clicks to open websites and files. This helps ensure
    /// developers intentionally attempted to open the request.
    /// </summary>
    public class ItsVRRequestEditor : EditorWindow {
        #region Variables
        
        private static string _requestNavigationLink;
        private static RequestTypes _requestType;
        
        public enum RequestTypes {
            NavigateToWebsite,
            OpenAFile
        }
        
        #endregion

        private void OnGUI() {
            GUILayout.Space(5);
            ItsVREditorElements.GUILabelElement("  It's VR has Requested to...", ItsVREditorElements.TextColors.White, ItsVREditorElements.TextStyleMedium, true);

            switch (_requestType) {
                case RequestTypes.NavigateToWebsite: {
                    GUILayout.Space(5);
                    ItsVREditorElements.GUILabelElement("  Open a Website: \n  " + _requestNavigationLink, ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);

                    GUILayout.Space(5);
                    GUILayout.BeginHorizontal();
            
                    if (GUILayout.Button("Navigate to Website")) {
                        Application.OpenURL(_requestNavigationLink);
                        Close();
                    }
                        
                    if (GUILayout.Button("Cancel")) 
                        Close();
                
                    GUILayout.EndHorizontal();
                    break;
                }
                case RequestTypes.OpenAFile: {
                    GUILayout.Space(5);
                    ItsVREditorElements.GUILabelElement("  Open a File: \n  " + _requestNavigationLink, ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);
                    
                    GUILayout.Space(5);
                    GUILayout.BeginHorizontal();
                    
                    if (GUILayout.Button("Open the File")) {
                        Application.OpenURL("Ya mama");
                        Close();
                    }
                        
                    if (GUILayout.Button("Cancel")) 
                        Close();
                    
                    GUILayout.EndHorizontal();
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Creates a request window and displays it to the developer.
        /// </summary>
        /// <param name="requestInformation">Request information to display.</param>
        /// <param name="requestType">The type of request.</param>
        public static void CreateAndShowRequest(string requestInformation, RequestTypes requestType) {
            _requestNavigationLink = requestInformation;
            _requestType = requestType;
            
            GetWindowWithRect<ItsVRRequestEditor>(new Rect(0, 0, 400, 90), true, "Request");
        }
    }
}