using Pixelfactor.IP.Common;

namespace Pixelfactor.IP.SavedGames.V2.Model
{
    public class ModelFactionCustomSettings
    {
        /// <summary>
        /// Defines if faction is a "freelancer" or not
        /// </summary>
        public bool PreferSingleShip { get; set; }

        public bool BuildShips { get; set; } = true;

        public bool RepairShips { get; set; }

        public bool UpgradeShips { get; set; }

        public float RepairMinHullDamage { get; set; }

        public int RepairMinCreditsBeforeRepair { get; set; }

        public float PreferenceToPlaceBounty { get; set; }

        public float LargeShipPreference { get; set; }

        /// <summary>
        /// Can be negative to build less cloak ships
        /// </summary>
        public float CloakShipPreference { get; set; }

        /// <summary>
        /// Artificial credit bonus
        /// </summary>
        public int DailyIncome { get; set; }

        public bool HostileWithAll { get; set; }

        /// <summary>
        /// Should ideally be 1-6 in 1.6.2
        /// </summary>
        public int MinFleetUnitCount { get; set; }

        /// <summary>
        /// Should ideally be 1-6 in 1.6.2
        /// </summary>
        public int MaxFleetUnitCount { get; set; }

        public float OffensiveStance { get; set; }

        public bool AllowOtherFactionToUseDocks { get; set; }

        public float PreferenceToBuildTurrets { get; set; }

        /// <summary>
        /// Not currently used
        /// </summary>
        public float PreferenceToBuildStations { get; set; }


        public bool IgnoreStationCreditsReserve { get; set; }
        public GenderChoice PilotGender { get; set; }
        public int MaxJumpDistanceFromHomeSector { get; set; } = -1;
        public int MaxStationBuildDistanceFromHomeSector { get; set;} = -1;
        public int FixedShipCount { get; set; } = -1;
        public float SectorControlLikelihood { get; set; } = 0.0f;
        public float PreferenceToHaveAmmo { get; set; } = 0.5f;
    }
}
