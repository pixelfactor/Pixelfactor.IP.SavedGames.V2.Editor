using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Utilities
{
    public static class SavedGameUtil
    {
        public static EditorSavedGame FindSavedGameOrErrorOut()
        {
            var editorSavedGame = FindSavedGame();
            if (editorSavedGame == null)
            {
                EditorUtility.DisplayDialog("IP Editor", $"Cannot find a {nameof(EditorSavedGame)} type object in the current scene", "Ok");
                return null;
            }

            return editorSavedGame;
        }

        public static EditorSavedGame FindSavedGame()
        {
            return GameObject.FindObjectOfType<EditorSavedGame>();
        }
    }
}
