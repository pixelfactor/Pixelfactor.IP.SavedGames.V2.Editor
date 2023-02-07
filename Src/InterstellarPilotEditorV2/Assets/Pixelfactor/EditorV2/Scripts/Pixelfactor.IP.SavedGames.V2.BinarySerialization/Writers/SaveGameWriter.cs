using Pixelfactor.IP.SavedGames.V2.Model;
using Pixelfactor.IP.SavedGames.V2.Model.Factions;
using Pixelfactor.IP.SavedGames.V2.Model.Factions.Bounty;
using Pixelfactor.IP.SavedGames.V2.Model.Jobs;
using Pixelfactor.IP.SavedGames.V2.BinarySerialization.Writers.Helpers;
using Pixelfactor.IP.SavedGames.V2.Model.Helpers;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using Pixelfactor.IP.Common;
using Pixelfactor.IP.SavedGames.V2.Model.Triggers;
using Pixelfactor.IP.Common.Triggers;
using Pixelfactor.IP.SavedGames.V2.Model.Actions;

namespace Pixelfactor.IP.SavedGames.V2.BinarySerialization.Writers
{
    public class SaveGameWriter : ISaveGameWriter
    {
        private readonly HeaderWriter headerWriter;

        public SaveGameWriter(HeaderWriter headerWriter)
        {
            this.headerWriter = headerWriter;
        }

        public static void WriteToPath(SavedGame savedGame, string path)
        {
            using (var writer = new BinaryWriter(File.OpenWrite(path)))
            {
                var savedGameWriter = new SaveGameWriter(new HeaderWriter());
                savedGameWriter.Write(writer, savedGame);
            }
        }

        public void Write(BinaryWriter writer, ISavedGame savedGame, Action<string> logger = null)
        {
            var _savedGame = (SavedGame)savedGame;

            this.headerWriter.Write(writer, _savedGame.Header);
            PrintStatus("Saved header", writer, logger);

            writer.WriteStringOrEmpty(_savedGame.MissionLog);

            writer.Write("[Sectors]");
            WriteSectors(writer, _savedGame.Sectors);
            PrintStatus("Saved sectors", writer, logger);

            writer.Write("[Factions]");
            WriteFactions(writer, _savedGame.Factions);
            PrintStatus("Saved factions", writer, logger);

            writer.Write("[Avatars]");
            WriteFactionAvatarProfiles(writer, _savedGame.Factions);
            PrintStatus("Saved faction avatar profiles", writer, logger);

            writer.Write("[PatrolPaths]");
            WritePatrolPaths(writer, _savedGame.PatrolPaths);
            PrintStatus("Saved patrol paths", writer, logger);

            writer.Write("[FactionRelations]");
            WriteAllFactionRelations(writer, _savedGame.Factions);
            PrintStatus("Saved faction relations", writer, logger);

            writer.Write("[FactionDamage]");
            WriteFactionRecentDamageReceived(writer, _savedGame.Factions);
            PrintStatus("Saved faction recent damage", writer, logger);

            writer.Write("[FactionOpinions]");
            WriteAllFactionOpinions(writer, _savedGame.Factions);
            PrintStatus("Saved faction opinions", writer, logger);

            writer.Write("[Units]");
            WriteUnits(writer, _savedGame.Units);
            PrintStatus("Saved units", writer, logger);

            writer.Write("[UnitRadius]");
            WriteUnitRadii(writer, _savedGame.Units);
            PrintStatus("Saved unit radii", writer, logger);

            writer.Write("[UnitsNamed]");
            WriteNamedUnits(writer, _savedGame.Units);
            PrintStatus("Saved named units", writer, logger);

            writer.Write("[UnitComponents]");
            WriteAllComponentUnits(writer, _savedGame.Units);
            PrintStatus("Saved all unit components", writer, logger);

            writer.Write("[UnitsConstructing]");
            WriteUnitsUnderConstruction(writer, _savedGame.Units);
            PrintStatus("Saved units under construction", writer, logger);

            writer.Write("[UnitDamageCounter]");
            WriteUnitTotalDamageReceived(writer, _savedGame.Units);
            PrintStatus("Saved unit damage received", writer, logger);

            writer.Write("[UnitComponnentMod]");
            WriteModdedComponents(writer, _savedGame.Units);
            PrintStatus("Saved modded components", writer, logger);

            writer.Write("[CapacitorCharge]");
            WriteUnitCapacitorCharges(writer, _savedGame.Units);
            PrintStatus("Saved unit capacitor charges", writer, logger);

            writer.Write("[CloakState]");
            WriteCloakedUnits(writer, _savedGame.Units);
            PrintStatus("Saved unit cloak states", writer, logger);

            writer.Write("[UnitsPoweredDown]");
            WritePoweredDownComponents(writer, _savedGame.Units);
            PrintStatus("Saved powered down units", writer, logger);

            writer.Write("[EngineThrottle]");
            WriteUnitEngineThrottles(writer, _savedGame.Units);
            PrintStatus("Saved engine throttle data", writer, logger);

            writer.Write("[Cargo]");
            WriteComponentUnitCargo(writer, _savedGame.Units);
            PrintStatus("Saved cargo", writer, logger);

            writer.Write("[ShieldHealth]");
            WriteAllShieldHealthData(writer, _savedGame.Units);
            PrintStatus("Saved damaged shields", writer, logger);

            writer.Write("[ComponentHealth]");
            WriteAllUnitComponentHealthData(writer, _savedGame.Units);
            PrintStatus("Saved damaged components", writer, logger);

            writer.Write("[ActiveUnits]");
            WriteActiveUnits(writer, _savedGame.Units);
            PrintStatus("Saved active units", writer, logger);

            writer.Write("[UnitHealth]");
            WriteAllUnitHealthDatas(writer, _savedGame.Units);
            PrintStatus("Saved destructable units", writer, logger);

            writer.Write("[FactionIntel]");
            WriteAllFactionIntel(writer, _savedGame.Factions);
            PrintStatus("Saved faction intel", writer, logger);

            writer.Write("[Passengers]");
            WritePassengerGroups(writer, _savedGame.Units);
            PrintStatus("Saved passenger groups", writer, logger);

            writer.Write("[Wormholes]");
            WriteWormholes(writer, _savedGame.Units);
            PrintStatus("Saved wormholes", writer, logger);

            writer.Write("[Hangars]");
            WriteHangars(writer, _savedGame.Units);
            PrintStatus("Saved hangers", writer, logger);

            writer.Write("[Fleets]");
            WriteFleets(writer, _savedGame.Fleets);
            PrintStatus("Saved fleets", writer, logger);

            writer.Write("[FleetsNamed]");
            WriteNamedFleets(writer, _savedGame.Fleets);

            writer.Write("[People]");
            WritePeople(writer, _savedGame.People, _savedGame.Player?.Person);
            PrintStatus("Saved people", writer, logger);

            writer.Write("[FleetOrders]");
            WriteFleetOrders(writer, _savedGame.Fleets);
            PrintStatus("Saved fleet orders", writer, logger);

            writer.Write("[NpcPilots]");
            WriteNpcPilots(writer, _savedGame.People);
            PrintStatus("Saved NPC pilots", writer, logger);

            writer.Write("[FactionLeaders]");
            WriteFactionLeaders(writer, _savedGame.Factions);
            PrintStatus("Saved faction leaders", writer, logger);

            writer.Write("[Jobs]");
            WriteJobs(writer, _savedGame.Units);
            PrintStatus("Saved jobs", writer, logger);

            writer.Write("[Bounty]");
            WriteAllFactionAIsAndBountyBoards(writer, _savedGame.Factions);
            PrintStatus("Saved faction AIs / bounty boards", writer, logger);

            writer.Write("[FactionAIExcludes]");
            WriteFactionAIExcludedUnits(writer, _savedGame.Factions);
            PrintStatus("Saved faction excluded unit data", writer, logger);

            writer.Write("[Mercenaries]");
            WriteFactionMercenaryData(writer, _savedGame.Factions);
            PrintStatus("Saved mercenary data", writer, logger);

            writer.Write("[FleetSpawners]");
            WriteFleetSpawners(writer, _savedGame.FleetSpawners);
            PrintStatus("Saved NPC fleet spawners", writer, logger);

            writer.Write("[Missions]");
            WriteMissions(writer, _savedGame.Missions);
            PrintStatus("Saved missions", writer, logger);

            writer.Write("[Player]");
            writer.Write(_savedGame.Player != null);
            if (_savedGame.Player != null)
            {
                WriteGamePlayer(writer, _savedGame.Player);
                PrintStatus("Saved player data", writer, logger);
            }

            writer.Write("[HUD]");
            writer.WriteUnitId(_savedGame.CurrentHudTarget);
            PrintStatus("Saved hud data", writer, logger);

            writer.Write("[FactionTransactions]");
            WriteAllFactionTransactions(writer, _savedGame.Factions);
            PrintStatus("Saved all faction transactions", writer, logger);

            writer.Write("[Scenario]");
            WriteScenarioData(writer, _savedGame.ScenarioData);
            PrintStatus("Saved world", writer, logger);

            writer.Write("[Moons]");
            WriteMoons(writer, _savedGame.Moons);
            PrintStatus("Saved moons", writer, logger);

            writer.Write("[SeedOptions]");
            WriteSeedOptions(writer, _savedGame.SeedOptions);
            PrintStatus("Saved seed options", writer, logger);

            writer.Write("[AutoTurrets]");
            WriteAutoTurretModuleData(writer, _savedGame.Units);
            PrintStatus("Saved auto-turret module data", writer, logger);

            writer.Write("[ComponentAutoFire]");
            WriteAutoFireComponents(writer, _savedGame.Units);
            PrintStatus("Saved auto-fire component data", writer, logger);

            writer.Write("[UnitCapture]");
            WriteUnitCaptureCooldownTimes(writer, _savedGame.Units);
            PrintStatus("Saved unit capture cooldown times", writer, logger);

            writer.Write("[PlayerFleetSettings]");
            WritePlayerFleetSettings(writer, _savedGame.Fleets);
            PrintStatus("Saved player fleet settings", writer, logger);

            writer.Write("[SectorAppearance]");
            WriteCustomSectorAppearances(writer, _savedGame.Sectors);
            PrintStatus("Saved custom sector appearance", writer, logger);

            writer.Write("[UnitMass]");
            WriteUnitMass(writer, _savedGame.Units);
            PrintStatus("Saved unit mass", writer, logger);

            writer.Write("[UnitCargoCapacity]");
            WriteUnitCargoCapacity(writer, _savedGame.Units);
            PrintStatus("Saved unit cargo capacity", writer, logger);

            writer.Write("[UnitScanRange]");
            WriteUnitScanRange(writer, _savedGame.Units);
            PrintStatus("Saved unit scan range", writer, logger);

            writer.Write("[UnitInvulnerable]");
            WriteInvulnerableUnits(writer, _savedGame.Units);
            PrintStatus("Saved invulnerable units", writer, logger);

            writer.Write("[UnitNoDestruction]");
            WriteNoDestructionUnits(writer, _savedGame.Units);
            PrintStatus("Saved units no destruction", writer, logger);

            writer.Write("[FleetOrderAvailableCredits]");
            WriteFleetOrderAvailableCredits(writer, _savedGame.Fleets);
            PrintStatus("Saved fleet order available credits", writer, logger);

            writer.Write("[PersonCustomTitles]");
            WritePersonCustomTitles(writer, _savedGame.People);
            PrintStatus("Saved person custom titles", writer, logger);

            writer.Write("[TriggerGroups]");
            WriteTriggerGroups(writer, _savedGame.TriggerGroups);
            PrintStatus("Saved trigger groups", writer, logger);

            writer.Write("[DitchedUnits]");
            WriteDitchedUnits(writer, _savedGame.DitchedUnitsToBeCleanedUp);
            PrintStatus("Saved ditched units", writer, logger);

            writer.Write("[EngineData]");
            WriteEngineData(writer, _savedGame.EngineData);
            PrintStatus("Saved engine data", writer, logger);

            writer.Write("[PlayerUnitFleetSettings]");
            WritePlayerUnitFleetSettings(writer, _savedGame.PlayerUnitFleetSettingItems);
            PrintStatus("Saved player unit fleet settings", writer, logger);

            writer.Write("[PlayerDefaultFleetSettings]");
            WritePlayerDefaultFleetSettings(writer, _savedGame.PlayerDefaultFleetSettings);
            PrintStatus("Saved player default fleet settings", writer, logger);

            writer.Write("[UnitTractorerInfo]");
            WriteUnitTractorerInfo(writer, _savedGame.TractorerDataItems);
            PrintStatus("Saved unit tractorer info", writer, logger);

            writer.Write("[CustomUnitClassNames]");
            WriteReadCustomUnitClasses(writer, _savedGame.Units);
            PrintStatus("Saved custom unit class names", writer, logger);

            writer.Write("[HappyModding_Pixelfactor2022]");
        }

        private void WriteReadCustomUnitClasses(BinaryWriter writer, IEnumerable<ModelUnit> units)
        {
            var unitsToWrite = units.Where(e => !string.IsNullOrWhiteSpace(e.CustomClassName));
            writer.Write(unitsToWrite.Count());
            foreach (var unit in unitsToWrite)
            {
                writer.WriteUnitId(unit);
                writer.WriteStringOrEmpty(unit.CustomClassName);
            }
        }

        private void WriteUnitTractorerInfo(BinaryWriter writer, IEnumerable<ModelTractorerDataItem> tractorerDataItems)
        {
            writer.Write(tractorerDataItems.Count());
            foreach (var item in tractorerDataItems)
            {
                writer.WriteUnitId(item.TractoringUnit);
                writer.WriteUnitId(item.TractoredUnit);
            }
        }

        private void WritePlayerDefaultFleetSettings(BinaryWriter writer, ModelPlayerFleetSettingsCombined modelPlayerFleetSettingsCombined)
        {
            writer.Write(modelPlayerFleetSettingsCombined != null);
            if (modelPlayerFleetSettingsCombined != null)
            {
                WritePlayerFleetSettingsCombined(writer, modelPlayerFleetSettingsCombined);
            }
        }

        private void WritePlayerUnitFleetSettings(BinaryWriter writer, List<ModelPlayerUnitFleetSettings> modelPlayerUnitFleetSettings)
        {
            writer.Write(modelPlayerUnitFleetSettings.Count);
            foreach (var modelPlayerUnitFleetSettingsItem in modelPlayerUnitFleetSettings)
            {
                WritePlayerUnitFleetSettingsItem(writer, modelPlayerUnitFleetSettingsItem);
            }
        }

        private void WritePlayerUnitFleetSettingsItem(BinaryWriter writer, ModelPlayerUnitFleetSettings modelPlayerUnitFleetSettings)
        {
            writer.WriteUnitId(modelPlayerUnitFleetSettings.Unit);

            if (modelPlayerUnitFleetSettings.Settings == null)
            {
                modelPlayerUnitFleetSettings.Settings = new ModelPlayerFleetSettingsCombined();
            }

            WritePlayerFleetSettingsCombined(writer, modelPlayerUnitFleetSettings.Settings);
        }

        private void WritePlayerFleetSettingsCombined(BinaryWriter writer, ModelPlayerFleetSettingsCombined modelPlayerFleetSettingsCombined)
        {
            WriteSectorTarget(writer, modelPlayerFleetSettingsCombined.HomeBase);
            writer.Write(modelPlayerFleetSettingsCombined.FormationId);
            WriteFleetSettings(writer, modelPlayerFleetSettingsCombined.FleetSettings);
            if (modelPlayerFleetSettingsCombined.FleetSettings.PlayerFleetSettings == null)
            {
                modelPlayerFleetSettingsCombined.FleetSettings.PlayerFleetSettings = new ModelPlayerFleetSettings();
            }

            WritePlayerFleetSettings(writer, modelPlayerFleetSettingsCombined.FleetSettings.PlayerFleetSettings);
        }


        private void WriteDitchedUnits(BinaryWriter writer, List<ModelDitchedUnit> ditchedUnitsToBeCleanedUp)
        {
            writer.Write(ditchedUnitsToBeCleanedUp.Count);
            foreach (var item in ditchedUnitsToBeCleanedUp)
            {
                writer.WriteUnitId(item.Unit);
                writer.Write(item.ExpiryTime);
            }
        }

        private void WriteEngineData(BinaryWriter writer, ModelEngineData modelEngineData)
        {
            writer.Write(modelEngineData.UnitIdCounter);
            writer.Write(modelEngineData.PlayerMessageIdCounter);
            writer.Write(modelEngineData.PersonIdCounter);
            writer.Write(modelEngineData.FactionIdCounter);
            writer.Write(modelEngineData.FleetOrderIdCounter);
            writer.Write(modelEngineData.PassengerGroupIdCounter);
            writer.Write(modelEngineData.JobIdCounter);
            writer.Write(modelEngineData.FleetIdCounter);
            writer.Write(modelEngineData.SectorIdCounter);
            writer.Write(modelEngineData.MissionIdCounter);
            writer.Write(modelEngineData.PatrolPathIdCounter);
            writer.Write(modelEngineData.MissionObjectiveIdCounter);
        }

        private void WriteTriggerGroups(BinaryWriter writer, List<ModelTriggerGroup> triggerGroups)
        {
            writer.Write(triggerGroups.Count);
            foreach (var triggerGroup in triggerGroups)
            {
                WriteTriggerGroup(writer, triggerGroup);
            }

            // write triggers/actions
            writer.Write(triggerGroups.Count);
            foreach (var triggerGroup in triggerGroups)
            {
                writer.Write(triggerGroup.Id);
                writer.Write(triggerGroup.Triggers.Count);
                foreach (var trigger in triggerGroup.Triggers)
                {
                    WriteTrigger(writer, trigger);
                }

                writer.Write(triggerGroup.Actions.Count);
                foreach (var action in triggerGroup.Actions)
                {
                    WriteAction(writer, action);
                }
            }
        }

        private void WriteTriggerGroup(BinaryWriter writer, ModelTriggerGroup triggerGroup)
        {
            writer.Write(triggerGroup.Id);
            writer.Write(triggerGroup.IsActive);
            writer.Write(triggerGroup.FireAndDisable);
            writer.Write(triggerGroup.EvaluateFrequency);

            writer.Write(triggerGroup.NextEvaluationTime);
            writer.Write(triggerGroup.FireCount);
            writer.Write(triggerGroup.MaxFireCount);
            writer.Write((byte)triggerGroup.MaxFiredAction);
        }

        private void WriteTrigger(BinaryWriter writer, ModelTrigger trigger)
        {
            writer.Write((int)trigger.Type);
            writer.Write(trigger.Invert);

            switch (trigger.Type)
            {
                case TriggerType.Player_CurrentHudTarget:
                    {
                        var triggerType = (ModelTrigger_Player_CurrentHudTarget)trigger;
                        writer.WriteUnitId(triggerType.TargetUnit);
                    }
                    break;
                case TriggerType.Player_IsPilotting:
                    {
                        var triggerType = (ModelTrigger_Player_IsPilotting)trigger;
                        writer.Write(triggerType.WaitForHud);
                    }
                    break;
                case TriggerType.Scenario_TimeElapsed:
                    {
                        var triggerType = (ModelTrigger_Scenario_TimeElapsed)trigger;
                        writer.Write(triggerType.Time);
                    }
                    break;
            }
        }

        private void WriteAction(BinaryWriter writer, ModelAction modelAction)
        {
            writer.Write((int)modelAction.Type);

            switch (modelAction.Type)
            {
                case ActionType.Player_NewMessageSimple:
                    {
                        var modelActionTyped = (ModelAction_Player_NewMessageSimple)modelAction;
                        writer.WriteStringOrEmpty(modelActionTyped.From);
                        writer.WriteStringOrEmpty(modelActionTyped.To);
                        writer.WriteStringOrEmpty(modelActionTyped.Subject);
                        writer.WriteStringOrEmpty(modelActionTyped.Message);
                        writer.Write(modelActionTyped.Notifications);
                    }
                    break;
                case ActionType.Mission_Activate:
                    {
                        var modelActionTyped = (ModelAction_Mission_Activate)modelAction;
                        writer.WriteMissionId(modelActionTyped.Mission);
                    }
                    break;
                case ActionType.Mission_ChangeStage:
                    {
                        var modelActionTyped = (ModelAction_Mission_ChangeStage)modelAction;
                        if (modelActionTyped.Stage != null && modelActionTyped.Stage.Mission != null)
                        {
                            writer.WriteMissionId(modelActionTyped.Stage.Mission);
                            writer.Write(modelActionTyped.Stage.Mission.Stages.IndexOf(modelActionTyped.Stage));
                        }
                        else
                        {
                            writer.Write(-1);
                            writer.Write(-1);
                        }

                    }
                    break;
                case ActionType.Mission_ActivateObjective:
                    {
                        var modelActionTyped = (ModelAction_Mission_ActivateObjective)modelAction;
                        writer.Write(modelActionTyped.Objectives.Count);
                        foreach (var objective in modelActionTyped.Objectives)
                        {
                            writer.WriteMissionObjectiveId(objective);
                        }
                    }
                    break;
                case ActionType.Mission_CompleteObjective:
                    {
                        var modelActionTyped = (ModelAction_Mission_CompleteObjective)modelAction;
                        writer.WriteMissionObjectiveId(modelActionTyped.MissionObjective);
                        writer.Write(modelActionTyped.Success);
                    }
                    break;
                case ActionType.TriggerGroup_Activate:
                    {
                        var modelActionTyped = (ModelAction_TriggerGroup_Activate)modelAction;
                        writer.WriteTriggerGroupId(modelActionTyped.TriggerGroup);
                    }
                    break;
            }
        }

        private void WritePersonCustomTitles(BinaryWriter writer, List<ModelPerson> people)
        {
            var peopleToWrite = people.Where(e => !string.IsNullOrWhiteSpace(e.CustomTitle));
            writer.Write(peopleToWrite.Count());

            foreach (var person in peopleToWrite)
            {
                writer.Write(person.Id);
                writer.WriteStringOrEmpty(person.CustomTitle);
            }
        }

        private void WriteFleetOrderAvailableCredits(BinaryWriter writer, List<ModelFleet> fleets)
        {
            var list = new List<(int, int, int)>(8);
            foreach (var fleet in fleets)
            {
                if (fleet.OrdersCollection != null)
                {
                    for (int i = 0; i < fleet.OrdersCollection.Orders.Count; i++)
                    {
                        var order = fleet.OrdersCollection.Orders[i];
                        if (order.AvailableCredits >= 0)
                        {
                            list.Add((fleet.Id, i, order.AvailableCredits));
                        }
                    }
                }
            }

            writer.Write(list.Count);
            foreach (var item in list)
            {
                writer.Write(item.Item1);
                writer.Write(item.Item2);
                writer.Write(item.Item3);
            }
        }

        private void WriteUnitCargoCapacity(BinaryWriter writer, List<ModelUnit> units)
        {
            var unitsToWrite = units.Where(e => e.ComponentUnitData != null && e.ComponentUnitData.CargoCapacity.HasValue);
            writer.Write(unitsToWrite.Count());
            foreach (var unit in unitsToWrite)
            {
                writer.WriteUnitId(unit);
                writer.Write(unit.ComponentUnitData.CargoCapacity.Value);
            }
        }

        private void WriteUnitScanRange(BinaryWriter writer, List<ModelUnit> units)
        {
            var unitsToWrite = units.Where(e => e.ComponentUnitData != null && e.ComponentUnitData.ScanRange.HasValue);
            writer.Write(unitsToWrite.Count());
            foreach (var unit in unitsToWrite)
            {
                writer.WriteUnitId(unit);
                writer.Write(unit.ComponentUnitData.ScanRange.Value);
            }
        }

        private void WriteInvulnerableUnits(BinaryWriter writer, List<ModelUnit> units)
        {
            var unitsToWrite = units.Where(e => e.IsInvulnerable);
            writer.Write(unitsToWrite.Count());
            foreach (var unit in unitsToWrite)
            {
                writer.WriteUnitId(unit);
            }
        }

        private void WriteNoDestructionUnits(BinaryWriter writer, List<ModelUnit> units)
        {
            var unitsToWrite = units.Where(e => e.AvoidDestruction);
            writer.Write(unitsToWrite.Count());
            foreach (var unit in unitsToWrite)
            {
                writer.WriteUnitId(unit);
            }
        }

        private void WriteUnitMass(BinaryWriter writer, List<ModelUnit> units)
        {
            var unitsToWrite = units.Where(e => e.Mass.HasValue);
            writer.Write(unitsToWrite.Count());
            foreach (var unit in unitsToWrite)
            {
                writer.WriteUnitId(unit);
                writer.Write(unit.Mass.Value);
            }
        }

        private void WriteUnitsUnderConstruction(BinaryWriter writer, List<ModelUnit> units)
        {
            var unitsToWrite = units.Where(e => e.ComponentUnitData != null && e.ComponentUnitData.ConstructionState != ConstructionState.Constructed);
            writer.Write(unitsToWrite.Count());
            foreach (var unit in unitsToWrite)
            {
                writer.WriteUnitId(unit);
                writer.Write((byte)unit.ComponentUnitData.ConstructionState);
                writer.Write(unit.ComponentUnitData.ConstructionProgress);
            }
        }

        private void WriteUnitTotalDamageReceived(BinaryWriter writer, List<ModelUnit> units)
        {
            var unitsToWrite = units.Where(e => e.HealthData != null && e.TotalDamagedReceived > 0.0f);
            writer.Write(unitsToWrite.Count());
            foreach (var unit in unitsToWrite)
            {
                writer.WriteUnitId(unit);
                writer.Write(unit.TotalDamagedReceived);
            }
        }

        private void WriteCustomSectorAppearances(BinaryWriter writer, List<ModelSector> sectors)
        {
            var sectorsToWrite = sectors.Where(e => e.CustomAppearance != null);
            writer.Write(sectorsToWrite.Count());
            foreach (var sector in sectorsToWrite)
            {
                writer.WriteSectorId(sector);
                WriteCustomSectorAppearance(writer, sector.CustomAppearance);

            }
        }

        private void WriteCustomSectorAppearance(BinaryWriter writer, ModelSectorAppearance customAppearance)
        {
            writer.Write((int)customAppearance.NebulaBrightness);
            writer.Write((int)customAppearance.NebulaColors);
            writer.Write(customAppearance.NebulaComplexity);
            writer.Write(customAppearance.NebulaCount);
            writer.Write(customAppearance.NebulaTextureCount);
            writer.Write((int)customAppearance.NebulaStyles);
            writer.Write((int)customAppearance.StarsCount);
            writer.Write(customAppearance.StarsIntensity);
        }

        private void WritePlayerFleetSettings(BinaryWriter writer, IEnumerable<ModelFleet> fleets)
        {
            var fleetsToWrite = fleets.Where(e => e.FleetSettings != null && e.FleetSettings.PlayerFleetSettings != null);
            writer.Write(fleetsToWrite.Count());
            foreach (var fleetToWrite in fleetsToWrite)
            {
                writer.WriteFleetId(fleetToWrite);

                WritePlayerFleetSettings(writer, fleetToWrite.FleetSettings.PlayerFleetSettings);
            }
        }

        private void WritePlayerFleetSettings(BinaryWriter writer, ModelPlayerFleetSettings modelPlayerFleetSettings)
        {
            writer.Write(modelPlayerFleetSettings.NotifyWhenOrderComplete);
            writer.Write(modelPlayerFleetSettings.NotifyWhenScannedHostile);
            writer.Write(modelPlayerFleetSettings.NotifyWhenAbandonedUnitFound);
            writer.Write(modelPlayerFleetSettings.NotifyWhenAbandonedCargoFound);
        }

        private void WriteUnitCaptureCooldownTimes(BinaryWriter writer, IEnumerable<ModelUnit> units)
        {
            var unitsToWrite = units.Where(e => e.ComponentUnitData != null && e.ComponentUnitData.CaptureCooldownTime.HasValue);
            writer.Write(unitsToWrite.Count());
            foreach (var unit in unitsToWrite)
            {
                writer.WriteUnitId(unit);
                writer.Write(unit.ComponentUnitData.CaptureCooldownTime.Value);
            }
        }

        private void WriteAutoTurretModuleData(BinaryWriter writer, List<ModelUnit> units)
        {
            var unitsToWrite = units.Where(e => e.ComponentUnitData != null && e.ComponentUnitData.AutoTurretFireMode.HasValue).ToList();
            writer.Write(unitsToWrite.Count);
            foreach (var  modelUnit in unitsToWrite)
            {
                writer.WriteUnitId(modelUnit);
                writer.Write((int)modelUnit.ComponentUnitData.AutoTurretFireMode);
            }
        }

        private void WriteUnitRadii(BinaryWriter writer, List<ModelUnit> units)
        {
            var unitsToWrite = units.Where(e => e.Radius.HasValue).ToList();
            writer.Write(unitsToWrite.Count);
            foreach (var unit in unitsToWrite)
            {
                writer.WriteUnitId(unit);
                writer.Write(unit.Radius.Value);
            }
        }

        private void WriteSeedOptions(BinaryWriter writer, ModelSeedOptions seedOptions)
        {
            writer.Write(seedOptions != null);
            if (seedOptions != null)
            {
                writer.Write(seedOptions.SeedAbandonedCargo);
                writer.Write(seedOptions.SeedAbandonedShips);
                writer.Write(seedOptions.SeedCargoHolds);
                writer.Write(seedOptions.SeedFactionIntel);
                writer.Write(seedOptions.SeedPassengerGroups);
            }
        }

        private void PrintStatus(string message, BinaryWriter writer, Action<string> logger)
        {
            if (logger != null) 
                logger($"{message} - {writer.BaseStream.Position - 1} bytes wrote");
        }

        /// <summary>
        /// Aka moons
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="moons"></param>
        private void WriteMoons(BinaryWriter writer, IList<ModelMoon> moons)
        {
            writer.Write(moons.Count);
            foreach (var moon in moons)
            {
                writer.WriteUnitId(moon.Unit);
                writer.WriteUnitId(moon.OrbitUnit);
                writer.WriteVec3(moon.OffsetFromPlanet);
            }
        }

        private void WriteScenarioData(BinaryWriter writer, ModelScenarioData scenarioData)
        {
            writer.Write(scenarioData.HasRandomEvents);
            if (scenarioData.HasRandomEvents)
            {
                writer.Write(scenarioData.NextRandomEventTime);
            }


            writer.Write(scenarioData.FactionSpawner != null);
            if (scenarioData.FactionSpawner != null)
            {
                writer.Write(scenarioData.FactionSpawner.NextUpdate);
            }

            writer.Write(scenarioData.TradeRouteScenarioData != null);
            if (scenarioData.TradeRouteScenarioData != null)
            {
                writer.Write(scenarioData.TradeRouteScenarioData.NumBlackSailShipsDestroyed);
                writer.WriteFactionId(scenarioData.TradeRouteScenarioData.PirateFaction);
            }

            writer.Write((int)scenarioData.RespawnOnDeath);
            writer.Write(scenarioData.AllowTeleporting);
            writer.Write(scenarioData.Permadeath);
            writer.Write(scenarioData.AsteroidRespawningEnabled);
            writer.Write(scenarioData.AsteroidRespawnTime);
            writer.Write(scenarioData.NextProcessOtherEventsTime);
            writer.Write(scenarioData.AllowStationCapture);
            writer.Write(scenarioData.AllowAbandonShip);
            writer.Write(scenarioData.AllowGodMode);
            writer.Write(scenarioData.PlayerPropertyAttackNotifications);
        }

        private void WriteAllFactionTransactions(
            BinaryWriter writer,
            IEnumerable<ModelFaction> factions)
        {
            // Only player faction should have (or need) these but allow support for others anyway
            var factionsWithTransactions = factions.Where(e => e.Transactions?.Count > 0).ToList();
            writer.Write(factionsWithTransactions.Count);

            foreach (var faction in factionsWithTransactions)
            {
                writer.WriteFactionId(faction);
                writer.Write(faction.Transactions.Count);

                foreach (var transaction in faction.Transactions)
                {
                    WriteFactionTransaction(writer, transaction);
                }
            }
        }

        private static void WriteFactionTransaction(
            BinaryWriter writer,
            ModelFactionTransaction transaction)
        {
            writer.Write((int)transaction.TransactionType);
            writer.Write(transaction.Value);
            writer.Write(transaction.CurrentBalance);
            writer.WriteUnitId(transaction.LocationUnit);
            writer.WriteFactionId(transaction.OtherFaction);
            writer.Write((int)transaction.RelatedCargoClass);
            writer.Write((int)transaction.RelatedUnitClass);
            writer.Write(transaction.GameWorldTime);
            writer.Write((int)transaction.TaxType);
            writer.Write(transaction.RelatedCount.HasValue ? transaction.RelatedCount.Value : -1);
        }

        private void WriteGamePlayer(
            BinaryWriter writer,
            ModelPlayer player)
        {
            writer.Write(player.VisitedUnits.Count);
            foreach (var unit in player.VisitedUnits)
            {
                writer.WriteUnitId(unit);
            }

            writer.Write(player.Messages.Count);
            foreach (var message in player.Messages)
            {
                WritePlayerMessage(writer, message);
            }

            writer.Write(player.DelayedMessages.Count);
            foreach (var delayedMessage in player.DelayedMessages)
            {
                writer.Write(delayedMessage.ShowTime);
                writer.Write(delayedMessage.Important);
                writer.Write(delayedMessage.Notifications);

                WritePlayerMessage(writer, delayedMessage.Message);
            }

            WritePlayerWaypointIfSet(writer, player.CustomWaypoint);

            writer.Write(player.ActiveJob != null ? player.ActiveJob.Id : -1);

            WritePlayerStats(writer, player.Stats);
        }

        private void WritePlayerStats(
            BinaryWriter writer, 
            ModelPlayerStats stats)
        {
            writer.Write(stats.SectorsVisited.Count);
            foreach (var sector in stats.SectorsVisited)
            {
                writer.Write(sector.Id);
            }

            writer.Write(stats.TotalBountyClaimed);
            writer.Write(stats.ShipsMinedToDeath);
        }

        private static void WritePlayerWaypointIfSet(
            BinaryWriter writer,
            ModelPlayerWaypoint playerWaypoint)
        {
            writer.Write(playerWaypoint != null);
            if (playerWaypoint != null)
            {
                WritePlayerWaypoint(writer, playerWaypoint);
            }
        }

        public static void WritePlayerWaypoint(
            BinaryWriter writer,
            ModelPlayerWaypoint waypoint)
        {
            writer.WriteVec3(waypoint.SectorPosition);
            writer.WriteSectorId(waypoint.Sector);
            writer.WriteUnitId(waypoint.TargetUnit);
            writer.Write(waypoint.HadTargetObject);
        }

        public static void WritePlayerMessage(
            BinaryWriter writer,
            ModelPlayerMessage message)
        {
            writer.Write(message.Id);
            writer.Write(message.EngineTimeStamp);
            writer.Write(message.AllowDelete);
            writer.Write(message.Opened);

            writer.WriteUnitId(message.SenderUnit);
            writer.WriteSectorId(message.SenderUnitSector);
            writer.WriteVec3(message.SenderUnitSectorPosition);

            writer.WriteUnitId(message.SubjectUnit);
            writer.WriteSectorId(message.SubjectUnitSector);
            writer.WriteVec3(message.SubjectUnitSectorPosition);

            writer.Write(message.MessageTemplateId > -1);
            if (message.MessageTemplateId > -1)
            {
                writer.Write(message.MessageTemplateId);
            }
            else
            {
                writer.WriteStringOrEmpty(message.ToText);
                writer.WriteStringOrEmpty(message.FromText);
                writer.WriteStringOrEmpty(message.MessageText);
                writer.WriteStringOrEmpty(message.SubjectText);
            }
        }

        private void WriteMissions(
            BinaryWriter writer,
            IList<ModelMission> missions)
        {
            writer.Write(missions.Count);
            foreach (var activeJob in missions)
            {
                MissionWriter.Write(writer, activeJob);
            }
        }

        private void WriteFleetSpawners(
            BinaryWriter writer,
            IList<ModelFleetSpawner> fleetSpawners)
        {
            writer.Write(fleetSpawners.Count);
            foreach (var fleetSpawner in fleetSpawners)
            {
                FleetSpawnerWriter.Write(
                    writer,
                    fleetSpawner);
            }
        }

        private void WriteFactionMercenaryData(BinaryWriter writer, IEnumerable<ModelFaction> factions)
        {
            var factionsToWrite = factions.Where(e => e.FactionAI?.FactionMercenaryHireInfo != null).ToList();
            writer.Write(factionsToWrite.Count);

            foreach (var faction in factionsToWrite)
            {
                writer.WriteFactionId(faction);
                writer.WriteFactionId(faction.FactionAI.FactionMercenaryHireInfo.HiringFaction);
                writer.Write(faction.FactionAI.FactionMercenaryHireInfo.HireTimeExpiry);
            }
        }

        private void WriteFactionAIExcludedUnits(
            BinaryWriter writer,
            IEnumerable<ModelFaction> factions)
        {
            var factionsToWrite = factions.Where(e => e.FactionAI != null && e.FactionAI.ExcludedUnits != null && e.FactionAI.ExcludedUnits.Count > 0).ToList();
            writer.Write(factionsToWrite.Count);
            foreach (var faction in factionsToWrite)
            {
                writer.WriteFactionId(faction);
                writer.Write(faction.FactionAI.ExcludedUnits.Count);

                foreach (var unit in faction.FactionAI.ExcludedUnits)
                {
                    writer.WriteUnitId(unit);
                }
            }
        }

        private void WriteAllFactionAIsAndBountyBoards(
            BinaryWriter writer,
            IList<ModelFaction> factions)
        {
            writer.Write(factions.Count);
            foreach (var faction in factions)
            {
                writer.WriteFactionId(faction);
                writer.Write(faction.FactionAI != null);

                if (faction.FactionAI != null)
                {
                    writer.Write((int)faction.FactionAI.AIType);
                    FactionAIWriter.Write(writer, faction.FactionAI.AIType, faction.FactionAI);
                }

                writer.Write(faction.BountyBoard != null);
                if (faction.BountyBoard != null)
                {
                    WriteFactionBountyBoard(writer, faction.BountyBoard);
                }
            }
        }

        private static void WriteFactionBountyBoard(
            BinaryWriter writer,
            ModelFactionBountyBoard bountyBoard)
        {
            writer.Write(bountyBoard.Items.Count);
            foreach (var item in bountyBoard.Items)
            {
                WriteFactionBountyBoardItem(writer, item);
            }
        }

        private static void WriteFactionBountyBoardItem(
            BinaryWriter writer,
            ModelFactionBountyBoardItem item)
        {
            writer.WritePersonId(item.TargetPerson);
            writer.Write(item.Reward);
            writer.WriteUnitId(item.LastKnownTargetUnit);
            writer.WriteSectorId(item.LastKnownTargetSector);
            writer.WriteNullableVec3(item.LastKnownTargetPosition);
            writer.Write(item.TimeOfLastSighting.HasValue ? item.TimeOfLastSighting.Value : -1d);
            writer.WriteFactionId(item.SourceFaction);
        }

        private void WriteJobs(
            BinaryWriter writer,
            IEnumerable<ModelUnit> units)
        {
            var jobs = units.SelectMany(e => e.Jobs).ToList();
            writer.Write(jobs.Count);
            foreach (var job in jobs)
            {
                writer.Write((int)job.JobType);
                JobWriter.Write(writer, job);
            }
        }

        private void WriteFactionLeaders(BinaryWriter writer, IEnumerable<ModelFaction> factions)
        {
            var factionsWithLeaders = factions.Where(e => e.Leader != null).ToList();
            writer.Write(factionsWithLeaders.Count);
            foreach (var faction in factionsWithLeaders)
            {
                writer.WriteFactionId(faction);
                writer.WritePersonId(faction.Leader);
            }
        }

        private void WriteNpcPilots(BinaryWriter writer, List<ModelPerson> people)
        {
            var npcPilots = people.Where(e => e.NpcPilot != null).ToList();
            writer.Write(npcPilots.Count);
            foreach (var npcPilot in npcPilots)
            {
                writer.WritePersonId(npcPilot);
                WriteNpcPilot(writer, npcPilot.NpcPilot);
            }
        }

        private void WriteNpcPilot(BinaryWriter writer, ModelNpcPilot npcPilot)
        {
            writer.Write(npcPilot.DestroyWhenNoUnit);
            writer.Write(npcPilot.DestroyWhenNotPilotting);
            writer.WriteFleetId(npcPilot.Fleet);
        }

        private void WritePeople(BinaryWriter writer, IList<ModelPerson> people, ModelPerson playerPerson)
        {
            writer.Write(people.Count);

            foreach (var person in people)
            {
                WritePerson(writer, person, person == playerPerson);
            }
        }

        private static void WritePerson(
            BinaryWriter writer,
            ModelPerson person,
            bool isPlayerPerson)
        {
            writer.WritePersonId(person);

            bool hasGeneratedName = string.IsNullOrWhiteSpace(person.CustomName);
            writer.Write(hasGeneratedName);
            if (hasGeneratedName)
            {
                writer.Write(person.GeneratedFirstNameId);
                writer.Write(person.GeneratedLastNameId);
            }
            else
            {
                writer.WriteStringOrEmpty(person.CustomName);
                writer.WriteStringOrEmpty(person.CustomShortName);
            }

            writer.Write(person.Seed);
            writer.Write(person.DialogId);
            writer.Write(person.IsMale);
            writer.Write(person.IsAutoPilot);
            writer.WriteFactionId(person.Faction);
            writer.Write(person.DestroyGameObjectOnKill);
            writer.WriteUnitId(person.CurrentUnit);
            writer.Write(person.IsPilot);
            writer.Write(person.Kills);
            writer.Write(person.Deaths);
            writer.Write(person.Properness);
            writer.Write(person.Aggression);
            writer.Write(person.Greed);
            writer.Write(person.RankId);
            writer.Write((sbyte)person.AvatarProfileId);
            writer.Write(person.DialogProfileId);

            writer.Write(isPlayerPerson);
            writer.Write(person.NpcPilotSettings != null);
            if (person.NpcPilotSettings != null)
            {
                WriteNpcPilotSettings(writer, person.NpcPilotSettings);
            }
        }

        private static void WriteNpcPilotSettings(BinaryWriter writer, ModelNpcPilotSettings settings)
        {
            writer.Write(settings.RestrictedWeaponPreference);
            writer.Write(settings.CombatEfficiency);
            writer.Write(settings.CheatAmmo);
            writer.Write(settings.AllowDitchShip);
        }

        private void WriteFleets(
            BinaryWriter writer,
            IList<ModelFleet> fleets)
        {
            writer.Write(fleets.Count);
            foreach (var fleet in fleets)
            {
                WriteFleet(writer, fleet);
            }
        }

        private void WriteNamedFleets(
            BinaryWriter writer,
            IList<ModelFleet> fleets)
        {
            var namedFleets = fleets.Where(e => !string.IsNullOrWhiteSpace(e.Name));
            writer.Write(namedFleets.Count());
            foreach (var fleet in namedFleets)
            {
                writer.Write(fleet.Id);
                writer.WriteStringOrEmpty(fleet.Name);
            }
        }

        private void WriteFleetOrders(
            BinaryWriter writer,
            IList<ModelFleet> fleets)
        {
            var fleetsWithOrders = fleets.Where(e => e.OrdersCollection.Orders.Count > 0).ToList();
            writer.Write(fleetsWithOrders.Count);
            foreach (var fleet in fleetsWithOrders)
            {
                writer.WriteFleetId(fleet);
                FleetOrdersWriter.Write(writer, fleet.OrdersCollection);
            }
        }

        private void WriteSectorTarget(BinaryWriter writer, ModelSectorTarget modelSectorTarget)
        {
            writer.Write(modelSectorTarget != null);
            if (modelSectorTarget != null)
            {
                SectorTargetWriter.Write(writer, modelSectorTarget);
            }
        }

        private void WriteFleet(
            BinaryWriter writer,
            ModelFleet fleet)
        {
            writer.Write(fleet.IsActive);
            writer.Write(fleet.Id);
            writer.Write(fleet.Seed);

            writer.WriteVec3(fleet.Position);
            writer.WriteVec4(fleet.Rotation);
            writer.WriteSectorId(fleet.Sector);
            writer.WriteFactionId(fleet.Faction);
            writer.Write(fleet.FormationId);

            writer.Write(fleet.HomeBase != null);
            if (fleet.HomeBase != null)
            { 
                SectorTargetWriter.Write(writer, fleet.HomeBase);
            }

            writer.Write(fleet.ExcludeFromFactionAI);
            writer.Write((int)fleet.Strategy);


            writer.Write(fleet.FleetSettings != null);
            if (fleet.FleetSettings != null)
            {
                WriteFleetSettings(writer, fleet.FleetSettings);
            }


        }

        private void WriteFleetSettings(BinaryWriter writer, ModelFleetSettings settings)
        {
            writer.Write(settings.PreferCloak);
            writer.Write((byte)settings.PreferToDock);
            writer.Write(settings.Aggression);
            writer.Write(settings.AllowAttack);
            writer.Write(settings.TargetInterceptionLowerDistance);
            writer.Write(settings.TargetInterceptionUpperDistance);
            writer.Write(settings.MaxJumpDistance);
            writer.Write(settings.AllowCombatInterception);
            writer.Write(settings.DestroyWhenNoPilots);
            writer.Write(settings.FormationTightness);
            writer.Write((int)settings.CargoCollectionPreference);
        }

        private void WriteHangars(BinaryWriter writer, IEnumerable<ModelUnit> units)
        {
            var unitsWithDockedShips = units
                .Where(e => e.ComponentUnitData?.DockData != null)
                .ToList();

            writer.Write(unitsWithDockedShips.Count);

            foreach (var unit in unitsWithDockedShips)
            {
                WriteHangerItems(writer, unit, unit.ComponentUnitData.DockData);
            }
        }

        private void WriteHangerItems(BinaryWriter writer, ModelUnit unit, ModelComponentUnitDockData dockData)
        {
            writer.WriteUnitId(unit);
            writer.Write(dockData.Items.Count);

            foreach (var dockedShip in dockData.Items)
            {
                writer.Write(dockedShip.BayId);
                writer.WriteUnitId(dockedShip.DockedUnit);
            }
        }

        private void WriteWormholes(BinaryWriter writer, IEnumerable<ModelUnit> allUnits)
        {
            var wormholes = allUnits.Where(e => e.WormholeData != null).ToList();
            writer.Write(wormholes.Count);
            foreach (var wormhole in wormholes)
            {
                writer.WriteUnitId(wormhole);
                WriteWormholeData(writer, wormhole.WormholeData);
            }
        }

        private void WriteWormholeData(BinaryWriter writer, ModelUnitWormholeData wormholeData)
        {
            writer.WriteUnitId(wormholeData.TargetWormholeUnit);
            writer.Write(wormholeData.IsUnstable);
            writer.Write(wormholeData.UnstableNextChangeTargetTime);
            writer.WriteVec3(wormholeData.UnstableTargetPosition);
            writer.WriteVec3(wormholeData.UnstableTargetRotation);
            writer.WriteSectorId(wormholeData.UnstableTargetSector);
        }

        private void WritePassengerGroups(BinaryWriter writer, IList<ModelUnit> units)
        {
            var passengerGroups = units.SelectMany(e => e.PassengerGroups).ToList();
            writer.Write(passengerGroups.Count);
            foreach (var passengerGroup in passengerGroups)
            {
                WritePassengerGroup(writer, passengerGroup);
            }
        }

        private void WritePassengerGroup(BinaryWriter writer, ModelPassengerGroup passengerGroup)
        {
            writer.Write(passengerGroup.Id);
            writer.WriteUnitId(passengerGroup.Unit);
            writer.WriteUnitId(passengerGroup.SourceUnit);
            writer.WriteUnitId(passengerGroup.DestinationUnit);
            writer.Write(passengerGroup.PassengerCount);
            writer.Write(passengerGroup.ExpiryTime);
            writer.Write(passengerGroup.Revenue);
        }

        private void WriteAllFactionIntel(BinaryWriter writer, IEnumerable<ModelFaction> factions)
        {
            var factionsWithIntel = factions.Where(e => e.Intel != null).ToList();
            writer.Write(factionsWithIntel.Count);
            foreach (var faction in factionsWithIntel)
            {
                writer.WriteFactionId(faction);
                WriteFactionIntel(writer, faction.Intel);
            }
        }

        private void WriteFactionIntel(BinaryWriter writer, ModelFactionIntel factionIntel)
        {
            writer.Write(factionIntel.Sectors.Count);
            foreach (var sector in factionIntel.Sectors)
            {
                writer.WriteSectorId(sector);
            }

            writer.Write(factionIntel.Units.Count);
            foreach (var unit in factionIntel.Units)
            {
                writer.WriteUnitId(unit);
            }

            writer.Write(factionIntel.EnteredWormholes.Count);
            foreach (var enteredWormhole in factionIntel.EnteredWormholes)
            {
                writer.WriteUnitId(enteredWormhole);
            }
        }

        private void WriteAllUnitHealthDatas(BinaryWriter writer, List<ModelUnit> units)
        {
            var unitsWithHealthData = units.Where(e => e.HealthData != null).ToList();
            writer.Write(unitsWithHealthData.Count);
            foreach (var unit in unitsWithHealthData)
            {
                writer.WriteUnitId(unit);
                WriteUnitHealthData(writer, unit.HealthData);
            }
        }

        private void WriteUnitHealthData(BinaryWriter writer, ModelUnitHealthData healthData)
        {
            writer.Write(healthData.IsDestroyed);
            writer.Write(healthData.Health);
        }

        private void WriteActiveUnits(BinaryWriter writer, List<ModelUnit> units)
        {
            var activeUnits = units.Where(e => e.ActiveData != null).ToList();

            writer.Write(activeUnits.Count);
            foreach (var unit in activeUnits)
            {
                writer.WriteUnitId(unit);
                WriteActiveUnitData(writer, unit.ActiveData);
            }
        }

        private void WriteActiveUnitData(BinaryWriter writer, ModelUnitActiveData activeData)
        {
            writer.WriteVec3(activeData.Velocity);
            writer.Write(activeData.CurrentTurn);
        }

        private void WriteAllUnitComponentHealthData(BinaryWriter writer, List<ModelUnit> units)
        {
            var unitsToWrite = units.Where(e => e.ComponentUnitData?.ComponentHealthData?.Items.Count > 0).ToList();
            writer.Write(unitsToWrite.Count);
            foreach (var unit in unitsToWrite)
            {
                writer.WriteUnitId(unit);
                WriteUnitComponentHealthData(writer, unit.ComponentUnitData.ComponentHealthData);
            }
        }

        private void WriteUnitComponentHealthData(BinaryWriter writer, ModelComponentUnitComponentHealthData componentHealthData)
        {
            writer.Write(componentHealthData.Items.Count);
            foreach (var item in componentHealthData.Items)
            {
                writer.Write(item.BayId);
                writer.Write(item.Health);
            }
        }

        private void WriteAllShieldHealthData(BinaryWriter writer, IEnumerable<ModelUnit> units)
        {
            var unitsWithShieldHealthData = units.Where(e => e.ComponentUnitData?.ShieldData?.Items.Count > 0).ToList();
            writer.Write(unitsWithShieldHealthData.Count);
            foreach (var unit in unitsWithShieldHealthData)
            {
                writer.WriteUnitId(unit);
                WriteShieldHealthData(writer, unit.ComponentUnitData.ShieldData);
            }
        }

        private void WriteShieldHealthData(BinaryWriter writer, ModelComponentUnitShieldHealthData shieldHealthData)
        {
            writer.Write((byte)shieldHealthData.Items.Count);
            foreach (var item in shieldHealthData.Items)
            {
                writer.Write((byte)item.ShieldPointIndex);
                writer.Write(item.Health);
            }
        }

        private void WriteComponentUnitCargo(BinaryWriter writer, List<ModelUnit> units)
        {
            var unitsWithCargo = units.Where(e => e.ComponentUnitData?.CargoData != null).ToList();
            writer.Write(unitsWithCargo.Count);
            foreach (var unit in unitsWithCargo)
            {
                writer.WriteUnitId(unit);
                WriteComponentUnitCargoData(writer, unit.ComponentUnitData.CargoData);
            }
        }

        private void WriteComponentUnitCargoData(BinaryWriter writer, ModelComponentUnitCargoData cargoData)
        {
            writer.Write(cargoData.Items.Count);
            foreach (var item in cargoData.Items)
            {
                ComponentUnitCargoDataItemWriter.Write(writer, item);
            }
        }

        private void WriteUnitEngineThrottles(BinaryWriter writer, List<ModelUnit> units)
        {
            var unitsWithEngineThrottle = units.Where(e => e.ComponentUnitData?.EngineThrottle.HasValue ?? false).ToList();
            writer.Write(unitsWithEngineThrottle.Count);
            foreach (var unit in unitsWithEngineThrottle)
            {
                writer.WriteUnitId(unit);
                writer.Write(unit.ComponentUnitData.EngineThrottle.Value);
            }
        }

        private void WritePoweredDownComponents(BinaryWriter writer, IEnumerable<ModelUnit> units)
        {
            var poweredDownUnits = units.Where(e => e.ComponentUnitData != null && 
                e.ComponentUnitData.PoweredDownBayIds != null &&
                e.ComponentUnitData.PoweredDownBayIds.Count > 0).ToList();

            writer.Write(poweredDownUnits.Count);
            foreach (var unit in poweredDownUnits)
            {
                writer.WriteUnitId(unit);

                writer.Write(unit.ComponentUnitData.PoweredDownBayIds.Count);
                foreach (var bayId in unit.ComponentUnitData.PoweredDownBayIds)
                {
                    writer.Write(bayId);
                }
            }
        }

        private void WriteAutoFireComponents(BinaryWriter writer, IEnumerable<ModelUnit> units)
        {
            var autoFireUnits = units.Where(e => e.ComponentUnitData != null &&
                e.ComponentUnitData.AutoFireBayIds != null &&
                e.ComponentUnitData.AutoFireBayIds.Count > 0).ToList();

            writer.Write(autoFireUnits.Count);
            foreach (var unit in autoFireUnits)
            {
                writer.WriteUnitId(unit);

                writer.Write(unit.ComponentUnitData.AutoFireBayIds.Count);
                foreach (var bayId in unit.ComponentUnitData.AutoFireBayIds)
                {
                    writer.Write(bayId);
                }
            }
        }

        private void WriteCloakedUnits(BinaryWriter writer, IEnumerable<ModelUnit> units)
        {
            var cloakedUnits = units.Where(e => e.ComponentUnitData?.IsCloaked ?? false).ToList();
            writer.Write(cloakedUnits.Count);
            foreach (var unit in cloakedUnits)
            {
                writer.WriteUnitId(unit);
            }
        }

        private void WriteUnitCapacitorCharges(BinaryWriter writer, List<ModelUnit> units)
        {
            var capactorChargeUnits = units.Where(e => e.ComponentUnitData?.CapacitorCharge.HasValue ?? false).ToList();
            writer.Write(capactorChargeUnits.Count);
            foreach (var capacitorChargeUnit in capactorChargeUnits)
            {
                writer.WriteUnitId(capacitorChargeUnit);
                writer.Write(capacitorChargeUnit.ComponentUnitData.CapacitorCharge.Value);
            }
        }

        private void WriteModdedComponents(BinaryWriter writer, List<ModelUnit> units)
        {
            var moddedUnits = units.Where(e => e.ComponentUnitData?.ModData?.Items.Count > 0).ToList();
            writer.Write(moddedUnits.SelectMany(e => e.ComponentUnitData.ModData.Items).Count());

            foreach (var moddedUnit in moddedUnits)
            {
                foreach (var item in moddedUnit.ComponentUnitData.ModData.Items)
                {
                    writer.WriteUnitId(moddedUnit);
                    writer.Write(item.BayId);
                    writer.Write((int)item.ComponentClass);
                }
            }
        }

        private void WriteAllComponentUnits(BinaryWriter writer, IEnumerable<ModelUnit> units)
        {
            var componentUnits = units.Where(e => e.ComponentUnitData != null).ToList();
            writer.Write(componentUnits.Count);

            foreach (var componentUnit in componentUnits)
            {
                writer.Write(componentUnit.Id);
                WriteComponentUnitData(writer, componentUnit.ComponentUnitData);
            }
        }

        private void WriteComponentUnitData(BinaryWriter writer, ModelComponentUnitData componentUnitData)
        {
            writer.Write(componentUnitData.ShipNameIndex);

            if (componentUnitData.ShipNameIndex == -1)
            {
                writer.WriteStringOrEmpty(componentUnitData.CustomShipName);
            }

            writer.Write(componentUnitData.FactoryData != null);
            if (componentUnitData.FactoryData != null)
            {
                WriteComponentUnitFactoryData(writer, componentUnitData.FactoryData);
            }
        }

        private void WriteComponentUnitFactoryData(BinaryWriter writer, ModelComponentUnitFactoryData factoryData)
        {
            writer.Write(factoryData.Items.Count);
            foreach (var item in factoryData.Items)
            {
                writer.Write((int)item.State);
                writer.Write(item.ProductionElapsed);
            }
        }

        private void WriteNamedUnits(BinaryWriter writer, IEnumerable<ModelUnit> units)
        {
            var namedUnits = units.Where(e => !string.IsNullOrWhiteSpace(e.Name)).ToList();
            writer.Write(namedUnits.Count);
            foreach (var unit in namedUnits)
            {
                writer.WriteUnitId(unit);
                writer.WriteStringOrEmpty(unit.Name);
                writer.WriteStringOrEmpty(unit.ShortName);
            }
        }

        private void WriteUnits(BinaryWriter writer, IList<ModelUnit> units)
        {
            writer.Write(units.Count);
            foreach (var unit in units)
            {
                WriteUnit(writer, unit);
            }
        }

        private void WriteUnit(BinaryWriter writer, ModelUnit unit)
        {
            writer.Write(unit.Id);
            writer.Write(unit.Seed);
            writer.Write((int)unit.Class);
            writer.WriteSectorId(unit.Sector);
            writer.WriteVec3(unit.Position);
            writer.WriteVec3(unit.Rotation);
            writer.WriteFactionId(unit.Faction);
            writer.Write(unit.CargoData != null);
            if (unit.CargoData != null)
            {
                WriteUnitCargoData(writer, unit.CargoData);
            }

            writer.Write(unit.DebrisData != null);
            if (unit.DebrisData != null)
            {
                WriteUnitDebrisData(writer, unit.DebrisData);
            }

            writer.Write(unit.AsteroidData != null);
            if (unit.AsteroidData != null)
            {
                WriterUnitAsteroidData(writer, unit.AsteroidData);
            }

            writer.Write(unit.ShipTraderData != null);
            if (unit.ShipTraderData != null)
            {
                WriteShipTrader(writer, unit.ShipTraderData);
            }

            if (UnitHelper.IsProjectile(unit.Class))
            {
                writer.Write(unit.ProjectileData != null);
                if (unit.ProjectileData != null)
                {
                    WriteUnitProjectileData(writer, unit.ProjectileData);
                }
            }
        }

        private void WriteUnitProjectileData(BinaryWriter writer, ModelUnitProjectileData projectileData)
        {
            writer.WriteUnitId(projectileData.SourceUnit);
            writer.WriteUnitId(projectileData.TargetUnit);
            writer.Write(projectileData.FireTime);
            writer.Write(projectileData.RemainingMovement);
            writer.WriteDamageType(projectileData.DamageType);
        }

        private void WriteShipTrader(BinaryWriter writer, ModelUnitShipTraderData shipTraderData)
        {
            writer.Write(shipTraderData.Items.Count);

            foreach (var item in shipTraderData.Items)
            {
                writer.Write(item.SellMultiplier);
                writer.Write((int)item.UnitClass);
            }
        }

        private void WriteUnitDebrisData(BinaryWriter writer, ModelUnitDebrisData debrisData)
        {
            writer.Write(debrisData.ScrapQuantity);
            writer.Write(debrisData.Expires);
            writer.Write(debrisData.ExpiryTime);
            writer.Write((int)debrisData.RelatedUnitClass);
        }

        private void WriterUnitAsteroidData(BinaryWriter writer, ModelUnitAsteroidData asteroidData)
        {
            writer.Write(asteroidData.RemainingYield);
        }

        private void WriteUnitCargoData(BinaryWriter writer, ModelUnitCargoData unitCargoData)
        {
            writer.Write((int)unitCargoData.CargoClass);
            writer.Write(unitCargoData.Quantity);
            writer.Write(unitCargoData.Expires);
            writer.Write(unitCargoData.SpawnTime);
        }

        private void WriteFactions(BinaryWriter writer, IList<ModelFaction> factions)
        {
            writer.Write(factions.Count);
            foreach (var faction in factions)
            {
                WriteFaction(writer, faction);
            }
        }

        private void WriteFactionAvatarProfiles(BinaryWriter writer, IList<ModelFaction> factions)
        {
            var factionsToWrite = factions.Where(e => e.AvatarProfileIds != null && e.AvatarProfileIds.Count > 0);
            writer.Write(factionsToWrite.Count());
            foreach (var faction in factionsToWrite)
            {
                writer.WriteFactionId(faction);
                writer.Write((byte)faction.AvatarProfileIds.Count);
                foreach (var avatarProfileId in faction.AvatarProfileIds)
                {
                    writer.Write(avatarProfileId);
                }
            }
        }

        private void WriteSectors(BinaryWriter writer, List<ModelSector> sectors)
        {
            writer.Write(sectors.Count);
            foreach (var sector in sectors)
            {
                WriteSector(writer, sector);
            }
        }

        private void WritePatrolPaths(BinaryWriter writer, IList<ModelSectorPatrolPath> patrolPaths)
        {
            writer.Write(patrolPaths.Count);
            foreach (var patrolPath in patrolPaths)
            {
                WritePatrolPath(writer, patrolPath);
            }
        }

        private void WriteAllFactionRelations(BinaryWriter writer, IEnumerable<ModelFaction> factions)
        {
            var factionWithRelations = factions.Where(e => e.Relations?.Items.Count > 0).ToList();

            writer.Write(factionWithRelations.Count);

            foreach (var faction in factionWithRelations)
            {
                writer.WriteFactionId(faction);
                WriteFactionRelationData(writer, faction.Relations);
            }
        }

        private void WriteFactionRecentDamageReceived(BinaryWriter writer, IEnumerable<ModelFaction> factions)
        {
            var factionsWithRecentDamage = factions.Where(e => e.RecentDamageItems?.Count > 0).ToList();

            writer.Write(factionsWithRecentDamage.Count);

            foreach (var faction in factionsWithRecentDamage)
            {
                writer.WriteFactionId(faction);
                writer.Write(faction.RecentDamageItems.Count);

                foreach (var item in faction.RecentDamageItems)
                {
                    writer.WriteFactionId(item.OtherFaction);
                    writer.Write(item.RecentDamageReceived);
                }
            }
        }

        private void WriteFactionRelationData(BinaryWriter writer, ModelFactionRelationData relationData)
        {
            writer.Write(relationData.Items.Count);
            foreach (var item in relationData.Items)
            {
                WriteFactionRelationDataItem(writer, item);
            }
        }

        private void WriteFactionRelationDataItem(BinaryWriter writer, ModelFactionRelationDataItem relation)
        {
            writer.WriteFactionId(relation.OtherFaction);
            writer.Write(relation.PermanentPeace);
            writer.Write(relation.RestrictHostilityTimeout);
            writer.Write((int)relation.Neutrality);
            writer.Write(relation.HostilityEndTime);
        }

        private void WriteAllFactionOpinions(BinaryWriter writer, IEnumerable<ModelFaction> factions)
        {
            var factionOpinions = factions
                .Where(e => e.Opinions?.Items.Count > 0)
                .Select(e => new { Faction = e, Items = e.Opinions.Items.Where(item => item.OtherFaction != e ).ToList() }) // Filter out invalid ones where faction has opinion with itself. This will crash the game's reader
                .ToList();
            writer.Write(factionOpinions.SelectMany(e => e.Items).Count());
            foreach (var faction in factionOpinions)
            {
                foreach (var item in faction.Items)
                {
                    writer.WriteFactionId(faction.Faction);
                    writer.WriteFactionId(item.OtherFaction);
                    writer.Write(item.Opinion);
                    writer.Write((uint)item.CreatedTime);
                }
            }
        }

        private void WriteSector(BinaryWriter writer, ModelSector sector)
        {
            writer.Write(sector.Id);
            writer.WriteStringOrEmpty(sector.Name);
            writer.WriteVec3(sector.MapPosition);
            writer.WriteStringOrEmpty(sector.Description);
            writer.Write(sector.GateDistanceMultiplier);
            writer.Write(sector.RandomSeed);
            writer.WriteVec3(sector.BackgroundRotation);
            writer.WriteVec3(sector.AmbientLightColor);
            writer.WriteVec3(sector.DirectionLightColor);
            writer.WriteVec3(sector.DirectionLightRotation);
            writer.Write(sector.DirectionLightIntensity);
            writer.Write(sector.LastTimeChangedControl);
            writer.Write(sector.LightDirectionFudge);
        }

        private void WriteFaction(BinaryWriter writer, ModelFaction faction)
        {
            writer.Write(faction.Id);
            writer.Write(faction.GeneratedNameId);
            writer.Write(faction.GeneratedSuffixId);
            writer.WriteStringOrEmpty(faction.CustomName);
            writer.WriteStringOrEmpty(faction.CustomShortName);

            writer.WriteSectorId(faction.HomeSector);
            writer.WriteNullableVec3(faction.HomeSectorPosition);

            writer.Write(faction.Credits);
            writer.WriteStringOrEmpty(faction.Description);
            writer.Write(faction.IsCivilian);
            writer.Write((int)faction.FactionType);
            writer.Write(faction.Aggression);
            writer.Write(faction.Virtue);
            writer.Write(faction.Greed);
            writer.Write(faction.Cooperation);
            writer.Write(faction.TradeEfficiency);
            writer.Write(faction.DynamicRelations);
            writer.Write(faction.ShowJobBoards);
            writer.Write(faction.CreateJobs);
            writer.Write(faction.RequisitionPointMultiplier);
            writer.Write(faction.DestroyWhenNoUnits);
            writer.Write(faction.MinNpcCombatEfficiency);
            writer.Write(faction.MaxNpcCombatEfficiency);
            writer.Write(faction.AdditionalRpProvision);
            writer.Write(faction.TradeIllegalGoods);
            writer.Write(faction.SpawnTime);
            writer.Write(faction.HighestEverNetWorth);
            writer.Write(faction.RankingSystemId);
            writer.Write(faction.PreferredFormationId);

            writer.Write(faction.CustomSettings != null);
            if (faction.CustomSettings != null)
            {
                WriteFactionCustomSettings(writer, faction.CustomSettings);
            }

            writer.Write(faction.Stats != null);
            if (faction.Stats != null)
            {
                WriteFactionStats(writer, faction.Stats);
            }

            writer.Write(faction.AutopilotExcludedSectors.Count);
            foreach (var sector in faction.AutopilotExcludedSectors)
            {
                writer.WriteSectorId(sector);
            }
        }

        private void WriteFactionCustomSettings(BinaryWriter writer, ModelFactionCustomSettings settings)
        {
            writer.Write(settings.BuildShips);
            writer.Write(settings.BuildStations);
            writer.Write(settings.RepairShips);
            writer.Write(settings.UpgradeShips);
            writer.Write(settings.UpgradeStations);
            writer.Write(settings.RepairMinCreditsBeforeRepair);
            writer.Write(settings.PreferenceToPlaceBounty);
            writer.Write(settings.LargeShipPreference);
            writer.Write(settings.CloakShipPreference);
            writer.Write(settings.DailyIncome);
            writer.Write(settings.HostileWithAll);
            writer.Write(settings.MinFleetUnitCount);
            writer.Write(settings.MaxFleetUnitCount);
            writer.Write(settings.OffensiveStance);
            writer.Write(settings.AllowOtherFactionToUseDocks);
            writer.Write(settings.PreferenceToBuildTurrets);
            writer.Write(settings.PreferenceToBuildStations);
            writer.Write(settings.PreferenceToHaveAmmo);
            writer.Write(settings.IgnoreStationCreditsReserve);
            writer.Write(settings.MaxJumpDistanceFromHomeSector);
            writer.Write(settings.MaxStationBuildDistanceFromHomeSector);
            writer.Write((int)settings.PilotGender);
            writer.Write(settings.FixedShipCount);
            writer.Write(settings.SectorControlLikelihood);
        }

        private void WriteFactionStats(BinaryWriter writer, ModelFactionStats factionStats)
        {
            writer.Write(factionStats.TotalShipsClaimed);
            WriteFactionStatsUnitCounts(writer, factionStats.UnitsDestroyedByClassId.Where(e => e.Value > 0));
            WriteFactionStatsUnitCounts(writer, factionStats.UnitLostByClassId.Where(e => e.Value > 0));
            writer.Write(factionStats.ScratchcardsScratched);
            writer.Write(factionStats.HighestScratchcardWin);
        }

        private void WriteFactionStatsUnitCounts(BinaryWriter writer, IEnumerable<KeyValuePair<ModelUnitClass, int>> unitCountsByClass)
        {
            writer.Write(unitCountsByClass.Count());
            foreach (var item in unitCountsByClass)
            {
                writer.Write((int)item.Key);
                writer.Write(item.Value);
            }
        }

        private void WritePatrolPath(BinaryWriter writer, ModelSectorPatrolPath path)
        {
            writer.Write(path.Id);
            writer.WriteSectorId(path.Sector);
            writer.Write(path.IsLoop);
            writer.Write(path.Nodes.Count);
            foreach (var node in path.Nodes)
            {
                writer.WriteVec3(node.SectorPosition);
                writer.Write(node.Order);
            }
        }
    }
}
