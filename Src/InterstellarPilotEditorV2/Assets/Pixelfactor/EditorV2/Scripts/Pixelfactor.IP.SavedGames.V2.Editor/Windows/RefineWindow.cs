using Pixelfactor.IP.SavedGames.V2.Editor.Tools;
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
        }
    }
}
