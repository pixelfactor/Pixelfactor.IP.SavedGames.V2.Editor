using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Sectors;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools.Edit
{
    public static class EditSectorTool
    {
        public static void RandomizeAll(EditorScenario editorScenario)
        {
            foreach (var sector in editorScenario.GetSectors())
            {
                Randomize(sector);
            }
        }

        public static void RandomizeAllWithoutDirty(EditorScenario editorScenario)
        {
            foreach (var sector in editorScenario.GetSectors())
            {
                RandomizeWithoutDirty(sector);
            }
        }

        public static void RandomizeWithoutDirty(EditorSector editorSector)
        {
            editorSector.Seed = Random.Range(0, int.MaxValue);

            // TODO: Assign name
            editorSector.Name = SectorNamingTool.GetUniqueSectorName(editorSector, editorSector.GetSavedGame());
            editorSector.WormholeDistance = Random.Range(0.6f, 1.0f);

            AutoNameObjects.AutoNameSector(editorSector);
        }

        public static void Randomize(EditorSector editorSector)
        {
            RandomizeWithoutDirty(editorSector);

            EditorUtility.SetDirty(editorSector);
        }

        public static void AutoNameSector(EditorSector editorSector)
        {
            var editorScenario = editorSector.GetComponentInParent<EditorScenario>();
            editorSector.Name = SectorNamingTool.GetUniqueSectorName(editorSector, editorScenario);
            EditorUtility.SetDirty(editorSector);
        }
    }
}
