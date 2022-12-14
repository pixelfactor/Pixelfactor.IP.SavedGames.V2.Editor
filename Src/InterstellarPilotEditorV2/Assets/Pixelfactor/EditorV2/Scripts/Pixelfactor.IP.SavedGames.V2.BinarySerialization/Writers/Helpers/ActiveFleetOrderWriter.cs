using Pixelfactor.IP.Common.FleetOrders;
using Pixelfactor.IP.SavedGames.V2.Model.FleetOrders;
using Pixelfactor.IP.SavedGames.V2.Model.FleetOrders.ActiveOrderTypes;
using System.IO;

namespace Pixelfactor.IP.SavedGames.V2.BinarySerialization.Writers.Helpers
{
    public static class ActiveFleetOrderWriter
    {
        public static void Write(BinaryWriter writer, ModelActiveFleetOrder activeFleetOrder)
        {
            writer.Write(activeFleetOrder.TimeoutTime);
            writer.Write(activeFleetOrder.StartTime);

            switch (activeFleetOrder.Order.OrderType)
            {
                case FleetOrderType.AttackGroup:
                    {
                        var a = (ModelActiveAttackFleetOrder)activeFleetOrder;
                        writer.WriteFleetId(a.TargetFleet);
                    }
                    break;
                case FleetOrderType.AttackTarget:
                    {
                        var a = (ModelActiveAttackTargetOrder)activeFleetOrder;
                        writer.WriteUnitId(a.TargetUnit);
                        writer.WriteFactionId(a.OriginalTargetFaction);
                    }
                    break;
                case FleetOrderType.AutonomousBountyHunterObjective:
                    {
                        var a = (ModelActiveUniverseBountyHunterOrder)activeFleetOrder;
                        writer.WritePersonId(a.TargetPerson);
                    }
                    break;
                case FleetOrderType.AutonomousRoamLocationsObjective:
                    {
                        var a = (ModelActiveUniverseRoamOrder)activeFleetOrder;
                        writer.WriteSectorId(a.CurrentTargetSector);
                        writer.WriteVec3(a.CurrentTargetPosition);
                    }
                    break;
                case FleetOrderType.Explore:
                    {
                        var a = (ModelActiveExploreOrder)activeFleetOrder;
                        writer.WriteSectorId(a.CurrentTargetSector);
                        writer.WriteUnitId(a.CurrentTargetWormhole);
                        writer.WriteVec3(a.CurrentTargetSectorPosition);
                    }
                    break;
                case FleetOrderType.Trade:
                case FleetOrderType.AutonomousTrade:
                case FleetOrderType.ManualTrade:
                    {
                        var a = (ModelActiveTradeOrder)activeFleetOrder;
                        writer.Write(a.TradeRoute != null);
                        if (a.TradeRoute != null)
                        {
                            CustomTraderRouteWriter.Write(writer, a.TradeRoute);
                        }

                        writer.Write(a.EndBuySellTime);
                        writer.Write(a.LastStateChangeTime);
                        writer.Write((int)a.CurrentState);
                    }
                    break;
                case FleetOrderType.AutonomousTransportPassengers:
                    {
                        var a = (ModelActiveUniversePassengerTransportOrder)activeFleetOrder;
                        writer.WritePassengerGroupId(a.PassengerGroup);
                        writer.Write(a.EndBuySellTime);
                        writer.Write(a.LastStateChangeTime);
                        writer.Write((int)a.CurrentState);
                    }
                    break;

                case FleetOrderType.Mine:
                    {
                        var a = (ModelActiveMineOrder)activeFleetOrder;
                        writer.WriteUnitId(a.MineTarget);
                        writer.Write((int)a.State);
                        writer.Write(a.AngleFromAsteroid);
                        writer.Write(a.DistanceFromAsteroid);
                    }
                    break;
                case FleetOrderType.Scavenge:
                    {
                        var a = (ModelActiveScavengeOrder)activeFleetOrder;
                        writer.WriteNullableVec3(a.Position);

                    }
                    break;
                case FleetOrderType.Dock:
                case FleetOrderType.DisposeCargo:
                case FleetOrderType.JoinFleet:
                case FleetOrderType.MoveTo:
                case FleetOrderType.Protect:
                case FleetOrderType.RTB:
                case FleetOrderType.CollectCargo:
                case FleetOrderType.WaitForAutoRepair:
                case FleetOrderType.BuildStation:
                case FleetOrderType.ClaimUnit:
                    {
                        // Nothing to write
                    }
                    break;
                case FleetOrderType.Patrol:
                case FleetOrderType.PatrolPath:
                    {
                        var a = (ModelActivePatrolOrder)activeFleetOrder;
                        writer.Write(a.PathDirection);
                        writer.Write(a.NodeIndex);
                        writer.Write(a.StartNodeIndex);
                    }
                    break;
                case FleetOrderType.RepairAtNearest:
                case FleetOrderType.ManualRepair:
                    {
                        var a = (ModelActiveRepairFleetOrder)activeFleetOrder;
                        writer.Write((int)a.RepairState);
                        writer.WriteUnitId(a.CurrentRepairLocationUnit);
                    }
                    break;
                case FleetOrderType.ManualRearm:
                case FleetOrderType.RearmAtNearest:
                    {
                        var a = (ModelActiveRearmFleetOrder)activeFleetOrder;
                        writer.Write((int)a.State);
                        writer.WriteUnitId(a.CurrentRearmLocationUnit);
                    }
                    break;
                case FleetOrderType.Wait:
                    {
                        var a = (ModelActiveWaitOrder)activeFleetOrder;
                        writer.Write(a.WaitExpiryTime);
                    }
                    break;
                case FleetOrderType.SellCargo:
                    {
                        var a = (ModelActiveSellCargoOrder)activeFleetOrder;
                        writer.Write(a.SellExpireTime);
                        writer.Write((int)a.SellCargoClass);
                        writer.WriteUnitId(a.TraderTargetUnit);
                        writer.Write((int)a.State);
                    }
                    break;
                case FleetOrderType.MoveToNearestFriendlyStation:
                    {
                        var a = (ModelActiveMoveToNearestFriendlyStationOrder)activeFleetOrder;
                        writer.WriteUnitId(a.TargetStationUnit);
                    }
                    break;
                case FleetOrderType.EnterWormhole:
                    {
                        var a = (ModelActiveEnterWormholeOrder)activeFleetOrder;
                        writer.Write((int)a.State);
                    }
                    break;
                case FleetOrderType.ExploreSector:
                    {
                        var a = (ModelActiveExploreSectorOrder)activeFleetOrder;
                        writer.WriteNullableVec3(a.CurrentTargetSectorPosition);
                    }
                    break;
            }
        }
    }
}
