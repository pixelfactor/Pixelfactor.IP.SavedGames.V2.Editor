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

        public static IEnumerable<string> GetUsedNames(EditorScenario editorScenario)
        {
            return editorScenario.GetComponentsInChildren<EditorSector>().Select(e => e.Name).Distinct();
        }

        public static IEnumerable<string> GetAvailableSectorNames(EditorScenario editorScenario)
        {
            var allNames = GetAllSectorNames();
            var usedNames = GetUsedNames(editorScenario);

            return allNames.Where(e => !usedNames.Contains(e));
        }

        public static string GetUniqueSectorName(EditorSector sector, EditorScenario editorScenario)
        {
            var name = GetAvailableSectorNames(editorScenario).GetRandom();
            if (!string.IsNullOrWhiteSpace(name))
            {
                return name;
            }

            var stringBuilder = new System.Text.StringBuilder();
            var designation = CalculateRandomSectorDesignation(sector.Seed, stringBuilder);

            return $"Unknown {designation}";
        }

        public static string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static string numerals = "0123456789";

        /// <summary>
        /// Creates a unique user-friendly alphanumeric identifier e.g. AE3-TS34<br />
        /// Note: Makes an allocation
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static string CalculateRandomSectorDesignation(int seed, System.Text.StringBuilder stringBuilder)
        {
            var random = new System.Random(seed);
            stringBuilder.Clear();
            stringBuilder.Append(RandomAlphabet(random));
            stringBuilder.Append(RandomNumeral(random));
            stringBuilder.Append("-");
            stringBuilder.Append(RandomNumeral(random));
            stringBuilder.Append(RandomNumeral(random));
            stringBuilder.Append(RandomNumeral(random));
            stringBuilder.Append(RandomAlphabet(random));
            return stringBuilder.ToString();
        }

        public static char RandomAlphabet(System.Random random)
        {
            return chars[random.Next(0, chars.Length)];
        }

        public static char RandomNumeral(System.Random random)
        {
            return numerals[random.Next(0, numerals.Length)];
        }

        public static string[] ReadAndSplitTextLines(TextAsset t)
        {
            return t.text.Replace("\r\n", "\n").Split(
                new[] { "\n" },
                System.StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
