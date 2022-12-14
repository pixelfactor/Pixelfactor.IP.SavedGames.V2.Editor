using Pixelfactor.IP.Common;
using Pixelfactor.IP.Common.FleetOrders;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    public class EditorFleetSettings : MonoBehaviour
    {
        /// <summary>
        /// When set to false, units won't intercept targets although they may still fire at them<br />
        /// When false, the equivalent setting on a fleet will not apply
        /// </summary>
        public bool AllowCombatInterception = true;

        public bool AllowAttack = true;

        /// <summary>
        /// Fleet will allow pilots to interecept targets that have a better score than this<br />
        /// Set the value very high if the group should ignore targets
        /// </summary>
        [Range(0.0f, 1.0f)]
        public float Aggression = 0.5f;

        /// <summary>
        /// When true, controllers will collect cargo
        /// </summary>
        public FleetCargoCollectionPreference CargoCollectionPreference = FleetCargoCollectionPreference.CompatibleEquipment;

        /// <summary>
        /// Distance at which fleet members will be allowed to intercept targets
        /// </summary>
        [Range(500.0f, 16000.0f)]
        public float TargetInterceptionLowerDistance = 1300.0f;

        /// <summary>
        /// Distance at which fleet members will be allowed to intercept targets
        /// </summary>
        [Range(500.0f, 16000.0f)]
        public float TargetInterceptionUpperDistance = 1500.0f;

        /// <summary>
        /// Determines if the fleets unit's should be cloaked when not in combat
        /// </summary>
        public bool PreferCloak = false;

        /// <summary>
        /// Affects objectives where docking at a target is optional
        /// </summary>
        public DockedPreference PreferToDock = DockedPreference.DontCare;

        /// <summary>
        /// Defines how far this fleet can travel from its home base (if it is assigned)<br />
        /// Applies only if restrict max jumps is true
        /// </summary>
        [Range(0, 99)]
        public int MaxJumpDistance = 99;
    }
}
