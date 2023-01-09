using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools.Sectors
{
    public static class SectorNamingTool
    {
        public static IEnumerable<string> GetAllSectorNames()
        {
            var namePath = CustomSettings.GetOrCreateSettings().SectorNamesPath;
            var namesAsset = (TextAsset)AssetDatabase.LoadAssetAtPath(namePath, typeof(TextAsset));

            return ReadAndSplitTextLines(namesAsset);
        }

        public static IEnumerable<string> GetUsedNames(EditorSavedGame editorSavedGame)
        {
            return editorSavedGame.GetComponentsInChildren<EditorSector>().Select(e => e.Name).Distinct();
        }

        public static IEnumerable<string> GetAvailableSectorNames(EditorSavedGame editorSavedGame)
        {
            var allNames = GetAllSectorNames();
            var usedNames = GetUsedNames(editorSavedGame);

            return allNames.Where(e => !usedNames.Contains(e));
        }

        public static string GetUniqueSectorName(EditorSavedGame editorSavedGame)
        {
            return GetAvailableSectorNames(editorSavedGame).GetRandom();
        }

        public static string[] ReadAndSplitTextLines(TextAsset t)
        {
            return t.text.Replace("\r\n", "\n").Split(
                new[] { "\n" },
                System.StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
