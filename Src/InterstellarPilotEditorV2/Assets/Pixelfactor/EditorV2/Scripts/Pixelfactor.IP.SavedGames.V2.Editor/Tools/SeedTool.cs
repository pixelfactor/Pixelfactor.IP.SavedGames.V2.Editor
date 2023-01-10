using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools
{
    public static class SeedTool
    {
        public static void AutosetSeedsIfPreferred(EditorScenario editorScenario)
        {
            var settings = CustomSettings.GetOrCreateSettings();

            if (settings.Export_AutosetSectorSeed)
            {
                AutosetSectorSeeds(editorScenario);
            }

            if (settings.Export_AutosetUnitSeed)
            {
                AutosetUnitSeeds(editorScenario);
            }
        }

        public static void AutosetUnitSeeds(EditorScenario editorScenario)
        {
            foreach (var unit in editorScenario.GetComponentsInChildren<EditorUnit>())
            {
                if (unit.Seed == -1)
                {
                    unit.Seed = Random.Range(0, int.MaxValue);
                    EditorUtility.SetDirty(unit);
                }
            }
        }

        public static void AutosetSectorSeeds(EditorScenario editorScenario)
        {
            foreach (var sector in editorScenario.GetComponentsInChildren<EditorSector>())
            {
                if (sector.Seed == -1)
                {
                    sector.Seed = Random.Range(0, int.MaxValue);
                    EditorUtility.SetDirty(sector);
                }
            }
        }
    }
}
