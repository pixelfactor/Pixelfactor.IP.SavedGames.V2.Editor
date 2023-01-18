using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using System;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools
{

    public class FixUpUnitOwnership : MonoBehaviour
    {
        public static void SetUnitFactionsToPilotFactionsMenuItem()
        {
            var editorScenario = SavedGameUtil.FindSavedGameOrErrorOut();

            SetUnitFactionsToPilotFactions(editorScenario);

            Debug.Log("Finished set unit factions to pilot factions");
        }

        public static void SetFleetChildrenToSameFactionMenuItem()
        {
            var editorScenario = SavedGameUtil.FindSavedGameOrErrorOut();

            SetFleetChildrenToSameFaction(editorScenario);

            Debug.Log("Finished set fleet children to same faction");
        }

        public static void SetFleetChildrenToSameFaction(EditorScenario editorScenario)
        {
            foreach (var editorFleet in editorScenario.GetComponentsInChildren<EditorFleet>())
            {
                if (editorFleet.Faction != null)
                {
                    MakeFleetOwnChildren(editorFleet);
                }
            }
        }

        public static void MakeFleetOwnChildren(EditorFleet editorFleet)
        {
            if (editorFleet.Faction != null)
            {
                foreach (var editorUnit in editorFleet.GetComponentsInChildren<EditorUnit>())
                {
                    if (editorUnit.Faction != editorFleet.Faction)
                    {
                        editorUnit.Faction = editorFleet.Faction;
                        EditorUtility.SetDirty(editorUnit);
                    }
                }

                foreach (var editorPerson in editorFleet.GetComponentsInChildren<EditorPerson>())
                {
                    if (editorPerson.Faction != editorFleet.Faction)
                    {
                        editorPerson.Faction = editorFleet.Faction;
                        EditorUtility.SetDirty(editorPerson);
                    }
                }
            }
        }

        public static void SetUnitFactionsToPilotFactions(EditorScenario editorScenario)
        {
            foreach (var editorSector in editorScenario.GetComponentsInChildren<EditorSector>())
            {
                foreach (var editorUnit in editorSector.GetComponentsInChildren<EditorUnit>())
                {
                    var editorPerson = editorUnit.GetComponentInChildren<EditorPerson>();
                    if (editorPerson != null && editorPerson.Faction != null && editorPerson.Faction != editorUnit.Faction)
                    {
                        editorUnit.Faction = editorPerson.Faction;
                        EditorUtility.SetDirty(editorUnit);
                    }
                }
            }
        }
    }
}
