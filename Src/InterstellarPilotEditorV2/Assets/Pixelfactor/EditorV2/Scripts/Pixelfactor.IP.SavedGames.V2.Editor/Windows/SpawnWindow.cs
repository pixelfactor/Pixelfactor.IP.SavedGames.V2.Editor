using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Windows.SpawnWindows;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Windows
{
    public class SpawnWindow
    {
        public const int SpawnPlanetId = 0;
        public const int SpawnAsteroidClustersId = 1;
        public const int SpawnAsteroidsId = 2;
        public const int SpawnGasCloudsId = 3;
        public const int SpawnShipId = 4;
        public const int SpawnStationId = 5;
        public const int SpawnFleetsId = 6;

        private int currentTab = 0;
        private EditorFaction spawnFaction = null;

        private SpawnFleetsWindow spawnFleetsWindow = new SpawnFleetsWindow();
        private SpawnAsteroidClustersWindow spawnAsteroidClustersWindow = new SpawnAsteroidClustersWindow();
        private SpawnGasCloudsWindow spawnGasCloudsWindow = new SpawnGasCloudsWindow();
        private SpawnPlanetsWindow spawnPlanetsWindow =  new SpawnPlanetsWindow();
        private SpawnAsteroidsWindow spawnAsteroidsWindow = new SpawnAsteroidsWindow();

        public void Draw()
        {
            currentTab = GUILayout.Toolbar(currentTab, new string[] { "Planet", "Cluster", "Asteroid", "Cloud", "Ship", "Station", "Fleet" });

            switch (currentTab)
            {
                case SpawnPlanetId:
                    {
                        this.spawnPlanetsWindow.Draw();
                    }
                    break;
                case SpawnAsteroidClustersId:
                    {
                        spawnAsteroidClustersWindow.Draw();
                    }
                    break;
                case SpawnGasCloudsId:
                    {
                        spawnGasCloudsWindow.Draw();
                    }
                    break;
                case SpawnAsteroidsId:
                    {
                        this.spawnAsteroidsWindow.Draw();
                    }
                    break;
                case SpawnShipId:
                    {
                        ShowSpawnOptionsAndSector("Ship", true, ref this.spawnFaction);
                    }
                    break;
                case SpawnStationId:
                    {
                        ShowSpawnOptionsAndSector("Station", true, ref this.spawnFaction);
                    }
                    break;
                case SpawnFleetsId:
                    {
                        this.spawnFleetsWindow.Draw(ref this.spawnFaction);
                    }
                    break;
            }

            EditorGUI.EndDisabledGroup();
        }

        public static void ShowSpawnOptionsAndSector(string subDirectory, bool allowFaction, ref EditorFaction spawnFaction)
        {
            var sector = Selector.GetSingleSelectedSectorOrNull();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Spawn Sector", WindowHelper.DescribeSectors(sector));
            EditorGUI.EndDisabledGroup();

            SpawnWindowHelper.ShowSpawnUnitOptions(subDirectory, allowFaction, ref spawnFaction);
        }
    }
}
