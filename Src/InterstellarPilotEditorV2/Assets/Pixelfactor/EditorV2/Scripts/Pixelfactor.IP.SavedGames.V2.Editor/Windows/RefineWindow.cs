using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Build.Connect;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Edit;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Windows
{
    public class RefineWindow
    {
        public void Draw()
        {
            var editorScenario = SavedGameUtil.FindSavedGame();

            EditorGUI.BeginDisabledGroup(editorScenario == null);
            if (GUILayout.Button(
                new GUIContent(
                    "Fleets own children",
                    "Sets the children (e.g. ships/people) of any fleets to be the same faction as the fleet itself"),
                GuiHelper.ButtonLayout))
            {
                FixUpUnitOwnership.SetFleetChildrenToSameFaction(editorScenario);
                Debug.Log("Finished setting fleet children to the same faction");
            }

            if (GUILayout.Button(
                new GUIContent(
                    "Auto-name editor objects",
                    "Gives a name to all units based on their type and faction"),
                GuiHelper.ButtonLayout))
            {
                AutoNameObjects.AutoNameAllObjects();
                Debug.Log("Finished auto-naming all objects");
            }

            EditorGUI.EndDisabledGroup();

            var selectedSectors = Selection.GetFiltered<EditorSector>(SelectionMode.TopLevel | SelectionMode.Assets | SelectionMode.Editable | SelectionMode.ExcludePrefab);
            EditorGUI.BeginDisabledGroup(editorScenario == null || selectedSectors.Length == 0);

            if (GUILayout.Button(
                new GUIContent(
                    "Randomize selected sectors",
                    "Gives a random name and setts to selected sectors"),
                GuiHelper.ButtonLayout))
            {
                foreach (var sector in selectedSectors)
                {
                    EditSectorTool.Randomize(sector);
                }
                Debug.Log($"Finished randomizing {selectedSectors.Length} selected sectors");
            }

            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(editorScenario == null || selectedSectors.Length == 0);

            if (GUILayout.Button(
                new GUIContent(
                    "Reposition wormholes",
                    "Moves and rotates wormholes based on sector size and their target"),
                GuiHelper.ButtonLayout))
            {
                foreach (var sector in selectedSectors)
                {
                    foreach (var editorWormhole in sector.GetValidStableWormholes())
                    {
                        ConnectSectorsTool.AutoPositionWormholeUnit(editorWormhole.GetComponent<EditorUnit>(), editorScenario.MaxWormholeDistance);
                    }
                }
                Debug.Log($"Finished auto-positioning wormholes in {selectedSectors.Length} selected sectors");
            }

            EditorGUI.EndDisabledGroup();


        }
    }
}
