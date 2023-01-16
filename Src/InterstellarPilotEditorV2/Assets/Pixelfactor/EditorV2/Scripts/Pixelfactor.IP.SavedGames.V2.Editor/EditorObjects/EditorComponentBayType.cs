namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    using UnityEngine;

    public class ComponentBayType : MonoBehaviour
    {
        public int UniqueId = 0;

        /// <summary>
        /// Determines whether this type of component has specific bay names e.g. left turret, top turret
        /// </summary>
        public bool UseCustomBayNames = false;

        public BayType BayType = BayType.Turret;

        public string FriendlyName = null;
    }
}