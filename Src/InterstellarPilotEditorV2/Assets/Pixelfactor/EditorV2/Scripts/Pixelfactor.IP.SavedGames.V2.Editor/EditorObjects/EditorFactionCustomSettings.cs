using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    public class EditorFactionCustomSettings : MonoBehaviour
    {
        /// <summary>
        /// Defines if faction is a "freelancer" or not
        /// </summary>
        public bool PreferSingleShip = false;

        public bool RepairShips = true;

        public bool UpgradeShips = true;

        [Range(0f, 1f)]
        public float RepairMinHullDamage = 0.2f;

        public int RepairMinCreditsBeforeRepair = 2000;

        [Range(0f, 1f)]
        public float PreferenceToPlaceBounty = 0.5f;

        [Range(0f, 1f)]
        public float LargeShipPreference = 0.25f;

        /// <summary>
        /// Artificial credit bonus
        /// </summary>
        public int DailyIncome = 0;

        public bool HostileWithAll = false;

        /// <summary>
        /// Should ideally be 1-6 in 1.6.2
        /// </summary>
        [Range(0, 8)]
        public int MinFleetUnitCount = 1;

        /// <summary>
        /// Should ideally be 1-6 in 1.6.2
        /// </summary>
        [Range(0, 8)]
        public int MaxFleetUnitCount = 6;

        [Range(0f, 1f)]
        public float OffensiveStance = 0.5f;

        public bool AllowOtherFactionToUseDocks = true;

        [Range(0f, 1f)]
        public float PreferenceToBuildTurrets = 0.5f;

        [Range(0f, 1f)]
        public float PreferenceToBuildStations = 0.5f;


        public bool IgnoreStationCreditsReserve = false;

        /// <summary>
        /// Gets or sets a fixed number of ships that the faction should own. This is intended for freelancers (where value should be 1) and outlaws. All other factions should keep the default value of -1
        /// </summary>
        public int FixedShipCount { get; set; } = -1;
    }
}
