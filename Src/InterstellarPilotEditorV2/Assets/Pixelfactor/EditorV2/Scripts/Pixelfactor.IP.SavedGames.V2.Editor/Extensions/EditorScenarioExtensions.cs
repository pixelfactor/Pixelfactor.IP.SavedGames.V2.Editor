using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using System.Collections.Generic;

namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    public static class EditorSavedGameExtensions
    {
        public static EditorSector[] GetSectors(this EditorScenario editorScenario)
        {
            if (editorScenario == null) 
                return new EditorSector[0];

            return editorScenario.GetSectorsRoot().GetComponentsInChildren<EditorSector>();
        }
    }
}
