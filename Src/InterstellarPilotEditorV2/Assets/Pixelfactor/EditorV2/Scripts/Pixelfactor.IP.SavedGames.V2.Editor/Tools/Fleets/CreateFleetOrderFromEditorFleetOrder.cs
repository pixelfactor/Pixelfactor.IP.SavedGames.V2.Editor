using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.FleetOrders;
using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.FleetOrders.Models;
using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.FleetOrders.OrderTypes;
using Pixelfactor.IP.SavedGames.V2.Model;
using Pixelfactor.IP.SavedGames.V2.Model.FleetOrders;
using Pixelfactor.IP.SavedGames.V2.Model.FleetOrders.Models;
using Pixelfactor.IP.SavedGames.V2.Model.FleetOrders.OrderTypes;
using System.Linq;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools
{
    public static class CreateFleetOrderFromEditorFleetOrder
    {
        /// <summary>
        /// Creates the fleet order type used in the save model, from the editor version
        /// </summary>
        /// <param name="editorFleetOrder"></param>
        /// <param name="editorScenario"></param>
        /// <param name="savedGame"></param>
        /// <returns></returns>
        public static ModelFleetOrder CreateFleetOrder(EditorFleetOrderBase editorFleetOrder, EditorScenario editorScenario, SavedGame savedGame)
        {
            // Set common stuff
            var editorCommonOrderStuff = editorFleetOrder.GetComponent<EditorFleetOrderCommon>();

            if (editorCommonOrderStuff.Id < 0)
            {
                Logging.LogAndThrow("Objective must have a valid (>=0) Id", editorCommonOrderStuff);
            }

            switch (editorFleetOrder)
            {
                case EditorAttackFleetOrder editorAttackOrder:
                    {
                        var o = CreateFleetOrderOfType<ModelAttackFleetOrder>(editorFleetOrder);
                        o.Target = savedGame.Fleets.FirstOrDefault(e => e.Id == editorAttackOrder.Target?.Id);
                        o.AttackPriority = editorAttackOrder.AttackPriority;

                        return o;
                    }
                case EditorCollectCargoOrder editorCollectCargoOrder:
                    {
                        var o = CreateFleetOrderOfType<ModelCollectCargoOrder>(editorFleetOrder);
                        o.TargetUnit = savedGame.Units.FirstOrDefault(e => e.Id == editorCollectCargoOrder.CargoUnit?.GetComponent<EditorUnit>()?.Id);

                        return o;
                    }

                case EditorScavengeOrder editorScavengeOrder:
                    {
                        var o = CreateFleetOrderOfType<ModelScavengeOrder>(editorFleetOrder);
                        o.TargetSector = savedGame.Sectors.FirstOrDefault(e => e.Id == editorScavengeOrder.TargetSector?.Id);
                        o.CollectOwnerMode = editorScavengeOrder.CollectOwnerMode;

                        return o;
                    }
                case EditorMineOrder editorMineOrder:
                    {
                        var o = CreateFleetOrderOfType<ModelMineOrder>(editorFleetOrder);
                        o.TargetSector = savedGame.Sectors.FirstOrDefault(e => e.Id == editorMineOrder.TargetSector?.Id);
                        o.CollectOwnerMode = editorMineOrder.CollectOwnerMode;
                        o.ManualMineTarget = savedGame.Units.FirstOrDefault(e => e.Id == editorMineOrder.ManualMineTarget?.Id);

                        return o;
                    }
                case EditorDockOrder editorDockOrder:
                    {
                        var o = CreateFleetOrderOfType<ModelDockOrder>(editorFleetOrder);
                        o.TargetDock = savedGame.Units.FirstOrDefault(e => e.Id == editorDockOrder.TargetDock?.Id);

                        return o;
                    }
                case EditorPatrolOrder editorPatrolOrder:
                    {
                        var o = CreateFleetOrderOfType<ModelPatrolOrder>(editorFleetOrder);

                        o.PathDirection = editorPatrolOrder.PathDirection;
                        o.IsLooping = editorPatrolOrder.IsLooping;

                        var editorPatrolPathNodes = editorPatrolOrder.GetComponentsInChildren<EditorPatrolPathNode>();
                        foreach (var editorPatrolPathNode in editorPatrolPathNodes)
                        {
                            if (editorPatrolPathNode.Target == null)
                            {
                                Logging.LogAndThrow("Editor patrol path node does not have a target", editorPatrolPathNode);
                            }

                            var editorSector = editorPatrolPathNode.Target.GetComponentInParent<EditorSector>();
                            if (editorSector == null)
                            {
                                Logging.LogAndThrow("Editor patrol path node must be a child of a sector", editorPatrolPathNode);
                            }

                            var node = new ModelPatrolPathNode();
                            node.Sector = savedGame.Sectors.FirstOrDefault(e => e.Id == editorSector.Id);
                            node.SectorPosition = editorPatrolPathNode.Target.transform.localPosition.ToVec3_ZeroY();
                            o.Nodes.Add(node);
                        }

                        o.IsLoop = editorPatrolOrder.IsLoop;

                        return o;
                    }
                case EditorPatrolPathOrder editorPatrolPathOrder:
                    {
                        var o = CreateFleetOrderOfType<ModelPatrolPathOrder>(editorFleetOrder);

                        o.PathDirection = editorPatrolPathOrder.PathDirection;
                        o.IsLooping = editorPatrolPathOrder.IsLooping;
                        o.PatrolPath = savedGame.PatrolPaths.FirstOrDefault(e => e.Id == editorPatrolPathOrder.PatrolPath?.Id);

                        if (o.PatrolPath == null)
                        {
                            Logging.LogAndThrow("Patrol path order is missing a patrol path", editorPatrolPathOrder);
                        }

                        return o;
                    }

                case EditorWaitOrder editorWaitOrder:
                    {
                        // If only all the orders were this simple
                        var o = CreateFleetOrderOfType<ModelWaitOrder>(editorFleetOrder);
                        o.WaitTime = editorWaitOrder.WaitTime;

                        return o;
                    }

                case EditorAttackTargetOrder editorAttackTargetOrder:
                    {
                        var o = CreateFleetOrderOfType<ModelAttackTargetOrder>(editorFleetOrder);
                        o.TargetUnit = savedGame.Units.FirstOrDefault(e => e.Id == editorAttackTargetOrder.TargetUnit?.Id);
                        o.AttackPriority = editorAttackTargetOrder.AttackPriority;

                        return o;
                    }
                case EditorManualTradeOrder editorManualTradeOrder:
                    {
                        var o = CreateFleetOrderOfType<ModelManualTradeOrder>(editorFleetOrder);
                        o.MinBuyQuantity = editorManualTradeOrder.MinBuyQuantity;
                        o.MinBuyCargoPercentage = editorManualTradeOrder.MinBuyCargoPercentage;
                        
                        var editorCustomTradeRoute = editorManualTradeOrder.GetComponentInChildren<EditorCustomTradeRoute>();
                        if (editorCustomTradeRoute == null)
                        {
                            Logging.LogAndThrow("Manual trade order should have a trade route", editorManualTradeOrder);
                        }

                        o.CustomTradeRoute = new ModelCustomTradeRoute
                        {
                            BuyLocation = savedGame.Units.FirstOrDefault(e => e.Id == editorCustomTradeRoute.BuyLocation?.Id),
                            SellLocation = savedGame.Units.FirstOrDefault(e => e.Id == editorCustomTradeRoute.SellLocation?.Id),
                            BuyPriceMultiplier = editorCustomTradeRoute.BuyPriceMultiplier,
                            CargoClass = editorCustomTradeRoute.CargoClass
                        };

                        return o;
                    }
                case EditorUniverseTradeOrder editorUniverseTradeOrder:
                    {
                        var o = CreateFleetOrderOfType<ModelUniverseTradeOrder>(editorFleetOrder);
                        o.MinBuyQuantity = editorUniverseTradeOrder.MinBuyQuantity;
                        o.MinBuyCargoPercentage = editorUniverseTradeOrder.MinBuyCargoPercentage;

                        var editorSpecificCargoTypes = editorUniverseTradeOrder.GetComponentsInChildren<EditorTradeOrderCargoClass>();

                        o.TradeOnlySpecificCargoClasses = editorSpecificCargoTypes.Any();
                        foreach (var editorCargoClass in editorSpecificCargoTypes)
                        {
                            if (!System.Enum.IsDefined(typeof(ModelCargoClass), editorCargoClass.CargoClass))
                            {
                                Logging.LogAndThrow("Unknown cargo class", editorCargoClass);
                            }

                            o.TradeSpecificCargoClasses.Add(editorCargoClass.CargoClass);
                        }

                        return o;
                    }
                case EditorJoinFleetOrder editorJoinFleetOrder:
                    {
                        var o = CreateFleetOrderOfType<ModelJoinFleetOrder>(editorFleetOrder);
                        o.TargetFleet = savedGame.Fleets.FirstOrDefault(e => e.Id == editorJoinFleetOrder.TargetFleet?.Id);

                        if (o.TargetFleet == null)
                        {
                            Logging.LogAndThrow("Join fleet order should have a target fleet", editorJoinFleetOrder);
                        }

                        return o;
                    }

                case EditorMoveToOrder editorMoveToOrder:
                    {
                        var o = CreateFleetOrderOfType<ModelMoveToOrder>(editorFleetOrder);
                        o.CompleteOnReachTarget = editorMoveToOrder.CompleteOnReachTarget;
                        o.ArrivalThreshold = editorMoveToOrder.ArrivalThreshold;
                        o.MatchTargetOrientation = editorMoveToOrder.MatchTargetOrientation;

                        if (editorMoveToOrder.Target == null)
                        {
                            Logging.LogAndThrow("Move to order needs a target", editorMoveToOrder);
                        }

                        o.Target = ModelSectorTargetUtil.FromTransform(editorMoveToOrder.Target, savedGame);

                        return o;
                    }

                case EditorProtectOrder editorProtectOrder:
                    {
                        var o = CreateFleetOrderOfType<ModelProtectOrder>(editorFleetOrder);
                        o.CompleteOnReachTarget = editorProtectOrder.CompleteOnReachTarget;
                        o.ArrivalThreshold = editorProtectOrder.ArrivalThreshold;
                        o.MatchTargetOrientation = editorProtectOrder.MatchTargetOrientation;

                        if (editorProtectOrder.Target == null)
                        {
                            Logging.LogAndThrow("Protect needs a target", editorProtectOrder);
                        }

                        o.Target = ModelSectorTargetUtil.FromTransform(editorProtectOrder.Target, savedGame);

                        return o;
                    }

                case EditorSellCargoOrder editorSellCargoOrder:
                    {
                        var o = CreateFleetOrderOfType<ModelSellCargoOrder>(editorFleetOrder);
                        o.FreeUnitsCompleteThreshold = editorSellCargoOrder.FreeUnitsCompleteThreshold;
                        o.MinBuyPriceMultiplier = editorSellCargoOrder.MinBuyPriceMultiplier;
                        o.CompleteWhenNoBuyerFound = editorSellCargoOrder.CompleteWhenNoBuyerFound;
                        o.CompleteWhenNoCargoToSell = editorSellCargoOrder.CompleteWhenNoCargoToSell;
                        o.ManualBuyerUnit = savedGame.Units.FirstOrDefault(e => e.Id == editorSellCargoOrder.ManualBuyerUnit?.Id);
                        o.CustomSellCargoTime = editorSellCargoOrder.CustomSellCargoTime;
                        o.SellEquipment = editorSellCargoOrder.SellEquipment;

                        var editorSpecificCargoTypes = editorSellCargoOrder.GetComponentsInChildren<EditorTradeOrderCargoClass>();

                        o.SellOnlyListedCargos = editorSpecificCargoTypes.Any();
                        foreach (var editorCargoClass in editorSpecificCargoTypes)
                        {
                            if (!System.Enum.IsDefined(typeof(ModelCargoClass), editorCargoClass.CargoClass))
                            {
                                Logging.LogAndThrow("Unknown cargo class", editorCargoClass);
                            }

                            o.SellCargoClasses.Add(editorCargoClass.CargoClass);
                        }

                        return o;
                    }
                case EditorReturnToBaseOrder:
                    return CreateFleetOrderOfType<ModelReturnToBaseOrder>(editorFleetOrder);
                case EditorDisposeCargoOrder:
                    return CreateFleetOrderOfType<ModelDisposeCargoOrder>(editorFleetOrder);
                case EditorUniversePassengerTransportOrder:
                    return CreateFleetOrderOfType<ModelUniversePassengerTransportOrder>(editorFleetOrder);
                case EditorUniverseBountyHunterOrder:
                    return CreateFleetOrderOfType<ModelUniverseBountyHunterOrder>(editorFleetOrder);
                case EditorUniverseRoamOrder:
                    return CreateFleetOrderOfType<ModelUniverseRoamOrder>(editorFleetOrder);
                case EditorExploreOrder:
                    return CreateFleetOrderOfType<ModelExploreOrder>(editorFleetOrder);
                case EditorManualRepairFleetOrder editorManualRepairFleetOrder:
                    {
                        var o = CreateFleetOrderOfType<ModelManualRepairFleetOrder>(editorFleetOrder);
                        o.InsufficientCreditsMode = editorManualRepairFleetOrder.InsufficientCreditsMode;
                        o.RepairLocationUnit = savedGame.Units.FirstOrDefault(e => e.Id == editorManualRepairFleetOrder.RepairLocationUnit?.Id);
                        return o;
                    }
                case EditorRepairAtNearestStationOrder editorRepairAtNearestStationOrder:
                    {
                        var o = CreateFleetOrderOfType<ModelRepairAtNearestStationOrder>(editorFleetOrder);
                        o.InsufficientCreditsMode = editorRepairAtNearestStationOrder.InsufficientCreditsMode;
                        return o;
                    }
                case EditorMoveToNearestFriendlyStationOrder editorMoveToNearestFriendlyStationOrder:
                    {
                        var o = CreateFleetOrderOfType<ModelMoveToNearestFriendlyStationOrder>(editorFleetOrder);
                        o.CompleteOnReachTarget = editorMoveToNearestFriendlyStationOrder.CompleteOnReachTarget;
                        return o;
                    }
                default:
                    {
                        Debug.LogErrorFormat("Unknown order type {0}", editorFleetOrder.GetType().Name);
                        return null;
                    }
            }
        }

        public static T CreateFleetOrderOfType<T>(EditorFleetOrderBase editorFleetOrderBase) where T : ModelFleetOrder, new()
        {
            var editorFleetOrderCommon = editorFleetOrderBase.GetComponent<EditorFleetOrderCommon>();
            return new T
            {
                AllowCombatInterception = editorFleetOrderCommon.AllowCombatInterception,
                AllowTimeout = editorFleetOrderCommon.AllowTimeout,
                CloakPreference = editorFleetOrderCommon.CloakPreference,
                CompletionMode = editorFleetOrderCommon.CompletionMode,
                Id = editorFleetOrderCommon.Id,
                MaxJumpDistance = editorFleetOrderCommon.MaxJumpDistance,
                TimeoutTime = editorFleetOrderCommon.TimeoutTime,
            };
        }
    }
}
