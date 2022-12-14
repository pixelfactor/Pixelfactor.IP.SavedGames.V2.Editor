using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    public class EditorSavedGame : MonoBehaviour
    {
        public bool RandomEventsEnabled = true;

        /// <summary>
        /// The distance from sector origin<br />
        /// Applies when connecting sectors
        /// </summary>
        public float PreferredWormholeDistance = 2000.0f;

        /// <summary>
        /// Change this to your prefered title. Value is displayed when loading game
        /// </summary>
        public string Title = "Custom scenario";

        /// <summary>
        /// Change this to your name if required
        /// </summary>
        public string Author = "";

        /// <summary>
        /// Changes this to a description of the scenario.
        /// </summary>
        public string Description = "";

        public int DateYear = 2236;
        public int DateMonth = 1;
        public int DateDay = 1;
        public int DateHour = 0;
        public int DateMinute = 0;
    }
}
