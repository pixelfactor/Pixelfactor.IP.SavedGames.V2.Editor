using Pixelfactor.IP.SavedGames.V2.Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools
{
    public class ValidatorTool : MonoBehaviour
    {
        [MenuItem("Window/IP Editor V2/Validate")]
        public static void ValidateMenuItem()
        {
            // Find the saved game
            var editorSavedGame = GameObject.FindObjectOfType<EditorSavedGame>();
            if (editorSavedGame == null)
            {
                Debug.LogError("No editor saved game found"); return;
            }

            Validator.Validate(editorSavedGame, false);
        }
    }
}
