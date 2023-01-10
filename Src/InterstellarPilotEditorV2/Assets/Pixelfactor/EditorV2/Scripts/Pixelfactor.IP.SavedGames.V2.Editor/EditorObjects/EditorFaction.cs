using Pixelfactor.IP.Common.Factions;
using Pixelfactor.IP.SavedGames.V2.Model;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    public class EditorFaction : MonoBehaviour
    {
        public int Id = 0;

        /// <summary>
        /// Applies when <see cref="HasCustomName"/>
        /// </summary>
        public string CustomName;

        /// <summary>
        /// Applies when <see cref="HasCustomName"/>
        /// </summary>
        public string CustomShortName;

        [Tooltip("A location that is the faction's home. Currently only used by the AI and can be changed during the game")]
        public Transform HomeSectorTransform;

        [Tooltip("The optional pilot ranking system that the faction uses")]
        public int PilotRankingSystemId = -1;

        /// <summary>
        /// Current number of credits the faction has
        /// </summary>
        public int Credits;

        public string Description;

        /// <summary>
        /// Trader, miner etc
        /// </summary>
        public FactionType FactionType = FactionType.None;

        /// <summary>
        /// 0 - 1
        /// </summary>
        [Range(0f, 1f)]
        public float Aggression = 0.5f;

        /// <summary>
        /// 0 - 1
        /// </summary>
        [Range(0f, 1f)]
        public float Virtue = 0.5f;

        /// <summary>
        /// 0 - 1
        /// </summary>
        [Range(0f, 1f)]
        public float Greed = 0.5f;

        /// <summary>
        /// 0 - 1
        /// </summary>
        [Range(0f, 1f)]
        public float TradeEfficiency = 0.5f;

        [Range(0f, 1f)]
        public float Cooperation = 0.5f;
        /// <summary>
        /// Determines if can declare was
        /// </summary>
        public bool DynamicRelations = true;

        /// <summary>
        /// Whether to show job board at stations
        /// </summary>
        public bool ShowJobBoards = false;

        /// <summary>
        /// If new jobs are created
        /// </summary>
        public bool CreateJobs = false;

        /// <summary>
        /// 0 - 1<br />
        /// Determines strength of NPCs when pilotting. Not used for much in v1.6.2
        /// </summary>
        [Range(0f, 1f)]
        public float MinNpcCombatEfficiency = 0.0f;

        /// <summary>
        /// 0 - 1<br />
        /// Determines strength of NPCs when pilotting. Not used for much in v1.6.2
        /// </summary>
        [Range(0f, 1f)]
        public float MaxNpcCombatEfficiency = 1.0f;

        /// <summary>
        /// Artificial bonus to the potential power of this faction
        /// </summary>
        public int AdditionalRpProvision = 0;

        /// <summary>
        /// Not used for much in 1.6.2
        /// </summary>
        public bool TradeIllegalGoods = false;

        [Header("Advanced")]
        [Tooltip("The id of the faction's name from the database. Using this is more efficient than passing around the string values")]
        public int GeneratedNameId = -1;
        [Tooltip("The id of the faction's suffix name from the database. Using this is more efficient than passing around the string values")]
        public int GeneratedSuffixId = -1;
        [Tooltip("The scenario time in seconds when the faction was spawned")]
        public double SpawnTime = 0.0;
        [Tooltip("The highest net worth value that the faction has achieved during the game")]
        public long HighestEverNetWorth = 0L;
        [Tooltip("Determines the fleet formation that the AI likes to use")]
        public int PreferredFormationId = -1;
        /// <summary>
        /// Destroy faction when no ships/stations left
        /// </summary>
        [Tooltip("Whether to destroy the faction when it has no valid ships/stations left")]
        public bool DestroyWhenNoUnits = true;
        /// <summary>
        /// The RP is a limit on how many ships/stations the faction can build. Increasing this will increase the factions potential power
        /// </summary>
        [Range(0f, 10f)]
        [Tooltip("Determines the amount of requisition points the faction has. RP are an artificial limiter to AI faction power")]
        public float RequisitionPointMultiplier = 1.0f;

        /// <summary>
        /// Determines whether the faction is a civilian faction
        /// </summary>
        [Tooltip("Determines whether the faction is a civilian faction. This is only relevant for 'Generic' type of faction. The AI interact with civilian factions slightly differently and there are harsher punishments for attacking them.")]
        public bool IsCivilian = false;
    }
}
