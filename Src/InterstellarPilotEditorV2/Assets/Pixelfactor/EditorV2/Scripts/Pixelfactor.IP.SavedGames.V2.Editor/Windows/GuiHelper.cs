using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    public static class GuiHelper
    {
        public static void HelpPrompt(string text)
        {
            var labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontStyle = FontStyle.Italic;
            labelStyle.wordWrap = true;

            EditorGUILayout.LabelField(text, labelStyle);
        }

        public static void SectionSpace()
        {
            EditorGUILayout.Space(30);
        }

        public static void Subtitle(string text, string helpPrompt)
        {
            var labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.normal.textColor = Color.grey;
            labelStyle.fontSize = 24;
            labelStyle.fontStyle = FontStyle.Bold;

            EditorGUILayout.LabelField(text, labelStyle, GUILayout.Height(30));
            HelpPrompt(helpPrompt);

            EditorGUILayout.Space(15);
        }

        public static GUILayoutOption[] BigButtonLayout
        {
            get
            {
                return new GUILayoutOption[]
                {
                    GUILayout.Width(200),
                    GUILayout.Height(50)
                };
            }
        }

        public static GUILayoutOption[] ButtonLayout
        {
            get
            {
                return new GUILayoutOption[]
                {
                    GUILayout.Width(200),
                    GUILayout.Height(30)
                };
            }
        }

        public struct ButtonInfo
        {
            public string Text;
            public string Description;
            public System.Action OnClick;
        }

        /// <summary>
        /// Lays out buttons in a flowing grid
        /// </summary>
        /// <param name="buttons"></param>
        public static void FlowButtons(IList<ButtonInfo> buttons)
        {
            var count = buttons.Count;
            if (count > 0)
            {
                var viewWidth = EditorGUIUtility.currentViewWidth;
                var columnCount = Mathf.Max(1, Mathf.FloorToInt(viewWidth / 200));

                var i = 0;
                while (i < count)
                {
                    var button = buttons[i];
                    if (columnCount > 1)
                    {
                        if (i == 0)
                        {
                            EditorGUILayout.BeginHorizontal();
                        }
                        else
                        {
                            var currentColumnIndex = i % columnCount;
                            if (currentColumnIndex == 0)
                            {
                                EditorGUILayout.EndHorizontal();
                                EditorGUILayout.BeginHorizontal();
                            }
                        }
                    }

                    if (GUILayout.Button(
                        new GUIContent(
                            button.Text,
                            button.Description),
                        GuiHelper.ButtonLayout))
                    {
                        if (button.OnClick != null)
                        { 
                            button.OnClick();
                        }
                    }

                    i++;
                }

                if (columnCount > 1)
                {
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUI.EndDisabledGroup();
        }
    }
}
