using Pixelfactor.IP.SavedGames.V2.Model;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.FleetOrders.Models
{
    public class EditorCustomTradeRoute : MonoBehaviour
    {
        public ModelCargoClass CargoClass;

        public EditorUnit BuyLocation;

        public EditorUnit SellLocation;

        public float BuyPriceMultiplier = 1.0f;
    }
}
