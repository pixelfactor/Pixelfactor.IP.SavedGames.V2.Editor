using Pixelfactor.IP.Common.FleetOrders;
using Pixelfactor.IP.SavedGames.V2.Model.FleetOrders.OrderTypes;
using System;

namespace Pixelfactor.IP.SavedGames.V2.Model.FleetOrders
{
    public static class CreateFleetOrderFromType
    {
        public static ModelFleetOrder Create(FleetOrderType orderType)
        {
            switch (orderType)
            {
                case FleetOrderType.AttackTarget:
                    return new ModelAttackTargetOrder();
                case FleetOrderType.AutonomousTrade:
                    return new ModelUniverseTradeOrder();
                case FleetOrderType.CollectCargo:
                    return new ModelCollectCargoOrder();
                case FleetOrderType.Dock:
                    return new ModelDockOrder();
                case FleetOrderType.ManualTrade:
                    return new ModelManualTradeOrder();
                case FleetOrderType.Mine:
                    return new ModelMineOrder();
                case FleetOrderType.MoveTo:
                    return new ModelMoveToOrder();
                case FleetOrderType.Patrol:
                    return new ModelPatrolOrder();
                case FleetOrderType.PatrolPath:
                    return new ModelPatrolPathOrder();
                case FleetOrderType.RTB:
                    return new ModelReturnToBaseOrder();
                case FleetOrderType.Scavenge:
                    return new ModelScavengeOrder();
                case FleetOrderType.SellCargo:
                    return new ModelSellCargoOrder();
                case FleetOrderType.Trade:
                    return new ModelTradeOrder();
                case FleetOrderType.Wait:
                    return new ModelWaitOrder();
                case FleetOrderType.AttackGroup:
                    return new ModelAttackFleetOrder();
                case FleetOrderType.JoinFleet:
                    return new ModelJoinFleetOrder();
                case FleetOrderType.DisposeCargo:
                    return new ModelDisposeCargoOrder();
                case FleetOrderType.Protect:
                    return new ModelProtectOrder();
                case FleetOrderType.AutonomousBountyHunterObjective:
                    return new ModelUniverseBountyHunterOrder();
                case FleetOrderType.AutonomousRoamLocationsObjective:
                    return new ModelUniverseRoamOrder();
                case FleetOrderType.ManualRepair:
                    return new ModelManualRepairFleetOrder();
                case FleetOrderType.RepairAtNearest:
                    return new ModelRepairAtNearestStationOrder();
                case FleetOrderType.ManualRearm:
                    return new ModelManualRearmFleetOrder();
                case FleetOrderType.RearmAtNearest:
                    return new ModelRearmAtNearestFleetOrder();
                case FleetOrderType.AutonomousTransportPassengers:
                    return new ModelUniversePassengerTransportOrder();
                case FleetOrderType.Explore:
                    return new ModelExploreOrder();
                case FleetOrderType.MoveToNearestFriendlyStation:
                    return new ModelMoveToNearestFriendlyStationOrder();
                case FleetOrderType.EnterWormhole:
                    return new ModelEnterWormholeOrder();
                case FleetOrderType.ExploreSector:
                    return new ModelExploreSectorOrder();
                case FleetOrderType.MoveToSector:
                    return new ModelMoveToSectorOrder();
                case FleetOrderType.WaitForAutoRepair:
                    return new ModelWaitForAutoRepairOrder();
                case FleetOrderType.BuildStation:
                    return new ModelBuildStationOrder();
                case FleetOrderType.ClaimUnit:
                    return new ModelClaimUnitOrder();
                default:
                    throw new NotImplementedException($"Order type {orderType}");
            }
        }
    }
}
