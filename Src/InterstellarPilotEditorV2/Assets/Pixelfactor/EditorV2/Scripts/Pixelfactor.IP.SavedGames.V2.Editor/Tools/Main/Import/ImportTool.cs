using Pixelfactor.IP.SavedGames.V2.Model;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools.Main.Import
{
    public static class ImportTool
    {
        public static void ImportFromFile(string filePath, EditorSavedGame editorSavedGame)
        {
            var saveModel = BinaryReadTool.ReadFromFile(filePath, out _);
            ImportFromModel((SavedGame)saveModel, editorSavedGame);
        }

        public static void ImportFromModel(SavedGame savedGameModel, EditorSavedGame editorSavedGame)
        {
            new ImportOperation(savedGameModel, editorSavedGame).Import();
        }
    }
}
