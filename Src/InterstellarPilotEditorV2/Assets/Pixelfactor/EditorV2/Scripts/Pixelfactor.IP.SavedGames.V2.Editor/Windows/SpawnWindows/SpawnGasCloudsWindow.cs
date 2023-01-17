using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Windows.SpawnWindows
{
    public class SpawnGasCloudsWindow
    {
        public void Draw()
        {
            DrawDeleteOptions();
        }

        private void DrawDeleteOptions()
        {
            GuiHelper.Subtitle("Delete gas clouds", "Deletes gas clouds from the selected sectors");
            var selectedSectors = Selector.GetInParents<EditorSector>();
            if (selectedSectors.Count() == 0)
            {
                var savedGame = SavedGameUtil.FindSavedGame();
                selectedSectors = savedGame.GetComponentsInChildren<EditorSector>();
            }

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Target sector", WindowHelper.DescribeSectors(selectedSectors));
            EditorGUI.EndDisabledGroup();

            var canDelete = selectedSectors.Count() > 0;

            EditorGUI.BeginDisabledGroup(!canDelete);
            if (GUILayout.Button(
                new GUIContent(
                    $"Delete gas clouds",
                    $"Deletes gas clouds from the selected sectors"),
                GuiHelper.ButtonLayout))
            {
                foreach (var sector in selectedSectors)
                {
                    foreach (var unit in sector.GetComponentsInChildren<EditorGasCloud>())
                    {
                        GameObject.DestroyImmediate(unit.gameObject);
                    }
                }
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}
