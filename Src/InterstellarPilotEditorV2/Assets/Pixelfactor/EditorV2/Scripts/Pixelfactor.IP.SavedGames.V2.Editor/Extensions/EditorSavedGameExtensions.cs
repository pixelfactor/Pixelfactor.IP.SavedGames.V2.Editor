using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using System.Collections.Generic;

namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    public static class EditorSavedGameExtensions
    {
        public static IEnumerable<EditorSector> GetSectors(this EditorSavedGame editorSavedGame)
        {
            return editorSavedGame.GetComponentsInChildren<EditorSector>();
        }
    }
}
