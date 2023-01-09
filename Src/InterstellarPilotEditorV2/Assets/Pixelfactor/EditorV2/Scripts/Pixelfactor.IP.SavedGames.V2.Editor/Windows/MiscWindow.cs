using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Misc;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Windows
{
    public class MiscWindow
    {
        public void Draw()
        {
            var content = new GUIContent("Selected distance", "Select two game objects to measure their distance");
            EditorGUILayout.LabelField("Selected distance", EditorStyles.boldLabel);
            var distance = MeasureTool.MeasureSelected();
            EditorGUILayout.LabelField($"{distance:N4}");
        }
    }
}
