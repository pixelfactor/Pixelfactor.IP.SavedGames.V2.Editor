using Pixelfactor.IP.SavedGames.V2.Editor.Tools;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools
{
    public class ValidatorTool : MonoBehaviour
    {
        public static void Validate()
        {
            // Find the saved game
            var editorScenario = GameObject.FindObjectOfType<EditorScenario>();
            if (editorScenario == null)
            {
                Debug.LogError("No editor saved game found"); return;
            }

            Validator.Validate(editorScenario, false);
        }
    }
}
