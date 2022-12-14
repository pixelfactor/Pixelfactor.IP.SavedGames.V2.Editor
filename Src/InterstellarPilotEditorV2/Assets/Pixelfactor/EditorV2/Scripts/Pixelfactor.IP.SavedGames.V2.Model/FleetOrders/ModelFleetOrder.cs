using Pixelfactor.IP.Common.FleetOrders;

namespace Pixelfactor.IP.SavedGames.V2.Model.FleetOrders
{
    public abstract class ModelFleetOrder
    {
        /// <summary>
        /// Determines the amount of credits given to this fleet order<br />
        /// When below zero the fleet order uses the factions credits
        /// </summary>
        public int AvailableCredits { get; set; } = -1;

        /// <summary>
        /// How important the order is. Used by the AI to determine how/when to replace orders
        /// </summary>
        public float Priority = 0.0f;

        public int Id { get; set; }

        public FleetOrderCompletionMode CompletionMode { get; set; }

        public bool AllowCombatInterception { get; set; }

        public FleetOrderCloakPreference CloakPreference { get; set; }

        /// <summary>
        /// Max sector distance from home base
        /// </summary>
        public int MaxJumpDistance { get; set; }

        /// <summary>
        /// Determines if after a period of inactivity (while order is active), the order is invalidated<br />
        /// </summary>
        public bool AllowTimeout { get; set; }

        /// <summary>
        /// The period of inactivity in seconds, after which the order will be invalidated
        /// </summary>
        public float TimeoutTime { get; set; }

        public float MaxDuration { get; set; }

        public abstract FleetOrderType OrderType { get;}

        public bool Notifications { get; set; } = true;
    }
}
