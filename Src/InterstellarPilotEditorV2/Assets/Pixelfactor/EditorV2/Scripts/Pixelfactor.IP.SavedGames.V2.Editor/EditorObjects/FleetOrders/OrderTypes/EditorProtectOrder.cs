﻿using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.FleetOrders.OrderTypes
{
    [RequireComponent(typeof(EditorFleetOrderCommon))]
    public class EditorProtectOrder : EditorFleetOrderBase
    {
        public bool CompleteOnReachTarget = true;
        public float ArrivalThreshold = 100.0f;
        public bool MatchTargetOrientation = false;
        public Transform Target;
    }
}
