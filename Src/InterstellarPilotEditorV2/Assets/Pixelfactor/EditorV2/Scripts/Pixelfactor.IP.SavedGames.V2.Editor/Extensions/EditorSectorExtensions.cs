using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;

namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    public static class EditorSectorExtensions
    {
        public static EditorSavedGame GetSavedGame(this EditorSector editorSector)
        {
            return editorSector.GetComponentInParent<EditorSavedGame>();
        }
    }
}
