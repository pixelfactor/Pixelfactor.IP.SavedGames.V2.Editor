using System;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    public class EditorScenario : MonoBehaviour
    {
        /// <summary>
        /// The distance from sector origin<br />
        /// Applies when connecting sectors
        /// </summary>
        [Tooltip("The max distance of wormholes from the sector center")]
        [Range(2500.0f, 6000.0f)]
        public float MaxWormholeDistance = 6000.0f;

        /// <summary>
        /// Change this to your prefered title. Value is displayed when loading game
        /// </summary>
        public string Title = "Custom scenario";

        /// <summary>
        /// Change this to your name if required
        /// </summary>
        [Tooltip("The optional author of the scenario. Reserved for future use")]
        public string Author = "";

        /// <summary>
        /// An optional short description of the scenario. Reserved for future use
        /// </summary>
        [Tooltip("An optional short description of the scenario. Reserved for future use")]
        [Multiline(3)]
        public string Description = "";

        /// <summary>
        /// An optional mission log for the scenario. Reserved for future use
        /// </summary>
        [Tooltip("An optional mission log for the scenario. Reserved for future use")]
        [Multiline(5)]
        public string MissionLog = "";

        public int DateYear = 2236;
        public int DateMonth = 1;
        public int DateDay = 1;
        public int DateHour = 0;
        public int DateMinute = 0;

        /// <summary>
        /// The editor will create sectors underneath this object
        /// </summary>
        public Transform SectorsRoot = null;

        /// <summary>
        /// The editor will create factions underneath this object
        /// </summary>
        public Transform FactionsRoot = null;

        [Tooltip("The current elapsed game time in seconds")]
        public double ScenarioTime = 0.0;

        public Transform GetSectorsRoot()
        {
            if (this.SectorsRoot != null) 
                return this.SectorsRoot;

            return this.transform;
        }

        public Transform GetFactionsRoot()
        {
            if (this.FactionsRoot != null)
                return this.FactionsRoot;

            return this.transform;
        }
    }
}
