using Pixelfactor.IP.Common;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    public class EditorScenarioOptions : MonoBehaviour
    {
        [Tooltip("Not currently implemented in engine - likely to be re-implemented in the future. Would control events like randomly spawned abandoned ships")]
        public bool RandomEventsEnabled = true;

        [Tooltip("Whether new factions (freelancer and major) can be spawned in the game. Note: To fine-tune which factions can spawn use EditorFactionSpawnSettings")]
        public bool FactionSpawningEnabled = true;

        [Tooltip("Scenario time in seconds when a new faction can be spawned")]
        [UnityEngine.Serialization.FormerlySerializedAs("MinTimeBeforeFactionSpawn")]
        public double NextFactionSpawnTime = 0.0;

        [Tooltip("Scenario time in seconds when a new freelancer can be spawned")]
        public double NextFreelancerSpawnTime = 0.0;

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

        [Tooltip("Whether the player can access god mode")]
        public bool AllowGodMode = true;

        /// <summary>
        /// Gets or sets if the player receives notifications when property is under attack. This can be turned off when the scenario is a skirmish
        /// </summary>
        [Tooltip("Whether the player receives notifications when property is under attack. This can be turned off when the scenario is a skirmish")]
        public bool PlayerPropertyAttackNotifications { get; set; } = true;
    }
}
