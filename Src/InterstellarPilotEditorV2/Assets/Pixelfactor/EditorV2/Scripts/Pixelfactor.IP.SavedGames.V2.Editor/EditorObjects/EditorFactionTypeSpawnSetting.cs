using Pixelfactor.IP.Common.Factions;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    [System.Serializable]
    public class EditorFactionTypeSpawnSetting
    {
        /// <summary>
        /// The type(s) of faction that this spawn setting affects
        /// </summary>
        [Tooltip("The type(s) of faction that this spawn setting affects")]
        public FactionType FactionType;

        /// <summary>
        /// The size of faction that this spawn setting affects 
        /// </summary>
        [Tooltip("The size of faction that this spawn setting affects")]
        public FactionFreelancerType FreelancerType;

        /// <summary>
        /// Whether this faction type can spawn
        /// </summary>
        [Tooltip("Whether this faction type can spawn")]
        public bool AllowSpawn;
    }
}
