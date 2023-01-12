using Pixelfactor.IP.SavedGames.V2.Editor.Tools;
using System;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Windows
{
    public class StatsWindow
    {
        public void Draw()
        {
            GuiHelper.Subtitle("Stats", "Info about the current scenario");

            var editorScenario = SavedGameUtil.FindSavedGame();
            var hasScenario = editorScenario != null;

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Current scenario", editorScenario != null ? editorScenario.Title : "[None]");
            EditorGUI.EndDisabledGroup();

            if (hasScenario)
            {
                Stat("Current time", $"{editorScenario.ScenarioTime:N2}", "The current game timer in seconds");

                Stat("Sectors", $"{editorScenario.GetSectorCount():N0}", "The number of sectors in the game");
                Stat("Wormholes", $"{editorScenario.GetStableWormholeCount():N0}", "The number of normal wormhols in the game");
                Stat("Unstable wormholes", $"{editorScenario.GetUnstableWormholeCount():N0}", "The number of unstable wormhols in the game");
                Stat("Factions", $"{editorScenario.GetFactionCount():N0}", "The number of factions in the game");
                Stat("People", $"{editorScenario.GetPersonCount():N0}", "The number of people in the game");
                Stat("Fleets", $"{editorScenario.GetFleetCount():N0}", "The number of people in the game");
                Stat("Stations", $"{editorScenario.GetMajorStationCount():N0}", "The number of major stations in the game");
                Stat("Minor stations", $"{editorScenario.GetMinorStationCount():N0}", "The number of minor stations (turrets, satellites) in the game");
                Stat("Asteroid clusters", $"{editorScenario.GetAsteroidClusterCount():N0}", "The number of asteroid clusters in the game");
                Stat("Asteroids", $"{editorScenario.GetAsteroidCount():N0}", "The number of asteroids in the game");
                Stat("Gas clouds", $"{editorScenario.GetGasCloudCount():N0}", "The number of gas clouds in the game");
                Stat("Planets", $"{editorScenario.GetPlanetCount():N0}", "The number of planets in the game");
                Stat("Abandoned ships", $"{editorScenario.GetAbandonedShipCount():N0}", "The number of ships without a faction in the game");
                Stat("Abandoned stations", $"{editorScenario.GetAbandonedStationCount():N0}", "The number of stations without a faction in the game");
            }
        }

        private void Stat(string prefix, string value, string description)
        {
            EditorGUILayout.LabelField(new GUIContent(prefix, description), new GUIContent(value, description));
        }
    }
}