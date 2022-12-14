﻿using Pixelfactor.IP.SavedGames.V2.Model.FleetOrders;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.FleetOrders.OrderTypes
{
    [RequireComponent(typeof(EditorFleetOrderCommon))]
    public class EditorAttackFleetOrder : EditorFleetOrderBase
    {
        public EditorFleet Target;
        public float AttackPriority = 8.0f;
    }
}
