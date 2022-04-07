// This script was updated on 03/22/2022 by Jack Randolph.

using System;
using System.Collections.Generic;
using System.Linq;
using ItsVR.Interaction;
using ItsVR.Player;
using UnityEditor;
using UnityEngine;

namespace ItsVR.Editor {
    /// <summary>
    /// It's VR problem debugger system.
    ///
    /// Displays problems reported by It's VR components active in the scene.
    ///
    /// Errors: Critical problems which MUST be fixed.
    /// Issues: Problems that can cause It's VR to not run properly.
    /// Recommendations: Recommendations It's VR recommends for best operation.
    /// </summary>
    public class ItsVRProblemDebuggerEditor : EditorWindow {
        #region Variables

        private const string EDITOR_WINDOW_TITLE = "It's VR Problem Debugger";
        private const int MAXIMUM_DISPLAYABLE_ERRORS = 50;
        private const int MAXIMUM_DISPLAYABLE_ISSUES = 50;
        private const int MAXIMUM_DISPLAYABLE_RECOMMENDATIONS = 50;
        
        private static List<ProblemData> ErrorEntries { get; set; } = new List<ProblemData>();
        private static List<ProblemData> IssueEntries { get; set; } = new List<ProblemData>();
        private static List<ProblemData> RecommendationEntries { get; set; } = new List<ProblemData>();
        private Vector2 _scrollPosition;
        private bool _addGreyBarToError;
        private bool _addGreyBarToIssue;
        private bool _addGreyBarToRecommendation;
        
        public enum ProblemLevels { Error, Issue, Recommendation }

        #endregion

        private void OnDidOpenScene() => SearchForProblems();

        private void OnGUI() {
            _addGreyBarToError = false;
            _addGreyBarToIssue = false;
            _addGreyBarToRecommendation = false;
            
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            
                ItsVREditorElements.GUILabelElement("  It's VR", ItsVREditorElements.TextColors.White, ItsVREditorElements.TextStyleLarge, true);
                ItsVREditorElements.GUILabelElement("Problem debugger", ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleMedium);
            
            GUILayout.EndHorizontal();

            if (ErrorEntries.Count == 0 && IssueEntries.Count == 0 && RecommendationEntries.Count == 0) {
                GUILayout.Space(5);
                ItsVREditorElements.GUILabelElement("No problems to report. Press the 'Search for problems' button to refresh.", ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);
               
                GUILayout.Space(5);
                if (GUILayout.Button("Search for problems"))
                    SearchForProblems();
                
                return;
            }
            
            GUILayout.BeginVertical();
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, false, false);
            
            if (ErrorEntries.Count != 0) {
                GUILayout.Space(5);
                ItsVREditorElements.GUILabelElement("Errors", ItsVREditorElements.TextColors.Red, ItsVREditorElements.TextStyleMedium, true);
                    
                GUILayout.Space(5);
                for (var i = 0; i < ErrorEntries.Count; i++) {
                    if (i == MAXIMUM_DISPLAYABLE_ERRORS) {
                        ItsVREditorElements.GUILabelElement($"<b>Maximum displayable errors threshold reached. {ErrorEntries.Count - MAXIMUM_DISPLAYABLE_ERRORS} error(s) hidden.</b>", ItsVREditorElements.TextColors.Red, ItsVREditorElements.TextStyleSmall);
                        break;
                    }
                        
                    GUILayout.BeginVertical(new GUIStyle {normal = new GUIStyleState {background = _addGreyBarToError ? Texture2D.grayTexture : null}});
                        
                        if (ErrorEntries[i].transform != null)
                            ItsVREditorElements.GUILabelElement($"<b>{ErrorEntries[i].transform.gameObject.name}</b> reported:", ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);
                        
                    GUILayout.BeginHorizontal();

                        ItsVREditorElements.GUILabelElement(ErrorEntries[i].problem, ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);

                        if (ErrorEntries[i].transform != null && GUILayout.Button("Select", GUILayout.Width(50))) 
                            Selection.SetActiveObjectWithContext(ErrorEntries[i].transform, ErrorEntries[i].transform.gameObject);

                    GUILayout.EndHorizontal();
                    GUILayout.Space(5);
                    GUILayout.EndVertical();
                    GUILayout.Space(5);
                        
                    _addGreyBarToError = !_addGreyBarToError;
                }
            }

            if (IssueEntries.Count != 0) {
                GUILayout.Space(5);
                ItsVREditorElements.GUILabelElement("Issues", ItsVREditorElements.TextColors.Yellow, ItsVREditorElements.TextStyleMedium, true);

                GUILayout.Space(5);
                for (var i = 0; i < IssueEntries.Count; i++) {
                    if (i == MAXIMUM_DISPLAYABLE_ISSUES) {
                        ItsVREditorElements.GUILabelElement($"<b>Maximum displayable issues threshold reached. {IssueEntries.Count - MAXIMUM_DISPLAYABLE_ISSUES} issue(s) hidden.</b>", ItsVREditorElements.TextColors.Red, ItsVREditorElements.TextStyleSmall);
                        break;
                    }
                    
                    GUILayout.BeginVertical(new GUIStyle {normal = new GUIStyleState {background = _addGreyBarToIssue ? Texture2D.grayTexture : null}});
                    
                        if (IssueEntries[i].transform != null) 
                            ItsVREditorElements.GUILabelElement($"<b>{IssueEntries[i].transform.gameObject.name}</b> reported:", ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);
                    
                    GUILayout.BeginHorizontal();
                                        
                        ItsVREditorElements.GUILabelElement(IssueEntries[i].problem, ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);

                        if (IssueEntries[i].transform != null && GUILayout.Button("Select", GUILayout.Width(50)))
                            Selection.SetActiveObjectWithContext(IssueEntries[i].transform, IssueEntries[i].transform.gameObject);

                    GUILayout.EndHorizontal();
                    GUILayout.Space(5);
                    GUILayout.EndVertical();
                    GUILayout.Space(5);

                    _addGreyBarToIssue = !_addGreyBarToIssue;
                }
            }

            if (RecommendationEntries.Count != 0) {
                GUILayout.Space(5);
                ItsVREditorElements.GUILabelElement("Recommendations", ItsVREditorElements.TextColors.Green, ItsVREditorElements.TextStyleMedium, true);

                GUILayout.Space(5);
                for (var i = 0; i < RecommendationEntries.Count; i++) {
                    if (i == MAXIMUM_DISPLAYABLE_RECOMMENDATIONS) {
                        ItsVREditorElements.GUILabelElement($"<b>Maximum displayable recommendations threshold reached. {RecommendationEntries.Count - MAXIMUM_DISPLAYABLE_RECOMMENDATIONS} recommendation(s) hidden.</b>", ItsVREditorElements.TextColors.Red, ItsVREditorElements.TextStyleSmall);
                        break;
                    }
                    
                    GUILayout.BeginVertical(new GUIStyle {normal = new GUIStyleState {background = _addGreyBarToRecommendation ? Texture2D.grayTexture : null}});

                        if (RecommendationEntries[i].transform != null)
                            ItsVREditorElements.GUILabelElement($"<b>{RecommendationEntries[i].transform.gameObject.name}</b> reported:", ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);
                    
                    GUILayout.BeginHorizontal();

                        ItsVREditorElements.GUILabelElement(RecommendationEntries[i].problem, ItsVREditorElements.TextColors.Grey, ItsVREditorElements.TextStyleSmall);

                        if (RecommendationEntries[i].transform != null && GUILayout.Button("Select", GUILayout.Width(50)))
                            Selection.SetActiveObjectWithContext(RecommendationEntries[i].transform, RecommendationEntries[i].transform.gameObject);

                    GUILayout.EndHorizontal();
                    GUILayout.Space(5);
                    GUILayout.EndVertical();
                    GUILayout.Space(5);

                    _addGreyBarToRecommendation = !_addGreyBarToRecommendation;
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndScrollView();
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button("Search for problems"))
                SearchForProblems();
        }
        
        private void SearchForProblems() {
            ErrorEntries.Clear();
            IssueEntries.Clear();
            RecommendationEntries.Clear();

            foreach (var debugger in FindObjectsOfType<MonoBehaviour>().OfType<IItsVRProblemDebugable>())
                debugger.RefreshProblems();

            FindComponentProblems();

            Repaint();
        }
        
        private static void FindComponentProblems() {
            if (FindObjectsOfType<VRRig>().Length > 1)
                SubmitProblem("More than one VR rig was found in the scene. You should only have one at a time.", ProblemLevels.Issue, null);
            
            if (FindObjectsOfType<VRController>().Length > 2)
                SubmitProblem("More than two VR controllers were found in the scene. You should only have a maximum of two.", ProblemLevels.Issue, null);

            if (FindObjectsOfType<VRTracker>().Length > 6)
                SubmitProblem("A lot of VR trackers were found in the scene. We recommend having a maximum of six VR trackers at a time.", ProblemLevels.Recommendation, null);

            if (FindObjectsOfType<VRBaseInteractable>().Length > 0 && FindObjectsOfType<VRBaseInteractor>().Length == 0)
                SubmitProblem("VR interactables were found in the scene but no VR interactors could be found. Add VR interactors to your player controller to enable interactions.", ProblemLevels.Issue, null);
            
            if (FindObjectsOfType<VRBaseInteractable>().Length == 0 && FindObjectsOfType<VRBaseInteractor>().Length > 0)
                SubmitProblem("VR interactors were found in the scene but no VR interactables could be found. If you do not need interactors, we recommend you remove them or disable them for performance.", ProblemLevels.Recommendation, null);
        }

        /// <summary>
        /// Open the input debugger window.
        /// </summary>
        [MenuItem("It's VR/Tools/Problem Debugger")]
        public static void OpenWindow() => GetWindow<ItsVRProblemDebuggerEditor>(EDITOR_WINDOW_TITLE);

        /// <summary>
        /// Submits a problem to the debugger.
        /// </summary>
        /// <param name="problemInformation">Information about the problem.</param>
        /// <param name="problemLevel">The severity of the problem.</param>
        /// <param name="componentTransform">The transform of the object which is used for the "select" feature.</param>
        public static void SubmitProblem(string problemInformation, ProblemLevels problemLevel, Transform componentTransform) {
            switch (problemLevel) {
                case ProblemLevels.Error:
                    var entryA = new ProblemData {
                        problem = problemInformation,
                        level = problemLevel,
                        transform = componentTransform
                    };
                    ErrorEntries ??= new List<ProblemData>();
                    ErrorEntries.Add(entryA);
                    break;
                case ProblemLevels.Issue:
                    var entryB = new ProblemData {
                        problem = problemInformation,
                        level = problemLevel,
                        transform = componentTransform
                    };
                    IssueEntries ??= new List<ProblemData>();
                    IssueEntries.Add(entryB);
                    break;
                case ProblemLevels.Recommendation:
                    var entryC = new ProblemData {
                        problem = problemInformation,
                        level = problemLevel,
                        transform = componentTransform
                    };
                    RecommendationEntries ??= new List<ProblemData>();
                    RecommendationEntries.Add(entryC);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(problemLevel), problemLevel, null);
            }
        }
    }

    /// <summary>
    /// Entry container for the It's VR problem debugger editor.
    /// </summary>
    public class ProblemData {
        public string problem;
        public ItsVRProblemDebuggerEditor.ProblemLevels level;
        public Transform transform;
    }
}