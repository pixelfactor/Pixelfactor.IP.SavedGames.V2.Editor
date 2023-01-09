using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools
{
    public static class SeedTool
    {
        public static void AutosetSeedsIfPreferred(EditorSavedGame editorSavedGame)
        {
            var settings = CustomSettings.GetOrCreateSettings();

            if (settings.Export_AutosetSectorSeed)
            {
                AutosetSectorSeeds(editorSavedGame);
            }

            if (settings.Export_AutosetUnitSeed)
            {
                AutosetUnitSeeds(editorSavedGame);
            }
        }

        public static void AutosetUnitSeeds(EditorSavedGame editorSavedGame)
        {
            foreach (var unit in editorSavedGame.GetComponentsInChildren<EditorUnit>())
            {
                if (unit.Seed == -1)
                {
                    unit.Seed = Random.Range(0, int.MaxValue);
                    EditorUtility.SetDirty(unit);
                }
            }
        }

        public static void AutosetSectorSeeds(EditorSavedGame editorSavedGame)
        {
            foreach (var sector in editorSavedGame.GetComponentsInChildren<EditorSector>())
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
