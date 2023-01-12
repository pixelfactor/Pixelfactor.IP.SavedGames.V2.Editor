using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.FleetOrders.OrderTypes
{
    [RequireComponent(typeof(EditorFleetOrderCommon))]
    public class EditorAttackTargetOrder : EditorFleetOrderBase
    {
        public EditorUnit TargetUnit;
        public float AttackPriority = 8.0f;

        public override Vector3? GetTargetPosition()
        {
            if (this.TargetUnit != null)
                return this.TargetUnit.transform.position;

            return base.GetTargetPosition();
        }
    }
}
