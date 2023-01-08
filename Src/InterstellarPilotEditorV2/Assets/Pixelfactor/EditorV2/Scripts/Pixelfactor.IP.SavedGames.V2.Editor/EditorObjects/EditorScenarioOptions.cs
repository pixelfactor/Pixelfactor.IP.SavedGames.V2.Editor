using Pixelfactor.IP.Common;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    public class EditorScenarioOptions : MonoBehaviour
    {
        [Tooltip("Not currently implemented in engine - likely to be re-implemented in the future. Would control events like randomly spawned abandoned ships")]
        public bool RandomEventsEnabled = true;

        [Tooltip("Whether new factions (freelancer and major) can be spawned in the game")]
        public bool FactionSpawningEnabled = true;

        [Tooltip("Min scenarion time in seconds before a new faction can be spawned")]
        public double MinTimeBeforeFactionSpawn = 0.0;

        [Tooltip("Whether asteroids can ever respawn")]
        public bool AsteroidRespawningEnabled = true;

        /// <summary>
        /// Value from 0-1 to indicate how long before asteroids are respawned
        /// </summary>
        [Range(0f, 1f)]
        [Tooltip("Determines the time before asteroids respawn. Use 1 for the longest duration")]
        public float AsteroidRespawnTime = 0.5f;

        [Tooltip("Determines what happens when the player dies. When not set, the player's preference is respected")]
        public RespawnOnDeathPreference RespawnOnDeath = RespawnOnDeathPreference.NotSet;

        [Tooltip("Whether the player's death means the end of the scenario")]
        public bool Permadeath = false;

        [Tooltip("Whether the player can jump into ships/stations that are far away")]
        public bool AllowTeleporting = true;

        [Tooltip("Whether station capture is allowed")]
        public bool AllowStationCapture = true;

        [Tooltip("Whether NPCs can abandoned ships")]
        public bool AllowAbandonShip = true;
    }
}
