using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.FleetOrders.OrderTypes
{
    [RequireComponent(typeof(EditorFleetOrderCommon))]
    public class EditorAttackTargetOrder : EditorFleetOrderBase
    {
        public EditorUnit TargetUnit;
        public float AttackPriority = 8.0f;
    }
}
