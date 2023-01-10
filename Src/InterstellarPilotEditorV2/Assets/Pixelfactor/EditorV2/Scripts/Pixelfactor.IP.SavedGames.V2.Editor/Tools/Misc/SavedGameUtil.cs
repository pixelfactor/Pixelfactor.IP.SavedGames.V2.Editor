using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools
{
    public static class SavedGameUtil
    {
        public static EditorScenario FindSavedGameOrErrorOut()
        {
            var editorScenario = FindSavedGame();
            if (editorScenario == null)
            {
                EditorUtility.DisplayDialog("IP Editor", $"Cannot find a {nameof(EditorScenario)} type object in the current scene", "Ok");
                return null;
            }

            return editorScenario;
        }

        public static EditorScenario FindSavedGame()
        {
            return GameObject.FindObjectOfType<EditorScenario>();
        }

        public static List<EditorSector> FindSectors()
        {
            var savedGame = FindSavedGame();
            if (savedGame != null)
            {
                return savedGame.GetComponentsInChildren<EditorSector>().ToList();
            }

            return new List<EditorSector>();
        }
    }
}
