namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    using UnityEngine;

    public class EditorComponentClass : MonoBehaviour
    {
        /// <summary>
        /// The default cost of buying this component
        /// </summary>
        public int BaseCost = 1000;

        /// <summary>
        /// Name of the component
        /// </summary>
        public string Name = string.Empty;

        public int UniqueId = 0;

        public EditorComponentType ComponentType = null;

        public ShipHullType MaxHullType = ShipHullType.Station;

        public ShipHullType MinHullType = ShipHullType.Fighter;
    }
}