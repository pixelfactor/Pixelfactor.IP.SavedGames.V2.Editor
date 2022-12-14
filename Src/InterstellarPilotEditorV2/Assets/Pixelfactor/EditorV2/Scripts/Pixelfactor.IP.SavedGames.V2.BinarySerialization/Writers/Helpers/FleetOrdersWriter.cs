using Pixelfactor.IP.Common.FleetOrders;
using Pixelfactor.IP.SavedGames.V2.Model.FleetOrders;
using Pixelfactor.IP.SavedGames.V2.Model.FleetOrders.OrderTypes;
using System;
using System.IO;

namespace Pixelfactor.IP.SavedGames.V2.BinarySerialization.Writers.Helpers
{
    public static class FleetOrdersWriter
    {
        public static void Write(BinaryWriter writer, ModelFleetOrderCollection fleetOrders)
        {
            writer.Write(fleetOrders.Orders.Count);

            foreach (var order in fleetOrders.Orders)
            {
                WriteOrder(writer, order);
            }

            writer.Write(fleetOrders.QueuedOrders.Count);
            foreach (var queuedOrder in fleetOrders.QueuedOrders)
            {
                writer.Write(fleetOrders.Orders.IndexOf(queuedOrder));
            }

            writer.Write(fleetOrders.CurrentOrder != null);
            if (fleetOrders.CurrentOrder != null)
            {
                writer.Write(fleetOrders.Orders.IndexOf(fleetOrders.CurrentOrder.Order));
                ActiveFleetOrderWriter.Write(writer, fleetOrders.CurrentOrder);
            }
        }

        public static void WriteOrder(BinaryWriter writer, ModelFleetOrder order)
        {
            writer.Write((int)order.OrderType);

            writer.Write(order.Id);
            writer.Write((int)order.CompletionMode);
            writer.Write(order.AllowCombatInterception);
            writer.Write((int)order.CloakPreference);
            writer.Write(order.MaxJumpDistance);
            writer.Write(order.AllowTimeout);
            writer.Write(order.TimeoutTime);
            writer.Write(order.MaxDuration);
            writer.Write(order.Priority);
            writer.Write(order.Notifications);

            switch (order.OrderType)
            {
                case FleetOrderType.AttackGroup:
                    {
                        var o = (ModelAttackFleetOrder)order;
                        writer.WriteFleetId(o.Target);
                        writer.Write(o.AttackPriority);
                    }
                    break;
                case FleetOrderType.CollectCargo:
                    {
                        var o = (ModelCollectCargoOrder)order;
                        writer.WriteUnitId(o.TargetUnit);
                    }
                    break;

                case FleetOrderType.Scavenge:
                    {
                        var o = (ModelScavengeOrder)order;
                        writer.WriteSectorId(o.TargetSector);
                        writer.Write((int)o.CollectOwnerMode);
                    }
                    break;
                case FleetOrderType.Mine:
                    {
                        var o = (ModelMineOrder)order;
                        writer.WriteSectorId(o.TargetSector);
                        writer.Write((int)o.CollectOwnerMode);
                        writer.WriteUnitId(o.ManualMineTarget);
                    }
                    break;
                case FleetOrderType.Dock:
                    {
                        var o = (ModelDockOrder)order;
                        writer.WriteUnitId(o.TargetDock);
                    }
                    break;
                case FleetOrderType.Patrol:
                    {
                        var o = (ModelPatrolOrder)order;

                        writer.Write(o.PathDirection);
                        writer.Write(o.IsLooping);

                        writer.Write(o.Nodes.Count);
                        foreach (var node in o.Nodes)
                        {
                            writer.WriteSectorId(node.Sector);
                            writer.WriteVec3(node.SectorPosition);
                        }

                        writer.Write(o.IsLoop);
                    }
                    break;
                case FleetOrderType.PatrolPath:
                    {
                        var o = (ModelPatrolPathOrder)order;

                        writer.Write(o.PathDirection);
                        writer.Write(o.IsLooping);
                        writer.WriteSectorPatrolPathId(o.PatrolPath);
                    }
                    break;

                case FleetOrderType.Wait:
                    {
                        var o = (ModelWaitOrder)order;
                        writer.Write(o.WaitTime);
                    }
                    break;

                case FleetOrderType.AttackTarget:
                    {
                        var o = (ModelAttackTargetOrder)order;
                        writer.WriteUnitId(o.TargetUnit);
                        writer.Write(o.AttackPriority);
                    }
                    break;
                case FleetOrderType.Trade:
                    {
                        var o = (ModelTradeOrder)order;
                        writer.Write(o.MinBuyQuantity);
                        writer.Write(o.MinBuyCargoPercentage);
                    }
                    break;
                case FleetOrderType.ManualTrade:
                    {
                        var o = (ModelManualTradeOrder)order;
                        writer.Write(o.MinBuyQuantity);
                        writer.Write(o.MinBuyCargoPercentage);

                        writer.Write(o.CustomTradeRoute != null);
                        if (o.CustomTradeRoute != null)
                        {
                            CustomTraderRouteWriter.Write(writer, o.CustomTradeRoute);
                        }
                    }
                    break;
                case FleetOrderType.AutonomousTrade:
                    {
                        var o = (ModelUniverseTradeOrder)order;
                        writer.Write(o.MinBuyQuantity);
                        writer.Write(o.MinBuyCargoPercentage);

                        writer.Write(o.TradeOnlySpecificCargoClasses);

                        writer.Write(o.TradeSpecificCargoClasses.Count);
                        foreach (var cargoClass in o.TradeSpecificCargoClasses)
                        {
                            writer.Write((int)cargoClass);
                        }
                    }
                    break;
                case FleetOrderType.JoinFleet:
                    {
                        var o = (ModelJoinFleetOrder)order;
                        writer.WriteFleetId(o.TargetFleet);
                    }
                    break;

                case FleetOrderType.MoveTo:
                    {
                        var o = (ModelMoveToOrder)order;
                        writer.Write(o.CompleteOnReachTarget);
                        writer.Write(o.ArrivalThreshold);
                        writer.Write(o.MatchTargetOrientation);
                        writer.WriteNullableVec3(o.PreferredRelativeVectorFromTarget);

                        writer.Write(o.Target != null);

                        if (o.Target != null)
                        {
                            SectorTargetWriter.Write(writer, o.Target);
                        }
                    }
                    break;

                case FleetOrderType.Protect:
                    {
                        var o = (ModelProtectOrder)order;
                        writer.Write(o.CompleteOnReachTarget);
                        writer.Write(o.ArrivalThreshold);
                        writer.Write(o.MatchTargetOrientation);
                        writer.WriteNullableVec3(o.PreferredRelativeVectorFromTarget);

                        writer.Write(o.Target != null);
                        if (o.Target != null)
                        {
                            SectorTargetWriter.Write(writer, o.Target);
                        }
                    }
                    break;

                case FleetOrderType.SellCargo:
                    {
                        var o = (ModelSellCargoOrder)order;
                        writer.Write(o.FreeUnitsCompleteThreshold);
                        writer.Write(o.MinBuyPriceMultiplier);
                        writer.Write(o.SellOnlyListedCargos);
                        writer.Write(o.CompleteWhenNoBuyerFound);
                        writer.Write(o.CompleteWhenNoCargoToSell);
                        writer.WriteUnitId(o.ManualBuyerUnit);
                        writer.Write(o.CustomSellCargoTime);

                        writer.Write(o.SellCargoClasses.Count);
                        foreach (var cargoClass in o.SellCargoClasses)
                        {
                            writer.Write((int)cargoClass);
                        }

                        writer.Write(o.SellEquipment);
                    }
                    break;
                case FleetOrderType.RTB:
                case FleetOrderType.DisposeCargo:
                case FleetOrderType.AutonomousTransportPassengers:
                case FleetOrderType.AutonomousBountyHunterObjective:
                case FleetOrderType.AutonomousRoamLocationsObjective:
                case FleetOrderType.Explore:
                    {
                        // nothing to read/write
                    }
                    break;
                case FleetOrderType.ManualRearm:
                    {
                        var o = (ModelManualRearmFleetOrder)order;
                        writer.Write(o.EquipmentCargoUsage);
                        writer.Write((int)o.InsufficientCreditsMode);
                        writer.WriteUnitId(o.RearmLocationUnit);
                    }
                    break;
                case FleetOrderType.RearmAtNearest:
                    {
                        var o = (ModelRearmAtNearestFleetOrder)order;
                        writer.Write(o.EquipmentCargoUsage);
                        writer.Write((int)o.InsufficientCreditsMode);
                    }
                    break;
                case FleetOrderType.ManualRepair:
                    {
                        var o = (ModelManualRepairFleetOrder)order;
                        writer.Write((int)o.InsufficientCreditsMode);
                        writer.WriteUnitId(o.RepairLocationUnit);
                    }
                    break;
                case FleetOrderType.RepairAtNearest:
                    {
                        var o = (ModelRepairAtNearestStationOrder)order;
                        writer.Write((int)o.InsufficientCreditsMode);
                    }
                    break;
                case FleetOrderType.MoveToNearestFriendlyStation:
                    {
                        var o = (ModelMoveToNearestFriendlyStationOrder)order;
                        writer.Write(o.CompleteOnReachTarget);
                    }
                    break;
                case FleetOrderType.EnterWormhole:
                    {
                        var o = (ModelEnterWormholeOrder)order;
                        writer.WriteUnitId(o.TargetWormhole);
                    }
                    break;
                case FleetOrderType.ExploreSector:
                    {
                        var o = (ModelExploreSectorOrder)order;
                        writer.WriteSectorId(o.Sector);
                    }
                    break;
                case FleetOrderType.MoveToSector:
                    {
                        var o = (ModelMoveToSectorOrder)order;
                        writer.WriteSectorId(o.TargetSector);
                    }
                    break;
                case FleetOrderType.WaitForAutoRepair:
                    {
                        var o = (ModelWaitForAutoRepairOrder)order;
                        writer.Write(o.HullConditionThreshold);
                        writer.Write(o.ComponentsConditionThreshold);
                        writer.Write(o.ShieldConditionThreshold);
                    }
                    break;
                case FleetOrderType.BuildStation:
                    {
                        var o = (ModelBuildStationOrder)order;
                        writer.Write((int)o.UnitClass);
                        writer.WriteSectorId(o.Sector);
                        writer.WriteVec3(o.SectorPosition);
                        writer.Write((int)o.InsufficientCreditsMode);
                    }
                    break;
                case FleetOrderType.ClaimUnit:
                    {
                        var o = (ModelClaimUnitOrder)order;
                        writer.WriteUnitId(o.Unit);
                    }
                    break;
                default:
                    {
                        throw new Exception($"Unable to read data for objective of type {order.OrderType}. Unknown type");
                    }
            }
        }
    }
}
