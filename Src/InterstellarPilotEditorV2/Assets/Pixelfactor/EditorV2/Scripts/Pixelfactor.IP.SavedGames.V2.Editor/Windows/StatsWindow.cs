using Pixelfactor.IP.SavedGames.V2.Editor.Tools;
using System;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Windows
{
    public class StatsWindow
    {
        private StringBuilder stringBuilder = new StringBuilder();
        private Stats stats = null;
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
                if (this.stats == null)
                {
                    this.RefreshStats(editorScenario);
                }

                Stat("Current time", $"{editorScenario.ScenarioTime:N2}", "The current game timer in seconds");

                Stat("Sectors", $"{this.stats.SectorCount:N0}", "The number of sectors in the game");
                Stat("Wormholes", $"{this.stats.StableWormholeCount:N0}", "The number of normal wormhols in the game");
                Stat("Unstable wormholes", $"{this.stats.UnstableWormholeCount:N0}", "The number of unstable wormhols in the game");
                Stat("Factions", $"{this.stats.FactionCount:N0}", "The number of factions in the game");
                Stat("People", $"{this.stats.PersonCount:N0}", "The number of people in the game");
                Stat("Fleets", $"{this.stats.FleetCount:N0}", "The number of people in the game");
                Stat("Stations", $"{this.stats.MajorStationCount:N0}", "The number of major stations in the game");
                Stat("Minor stations", $"{this.stats.MinorStationCount:N0}", "The number of minor stations (turrets, satellites) in the game");
                Stat("Asteroid clusters", $"{this.stats.AsteroidClusterCount:N0}", "The number of asteroid clusters in the game");
                Stat("Asteroids", $"{this.stats.AsteroidCount:N0}", "The number of asteroids in the game");
                Stat("Gas clouds", $"{this.stats.GasCloudCount:N0}", "The number of gas clouds in the game");
                Stat("Planets", $"{this.stats.PlanetCount:N0}", "The number of planets in the game");
                Stat("Abandoned ships", $"{this.stats.AbandonedShipCount:N0}", "The number of ships without a faction in the game");
                Stat("Abandoned stations", $"{this.stats.AbandonedStationCount:N0}", "The number of stations without a faction in the game");

                if (GUILayout.Button(
                    new GUIContent(
                        "Refresh stats",
                        "Refreshes stats"),
                    GuiHelper.ButtonLayout))
                {
                    this.RefreshStats(editorScenario);
                }
            }
        }

        private void RefreshStats(EditorScenario editorScenario)
        {
            this.stats = new Stats();
            this.stats.SectorCount = editorScenario.GetSectorCount();
            this.stats.StableWormholeCount = editorScenario.GetStableWormholeCount();
            this.stats.UnstableWormholeCount = editorScenario.GetUnstableWormholeCount();
            this.stats.FactionCount = editorScenario.GetFactionCount();
            this.stats.PersonCount = editorScenario.GetPersonCount();
            this.stats.FleetCount = editorScenario.GetFleetCount();
            this.stats.MajorStationCount = editorScenario.GetMajorStationCount();
            this.stats.MinorStationCount = editorScenario.GetMinorStationCount();
            this.stats.AsteroidClusterCount = editorScenario.GetAsteroidClusterCount();
            this.stats.AsteroidCount = editorScenario.GetAsteroidCount();
            this.stats.GasCloudCount = editorScenario.GetGasCloudCount();
            this.stats.PlanetCount = editorScenario.GetPlanetCount();
            this.stats.AbandonedShipCount = editorScenario.GetAbandonedShipCount();
            this.stats.AbandonedStationCount = editorScenario.GetAbandonedStationCount();
        }

        private void Stat(string prefix, string value, string description)
        {
            EditorGUILayout.LabelField(new GUIContent(prefix, description), new GUIContent(value, description));
        }

        public class Stats
        {
            public int SectorCount { get; set; }
            public int StableWormholeCount { get; set; }
            public int UnstableWormholeCount { get; set; }
            public int FactionCount { get; set; }
            public int PersonCount { get; set; }
            public int FleetCount { get; set; }
            public int MajorStationCount { get; set; }
            public int MinorStationCount { get; set; }
            public int AsteroidClusterCount { get; set; }
            public int AsteroidCount { get; set; }
            public int GasCloudCount { get; set; }
            public int PlanetCount { get; set; }
            public int AbandonedShipCount { get; set; }
            public int AbandonedStationCount { get; set; }
        }
    }
}