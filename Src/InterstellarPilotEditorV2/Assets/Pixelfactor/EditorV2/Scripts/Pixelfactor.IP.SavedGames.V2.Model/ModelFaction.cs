using Pixelfactor.IP.Common.Factions;
using Pixelfactor.IP.SavedGames.V2.Model.Factions;
using Pixelfactor.IP.SavedGames.V2.Model.Factions.Bounty;
using System.Collections.Generic;

namespace Pixelfactor.IP.SavedGames.V2.Model
{
    public class ModelFaction
    {
        public int Id { get; set; }

        public ModelSector HomeSector { get; set; }

        public Vec3? HomeSectorPosition { get; set; }

        /// <summary>
        /// The id of the name that was generated for this faction. Stops duplicates being generated<br />
        /// Applies when <see cref="HasGeneratedName"/>
        /// </summary>
        public int GeneratedNameId { get; set; } = -1;

        /// <summary>
        /// The id of the name that was generated for this faction. Stops duplicates being generated<br />
        /// Applies when <see cref="HasGeneratedName"/>
        /// </summary>
        public int GeneratedSuffixId { get; set; } = -1;

        /// <summary>
        /// Applies when <see cref="HasCustomName"/>
        /// </summary>
        public string CustomName { get; set; }

        /// <summary>
        /// Applies when <see cref="HasCustomName"/>
        /// </summary>
        public string CustomShortName { get; set; }

        /// <summary>
        /// Money
        /// </summary>
        public int Credits { get; set; }

        public string Description { get; set; }

        public bool IsCivilian { get; set; }

        /// <summary>
        /// Trader, miner etc
        /// </summary>
        public FactionType FactionType { get; set; }

        /// <summary>
        /// 0 - 1
        /// </summary>
        public float Aggression { get; set; }

        /// <summary>
        /// 0 - 1
        /// </summary>
        public float Virtue { get; set; }

        /// <summary>
        /// 0 - 1
        /// </summary>
        public float Greed { get; set; }

        /// <summary>
        /// 0 - 1
        /// </summary>
        public float Cooperation { get; set; }

        /// <summary>
        /// 0 - 1
        /// </summary>
        public float TradeEfficiency { get; set; }

        /// <summary>
        /// Determines if can declare was
        /// </summary>
        public bool DynamicRelations { get; set; }

        /// <summary>
        /// Whether to show job board at stations
        /// </summary>
        public bool ShowJobBoards { get; set; }

        /// <summary>
        /// If new jobs are created
        /// </summary>
        public bool CreateJobs { get; set; }

        /// <summary>
        /// The RP is a limit on how many ships/stations the faction can build. Increasing this will increase the factions potential power
        /// </summary>
        public float RequisitionPointMultiplier { get; set; }

        /// <summary>
        /// Destroy faction when no ships/stations left
        /// </summary>
        public bool DestroyWhenNoUnits { get; set; }

        /// <summary>
        /// 0 - 1<br />
        /// Determines strength of NPCs when pilotting. Not used for much in v1.6.2
        /// </summary>
        public float MinNpcCombatEfficiency { get; set; } = 0.25f;

        /// <summary>
        /// 0 - 1<br />
        /// Determines strength of NPCs when pilotting. Not used for much in v1.6.2
        /// </summary>
        public float MaxNpcCombatEfficiency { get; set; } = 1.0f;

        /// <summary>
        /// Artificial bonus to the potential power of this faction
        /// </summary>
        public int AdditionalRpProvision { get; set; }

        /// <summary>
        /// Not used for much in 1.6.2
        /// </summary>
        public bool TradeIllegalGoods { get; set; }

        /// <summary>
        /// Time entered the game in seconds
        /// </summary>
        public double SpawnTime { get; set; }

        /// <summary>
        /// Stat for debug
        /// </summary>
        public long HighestEverNetWorth { get; set; }

        public ModelFactionCustomSettings CustomSettings { get; set; }
        public ModelFactionStats Stats { get; set; }
        public List<ModelSector> AutopilotExcludedSectors { get; set; } = new List<ModelSector>();
        public ModelFactionIntel Intel { get; set; } = new ModelFactionIntel();
        public ModelFactionRelationData Relations { get; set; } = new ModelFactionRelationData();
        public List<ModelFactionRecentDamageItem> RecentDamageItems { get; set; } = new List<ModelFactionRecentDamageItem>();
        public ModelFactionOpinionData Opinions { get; set; } = new ModelFactionOpinionData();

        public ModelPerson Leader { get; set; }
        public ModelFactionAI FactionAI { get; set; }
        public ModelFactionBountyBoard BountyBoard { get; set; }
        public List<ModelFactionTransaction> Transactions { get; set; } = new List<ModelFactionTransaction>();

        /// <summary>
        /// E.g. Cadet, Captain
        /// </summary>
        public int RankingSystemId { get; set; } = -1;

        public int PreferredFormationId { get; set; } = -1;

        public List<byte> AvatarProfileIds { get; set; }
    }
}
