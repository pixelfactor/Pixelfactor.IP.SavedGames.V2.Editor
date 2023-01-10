using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using System.Collections.Generic;

namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    public static class EditorSavedGameExtensions
    {
        public static EditorSector[] GetSectors(this EditorSavedGame editorSavedGame)
        {
            if (editorSavedGame == null) 
                return new EditorSector[0];

            return editorSavedGame.GetSectorsRoot().GetComponentsInChildren<EditorSector>();
        }
    }
}
