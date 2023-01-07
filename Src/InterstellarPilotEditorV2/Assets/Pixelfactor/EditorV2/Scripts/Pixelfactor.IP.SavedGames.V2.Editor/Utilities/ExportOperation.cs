using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.FleetOrders.OrderTypes;
using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.Missions;
using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.Scripting;
using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.Scripting.Actions;
using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.Scripting.Triggers;
using Pixelfactor.IP.SavedGames.V2.Model;
using Pixelfactor.IP.SavedGames.V2.Model.Actions;
using Pixelfactor.IP.SavedGames.V2.Model.Jobs;
using Pixelfactor.IP.SavedGames.V2.Model.Triggers;
using System;
using System.Linq;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Utilities
{
    public class ExportOperation
    {
        private EditorSavedGame editorSavedGame = null;
        private SavedGame savedGame = null;
        private SavedGameExportOptions options = null;

        private EditorFaction playerFaction = null;
        private ModelFaction playerModelFaction = null;
        private EditorPlayer player = null;
        private ModelPlayer modelPlayer = null;

        public SavedGame Export(EditorSavedGame editorSavedGame, SavedGameExportOptions options)
        {
            this.editorSavedGame = editorSavedGame;
            this.savedGame = new SavedGame();
            this.options = options;

            this.ExportSectors();
            this.SetSectorMapPositions();
            this.ExportFactions();
            this.ExportFactionRelations();
            this.ExportUnits();
            this.ExportWormholes();
            this.ExportFleets();
            this.ExportPeople();
            this.ExportPlayer();
            this.ExportScenarioData();
            this.ExportFleetSpawners();
            this.ExportMissions();
            this.ExportTriggers();

            savedGame.EngineData = new ModelEngineData();

            if (options.AutoCreateFleets)
            {
                this.AutoCreateFleetsWhereNeeded();
            }

            this.ExportHeader();
            this.SeedFactionIntel();

            return savedGame;
        }

        private void ExportTriggers()
        {
            foreach (var triggerGroup in this.editorSavedGame.GetComponentsInChildren<EditorTriggerGroup>())
            {
                var modelTriggerGroup = new ModelTriggerGroup
                {
                    Id = triggerGroup.Id,
                    IsActive = triggerGroup.IsActive,
                    EvaluateFrequency = triggerGroup.EvaluateFrequency,
                    FireAndDisable = triggerGroup.FireAndDisable,
                    FireCount = 0,
                    MaxFireCount = triggerGroup.MaxFireCount,
                    MaxFiredAction = triggerGroup.MaxFiredAction,
                };

                var triggers = triggerGroup.GetComponentsInChildren<EditorTrigger_Base>();
                if (triggers.Length == 0)
                {
                    Debug.LogWarning($"Trigger group {triggerGroup.Id} does not have any triggers so will never fire", triggerGroup);
                }

                foreach (var trigger in triggers)
                {
                    var modelTrigger = ExportTrigger(trigger);
                    modelTriggerGroup.Triggers.Add(modelTrigger);
                }

                var actions = triggerGroup.GetComponentsInChildren<EditorAction_Base>();
                if (actions.Length == 0)
                {
                    Debug.LogWarning($"Trigger group {triggerGroup.Id} does not have any actions so will not do anything", triggerGroup);
                }

                foreach (var action in actions)
                {
                    var modelAction = ExportAction(action);
                    modelTriggerGroup.Actions.Add(modelAction);
                }

                savedGame.TriggerGroups.Add(modelTriggerGroup);
            }
        }

        private ModelTrigger ExportTrigger(EditorTrigger_Base trigger)
        {
            var specializedModelTrigger = GetSpecializedModelTrigger(trigger);
            specializedModelTrigger.Invert = trigger.Invert;
            return specializedModelTrigger;
        }

        private ModelTrigger GetSpecializedModelTrigger(EditorTrigger_Base trigger)
        {
            switch (trigger)
            {
                case EditorTrigger_ScenarioTimeElapsed editorTrigger_ScenarioTimeElapsed:
                    {
                        var modelTrigger = new ModelTrigger_Scenario_TimeElapsed
                        {
                            Time = editorTrigger_ScenarioTimeElapsed.Time
                        };

                        return modelTrigger;
                    }
                case EditorTrigger_PlayerHoHostileFactions:
                    {
                        return new ModelTrigger_Player_NoHostileFactions();
                    }
                default:
                    throw new NotSupportedException($"Unknown trigger type: {trigger.GetType()}");
            }
        }

        private ModelAction ExportAction(EditorAction_Base action)
        {
            var specializedModelAction = GetSpecializedModelAction(action);

            // No common properties for actions yet but they would be assigned here..

            return specializedModelAction;
        }

        private ModelAction GetSpecializedModelAction(EditorAction_Base action)
        {
            switch (action)
            {
                case EditorAction_MissionActivate editorAction_missionActivate:
                    {
                        var modelAction = new ModelAction_Mission_Activate
                        {
                            Mission = GetModelMission(editorAction_missionActivate.Mission)
                        };

                        return modelAction;
                    }
                case EditorAction_MissionChangeStage editorAction_MissionChangeStage:
                    {
                        var editorMission = editorAction_MissionChangeStage.EditorMissionStage.GetComponentInParent<EditorMission>();
                        var stages = editorMission.GetComponentsInChildren<EditorMissionStage>();

                        var stageIndex = UnityEditor.ArrayUtility.IndexOf(stages, editorAction_MissionChangeStage.EditorMissionStage);

                        var modelMission = GetModelMission(editorMission);
                        var modelStage = modelMission.Stages[stageIndex];

                        var modelAction = new ModelAction_Mission_ChangeStage
                        {
                            Stage = modelStage,
                        };

                        return modelAction;
                    }
                case EditorAction_MissionCompleteObjective editorAction_MissionCompleteObjective:
                    {
                        var editorMission = editorAction_MissionCompleteObjective.MissionObjective.GetComponentInParent<EditorMission>();
                        var objectives = editorMission.GetComponentsInChildren<EditorMissionObjective>();

                        var objectiveIndex = UnityEditor.ArrayUtility.IndexOf(objectives, editorAction_MissionCompleteObjective.MissionObjective);

                        var modelMission = GetModelMission(editorMission);
                        var modelObjective = modelMission.Objectives[objectiveIndex];

                        var modelAction = new ModelAction_Mission_CompleteObjective
                        {
                            MissionObjective = modelObjective,
                            Success = editorAction_MissionCompleteObjective.Success
                        };

                        return modelAction;
                    }

                default:
                    throw new NotSupportedException($"Unknown action type: {action.GetType()}");
            }
        }

        private void ExportMissions()
        {
            foreach (var editorMission in editorSavedGame.GetComponentsInChildren<EditorMission>())
            {
                var modelMission = new Model.Jobs.ModelMission
                {
                    Id = editorMission.Id,
                    Title = editorMission.Title,
                    CompletionOpinionChange = editorMission.CompletionOpinionChange,
                    FailureOpinionChange = editorMission.FailureOpinionChange,
                    RewardCredits = editorMission.RewardCredits,
                    NotificationsEnabled = editorMission.NotifactionsEnabled,
                    IsPrimary = editorMission.IsPrimaryMission,
                    IsActive = editorMission.IsActive,
                    ShowInJournal = editorMission.IsActive,
                    IsFinished = editorMission.IsFinished,
                    MissionGiverFaction = GetModelFaction(editorMission.MissionGiverFaction),
                    // Currently only player missions are supported so we can just assign this automatically
                    OwnerFaction = this.playerModelFaction
                };

                savedGame.Missions.Add(modelMission);

                // Mission stages
                var editorMissionStages = editorMission.GetComponentsInChildren<EditorMissionStage>();
                foreach (var editorMissionStage in editorMissionStages)
                {
                    var modelMissionStage = new ModelMissionStage
                    {
                        CompletesMission = editorMissionStage.CompletesMission,
                        MissionSuccess = editorMissionStage.CompletesMissionSuccess,
                        JournalEntry = editorMissionStage.Description,
                        Mission = modelMission,
                    };

                    modelMission.Stages.Add(modelMissionStage);
                }

                // Mission objectives
                var objectiveOrder = 0;
                foreach (var editorMissionObjective in editorMission.GetComponentsInChildren<EditorMissionObjective>())
                {
                    var modelMissionObjective = new ModelMissionObjective
                    {
                        Id = editorMissionObjective.Id,
                        Title = editorMission.Title,
                        Description = editorMissionObjective.Description,
                        IsActive = editorMissionObjective.IsActive,
                        ShowInJournal = editorMissionObjective.IsActive,
                        IsComplete = editorMissionObjective.IsComplete,
                        IsOptional = editorMissionObjective.IsOptional,
                        Order = ++objectiveOrder,
                        Success = editorMissionObjective.WasSuccessful
                    };

                    modelMission.Objectives.Add(modelMissionObjective);
                }

                if (this.player != null && this.player.ActiveMission != null && this.player.ActiveMission == editorMission)
                {
                    this.modelPlayer.ActiveJob = modelMission;
                }
            }
        }

        private ModelSector GetModelSector(EditorSector editorSector)
        {
            if (editorSector == null)
                return null;

            return savedGame.Sectors.FirstOrDefault(e => e.Id == editorSector.Id);
        }

        private ModelFaction GetModelFaction(EditorFaction editorFaction)
        {
            if (editorFaction == null)
                return null;

            return savedGame.Factions.FirstOrDefault(e => e.Id == editorFaction.Id);
        }

        private ModelMission GetModelMission(EditorMission editorMission)
        {
            if (editorMission == null)
                return null;

            return savedGame.Missions.FirstOrDefault(e => e.Id == editorMission.Id);
        }

        private void ExportFleets()
        {
            foreach (var editorSector in editorSavedGame.GetComponentsInChildren<EditorSector>())
            {
                var sector = savedGame.Sectors.Single(e => e.Id == editorSector.Id);
                if (sector == null)
                {
                    LogAndThrow("Expected to find sector", editorSector);
                }

                foreach (var editorFleet in editorSector.GetComponentsInChildren<EditorFleet>())
                {
                    if (editorFleet.Faction == null)
                    {
                        LogAndThrow("Fleets must have a faction", editorFleet);
                    }

                    var fleet = new ModelFleet
                    {
                        Id = editorFleet.Id,
                        ExcludeFromFactionAI = editorFleet.ExcludeFromFactionAI,
                        Faction = savedGame.Factions.FirstOrDefault(e => e.Id == editorFleet.Faction?.Id),
                        IsActive = true,
                        HomeBase = ModelSectorTargetUtil.FromTransform(editorFleet.transform, savedGame),
                        Position = editorFleet.transform.localPosition.ToVec3_ZeroY(),
                        Sector = sector,
                        Name = editorFleet.Designation
                    };

                    var editorFleetSettings = editorFleet.GetComponentInChildren<EditorFleetSettings>();
                    if (editorFleetSettings != null)
                    {
                        fleet.FleetSettings = new ModelFleetSettings
                        {
                            AllowAttack = editorFleetSettings.AllowAttack,
                            AllowCombatInterception = editorFleetSettings.AllowCombatInterception,
                            Aggression = editorFleetSettings.Aggression,
                            CargoCollectionPreference = editorFleetSettings.CargoCollectionPreference,
                            DestroyWhenNoPilots = true,
                            MaxJumpDistance = editorFleetSettings.MaxJumpDistance >= 0 ? editorFleetSettings.MaxJumpDistance : 99,
                            PreferCloak = editorFleetSettings.PreferCloak,
                            PreferToDock = editorFleetSettings.PreferToDock,
                            TargetInterceptionLowerDistance = editorFleetSettings.TargetInterceptionLowerDistance,
                            TargetInterceptionUpperDistance = editorFleetSettings.TargetInterceptionUpperDistance,
                        };
                    }

                    // Orders
                    var editorFleetOrders = editorFleet.GetComponentsInChildren<EditorFleetOrderBase>();
                    foreach (var editorFleetOrder in editorFleetOrders)
                    {
                        var fleetOrder = CreateFleetOrderFromEditorFleetOrder.CreateFleetOrder(editorFleetOrder, editorSavedGame, savedGame);
                        fleet.OrdersCollection.Orders.Add(fleetOrder);
                        fleet.OrdersCollection.QueuedOrders.Add(fleetOrder);
                    }

                    savedGame.Fleets.Add(fleet);
                }
            }
        }

        private void ExportFleetSpawners()
        {
            foreach (var editorSector in editorSavedGame.GetComponentsInChildren<EditorSector>())
            {
                var modelSector = savedGame.Sectors.Single(e => e.Id == editorSector.Id);

                foreach (var editorFleetSpawner in editorSector.GetComponentsInChildren<EditorFleetSpawner>())
                {
                    var editorShipTypes = editorFleetSpawner.GetComponentsInChildren<EditorFleetSpawnerItem>();
                    if (editorShipTypes.Length == 0)
                    {
                        Debug.LogWarning("Fleet spawner has no items", editorFleetSpawner);
                        return;
                    }

                    var fleetSpawner = new ModelFleetSpawner
                    {
                        AllowRespawnInActiveScene = editorFleetSpawner.AllowRespawnInActiveScene,
                        FleetHomeBase = savedGame.Units.FirstOrDefault(e => e.Id == editorFleetSpawner.FleetHomeBase?.Id),
                        Position = (editorFleetSpawner.transform.position - editorSector.transform.position).ToVec3_ZeroY(),
                        FleetResourceName = editorFleetSpawner.FleetType.ToString(),
                        PilotResourceNames = new string[] { editorFleetSpawner.PilotType.ToString() }.ToList(),
                        MinGroupUnitCount = editorFleetSpawner.MinShipCount,
                        MaxGroupUnitCount = editorFleetSpawner.MaxShipCount,
                        MinTimeBeforeSpawn = editorFleetSpawner.MinTimeBeforeSpawn,
                        MaxTimeBeforeSpawn = editorFleetSpawner.MaxTimeBeforeSpawn,
                        OwnerFaction = savedGame.Factions.FirstOrDefault(e => e.Id == editorFleetSpawner.OwnerFaction?.Id),
                        UnitClasses = editorShipTypes.Select(e => e.UnitClass).ToList(),
                        NextSpawnTime = editorFleetSpawner.NextSpawnTime,
                        SpawnTimeRandomness = editorFleetSpawner.SpawnTimeRandomness,
                        RespawnWhenNoObjectives = editorFleetSpawner.RespawnWhenNoObjectives,
                        RespawnWhenNoPilots = editorFleetSpawner.RespawnWhenNoPilots,
                        Sector = modelSector,
                    };

                    var editorParentUnit = editorFleetSpawner.GetComponentInParent<EditorUnit>();
                    if (editorParentUnit != null)
                    {
                        fleetSpawner.SpawnDock = savedGame.Units.FirstOrDefault(e => e.Id == editorParentUnit.Id);
                    }

                    fleetSpawner.FleetHomeSector = fleetSpawner?.FleetHomeBase?.Sector;
                    savedGame.FleetSpawners.Add(fleetSpawner);
                }
            }
        }

        /// <summary>
        /// Npc factions rely on the faction intel database. Without it they will have a hard time navigating<br />
        /// This could be built up in the editor. But because I am lazy, just have all factions discover eachother.
        /// </summary>
        /// <param name="editorSavedGame"></param>
        /// <param name="savedGame"></param>
        private void SeedFactionIntel()
        {
            // Npc factions rely on the faction intel database. Without it they will have a hard time navigating
            // This could be built up in the editor
            foreach (var faction in savedGame.Factions)
            {
                if (faction.Intel == null)
                {
                    faction.Intel = new ModelFactionIntel();
                }

                foreach (var unit in savedGame.Units)
                {
                    if (unit.IsStation())
                    {
                        faction.Intel.Units.Add(unit);
                    }
                }
            }
        }

        private void ExportWormholes()
        {
            foreach (var editorSector in editorSavedGame.GetComponentsInChildren<EditorSector>())
            {
                foreach (var editorWormholeData in editorSector.GetComponentsInChildren<EditorUnitWormholeData>())
                {
                    var editorUnit = editorWormholeData.GetComponentInParent<EditorUnit>();
                    if (editorUnit == null)
                    {
                        LogAndThrow("Wormhole must be child of a unit", editorWormholeData);
                    }

                    var unit = savedGame.Units.FirstOrDefault(e => e.Id == editorUnit.Id);
                    if (unit == null)
                    {
                        LogAndThrow("Unit already exist in the saved game", editorWormholeData);
                    }

                    unit.WormholeData = new ModelUnitWormholeData
                    {
                        IsUnstable = editorWormholeData.IsUnstable,
                        UnstableNextChangeTargetTime = editorWormholeData.UnstableNextChangeTargetTime,
                    };

                    if (editorWormholeData.IsUnstable)
                    {
                        if (editorWormholeData.UnstableTarget != null)
                        {
                            var targetSector = editorWormholeData.UnstableTarget.GetComponentInParent<EditorSector>();
                            if (targetSector == null)
                            {
                                LogAndThrow("Wormhole has a target transform that isn't a child of a sector", editorWormholeData);
                            }

                            unit.WormholeData.UnstableTargetSector = GetModelSector(targetSector);
                            unit.WormholeData.UnstableTargetRotation = editorWormholeData.UnstableTarget.rotation.ToVec3();

                            var targetSectorPosition = (editorWormholeData.UnstableTarget.position - targetSector.transform.position).ToVec3();
                            targetSectorPosition.Y = 0.0f;
                            unit.WormholeData.UnstableTargetPosition = targetSectorPosition;
                        }
                        else
                        {
                            Debug.LogWarning("Unstable wormhole should have a target", editorWormholeData);
                        }
                    }
                    else
                    {
                        if (editorWormholeData.TargetWormholeUnit == null)
                        {
                            Debug.LogWarning("Wormhole should have a target", editorWormholeData);
                        }
                        else
                        {
                            ModelUnit targetUnit = null;
                            if (editorWormholeData.TargetWormholeUnit != null)
                            {
                                targetUnit = savedGame.Units.FirstOrDefault(e =>
                                    e.Id == editorWormholeData.TargetWormholeUnit.Id);
                            }

                            unit.WormholeData.TargetWormholeUnit = targetUnit;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Used to set up the universe map
        /// </summary>
        /// <param name="editorSavedGame"></param>
        /// <param name="savedGame"></param>
        private void SetSectorMapPositions()
        {
            // TODO: 
            foreach (var editorSector in editorSavedGame.GetComponentsInChildren<EditorSector>())
            {
                var multiplier = 0.02f;
                var sector = savedGame.Sectors.Single(e => e.Id == editorSector.Id);
                sector.MapPosition = new Vec3
                {
                    X = editorSector.transform.position.x * multiplier,
                    Z = editorSector.transform.position.z * multiplier
                };
            }
        }

        private void ExportScenarioData()
        {
            savedGame.ScenarioData = new ModelScenarioData
            {

                HasRandomEvents = editorSavedGame.RandomEventsEnabled,
                NextRandomEventTime = 240d
            };
        }

        private void ExportHeader()
        {
            savedGame.Header = new ModelHeader
            {
                Credits = savedGame.Player?.Faction?.Credits ?? 0,
                GlobalSaveNumber = 1,
                SaveNumber = 1,
                HavePlayer = savedGame.Player != null,
                IsAutoSave = false,
                PlayerName = savedGame.Player?.Person?.CustomName,
                PlayerSectorName = savedGame.Player?.Person?.CurrentUnit?.Sector?.Name ?? null,
                ScenarioInfoId = 100000, // Don't change this
                TimeStamp = System.DateTime.Now,
                Version = new System.Version(2, 0, 50), // Should be tied to the binary writer package being used
                CreatedVersion = new System.Version(2, 0, 50),
                ScenarioTitle = editorSavedGame.Title,
                ScenarioAuthor = editorSavedGame.Author,
                ScenarioAuthoringTool = $"IP2 Unity Editor {Application.version}",
                ScenarioDescription = editorSavedGame.Description,
                GameStartDate = new System.DateTime(editorSavedGame.DateYear, editorSavedGame.DateMonth, editorSavedGame.DateDay, editorSavedGame.DateHour, editorSavedGame.DateMinute, 0),
            };
        }

        private void ExportPlayer()
        {
            var editorPlayers = editorSavedGame.GetComponentsInChildren<EditorPlayer>();
            if (editorPlayers.Count() > 1)
            {
                throw new System.Exception("More than one player object found");
            }

            var editorPlayer = editorPlayers.SingleOrDefault();

            if (editorPlayer == null)
            {
                throw new System.Exception("A player object is required");
            }

            var modelPlayer = new ModelPlayer
            {
                Person = savedGame.People.FirstOrDefault(e => e.Id == editorPlayer.Person?.Id),
            };

            if (modelPlayer.Person == null)
            {
                LogAndThrow("A player object is missing a person reference", editorPlayer);
            }

            if (modelPlayer.Faction == null)
            {
                LogAndThrow("A player object's person object must have a faction", editorPlayer.Person);
            }

            this.modelPlayer = modelPlayer;
            this.player = editorPlayer;
            this.playerFaction = editorPlayer.Person.Faction;
            this.playerModelFaction = GetModelFaction(editorPlayer.Person.Faction);

            if (string.IsNullOrWhiteSpace(editorPlayer.Person.CustomName))
            {
                Debug.LogWarning("Player person should have a name", editorPlayer.Person);
            }

            var editorMessages = editorPlayer.GetComponentsInChildren<EditorPlayerMessage>();
            foreach (var editorMessage in editorMessages)
            {
                var message = new ModelPlayerMessage
                {
                    AllowDelete = editorMessage.AllowDelete,
                    EngineTimeStamp = editorMessage.EngineTimeStamp,
                    FromText = editorMessage.FromText,
                    Id = editorMessage.Id,
                    MessageTemplateId = editorMessage.MessageTemplateId,
                    MessageText = editorMessage.MessageText,
                    Opened = editorMessage.Opened,
                    SenderUnit = savedGame.Units.FirstOrDefault(e => e.Id == editorMessage.SenderUnit?.Id),
                    SubjectUnit = savedGame.Units.FirstOrDefault(e => e.Id == editorMessage.SubjectUnit?.Id),
                    SubjectText = editorMessage.SubjectText,
                    ToText = editorMessage.ToText,
                };

                if (editorMessage.ShowTime >= 0.0f)
                {
                    modelPlayer.DelayedMessages.Add(new ModelPlayerDelayedMessage
                    {
                        Message = message,
                        ShowTime = editorMessage.ShowTime,
                        Important = editorMessage.Important,
                        Notifications = editorMessage.Notifications,
                    });
                }
                else
                {
                    modelPlayer.Messages.Add(message);
                }
            }

            savedGame.Player = modelPlayer;
        }

        private void LogAndThrow(string error, UnityEngine.Object context)
        {
            Debug.LogError(error, context);
            throw new System.Exception("A critical error occured while exporting and the operation cannot continue.");
        }
        private void ExportPeople()
        {
            foreach (var editorPerson in editorSavedGame.GetComponentsInChildren<EditorPerson>())
            {
                var modelPerson = new ModelPerson
                {
                    IsMale = editorPerson.IsMale,
                    Id = editorPerson.Id,
                    CustomName = editorPerson.CustomName,
                    Faction = savedGame.Factions.FirstOrDefault(e => e.Id == editorPerson.Faction?.Id),
                    Kills = editorPerson.Kills,
                };

                if (string.IsNullOrEmpty(modelPerson.CustomName))
                {
                    modelPerson.GeneratedFirstNameId = -1;
                    modelPerson.GeneratedLastNameId = -1;
                }

                // Link any pilot
                var editorUnit = editorPerson.GetComponentInParent<EditorUnit>();
                if (editorUnit != null)
                {
                    modelPerson.CurrentUnit = savedGame.Units.FirstOrDefault(e => e.Id == editorUnit.Id);

                    var editorComponentUnit = editorUnit.GetComponentInChildren<EditorComponentUnitData>();
                    if (editorComponentUnit != null)
                    {
                        modelPerson.CurrentUnit.ComponentUnitData.People.Add(modelPerson);
                    }

                    // For reasons of laziness, if a pilot is in a ship, just set it to be the pilot and not require it to be linked.
                    const bool autoPilot = true;

                    if (editorComponentUnit != null && (editorComponentUnit.Pilot == editorPerson || autoPilot))
                    {
                        if (modelPerson.Faction == null)
                        {
                            LogAndThrow("Pilot must always have a faction", editorPerson);
                        }

                        modelPerson.IsPilot = true;

                        if (modelPerson.CurrentUnit.Faction != modelPerson.Faction)
                        {
                            Debug.LogWarning("Person set to be a pilot of a unit that has a different faction. Unit faction will be changed to match pilot", editorPerson);
                            modelPerson.CurrentUnit.Faction = modelPerson.Faction;
                        }
                    }

                }

                // Init any Npc pilot
                var editorNpcPilot = editorPerson.GetComponent<EditorNpcPilot>();
                if (editorNpcPilot != null)
                {
                    modelPerson.NpcPilot = new ModelNpcPilot
                    {
                        DestroyWhenNotPilotting = true,
                        DestroyWhenNoUnit = true,
                        Person = modelPerson
                    };

                    // Find fleet
                    var editorFleet = editorNpcPilot.GetComponentInParent<EditorFleet>();
                    if (editorFleet != null)
                    {
                        var fleet = savedGame.Fleets.FirstOrDefault(e => e.Id == editorFleet.Id);
                        if (fleet == null)
                        {
                            LogAndThrow("Expecting to find a fleet", editorPerson);
                        }

                        if (fleet.Faction != modelPerson.Faction)
                        {
                            LogAndThrow("Fleet has a different faction to pilot. This is not supported", editorPerson);
                        }

                        modelPerson.NpcPilot.Fleet = fleet;
                        modelPerson.NpcPilot.Fleet.Npcs.Add(modelPerson.NpcPilot);
                    }

                    // Custom settings 
                    var editorNpcPilotSettings = editorNpcPilot.GetComponentInChildren<EditorNpcPilotSettings>();
                    if (editorNpcPilotSettings != null)
                    {
                        modelPerson.NpcPilotSettings = new ModelNpcPilotSettings
                        {
                            AllowDitchShip = editorNpcPilotSettings.AllowDitchShip,
                            CheatAmmo = editorNpcPilotSettings.CheatAmmo,
                            CombatEfficiency = editorNpcPilotSettings.CombatEfficiency,
                            RestrictedWeaponPreference = editorNpcPilotSettings.RestrictedWeaponPreference,
                        };
                    }
                }

                savedGame.People.Add(modelPerson);
            }
        }

        /// <summary>
        /// There's a bug in the engine where, if the npc pilot doesn't have a fleet, the faction won't create one for it. So the npc will be left idle
        /// So create one automatically here
        /// </summary>
        /// <param name="editorSavedGame"></param>
        /// <param name="savedGame"></param>
        private void AutoCreateFleetsWhereNeeded()
        {
            foreach (var person in savedGame.People)
            {
                if (person.IsPilot && person.NpcPilot != null && person.NpcPilot.Fleet == null)
                {
                    person.NpcPilot.Fleet = new ModelFleet
                    {
                        Faction = person.Faction,
                        Id = savedGame.Fleets.Select(e => e.Id).DefaultIfEmpty().Max() + 1,
                        Position = person.CurrentUnit.Position,
                        Sector = person.CurrentUnit.Sector,
                        IsActive = true, // Again, should be autoset by model
                    };

                    // Older game version have trouble loading when this is null
                    person.NpcPilot.Fleet.FleetSettings = new ModelFleetSettings();

                    savedGame.Fleets.Add(person.NpcPilot.Fleet);
                    person.NpcPilot.Fleet.Npcs.Add(person.NpcPilot);
                }
            }
        }

        private void ExportUnits()
        {
            foreach (var editorSector in editorSavedGame.GetComponentsInChildren<EditorSector>())
            {
                foreach (var editorUnit in editorSector.GetComponentsInChildren<EditorUnit>())
                {
                    var unit = new ModelUnit
                    {
                        Id = editorUnit.Id,
                        Class = editorUnit.ModelUnitClass,
                        Faction = savedGame.Factions.FirstOrDefault(e => e.Id == editorUnit.Faction?.Id),
                        RpProvision = editorUnit.RpProvision,
                        Name = editorUnit.Name, // Set the unit name here but it may later be overriden
                        Sector = savedGame.Sectors.FirstOrDefault(e => e.Id == editorSector.Id),
                        // Radius is only relevant to some units like gas clouds and asteroid clusters
                        Radius = editorUnit.Radius >= 0.0f ? editorUnit.Radius : null,
                    };

                    unit.Rotation = editorUnit.transform.localRotation.ToVec3();
                    if (unit.Sector != null)
                    {
                        // Get position local to sector (allowing nested units in the heirachy)
                        var localSectorPosition = editorUnit.transform.position - editorSector.transform.position;

                        // Constrain Y
                        if (unit.IsShipOrStation())
                        {
                            localSectorPosition.y = 0.0f;
                        }

                        unit.Position = localSectorPosition.ToVec3();
                    };

                    ExportComponentData(editorUnit, unit);

                    ExportCargoContainerData(editorUnit, unit);

                    savedGame.Units.Add(unit);
                }
            }
        }

        /// <summary>
        /// Applies if the unit is a cargo container
        /// </summary>
        /// <param name="editorUnit"></param>
        /// <param name="unit"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ExportCargoContainerData(EditorUnit editorUnit, ModelUnit unit)
        {
            var editorCargoContainerData = editorUnit.GetComponentInChildren<EditorUnitCargoData>();
            if (editorCargoContainerData != null)
            {
                if (unit.Class != ModelUnitClass.Cargo_Container &&
                    unit.Class != ModelUnitClass.Cargo_Ice &&
                    unit.Class != ModelUnitClass.Cargo_Rock)
                {
                    LogAndThrow("This type of unit does not support being a cargo container", editorCargoContainerData);
                }

                if (!System.Enum.IsDefined(typeof(ModelCargoClass), editorCargoContainerData.ModelCargoClass))
                {
                    LogAndThrow($"Found unknown cargo type on {editorCargoContainerData.name}: {editorCargoContainerData.ModelCargoClass}", editorCargoContainerData);
                }

                if (editorCargoContainerData.Quantity < 0)
                {
                    LogAndThrow($"Invalid cargo quantity on {editorCargoContainerData.name}: {editorCargoContainerData.Quantity}", editorCargoContainerData);
                }

                unit.CargoData = new ModelUnitCargoData
                {
                    CargoClass = editorCargoContainerData.ModelCargoClass,
                    Expires = editorCargoContainerData.Expires,
                    SpawnTime = editorCargoContainerData.SpawnTime,
                    Quantity = editorCargoContainerData.Quantity
                };
            }
        }

        /// <summary>
        /// Applies to ships and stations
        /// </summary>
        /// <param name="editorUnit"></param>
        /// <param name="unit"></param>
        private void ExportComponentData(EditorUnit editorUnit, ModelUnit unit)
        {
            var editorComponentData = editorUnit.GetComponentInChildren<EditorComponentUnitData>();
            if (editorComponentData != null)
            {
                unit.ComponentUnitData = new ModelComponentUnitData
                {
                    ShipNameIndex = -1,
                    CapacitorCharge = editorComponentData.CapacitorCharge >= 0.0f ? editorComponentData.CapacitorCharge : null,
                    ConstructionState = editorComponentData.ConstructionState,
                    ConstructionProgress = editorComponentData.ConstructionProgress,
                    IsCloaked = editorComponentData.IsCloaked,
                    EngineThrottle = editorComponentData.EngineThrottle >= 0.0f ? editorComponentData.EngineThrottle : null,
                };

                ExportComponentUnitCargo(unit, editorComponentData);
                ExportComponentUnitMods(unit, editorComponentData);

                if (unit.IsShip())
                {
                    unit.ComponentUnitData.CustomShipName = unit.Name;
                    if (!string.IsNullOrWhiteSpace(unit.Name))
                    {
                        unit.Name = null;
                    }
                }
            }
        }

        private void ExportComponentUnitMods(ModelUnit unit, EditorComponentUnitData editorComponentData)
        {
            var editorComponentBayMods = editorComponentData.GetComponentsInChildren<EditorComponentBayMod>();
            if (editorComponentBayMods.Length > 0)
            {
                unit.ComponentUnitData.ModData = new ModelComponentUnitModData();
                foreach (var editorComponentBayMod in editorComponentBayMods)
                {
                    if (!System.Enum.IsDefined(typeof(ModelComponentClass), editorComponentBayMod.LegacyComponentClass))
                    {
                        LogAndThrow("Unknown component type", editorComponentBayMod);
                    }

                    unit.ComponentUnitData.ModData.Items.Add(new ModelComponentUnitModDataItem
                    {
                        BayId = editorComponentBayMod.LegacyBayId,
                        ComponentClass = editorComponentBayMod.LegacyComponentClass
                    });
                }
            }
        }

        private void ExportComponentUnitCargo(ModelUnit unit, EditorComponentUnitData editorComponentData)
        {
            var editorCargoDataItmes = editorComponentData.GetComponentsInChildren<EditorComponentUnitCargoDataItem>();
            if (editorCargoDataItmes.Length > 0)
            {
                unit.ComponentUnitData.CargoData = new ModelComponentUnitCargoData();
                foreach (var item in editorCargoDataItmes)
                {
                    if (!System.Enum.IsDefined(typeof(ModelCargoClass), item.CargoClass))
                    {
                        LogAndThrow($"Unknown cargo type found on {editorComponentData.name}: {item.CargoClass}", item);
                    }

                    if (item.Quantity < 0)
                    {
                        LogAndThrow("Invalid cargo quantity", item);
                    }

                    if (item.Quantity > 0)
                    {
                        unit.ComponentUnitData.CargoData.Items.Add(new ModelComponentUnitCargoDataItem
                        {
                            CargoClass = item.CargoClass,
                            Quantity = item.Quantity
                        });
                    }
                }
            }
        }

        private void ExportSectors()
        {
            foreach (var editorSector in editorSavedGame.GetComponentsInChildren<EditorSector>())
            {
                var modelSector = new ModelSector
                {
                    Id = editorSector.Id,
                    Name = editorSector.Name,
                    Description = editorSector.Description,
                    GateDistanceMultiplier = editorSector.WormholeDistanceMultiplier,
                    RandomSeed = editorSector.RandomSeed,
                    BackgroundRotation = editorSector.BackgroundRotation.ToVec3(),
                    DirectionLightRotation = editorSector.DirectionLightRotation.ToVec3(),
                    DirectionLightColor = editorSector.DirectionLightColor.ToVec3(),
                    AmbientLightColor = editorSector.AmbientLightColor.ToVec3(),
                    LightDirectionFudge = editorSector.LightDirectionFudge,
                };

                var editorSectorSky = editorSector.GetComponentInChildren<EditorSectorSky>();
                if (editorSectorSky != null)
                {
                    modelSector.CustomAppearance = new ModelSectorAppearance
                    {
                        NebulaColors = editorSectorSky.NebulaColors,
                        NebulaCount = editorSectorSky.NebulaCount,
                        StarsIntensity = editorSectorSky.StarsIntensity
                    };
                }

                savedGame.Sectors.Add(modelSector);
            }
        }

        private void ExportFactions()
        {
            var factions = editorSavedGame.GetComponentsInChildren<EditorFaction>();
            foreach (var editorFaction in factions)
            {
                var modelFaction = new ModelFaction
                {
                    Id = editorFaction.Id,
                    CustomName = editorFaction.CustomName,
                    CustomShortName = editorFaction.CustomShortName,
                    Credits = editorFaction.Credits,
                    Description = editorFaction.Description,
                    IsCivilian = editorFaction.IsCivilian,
                    FactionType = editorFaction.FactionType,
                    Aggression = editorFaction.Aggression,
                    Virtue = editorFaction.Virtue,
                    Greed = editorFaction.Greed,
                    TradeEfficiency = editorFaction.TradeEfficiency,
                    DynamicRelations = editorFaction.DynamicRelations,
                    ShowJobBoards = editorFaction.ShowJobBoards,
                    CreateJobs = editorFaction.CreateJobs,
                    RequisitionPointMultiplier = editorFaction.RequisitionPointMultiplier,
                    MinNpcCombatEfficiency = editorFaction.MinNpcCombatEfficiency,
                    MaxNpcCombatEfficiency = editorFaction.MaxNpcCombatEfficiency,
                    AdditionalRpProvision = editorFaction.AdditionalRpProvision,
                    TradeIllegalGoods = editorFaction.TradeIllegalGoods,
                };

                var editorFactionSettings = editorFaction.GetComponentInChildren<EditorFactionCustomSettings>();
                if (editorFactionSettings != null)
                {
                    modelFaction.CustomSettings = new ModelFactionCustomSettings
                    {
                        AllowOtherFactionToUseDocks = editorFactionSettings.AllowOtherFactionToUseDocks,
                        DailyIncome = editorFactionSettings.DailyIncome,
                        HostileWithAll = editorFactionSettings.HostileWithAll,
                        IgnoreStationCreditsReserve = editorFactionSettings.IgnoreStationCreditsReserve,
                        LargeShipPreference = editorFactionSettings.LargeShipPreference,
                        MinFleetUnitCount = editorFactionSettings.MinFleetUnitCount,
                        MaxFleetUnitCount = editorFactionSettings.MaxFleetUnitCount,
                        OffensiveStance = editorFactionSettings.OffensiveStance,
                        PreferenceToBuildStations = editorFactionSettings.PreferenceToBuildStations,
                        PreferenceToBuildTurrets = editorFactionSettings.PreferenceToBuildTurrets,
                        PreferenceToPlaceBounty = editorFactionSettings.PreferenceToPlaceBounty,
                        PreferSingleShip = editorFactionSettings.PreferSingleShip,
                        RepairMinCreditsBeforeRepair = editorFactionSettings.RepairMinCreditsBeforeRepair,
                        RepairMinHullDamage = editorFactionSettings.RepairMinHullDamage,
                        RepairShips = editorFactionSettings.RepairShips,
                        UpgradeShips = editorFactionSettings.UpgradeShips,
                    };
                }

                savedGame.Factions.Add(modelFaction);
            }
        }

        private void ExportFactionRelations()
        {
            var editorFactions = editorSavedGame.GetComponentsInChildren<EditorFaction>();
            foreach (var editorFaction in editorFactions)
            {
                var editorFactionRelations = editorFaction.GetComponentsInChildren<EditorFactionRelation>();
                foreach (var editorFactionRelation in editorFactionRelations)
                {
                    var faction = savedGame.Factions.Single(e => e.Id == editorFaction.Id);

                    if (faction.Relations == null)
                    {
                        faction.Relations = new ModelFactionRelationData();
                    }

                    if (faction.Opinions == null)
                    {
                        faction.Opinions = new ModelFactionOpinionData();
                    }

                    if (editorFactionRelation.OtherFaction == null)
                    {
                        LogAndThrow("Faction relation missing other faction", editorFactionRelation.OtherFaction);
                    }

                    if (editorFactionRelation.OtherFaction == editorFaction)
                    {
                        LogAndThrow("Faction relation points to same faction", editorFactionRelation);
                    }

                    ApplyFactionRelation(savedGame, editorFactionRelation, faction);
                }
            }
        }

        private void ApplyFactionRelation(SavedGame savedGame, EditorFactionRelation editorFactionRelation, ModelFaction faction)
        {
            var otherFaction = savedGame.Factions.Single(e => e.Id == editorFactionRelation.OtherFaction.Id);

            var factionOpinionDataItem = new ModelFactionOpinionDataItem
            {
                Opinion = editorFactionRelation.Opinion,
                OtherFaction = otherFaction
            };

            faction.Opinions.Items.Add(factionOpinionDataItem);

            var factionRelationDataItem = new ModelFactionRelationDataItem
            {
                HostilityEndTime = editorFactionRelation.HostilityEndTime,
                Neutrality = editorFactionRelation.Neutrality,
                OtherFaction = otherFaction,
                PermanentPeace = editorFactionRelation.PermanentPeace,
                RestrictHostilityTimeout = editorFactionRelation.RestrictHostilityTimeout
            };

            faction.Relations.Items.Add(factionRelationDataItem);

            // Do the same thing for the opposing faction if necessary
            if (editorFactionRelation.ApplyTwoWay)
            {
                var otherFactionOpinionDataItem = new ModelFactionOpinionDataItem
                {
                    Opinion = editorFactionRelation.Opinion,
                    OtherFaction = faction
                };

                otherFaction.Opinions.Items.Add(otherFactionOpinionDataItem);

                var otherFactionRelationDataItem = new ModelFactionRelationDataItem
                {
                    HostilityEndTime = editorFactionRelation.HostilityEndTime,
                    Neutrality = editorFactionRelation.Neutrality,
                    OtherFaction = faction,
                    PermanentPeace = editorFactionRelation.PermanentPeace,
                    RestrictHostilityTimeout = editorFactionRelation.RestrictHostilityTimeout
                };

                otherFaction.Relations.Items.Add(otherFactionRelationDataItem);
            }
        }
    }
}
