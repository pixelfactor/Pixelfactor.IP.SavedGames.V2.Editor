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
    }
}
