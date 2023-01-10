using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Sectors;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools.Edit
{
    public static class EditSectorTool
    {
        public static void RandomizeAll(EditorSavedGame editorSavedGame)
        {
            foreach (var sector in editorSavedGame.GetComponentsInChildren<EditorSector>())
            {
                Randomize(sector);
            }
        }

        public static void Randomize(EditorSector editorSector)
        {
            editorSector.Seed = Random.Range(0, int.MaxValue);

            // TODO: Assign name
            editorSector.Name = SectorNamingTool.GetUniqueSectorName(editorSector, editorSector.GetSavedGame());

            AutoNameObjects.AutoNameSector(editorSector);

            EditorUtility.SetDirty(editorSector);
        }

        public static void AutoNameSector(EditorSector editorSector)
        {
            var editorSavedGame = editorSector.GetComponentInParent<EditorSavedGame>();
            editorSector.Name = SectorNamingTool.GetUniqueSectorName(editorSector, editorSavedGame);
            EditorUtility.SetDirty(editorSector);
        }
    }
}
