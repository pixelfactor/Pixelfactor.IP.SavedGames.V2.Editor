using Pixelfactor.IP.Common.FleetOrders;
using Pixelfactor.IP.SavedGames.V2.Model;
using Pixelfactor.IP.SavedGames.V2.Model.FleetOrders;
using Pixelfactor.IP.SavedGames.V2.Model.FleetOrders.ActiveOrderTypes;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pixelfactor.IP.SavedGames.V2.BinarySerialization.Readers.Helpers
{
    public static class ActiveFleetOrderReader
    {
        public static ModelActiveFleetOrder Read(
            BinaryReader reader,
            FleetOrderType fleetOrderType,
            int fleetId,
            Dictionary<int, ModelFaction> factions,
            Dictionary<int, ModelSector> sectors,
            Dictionary<int, ModelUnit> units,
            Dictionary<int, ModelFleet> fleets,
            Dictionary<int, ModelSectorPatrolPath> patrolPaths,
            Dictionary<int, ModelPerson> people,
            Dictionary<int, ModelPassengerGroup> passengerGroups)
        {
            var activeFleetOrder = CreateActiveFleetOrderFromType.Create(fleetOrderType);
            activeFleetOrder.TimeoutTime = reader.ReadDouble();
            activeFleetOrder.StartTime = reader.ReadDouble();

            switch (fleetOrderType)
            {
                case FleetOrderType.AttackGroup:
                    {
                        var a = (ModelActiveAttackFleetOrder)activeFleetOrder;
                        var targetFleetId = reader.ReadInt32();

                        // TODO: Validate
                        a.TargetFleet = fleets.GetValueOrDefault(targetFleetId);
                    }
                    break;
                case FleetOrderType.AttackTarget:
                    {
                        var a = (ModelActiveAttackTargetOrder)activeFleetOrder;

                        // TODO: Validate
                        var targetUnitId = reader.ReadInt32();
                        var originalTargetFactionId = reader.ReadInt32();

                        a.TargetUnit = units.GetValueOrDefault(targetUnitId);
                        a.OriginalTargetFaction = factions.GetValueOrDefault(originalTargetFactionId);
                    }
                    break;
                case FleetOrderType.AutonomousBountyHunterObjective:
                    {
                        var a = (ModelActiveUniverseBountyHunterOrder)activeFleetOrder;

                        // TOOD: Validate
                        var targetPersonId = reader.ReadInt32();

                        // NOTE: Bug with the 1.6.x game version file reader - 'People' have not been populated yet so the following does nothing
                        // Bounty hunters forced to find a new target after loading save
                        a.TargetPerson = people.GetValueOrDefault(targetPersonId);
                    }
                    break;
                case FleetOrderType.AutonomousRoamLocationsObjective:
                    {
                        var a = (ModelActiveUniverseRoamOrder)activeFleetOrder;

                        // TODO: Validate
                        var targetSectorId = reader.ReadInt32();
                        a.CurrentTargetSector = sectors.GetValueOrDefault(targetSectorId);
                        a.CurrentTargetPosition = reader.ReadVec3();
                    }
                    break;
                case FleetOrderType.Explore:
                    {
                        var a = (ModelActiveExploreOrder)activeFleetOrder;

                        // TODO: Validate
                        a.CurrentTargetSector = reader.ReadSector(sectors);
                        a.CurrentTargetWormhole = reader.ReadUnit(units);
                        a.CurrentTargetSectorPosition = reader.ReadVec3();
                    }
                    break;
                case FleetOrderType.Trade:
                case FleetOrderType.AutonomousTrade:
                case FleetOrderType.ManualTrade:
                    {
                        var a = (ModelActiveTradeOrder)activeFleetOrder;
                        var hasTradeRoute = reader.ReadBoolean();
                        if (hasTradeRoute)
                        {
                            a.TradeRoute = CustomTradeRouteReader.Read(reader, units);
                        }

                        a.EndBuySellTime = reader.ReadDouble();
                        a.LastStateChangeTime = reader.ReadDouble();

                        var state = (ActiveTradeOrderState)reader.ReadInt32();

                        if (a.TradeRoute != null)
                        {
                            a.CurrentState = state;
                        }
                    }
                    break;
                case FleetOrderType.AutonomousTransportPassengers:
                    {
                        var a = (ModelActiveUniversePassengerTransportOrder)activeFleetOrder;

                        // TODO: Validate passenger group
                        var passengerGroupId = reader.ReadInt32();
                        var passengerGroup = passengerGroups.GetValueOrDefault(passengerGroupId);

                        a.PassengerGroup = passengerGroup;
                        a.EndBuySellTime = reader.ReadDouble();
                        a.LastStateChangeTime = reader.ReadDouble();

                        var state = (ActiveTransportPassengerOrderState)reader.ReadInt32();

                        if (a.PassengerGroup != null)
                        {
                            a.CurrentState = state;
                        }
                    }
                    break;

                case FleetOrderType.Mine:
                    {
                        var a = (ModelActiveMineOrder)activeFleetOrder;
                        a.MineTarget = reader.ReadUnit(units);
                        a.State = (ActiveMineOrderState)reader.ReadInt32();
                        a.AngleFromAsteroid = reader.ReadSingle();
                        a.DistanceFromAsteroid = reader.ReadSingle();
                    }
                    break;
                case FleetOrderType.Scavenge:
                    {
                        var a = (ModelActiveScavengeOrder)activeFleetOrder;
                        a.Position = reader.ReadNullableVec3();
                    }
                    break;
                case FleetOrderType.Dock:
                case FleetOrderType.DisposeCargo:
                case FleetOrderType.JoinFleet:
                case FleetOrderType.MoveTo:
                case FleetOrderType.Protect:
                case FleetOrderType.RTB:
                case FleetOrderType.MoveToSector:
                case FleetOrderType.Undock:
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
                        a.PathDirection = reader.ReadInt32();
                        a.NodeIndex = reader.ReadInt32();
                        a.StartNodeIndex = reader.ReadInt32();
                    }
                    break;
                case FleetOrderType.RepairAtNearest:
                case FleetOrderType.ManualRepair:
                    {
                        var a = (ModelActiveRepairFleetOrder)activeFleetOrder;
                        a.RepairState = (ActiveRepairFleetOrderState)reader.ReadInt32();

                        // TODO: Validate
                        var repairLocationUnitId = reader.ReadInt32();
                        a.CurrentRepairLocationUnit = units.GetValueOrDefault(repairLocationUnitId);
                    }
                    break;
                case FleetOrderType.ManualRearm:
                case FleetOrderType.RearmAtNearest:
                    {
                        var a = (ModelActiveRearmFleetOrder)activeFleetOrder;
                        a.State = (ActiveRearmFleetOrderState)reader.ReadInt32();

                        // TODO: Validate
                        var rearmLocationUnitId = reader.ReadInt32();
                        a.CurrentRearmLocationUnit = units.GetValueOrDefault(rearmLocationUnitId);
                    }
                    break;
                case FleetOrderType.Wait:
                    {
                        var a = (ModelActiveWaitOrder)activeFleetOrder;
                        a.WaitExpiryTime = reader.ReadDouble();
                    }
                    break;
                case FleetOrderType.SellCargo:
                    {
                        var a = (ModelActiveSellCargoOrder)activeFleetOrder;

                        a.SellExpireTime = reader.ReadDouble();

                        // TODO: Validate cargo class.
                        var cargoClassId = reader.ReadInt32();
                        a.SellCargoClass = (ModelCargoClass)cargoClassId;

                        // TODO: Validate
                        var targetUnitId = reader.ReadInt32();
                        a.TraderTargetUnit = units.GetValueOrDefault(targetUnitId);

                        a.State = (ActiveSellCargoOrderState)reader.ReadInt32();
                    }
                    break;
                case FleetOrderType.MoveToNearestFriendlyStation:
                    {
                        var a = (ModelActiveMoveToNearestFriendlyStationOrder)activeFleetOrder;

                        // TODO: Validate
                        var targetStationId = reader.ReadInt32();

                        a.TargetStationUnit = units.GetValueOrDefault(targetStationId);
                    }
                    break;
                case FleetOrderType.EnterWormhole:
                    {
                        var a = (ModelActiveEnterWormholeOrder)activeFleetOrder;
                        a.State = (EnterWormholeState)reader.ReadInt32();
                    }
                    break;
                case FleetOrderType.ExploreSector:
                    {
                        var a = (ModelActiveExploreSectorOrder)activeFleetOrder;
                        a.CurrentTargetSectorPosition = reader.ReadNullableVec3();
                    }
                    break;
            }

            return activeFleetOrder;
        }
    }
}
