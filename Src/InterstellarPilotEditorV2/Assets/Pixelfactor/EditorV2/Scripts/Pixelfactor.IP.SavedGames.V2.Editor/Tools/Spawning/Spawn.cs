using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Edit;
using UnityEditor;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools.Spawning
{
    public static class Spawn
    {
        public static EditorSector NewSector(EditorSavedGame editorSavedGame)
        {
            return NewSector(editorSavedGame, CustomSettings.GetOrCreateSettings().SectorPrefabPath);
        }

        public static EditorSector NewSector(EditorSavedGame editorSavedGame, string prefabPath)
        {
            var sector = PrefabHelper.Instantiate<EditorSector>(prefabPath, editorSavedGame.GetSectorsRoot());
            EditSectorTool.Randomize(sector);
            return sector;
        }
    }
}
