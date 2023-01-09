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
            labelStyle.fontSize = 30;
            labelStyle.fontStyle = FontStyle.Bold;

            EditorGUILayout.LabelField(text, labelStyle, GUILayout.Height(30));
            HelpPrompt(helpPrompt);

            EditorGUILayout.Space(15);
        }
    }
}
