using Pixelfactor.IP.SavedGames.V2.Editor.Tools;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Windows
{
    public class RefineWindow
    {
        public void Draw()
        {
            if (GUILayout.Button(
                new GUIContent(
                    "Auto-name all objects",
                    "Gives a name to all units based on their type and faction"),
                GuiHelper.ButtonLayout))
            {
                AutoNameObjects.AutoNameAllObjects();
            }
        }
    }
}
