﻿using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorHelpers
{
    public class EditorGuiText : MonoBehaviour
    {
        [Multiline(5)]
        public string Text;

        public Vector2 CustomScreenOffset = Vector2.zero;

        private void OnDrawGizmos()
        {
            UnityEditor.Handles.BeginGUI();
            var guiStyle = new GUIStyle();
            guiStyle.alignment = TextAnchor.UpperLeft;
            guiStyle.normal.textColor = Color.white;
            guiStyle.fontSize = 12;
            guiStyle.wordWrap = true;

            var insetX = 70;
            var insetY = 20;
            GUI.Label(new Rect(insetX + CustomScreenOffset.x, insetY + CustomScreenOffset.y, Screen.width - (insetX * 2), Screen.height - insetY), this.Text, guiStyle);
            UnityEditor.Handles.EndGUI();
        }
    }
}
