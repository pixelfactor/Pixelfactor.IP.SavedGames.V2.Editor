using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using UnityEditor;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Utilities
{
    /// <summary>
    /// Runs the scenario when the unity play button is pressed
    /// </summary>
    [InitializeOnLoadAttribute]
    public class HijackPlayButton
    {
        static HijackPlayButton()
        {
            EditorApplication.playModeStateChanged += LogPlayModeState;
        }

        private static void LogPlayModeState(PlayModeStateChange state)
        {
            var settings = CustomSettings.GetOrCreateSettings();
            if (!settings.RunScenarioOnPlayMode)
                return;

            if (EditorApplication.isPlaying)
            { 
                PlayTool.Play();
                EditorApplication.ExitPlaymode();
            }
        }
    }
}
