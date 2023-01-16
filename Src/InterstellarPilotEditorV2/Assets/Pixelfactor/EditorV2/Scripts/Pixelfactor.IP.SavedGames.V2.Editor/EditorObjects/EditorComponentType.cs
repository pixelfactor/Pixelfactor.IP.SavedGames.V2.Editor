namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    using UnityEngine;

    public class EditorComponentType : MonoBehaviour
    {
        /// <summary>
        /// Determines if components of this type can be removed from the ship
        /// </summary>
        public bool AllowSell = true;

        public bool CanBeDamaged = true;

        /// <summary>
        /// Determines if the player can turn the power to the component on or off
        /// </summary>
        public bool CanSetUserPower = true;

        /// <summary>
        /// Text that describes this component type. This should be used when a component doesn't provide a specific decsription
        /// </summary>
        public string Description = null;

        public string FriendlyName = null;

        /// <summary>
        /// Determines the max number of components that must be installed
        /// </summary>
        public int MaxCount = 100;

        /// <summary>
        /// Determines the min number of components that must be installed
        /// </summary>
        public int MinCount = 0;

        public int RechargePriority = 0;

        public bool RequiresRecharge = false;
    }
}