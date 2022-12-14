using Pixelfactor.IP.Common;
using Pixelfactor.IP.Common.FleetOrders;

namespace Pixelfactor.IP.SavedGames.V2.Model
{
    public class ModelFleetSettings
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
        public float Aggression = 13.0f;

        /// <summary>
        /// Distance at which fleet members will be allowed to intercept targets
        /// </summary>
        public float TargetInterceptionLowerDistance = 1300.0f;

        /// <summary>
        /// Distance at which fleet members will be allowed to intercept targets
        /// </summary>
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
        public int MaxJumpDistance = 99;

        public bool DestroyWhenNoPilots = false;

        public float FormationTightness = 0.9f;

        public FleetCargoCollectionPreference CargoCollectionPreference = FleetCargoCollectionPreference.CompatibleEquipment;

        public ModelPlayerFleetSettings PlayerFleetSettings = null;

        public bool AreSameAs(ModelFleetSettings modelFleetSettings)
        {
            if ((this.PlayerFleetSettings == null) != (modelFleetSettings.PlayerFleetSettings == null))
                return false;

            if (this.PlayerFleetSettings != null && !this.PlayerFleetSettings.AreSameAs(modelFleetSettings.PlayerFleetSettings))
                return false;

            if (this.AllowAttack != modelFleetSettings.AllowAttack)
                return false;

            if (this.AllowCombatInterception != modelFleetSettings.AllowCombatInterception)
                return false;

            if (this.Aggression != modelFleetSettings.Aggression)
                return false;

            if (this.TargetInterceptionLowerDistance != modelFleetSettings.TargetInterceptionLowerDistance)
                return false;

            if (this.TargetInterceptionUpperDistance != modelFleetSettings.TargetInterceptionUpperDistance)
                return false;

            if (this.PreferCloak != modelFleetSettings.PreferCloak)
                return false;

            if (this.PreferToDock != modelFleetSettings.PreferToDock)
                return false;

            if (this.MaxJumpDistance != modelFleetSettings.MaxJumpDistance)
                return false;

            if (this.DestroyWhenNoPilots != modelFleetSettings.DestroyWhenNoPilots)
                return false;

            if (this.FormationTightness != modelFleetSettings.FormationTightness)
                return false;
            
            if (this.CargoCollectionPreference != modelFleetSettings.CargoCollectionPreference)
                return false;

            return true;
        }
    }
}
