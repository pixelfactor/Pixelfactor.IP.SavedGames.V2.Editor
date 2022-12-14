using Pixelfactor.IP.SavedGames.V2.Model.Helpers;
using Pixelfactor.IP.SavedGames.V2.BinarySerialization.Readers.Helpers;
using Pixelfactor.IP.SavedGames.V2.Model;
using Pixelfactor.IP.SavedGames.V2.Model.Factions;
using Pixelfactor.IP.SavedGames.V2.Model.Factions.Bounty;
using Pixelfactor.IP.SavedGames.V2.Model.Jobs;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Pixelfactor.IP.Common;
using Pixelfactor.IP.Common.Factions;
using Pixelfactor.IP.Common.FleetOrders;
using Pixelfactor.IP.SavedGames.V2.Model.Jobs.Missions;
using Pixelfactor.IP.SavedGames.V2.Model.FleetOrders;
using Pixelfactor.IP.SavedGames.V2.Model.FleetOrders.OrderTypes;
using Pixelfactor.IP.SavedGames.V2.Model.FleetOrders.Models;

namespace Pixelfactor.IP.SavedGames.V2.BinarySerialization.Readers
{
    public class SaveGameReader1721 : ISaveGameReader
    {
        private readonly HeaderReader2019 headerReader = new HeaderReader2019();

        private Dictionary<int, ModelUnit> unitsById = new Dictionary<int, ModelUnit>(512);
        private Dictionary<int, ModelSector> sectorsById = new Dictionary<int, ModelSector>(64);
        private Dictionary<int, ModelFaction> factionsById = new Dictionary<int, ModelFaction>(512);
        private Dictionary<int, ModelSectorPatrolPath> patrolPathsById = new Dictionary<int, ModelSectorPatrolPath>(512);
        private Dictionary<int, ModelPerson> peopleById = new Dictionary<int, ModelPerson>(512);
        private Dictionary<int, ModelPassengerGroup> passengerGroupsById = new Dictionary<int, ModelPassengerGroup>(512);
        private Dictionary<int, ModelFleet> fleetsById = new Dictionary<int, ModelFleet>(512);
        private Dictionary<int, ModelMission> missionsById = new Dictionary<int, ModelMission>(8);

        public static ISavedGame ReadFromPath(string path)
        {
            using (var reader = new BinaryReader(File.OpenRead(path)))
            {
                var savedGameReader = new SaveGameReader();
                return savedGameReader.Read(reader);
            }
        }

        public ISavedGame Read(BinaryReader reader)
        {
            var savedGame = new SavedGame();

            savedGame.Header = (ModelHeader)this.headerReader.Read(reader);
            PrintStatus("Loaded header", reader);

            reader.ReadString(); // Heading
            savedGame.Sectors.AddRange(ReadSectors(reader));
            foreach (var sector in savedGame.Sectors)
            {
                this.sectorsById.Add(sector.Id, sector);
            }
            PrintStatus("Loaded sectors", reader);

            reader.ReadString(); // Heading
            savedGame.Factions.AddRange(ReadFactions(reader));
            foreach (var faction in savedGame.Factions)
            {
                this.factionsById.Add(faction.Id, faction);
            }

            PrintStatus("Loaded factions", reader);

            reader.ReadString(); // Heading
            ReadAllFactionAvatarProfileIds(reader);
            PrintStatus("Loaded faction avatar profile Ids", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            savedGame.PatrolPaths.AddRange(ReadPatrolPaths(reader));
            foreach (var patrolPath in savedGame.PatrolPaths)
            {
                this.patrolPathsById.Add(patrolPath.Id, patrolPath);
            }
            PrintStatus("Loaded patrol paths", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadAllFactionRelations(reader);
            PrintStatus("Loaded faction relations", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadAllFactionRecentDamageReceived(reader);
            PrintStatus("Loaded faction recent damage", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadAllFactionOpinions(reader);
            PrintStatus("Loaded faction opinions", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadUnits(reader, savedGame);
            foreach (var unit in savedGame.Units)
            {
                this.unitsById.Add(unit.Id, unit);
            }
            PrintStatus("Loaded units", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadUnitRadii(reader);
            PrintStatus("Loaded unit radii", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadNamedUnits(reader);
            PrintStatus("Loaded named units", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadAllComponentUnits(reader);
            PrintStatus("Loaded all unit components", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadUnitsUnderConstruction(reader);
            PrintStatus("Loaded units under construction", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadUnitTotalDamageReceived(reader);
            PrintStatus("Loaded units under construction", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadModdedComponents(reader);
            PrintStatus("Loaded modded components", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadUnitCapacitorCharges(reader);
            PrintStatus("Loaded unit capacitor charges", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadCloakedUnits(reader);
            PrintStatus("Loaded unit cloak states", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadPoweredDownComponents(reader);
            PrintStatus("Loaded powered down units", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadUnitEngineThrottles(reader);
            PrintStatus("Loaded engine throttle data", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadComponentUnitCargo(reader);
            PrintStatus("Loaded cargo", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadAllShieldHealthData(reader);
            PrintStatus("Loaded damaged shields", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadAllUnitComponentHealthData(reader);
            PrintStatus("Loaded damaged components", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadActiveUnits(reader);
            PrintStatus("Loaded active units", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadAllUnitHealthDatas(reader);
            PrintStatus("Loaded destructable units", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadAllFactionIntel(reader);
            PrintStatus("Loaded faction intel", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadPassengerGroups(reader);
            PrintStatus("Loaded passenger groups", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadWormholes(reader);
            PrintStatus("Loaded wormholes", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadHangars(reader);
            PrintStatus("Loaded hangers", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            savedGame.Fleets.AddRange(ReadFleets(reader));
            foreach (var fleet in savedGame.Fleets)
            {
                this.fleetsById.Add(fleet.Id, fleet);
            }

            PrintStatus("Loaded fleets", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadNamedFleets(reader);

            // ----------------------------
            reader.ReadString(); // Heading
            var people = ReadPeople(reader, out ModelPerson playerPerson);
            savedGame.People.AddRange(people);
            foreach (var person in people)
            {
                this.peopleById.Add(person.Id, person);
            }

            PrintStatus("Loaded people", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadFleetOrders(reader);
            PrintStatus("Loaded fleet orders", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadNpcPilots(reader);
            PrintStatus("Loaded NPC pilots", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadFactionLeaders(reader);
            PrintStatus("Loaded faction leaders", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadJobs(reader);
            PrintStatus("Loaded jobs", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadAllFactionAIsAndBountyBoards(reader);
            PrintStatus("Loaded faction AIs / bounty boards", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadFactionAIExcludedUnits(reader);
            PrintStatus("Loaded faction excluded unit data", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadFactionMercenaryData(reader);
            PrintStatus("Loaded mercenary data", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            savedGame.FleetSpawners.AddRange(ReadFleetSpawners(reader));
            PrintStatus("Loaded NPC fleet spawners", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            savedGame.Missions.AddRange(ReadMissions(reader));
            foreach (var mission in savedGame.Missions)
            {
                this.missionsById.Add(mission.Id, mission);
            }
            PrintStatus("Loaded jobs", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            var havePlayer = reader.ReadBoolean();
            if (havePlayer)
            {
                savedGame.Player = ReadGamePlayer(reader);

                savedGame.Player.Person = playerPerson;

                PrintStatus("Loaded player data", reader);
            }

            // ----------------------------
            reader.ReadString(); // Heading
            savedGame.CurrentHudTarget = reader.ReadUnit(this.unitsById);
            PrintStatus("Loaded hud data", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadAllFactionTransactions(reader);
            PrintStatus("Loaded all faction transactions", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            savedGame.ScenarioData = ReadScenarioData(reader);
            PrintStatus("Loaded world", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            savedGame.Moons.AddRange(ReadMoons(reader));
            PrintStatus("Loaded moons", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            savedGame.SeedOptions = ReadSeedOptions(reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadAutoTurretModuleData(reader);
            PrintStatus("Loaded auto-turret module data", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadAutoFireComponents(reader);
            PrintStatus("Loaded auto-fire component data", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadUnitCaptureCooldownTimes(reader);
            PrintStatus("Loaded unit capture cooldown times", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadPlayerFleetSettings(reader);
            PrintStatus("Loaded player fleet settings", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadCustomSectorAppearances(reader);
            PrintStatus("Loaded custom sector appearances", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadUnitMass(reader);
            PrintStatus("Loaded unit mass", reader);

            // ----------------------------
            reader.ReadString(); // Heading
            ReadUnitCargoCapacity(reader);
            PrintStatus("Loaded unit cargo capacity", reader);

            return savedGame;
        }

        private void ReadUnitCargoCapacity(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var unit = reader.ReadUnit(this.unitsById);
                var capacity = reader.ReadSingle();
                if (unit != null && unit.ComponentUnitData != null)
                {
                    unit.ComponentUnitData.CargoCapacity = capacity;
                }
            }
        }

        private void ReadUnitMass(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var unit = reader.ReadUnit(this.unitsById);
                var mass = reader.ReadSingle();
                if (unit != null)
                {
                    unit.Mass = mass;
                }
            }
        }

        private void ReadUnitsUnderConstruction(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var unitId = reader.ReadInt32();
                var constructionState = (ConstructionState)reader.ReadByte();
                var constructionProgress = reader.ReadSingle();

                var unit = this.unitsById.GetValueOrDefault(unitId);
                if (unit != null)
                {
                    unit.ComponentUnitData.ConstructionState = constructionState;
                    unit.ComponentUnitData.ConstructionProgress = constructionProgress;
                }
            }
        }

        private void ReadUnitTotalDamageReceived(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var unit = reader.ReadUnit(this.unitsById);
                var damageReceived = reader.ReadSingle();

                if (unit != null)
                {
                    unit.TotalDamagedReceived = damageReceived;
                }
            }
        }

        private void ReadCustomSectorAppearances(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var sector = reader.ReadSector(sectorsById);
                var modelSectorAppearance = ReadCustomSectorAppearance(reader);

                if (sector != null)
                {
                    sector.CustomAppearance = modelSectorAppearance;
                }
            }
        }

        private ModelSectorAppearance ReadCustomSectorAppearance(BinaryReader reader)
        {
            var customAppearance = new ModelSectorAppearance();
            customAppearance.NebulaBrightness = (NebulaBrightness)reader.ReadInt32();
            customAppearance.NebulaColors = (NebulaColour)reader.ReadInt32();
            customAppearance.NebulaComplexity = reader.ReadSingle();
            customAppearance.NebulaCount = reader.ReadInt32();
            customAppearance.NebulaTextureCount = reader.ReadInt32();
            customAppearance.NebulaStyles = (NebulaStyle)reader.ReadInt32();
            customAppearance.StarsCount = (StarsCount)reader.ReadInt32();
            customAppearance.StarsIntensity = reader.ReadSingle();

            return customAppearance;
        }

        private void ReadPlayerFleetSettings(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var fleet = reader.ReadFleet(this.fleetsById);
                var notifyOrderCompleted = reader.ReadBoolean();
                var notifyWhenScannedHostile = reader.ReadBoolean();
                var notifyWhenAbandonedUnit = reader.ReadBoolean();
                var notifyWhenAbandonedCargoFound = reader.ReadBoolean();

                if (fleet != null)
                {
                    fleet.FleetSettings.PlayerFleetSettings = new ModelPlayerFleetSettings();
                    fleet.FleetSettings.PlayerFleetSettings.NotifyWhenOrderComplete = notifyOrderCompleted;
                    fleet.FleetSettings.PlayerFleetSettings.NotifyWhenScannedHostile = notifyWhenScannedHostile;
                    fleet.FleetSettings.PlayerFleetSettings.NotifyWhenAbandonedUnitFound = notifyWhenAbandonedUnit;
                    fleet.FleetSettings.PlayerFleetSettings.NotifyWhenAbandonedCargoFound = notifyWhenAbandonedCargoFound;
                }
            }
        }

        private void ReadUnitCaptureCooldownTimes(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var unit = reader.ReadUnit(this.unitsById);
                var cooldownTime = reader.ReadDouble();
                if (unit != null && unit.ComponentUnitData != null)
                {
                    unit.ComponentUnitData.CaptureCooldownTime = cooldownTime;
                }
            }
        }

        private void ReadAutoTurretModuleData(BinaryReader reader)
        {
            var unitCount = reader.ReadInt32();
            for (int i = 0; i < unitCount; i++)
            {
                var unit = reader.ReadUnit(this.unitsById);
                var fireMode = (AutoTurretFireMode)reader.ReadInt32();

                if (unit != null && unit.ComponentUnitData != null)
                {
                    unit.ComponentUnitData.AutoTurretFireMode = fireMode;
                }
            }
        }

        private void ReadNamedFleets(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var id = reader.ReadInt32();
                var name = reader.ReadString();

                var fleet = this.fleetsById.GetValueOrDefault(id);
                if (fleet != null)
                {
                    fleet.Name = name;
                }
            }
        }

        private void ReadUnitRadii(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var unitId = reader.ReadInt32();
                var radius = reader.ReadSingle();

                var unit = this.unitsById.GetValueOrDefault(unitId);
                if (unit != null)
                {
                    unit.Radius = radius;
                }
                else
                {
                    // missing unit
                }
            }
        }

        private ModelSeedOptions ReadSeedOptions(BinaryReader reader)
        {
            var hasSeedOptions = reader.ReadBoolean();

            if (hasSeedOptions)
            {
                var seedOptions = new ModelSeedOptions();
                seedOptions.SeedAbandonedCargo = reader.ReadBoolean();
                seedOptions.SeedAbandonedShips = reader.ReadBoolean();
                seedOptions.SeedCargoHolds = reader.ReadBoolean();
                seedOptions.SeedFactionIntel = reader.ReadBoolean();
                seedOptions.SeedPassengerGroups = reader.ReadBoolean();
                return seedOptions;
            }

            return null;
        }

        /// <summary>
        /// Aka moons
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="units"></param>
        private IEnumerable<ModelMoon> ReadMoons(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var unitId = reader.ReadInt32();
                var orbitUnitId = reader.ReadInt32();
                var offsetFromPlanet = reader.ReadVec3();

                var unit = this.unitsById.GetValueOrDefault(unitId);
                var orbitUnit = this.unitsById.GetValueOrDefault(orbitUnitId);

                if (unit != null && orbitUnit != null)
                {
                    yield return new ModelMoon
                    {
                        Unit = unit,
                        OrbitUnit = orbitUnit,
                        OffsetFromPlanet = offsetFromPlanet
                    };
                }
            }
        }

        private ModelScenarioData ReadScenarioData(BinaryReader reader)
        {
            var scenarioData = new ModelScenarioData();
            scenarioData.HasRandomEvents = reader.ReadBoolean();
            if (scenarioData.HasRandomEvents)
            {
                scenarioData.NextRandomEventTime = reader.ReadDouble();
            }

            var hasFactionSpawner = reader.ReadBoolean();
            if (hasFactionSpawner)
            {
                scenarioData.FactionSpawner = new ModelFactionSpawner();
                scenarioData.FactionSpawner.NextUpdate = reader.ReadDouble();
            }

            var hasTradeRouteScenarioData = reader.ReadBoolean();
            if (hasTradeRouteScenarioData)
            {
                scenarioData.TradeRouteScenarioData = new Model.Scenarios.ModelTradeRouteScenarioData();
                scenarioData.TradeRouteScenarioData.NumBlackSailShipsDestroyed = reader.ReadInt32();
                scenarioData.TradeRouteScenarioData.PirateFaction = reader.ReadFaction(this.factionsById);
            }

            scenarioData.RespawnOnDeath = (RespawnOnDeathPreference)reader.ReadInt32();
            scenarioData.Permadeath = reader.ReadBoolean();

            return scenarioData;
        }

        private void ReadAllFactionTransactions(BinaryReader reader)
        {
            var factionCount = reader.ReadInt32();

            for (var i = 0; i < factionCount; i++)
            {
                var faction = reader.ReadFaction(this.factionsById);
                var transactionCount = reader.ReadInt32();

                for (int j = 0; j < transactionCount; j++)
                {
                    var transaction = ReadFactionTransaction(reader);
                    faction.Transactions.Add(transaction);
                }
            }
        }

        private ModelFactionTransaction ReadFactionTransaction(BinaryReader reader)
        {
            var transaction = new ModelFactionTransaction();
            transaction.TransactionType = (FactionTransactionType)reader.ReadInt32();
            transaction.Value = reader.ReadInt32();
            transaction.CurrentBalance = reader.ReadInt32();
            transaction.LocationUnit = reader.ReadUnit(this.unitsById);
            transaction.OtherFaction = reader.ReadFaction(this.factionsById);
            transaction.RelatedCargoClass = (ModelCargoClass)reader.ReadInt32();
            transaction.RelatedUnitClass = (ModelUnitClass)reader.ReadInt32();
            transaction.GameWorldTime = reader.ReadDouble();
            transaction.TaxType = (FactionTransactionTaxType)reader.ReadInt32();
            return transaction;
        }

        private ModelPlayer ReadGamePlayer(BinaryReader reader)
        {
            // Visited docks
            var player = new ModelPlayer();
            var visitedDockCount = reader.ReadInt32();
            for (var i = 0; i < visitedDockCount; i++)
            {
                var u = reader.ReadUnit(this.unitsById);
                if (u != null)
                {
                    player.VisitedUnits.Add(u);
                }
            }

            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                player.Messages.Add(ReadPlayerMessage(reader));
            }

            var delayedMessageCount = reader.ReadInt32();
            for (var i = 0; i < delayedMessageCount; i++)
            {
                var delayedMessage = new ModelPlayerDelayedMessage();
                delayedMessage.ShowTime = reader.ReadDouble();
                delayedMessage.Message = ReadPlayerMessage(reader);
                player.DelayedMessages.Add(delayedMessage);
            }

            player.CustomWaypoint = ReadPlayerWaypointIfSet(reader);

            var activeJobId = reader.ReadInt32();
            player.ActiveJob = this.missionsById.GetValueOrDefault(activeJobId);

            ReadPlayerStats(reader, player);

            return player;
        }

        private void ReadPlayerStats(BinaryReader reader, ModelPlayer player)
        {
            var visitedSectorCount = reader.ReadInt32();
            if (visitedSectorCount > 0)
            {
                if (player.Stats.SectorsVisited == null)
                {
                    player.Stats.SectorsVisited = new List<ModelSector>(64);
                }

                for (int i = 0; i < visitedSectorCount; i++)
                {
                    var sector = reader.ReadSector(this.sectorsById);
                    if (sector != null)
                    {
                        player.Stats.SectorsVisited.Add(sector);
                    }
                }
            }

            player.Stats.TotalBountyClaimed = reader.ReadInt64();
            player.Stats.ShipsMinedToDeath = reader.ReadInt32();
        }

        private ModelPlayerWaypoint ReadPlayerWaypointIfSet(BinaryReader reader)
        {
            var hasWaypoint = reader.ReadBoolean();
            if (hasWaypoint)
            {
                return ReadPlayerWaypoint(reader);
            }

            return null;
        }

        public ModelPlayerWaypoint ReadPlayerWaypoint(BinaryReader reader)
        {
            var waypoint = new ModelPlayerWaypoint();
            waypoint.SectorPosition = reader.ReadVec3();
            waypoint.Sector = reader.ReadSector(this.sectorsById);
            waypoint.TargetUnit = reader.ReadUnit(this.unitsById);
            waypoint.HadTargetObject = reader.ReadBoolean();
            return waypoint;
        }

        public ModelPlayerMessage ReadPlayerMessage(BinaryReader reader)
        {
            var message = new ModelPlayerMessage();
            message.Id = reader.ReadInt32();
            message.EngineTimeStamp = reader.ReadDouble();
            message.AllowDelete = reader.ReadBoolean();
            message.Opened = reader.ReadBoolean();

            message.SenderUnit = reader.ReadUnit(this.unitsById);
            message.SenderUnitSector = reader.ReadSector(this.sectorsById);
            message.SenderUnitSectorPosition = reader.ReadVec3();

            message.SubjectUnit = reader.ReadUnit(this.unitsById);
            message.SubjectUnitSector = reader.ReadSector(this.sectorsById);
            message.SubjectUnitSectorPosition = reader.ReadVec3();

            var hasTemplate = reader.ReadBoolean();
            if (hasTemplate)
            {
                message.MessageTemplateId = reader.ReadInt32();
            }
            else
            {
                message.MessageTemplateId = -1;
                message.ToText = reader.ReadString();
                message.FromText = reader.ReadString();
                message.MessageText = reader.ReadString();
                message.SubjectText = reader.ReadString();
            }

            return message;
        }

        private IEnumerable<ModelMission> ReadMissions(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                var mission = ReadMission(reader, this.sectorsById, this.factionsById, this.unitsById, this.fleetsById);
                if (mission != null)
                {
                    yield return mission;
                }
            }
        }

        private IEnumerable<ModelFleetSpawner> ReadFleetSpawners(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                yield return ReadFleetSpawner(
                    reader,
                    this.factionsById,
                    this.sectorsById,
                    this.unitsById,
                    this.fleetsById,
                    this.peopleById,
                    this.patrolPathsById);
            }
        }

        private void ReadFactionMercenaryData(BinaryReader reader)
        {
            var count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                var factionId = reader.ReadInt32();
                var hiringFactionId = reader.ReadInt32();
                var hireExpiryTime = reader.ReadDouble();

                var faction = this.factionsById.GetValueOrDefault(factionId);
                var hiringFaction = this.factionsById.GetValueOrDefault(hiringFactionId);

                if (faction != null)
                {
                    if (hiringFaction != null)
                    {
                        if (faction.FactionAI != null)
                        {
                            faction.FactionAI.FactionMercenaryHireInfo = new ModelFactionMercenaryHireInfo
                            {
                                HireTimeExpiry = hireExpiryTime,
                                HiringFaction = hiringFaction
                            };
                        }
                        else
                        {
                            Logging.Warning($"Expecting mercenary faction {factionId} to have FactionAI");
                        }
                    }
                    else
                    {
                        Logging.UnknownFactionMessage(hiringFactionId, $"loading mercenary info for faction {factionId}");
                    }
                }
                else
                {
                    Logging.UnknownFactionMessage(factionId, $"loading mercenary info");
                }
            }
        }

        private void ReadFactionAIExcludedUnits(BinaryReader reader)
        {
            var factionCount = reader.ReadInt32();
            for (int i = 0; i < factionCount; i++)
            {
                var faction = reader.ReadFaction(this.factionsById);

                var unitCount = reader.ReadInt32();
                for (int j = 0; j < unitCount; j++)
                {
                    var unit = reader.ReadUnit(this.unitsById);
                    if (faction != null && faction.FactionAI != null && unit != null)
                    {
                        faction.FactionAI.ExcludedUnits.Add(unit);
                    }
                }
            }
        }

        private void ReadAllFactionAIsAndBountyBoards(BinaryReader reader)
        {
            var factionCount = reader.ReadInt32();

            for (var i = 0; i < factionCount; i++)
            {
                var faction = reader.ReadFaction(this.factionsById);
                var hasAI = reader.ReadBoolean();

                if (hasAI)
                {
                    var aiType = (FactionAIType)reader.ReadInt32();

                    var factionAI = FactionAIReader.Read(reader, aiType, this.sectorsById);
                    faction.FactionAI = factionAI;
                }

                bool hasBountyBoard = reader.ReadBoolean();
                if (hasBountyBoard)
                {
                    faction.BountyBoard = ReadFactionBountyBoard(reader);
                }
            }
        }

        private ModelFactionBountyBoard ReadFactionBountyBoard(BinaryReader reader)
        {
            var bountyBoard = new ModelFactionBountyBoard();
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var item = ReadFactionBountyBoardItem(reader);
                bountyBoard.Items.Add(item);
            }

            return bountyBoard;
        }

        private ModelFactionBountyBoardItem ReadFactionBountyBoardItem(BinaryReader reader)
        {
            var item = new ModelFactionBountyBoardItem();
            item.TargetPerson = reader.ReadPerson(this.peopleById);
            item.Reward = reader.ReadInt32();
            item.LastKnownTargetUnit = reader.ReadUnit(this.unitsById);
            item.LastKnownTargetSector = reader.ReadSector(this.sectorsById);
            item.LastKnownTargetPosition = reader.ReadNullableVec3();

            var timeOfLastSighting = reader.ReadDouble();
            item.TimeOfLastSighting = timeOfLastSighting >= 0d ? timeOfLastSighting : null;

            item.SourceFaction = reader.ReadFaction(this.factionsById);

            return item;
        }

        private void ReadJobs(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                var jobType = (JobType)reader.ReadInt32();
                var job = JobReader.Read(reader, jobType, this.sectorsById, this.factionsById, this.unitsById);

                if (job.Unit != null)
                {
                    job.Unit.Jobs.Add(job);
                }
            }
        }

        private void ReadFactionLeaders(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var factionId = reader.ReadInt32();
                var personId = reader.ReadInt32();

                var faction = this.factionsById.GetValueOrDefault(factionId);
                var person = this.peopleById.GetValueOrDefault(personId);

                if (faction != null)
                {
                    if (person != null)
                    {
                        faction.Leader = person;
                    }
                    else
                    {
                        Logging.UnknownPersonMessage(personId, "loading faction leaders");
                    }
                }
                else
                {
                    Logging.UnknownFactionMessage(factionId, "loading faction leaders");
                }
            }
        }

        private void ReadNpcPilots(BinaryReader reader)
        {
            var count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                var npcPilot = ReadNpcPilot(reader);
                if (npcPilot.Person != null)
                {
                    npcPilot.Person.NpcPilot = npcPilot;
                }

                var fleetId = reader.ReadInt32();
                npcPilot.Fleet = this.fleetsById.GetValueOrDefault(fleetId);

                if (npcPilot.Fleet != null)
                {
                    npcPilot.Fleet.Npcs.Add(npcPilot);
                }
            }
        }

        private ModelNpcPilot ReadNpcPilot(BinaryReader reader)
        {
            var npcPilot = new ModelNpcPilot();

            var personId = reader.ReadInt32();
            var person = this.peopleById.GetValueOrDefault(personId);

            npcPilot.Person = person;
            npcPilot.DestroyWhenNoUnit = reader.ReadBoolean();
            npcPilot.DestroyWhenNotPilotting = reader.ReadBoolean();
            return npcPilot;
        }

        private IEnumerable<ModelPerson> ReadPeople(BinaryReader reader, out ModelPerson playerPerson)
        {
            playerPerson = null;

            var people = new List<ModelPerson>(100);
            var count = reader.ReadInt32();

            for (var i = 0; i < count; i++)
            {
                var person = ReadPerson(reader, out bool isPlayer);
                if (isPlayer)
                {
                    playerPerson = person;
                }

                people.Add(person);
            }

            return people;
        }

        private ModelPerson ReadPerson(BinaryReader reader,
            out bool isPlayer)
        {
            var id = reader.ReadInt32();

            var person = new ModelPerson();
            person.Id = id;

            var generatedName = reader.ReadBoolean();
            if (generatedName)
            {
                person.GeneratedFirstNameId = reader.ReadInt32();
                person.GeneratedLastNameId = reader.ReadInt32();
            }
            else
            {
                person.CustomName = reader.ReadString();
                person.CustomShortName = reader.ReadString();
            }

            person.Seed = reader.ReadInt32();
            person.DialogId = reader.ReadInt32();
            person.IsMale = reader.ReadBoolean();
            person.IsAutoPilot = reader.ReadBoolean();

            var factionId = reader.ReadInt32();
            person.Faction = this.factionsById.GetValueOrDefault(factionId);
            person.DestroyGameObjectOnKill = reader.ReadBoolean();

            var unitId = reader.ReadInt32();
            var unit = this.unitsById.GetValueOrDefault(unitId);

            if (unitId > -1 && unit == null)
            {
                Logging.UnknownUnitMessage(unitId, $"loading person id {id}");
            }

            if (unit != null)
            {
                unit.ComponentUnitData.People.Add(person);
                person.CurrentUnit = unit;
            }

            var isPilot = reader.ReadBoolean();
            if (isPilot && unit != null)
            {
                unit.ComponentUnitData.Pilot = person;
                person.IsPilot = isPilot;
            }

            person.Kills = reader.ReadInt32();
            person.Deaths = reader.ReadInt32();
            person.Properness = reader.ReadSingle();
            person.Aggression = reader.ReadSingle();
            person.Greed = reader.ReadSingle();
            person.RankId = reader.ReadInt32();
            person.AvatarProfileId = reader.ReadSByte();
            person.DialogProfileId = reader.ReadSByte();

            isPlayer = reader.ReadBoolean();

            var hasUnitControllerProfile = reader.ReadBoolean();
            if (hasUnitControllerProfile)
            {
                person.NpcPilotSettings = ReadNpcPilotSettings(reader);
            }

            return person;
        }

        private static ModelNpcPilotSettings ReadNpcPilotSettings(BinaryReader reader)
        {
            var settings = new ModelNpcPilotSettings();
            settings.RestrictedWeaponPreference = reader.ReadSingle();
            settings.CombatEfficiency = reader.ReadSingle();
            settings.CheatAmmo = reader.ReadBoolean();
            return settings;
        }

        private IEnumerable<ModelFleet> ReadFleets(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            var fleets = new List<ModelFleet>();
            for (var i = 0; i < count; i++)
            {
                // TODO: Bug in 1.6.x where some relying on fleets are using the fleet collection that hasn't been fully built yet. Objectives should be loaded after all fleets are loaded
                fleets.Add(ReadFleet(reader));
            }

            return fleets;
        }

        private ModelFleet ReadFleet(BinaryReader reader)
        {
            var fleet = new ModelFleet();
            fleet.IsActive = reader.ReadBoolean();
            fleet.Id = reader.ReadInt32();
            fleet.Seed = reader.ReadInt32();

            fleet.Position = reader.ReadVec3();
            fleet.Rotation = reader.ReadVec4();
            var sectorId = reader.ReadInt32();
            fleet.Sector = this.sectorsById.GetValueOrDefault(sectorId);
            var factionId = reader.ReadInt32();
            fleet.Faction = this.factionsById.GetValueOrDefault(factionId);
            fleet.FormationId = reader.ReadInt32();

            var hasHomeBase = reader.ReadBoolean();
            if (hasHomeBase)
            {
                fleet.HomeBase = SectorTargetReader.Read(reader, this.sectorsById, this.unitsById, this.fleetsById);
            }

            fleet.ExcludeFromFactionAI = reader.ReadBoolean();
            fleet.Strategy = (FactionStrategy)reader.ReadInt32();

            bool hasSettings = reader.ReadBoolean();
            if (hasSettings)
            {
                fleet.FleetSettings = ReadFleetSettings(reader);
            }

            return fleet;
        }

        private void ReadFleetOrders(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                var fleet = reader.ReadFleet(this.fleetsById);

                fleet.OrdersCollection = ReadOrders(
                    reader,
                    fleet.Id,
                    this.factionsById,
                    this.sectorsById,
                    this.unitsById,
                    this.fleetsById,
                    this.patrolPathsById,
                    this.peopleById,
                    this.passengerGroupsById);
            }

        }

        private ModelFleetSettings ReadFleetSettings(BinaryReader reader)
        {
            var settings = new ModelFleetSettings();
            settings.PreferCloak = reader.ReadBoolean();
            settings.PreferToDock = (DockedPreference)reader.ReadByte();
            settings.Aggression = reader.ReadSingle();
            settings.AllowAttack = reader.ReadBoolean();
            settings.TargetInterceptionLowerDistance = reader.ReadSingle();
            settings.TargetInterceptionUpperDistance = reader.ReadSingle();
            settings.MaxJumpDistance = reader.ReadInt32();
            settings.AllowCombatInterception = reader.ReadBoolean();
            settings.DestroyWhenNoPilots = reader.ReadBoolean();
            settings.FormationTightness = reader.ReadSingle();
            settings.CargoCollectionPreference = (FleetCargoCollectionPreference)reader.ReadInt32();

            return settings;
        }

        private void ReadHangars(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                var unitId = reader.ReadInt32();
                var hangarItems = ReadHangerItems(reader).ToList();
                var hangarUnit = this.unitsById.GetValueOrDefault(unitId);

                if (hangarUnit != null)
                {
                    if (hangarUnit.ComponentUnitData.DockData == null)
                    {
                        hangarUnit.ComponentUnitData.DockData = new ModelComponentUnitDockData();
                    }

                    foreach (var item in hangarItems)
                    {
                        if (item.dockedUnitId > -1)
                        {
                            var dockedUnit = this.unitsById.GetValueOrDefault(item.dockedUnitId);
                            if (dockedUnit != null)
                            {
                                hangarUnit.ComponentUnitData.DockData.Items.Add(new ModelComponentUnitDockDataItem
                                {
                                    BayId = item.bayId,
                                    DockedUnit = dockedUnit
                                });
                            }
                            else
                            {
                                Logging.UnknownUnitMessage(item.dockedUnitId, $"loading hangars. Unknown docked unit inside unit {unitId}");
                            }
                        }
                    }
                }
                else
                {
                    Logging.UnknownUnitMessage(unitId, "loading hangars. Unknown hangar unit.");
                }
            }
        }

        private IEnumerable<(int bayId, int dockedUnitId)> ReadHangerItems(BinaryReader reader)
        {
            var dockedUnitCount = reader.ReadInt32();

            for (var j = 0; j < dockedUnitCount; j++)
            {
                var bayId = reader.ReadInt32();
                var dockedUnitId = reader.ReadInt32();
                yield return (bayId, dockedUnitId);
            }
        }

        private void ReadWormholes(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                var unitId = reader.ReadInt32();
                var wormholeData = ReadWormholeData(reader, unitId);
                var unit = this.unitsById.GetValueOrDefault(unitId);

                if (unit != null)
                {
                    unit.WormholeData = wormholeData;
                }
                else
                {
                    Logging.UnknownUnitMessage(unitId, "loading wormhole data");
                }
            }
        }

        private ModelUnitWormholeData ReadWormholeData(BinaryReader reader, int unitId)
        {
            var wormholeData = new ModelUnitWormholeData();

            var targetUnitId = reader.ReadInt32();
            wormholeData.TargetWormholeUnit = this.unitsById.GetValueOrDefault(targetUnitId);
            if (wormholeData.TargetWormholeUnit == null)
            {
                Logging.UnknownUnitMessage(targetUnitId, $"loading wormhole data for unit {unitId}");
            }

            wormholeData.IsUnstable = reader.ReadBoolean();
            wormholeData.UnstableNextChangeTargetTime = reader.ReadDouble();

            wormholeData.UnstableTargetPosition = reader.ReadVec3();
            wormholeData.UnstableTargetRotation = reader.ReadVec3();

            var targetSectorId = reader.ReadInt32();
            wormholeData.UnstableTargetSector = this.sectorsById.GetValueOrDefault(targetSectorId);

            if (wormholeData.IsUnstable && wormholeData.UnstableTargetSector == null)
            {
                Logging.MissingSectorMessage($"loading wormhole data for unit {unitId}. Unstable wormhole must have target");
            }

            return wormholeData;
        }

        private void ReadPassengerGroups(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                var passengerGroup = ReadPassengerGroup(reader);
                if (passengerGroup.Unit != null)
                {
                    passengerGroup.Unit.PassengerGroups.Add(passengerGroup);
                    this.passengerGroupsById.Add(passengerGroup.Id, passengerGroup);
                }
            }
        }

        private ModelPassengerGroup ReadPassengerGroup(BinaryReader reader)
        {
            var passengerGroup = new ModelPassengerGroup();
            passengerGroup.Id = reader.ReadInt32();

            var currentUnitId = reader.ReadInt32();
            var sourceUnitId = reader.ReadInt32();
            var destinationUnit = reader.ReadInt32();

            passengerGroup.Unit = this.unitsById.GetValueOrDefault(currentUnitId);
            passengerGroup.SourceUnit = this.unitsById.GetValueOrDefault(sourceUnitId);
            passengerGroup.DestinationUnit = this.unitsById.GetValueOrDefault(destinationUnit);

            passengerGroup.PassengerCount = reader.ReadInt32();
            passengerGroup.ExpiryTime = reader.ReadDouble();

            passengerGroup.Revenue = reader.ReadInt32();
            return passengerGroup;
        }

        private void ReadAllFactionIntel(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var factionId = reader.ReadInt32();
                var factionIntel = ReadFactionIntel(reader, factionId);

                var faction = this.factionsById.GetValueOrDefault(factionId);
                if (faction != null)
                {
                    faction.Intel = factionIntel;
                }
                else
                {
                    Logging.UnknownFactionMessage(factionId, "loading intel");
                }
            }
        }

        private ModelFactionIntel ReadFactionIntel(BinaryReader reader, int factionId)
        {
            var factionIntel = new ModelFactionIntel();
            var discoveredSectorCount = reader.ReadInt32();
            for (var i = 0; i < discoveredSectorCount; i++)
            {
                var sectorId = reader.ReadInt32();
                var sector = this.sectorsById.GetValueOrDefault(sectorId);
                if (sector != null)
                {
                    factionIntel.Sectors.Add(sector);
                }
                else
                {
                    Logging.UnknownSectorMessage(sectorId, $"loading intel for faction {factionId}");
                }
            }

            var discoveredUnitCount = reader.ReadInt32();
            for (var i = 0; i < discoveredUnitCount; i++)
            {
                var unitId = reader.ReadInt32();
                var unit = this.unitsById.GetValueOrDefault(unitId);
                if (unit != null)
                {
                    factionIntel.Units.Add(unit);
                }
                else
                {
                    Logging.UnknownUnitMessage(unitId, $"loading intel for faction {factionId}");
                }
            }

            var enteredWormholeCount = reader.ReadInt32();
            for (var i = 0; i < enteredWormholeCount; i++)
            {
                var unitId = reader.ReadInt32();
                var unit = this.unitsById.GetValueOrDefault(unitId);
                if (unit != null)
                {
                    factionIntel.EnteredWormholes.Add(unit);
                }
                else
                {
                    Logging.UnknownUnitMessage(unitId, $"loading intel for faction {factionId}");
                }
            }

            return factionIntel;
        }

        private void ReadAllUnitHealthDatas(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var unitId = reader.ReadInt32();
                var healthData = ReadUnitHealthData(reader);
                var unit = this.unitsById.GetValueOrDefault(unitId);

                if (unit != null)
                {
                    unit.HealthData = healthData;
                }
                else
                {
                    Logging.UnknownUnitMessage(unitId, $"loading health data for unit {unitId}");
                }
            }
        }

        private ModelUnitHealthData ReadUnitHealthData(BinaryReader reader)
        {
            var healthData = new ModelUnitHealthData();
            healthData.IsDestroyed = reader.ReadBoolean();
            healthData.Health = reader.ReadSingle();
            return healthData;
        }

        private void ReadActiveUnits(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var unitId = reader.ReadInt32();
                var activeData = ReadActiveUnitData(reader);
                var unit = this.unitsById.GetValueOrDefault(unitId);
                if (unit != null)
                {
                    unit.ActiveData = activeData;
                }
                else
                {
                    Logging.UnknownUnitMessage(unitId, $"loading active data for unit {unitId}");
                }
            }
        }

        private ModelUnitActiveData ReadActiveUnitData(BinaryReader reader)
        {
            var activeData = new ModelUnitActiveData();
            activeData.Velocity = reader.ReadVec3();
            activeData.CurrentTurn = reader.ReadSingle();
            return activeData;
        }

        private void ReadAllUnitComponentHealthData(BinaryReader reader)
        {
            var unitCount = reader.ReadInt32();

            for (int i = 0; i < unitCount; i++)
            {
                var unitId = reader.ReadInt32();
                var unit = this.unitsById.GetValueOrDefault(unitId);

                var componentHealthData = ReadUnitComponentHealthData(reader);

                if (unit?.ComponentUnitData != null)
                {
                    unit.ComponentUnitData.ComponentHealthData = componentHealthData;
                }
                else
                {
                    Logging.UnknownUnitMessage(unitId, $"loading component heatlh data for unit {unitId}");
                }
            }
        }

        private ModelComponentUnitComponentHealthData ReadUnitComponentHealthData(BinaryReader reader)
        {
            var componentHealthData = new ModelComponentUnitComponentHealthData();
            var damagedComponentCount = reader.ReadInt32();

            for (int i = 0; i < damagedComponentCount; i++)
            {
                var item = new ModelComponentUnitComponentHealthDataItem();
                item.BayId = reader.ReadInt32();
                item.Health = reader.ReadSingle();
                componentHealthData.Items.Add(item);
            }

            return componentHealthData;
        }

        private void ReadAllShieldHealthData(BinaryReader reader)
        {
            var count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                var unitId = reader.ReadInt32();
                var shieldData = ReadShieldHealthData(reader);

                var unit = this.unitsById.GetValueOrDefault(unitId)?.ComponentUnitData;

                if (unit != null)
                {
                    unit.ShieldData = shieldData;
                }
                else
                {
                    Logging.UnknownUnitMessage(unitId, $"loading shield health data for unit {unitId}");
                }
            }
        }

        private ModelComponentUnitShieldHealthData ReadShieldHealthData(BinaryReader reader)
        {
            var damagedShieldCount = reader.ReadByte();

            var shieldData = new ModelComponentUnitShieldHealthData();
            for (int j = 0; j < damagedShieldCount; j++)
            {
                var item = new ModelComponentUnitShieldHealthDataItem();
                item.ShieldPointIndex = reader.ReadByte();
                item.Health = reader.ReadSingle();
                shieldData.Items.Add(item);
            }

            return shieldData;
        }

        private void ReadComponentUnitCargo(BinaryReader reader)
        {
            var unitCount = reader.ReadInt32();
            for (int i = 0; i < unitCount; i++)
            {
                var unitId = reader.ReadInt32();
                var unit = this.unitsById.GetValueOrDefault(unitId)?.ComponentUnitData;

                var cargoData = ReadComponentUnitCargoData(reader);

                if (unit != null)
                {
                    unit.CargoData = cargoData;
                }
                else
                {
                    Logging.UnknownUnitMessage(unitId, $"loading cargo data for unit {unitId}");
                }
            }
        }

        private ModelComponentUnitCargoData ReadComponentUnitCargoData(BinaryReader reader)
        {
            var cargoData = new ModelComponentUnitCargoData();
            var itemCount = reader.ReadInt32();

            for (var i = 0; i < itemCount; i++)
            {
                cargoData.Items.Add(ComponentUnitCargoDataItemReader.Read(reader));
            }

            return cargoData;
        }

        private void ReadUnitEngineThrottles(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var unitId = reader.ReadInt32();
                var engineThrottle = reader.ReadSingle();

                var unit = this.unitsById.GetValueOrDefault(unitId);
                if (unit?.ComponentUnitData != null)
                {
                    unit.ComponentUnitData.EngineThrottle = engineThrottle;
                }
                else
                {
                    Logging.UnknownUnitMessage(unitId, $"loading engine throttle data for unit {unitId}");
                }
            }
        }

        private void ReadPoweredDownComponents(BinaryReader reader)
        {
            var count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                var unitId = reader.ReadInt32();

                var bayCount = reader.ReadInt32();

                var unit = this.unitsById.GetValueOrDefault(unitId);

                for (int j = 0; j < bayCount; j++)
                {
                    var bayId = reader.ReadInt32();

                    if (unit != null && unit.ComponentUnitData != null)
                    {
                        unit.ComponentUnitData.PoweredDownBayIds.Add(bayId);
                    }
                    else
                    {
                        Logging.UnknownUnitMessage(unitId, $"loading powered-down component data for unit {unitId}");
                    }
                }
            }
        }

        private void ReadAutoFireComponents(BinaryReader reader)
        {
            var count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                var unitId = reader.ReadInt32();

                var bayCount = reader.ReadInt32();

                var unit = this.unitsById.GetValueOrDefault(unitId);

                for (int j = 0; j < bayCount; j++)
                {
                    var bayId = reader.ReadInt32();

                    if (unit != null && unit.ComponentUnitData != null)
                    {
                        unit.ComponentUnitData.AutoFireBayIds.Add(bayId);
                    }
                    else
                    {
                        Logging.UnknownUnitMessage(unitId, $"loading auto-fire component data for unit {unitId}");
                    }
                }
            }
        }

        private void ReadCloakedUnits(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var unitId = reader.ReadInt32();

                var unit = this.unitsById.GetValueOrDefault(unitId);

                if (unit?.ComponentUnitData != null)
                {
                    unit.ComponentUnitData.IsCloaked = true;
                }
                else
                {
                    Logging.UnknownUnitMessage(unitId, $"loading cloak state data for unit {unitId}");
                }
            }
        }

        private void ReadUnitCapacitorCharges(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var unitId = reader.ReadInt32();
                var chargeNormalized = reader.ReadSingle();

                var unit = this.unitsById.GetValueOrDefault(unitId);
                if (unit?.ComponentUnitData != null)
                {
                    unit.ComponentUnitData.CapacitorCharge = chargeNormalized;
                }
                else
                {
                    Logging.UnknownUnitMessage(unitId, $"loading capacitor charge data for unit {unitId}");
                }
            }
        }

        private void ReadModdedComponents(BinaryReader reader)
        {
            var count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                var unitId = reader.ReadInt32();
                var bayId = reader.ReadInt32();
                var componentClassId = reader.ReadInt32();

                var unit = this.unitsById.GetValueOrDefault(unitId);
                if (unit != null)
                {
                    if (unit.ComponentUnitData.ModData == null)
                    {
                        unit.ComponentUnitData.ModData = new ModelComponentUnitModData();
                    }

                    unit.ComponentUnitData.ModData.Items.Add(new ModelComponentUnitModDataItem
                    {
                        BayId = bayId,
                        ComponentClass = (ModelComponentClass)componentClassId
                    });
                }
            }
        }

        private void ReadAllComponentUnits(BinaryReader reader)
        {
            var count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                var unitId = reader.ReadInt32();

                var unit = this.unitsById.GetValueOrDefault(unitId);
                unit.ComponentUnitData = ReadComponentUnitData(reader);
            }
        }

        private ModelComponentUnitData ReadComponentUnitData(BinaryReader reader)
        {
            var componentUnitData = new ModelComponentUnitData();
            componentUnitData.ShipNameIndex = reader.ReadInt32();

            if (componentUnitData.ShipNameIndex == -1)
            {
                componentUnitData.CustomShipName = reader.ReadString();
            }

            var hasFactory = reader.ReadBoolean();

            if (hasFactory)
            {
                componentUnitData.FactoryData = ReadComponentUnitFactoryData(reader);
            }

            return componentUnitData;
        }

        private ModelComponentUnitFactoryData ReadComponentUnitFactoryData(BinaryReader reader)
        {
            var factoryData = new ModelComponentUnitFactoryData();
            var count = reader.ReadInt32();

            for (var i = 0; i < count; i++)
            {
                var item = new ModelComponentUnitFactoryItemData();
                item.State = (CargoFactoryItemState)reader.ReadInt32();
                item.ProductionElapsed = reader.ReadSingle();
                factoryData.Items.Add(item);
            }

            return factoryData;
        }

        private void ReadNamedUnits(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var unitId = reader.ReadInt32();
                var unitName = reader.ReadString();
                var unitShortName = reader.ReadString();

                var unit = this.unitsById.GetValueOrDefault(unitId);
                if (unit != null)
                {
                    unit.Name = unitName;
                    unit.ShortName = unitShortName;
                }
            }
        }

        private void PrintStatus(string message, BinaryReader reader)
        {
            Console.WriteLine($"{message} - {reader.BaseStream.Position - 1} bytes read");
        }

        private void ReadUnits(
            BinaryReader reader,
            SavedGame savedGame)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var unit = ReadUnit(reader, savedGame.Sectors, savedGame.Factions, savedGame.Units);
                savedGame.Units.Add(unit);
            }
        }

        private ModelUnit ReadUnit(
            BinaryReader reader,
            IEnumerable<ModelSector> sectors,
            IEnumerable<ModelFaction> factions,
            IEnumerable<ModelUnit> units)
        {
            var unit = new ModelUnit();

            unit.Id = reader.ReadInt32();
            unit.Seed = reader.ReadInt32();

            var classId = reader.ReadInt32();

            if (!Enum.IsDefined(typeof(ModelUnitClass), unit.Class))
                throw new Exception($"Unrecognised unit class {unit.Class}");

            unit.Class = (ModelUnitClass)classId;

            var sectorId = reader.ReadInt32();
            unit.Sector = sectors.FirstOrDefault(e => e.Id == sectorId);

            if (sectorId > -1 && unit.Sector == null)
            {
                Logging.UnknownSectorMessage(sectorId, $"loading unit {unit.Id}");
            }

            unit.Position = reader.ReadVec3();
            unit.Rotation = reader.ReadVec3();

            var factionId = reader.ReadInt32();
            unit.Faction = factions.FirstOrDefault(e => e.Id == factionId);

            if (factionId > -1 && unit.Faction == null)
            {
                Logging.UnknownFactionMessage(factionId, $"loading unit {unit.Id}");
            }

            unit.RpProvision = reader.ReadInt32();

            var hasCargo = reader.ReadBoolean();
            if (hasCargo)
            {
                unit.CargoData = ReadUnitCargoData(reader);
            }

            var hasDebris = reader.ReadBoolean();
            if (hasDebris)
            {
                unit.DebrisData = ReadUnitDebrisData(reader);
            }

            var hasAsteroid = reader.ReadBoolean();
            if (hasAsteroid)
            {
                unit.AsteroidData = ReadUnitAsteroidData(reader);
            }

            var hasShipTrader = reader.ReadBoolean();
            if (hasShipTrader)
            {
                unit.ShipTraderData = ReadShipTrader(reader);
            }

            if (UnitHelper.IsProjectile(unit.Class))
            {
                var hasProjectile = reader.ReadBoolean();
                if (hasProjectile)
                {
                    unit.ProjectileData = ReadUnitProjectileData(reader);
                }
            }

            return unit;
        }

        private ModelUnitProjectileData ReadUnitProjectileData(BinaryReader reader)
        {
            var projectileData = new ModelUnitProjectileData();
            projectileData.SourceUnit = reader.ReadUnit(this.unitsById);
            projectileData.TargetUnit = reader.ReadUnit(this.unitsById);
            projectileData.FireTime = reader.ReadDouble();
            projectileData.RemainingMovement = reader.ReadSingle();
            projectileData.DamageType = reader.ReadDamageType();
            return projectileData;
        }

        private ModelUnitShipTraderData ReadShipTrader(BinaryReader reader)
        {
            var shipTraderData = new ModelUnitShipTraderData();

            var count = reader.ReadInt32();

            for (var i = 0; i < count; i++)
            {
                var item = new ModelUnitShipTraderItem();
                item.SellMultiplier = reader.ReadSingle();
                item.UnitClass = (ModelUnitClass)reader.ReadInt32();
                shipTraderData.Items.Add(item);
            }

            return shipTraderData;
        }

        private ModelUnitAsteroidData ReadUnitAsteroidData(BinaryReader reader)
        {
            var asteroidData = new ModelUnitAsteroidData();
            asteroidData.RemainingYield = reader.ReadInt32();
            return asteroidData;
        }

        private ModelUnitDebrisData ReadUnitDebrisData(BinaryReader reader)
        {
            var debrisData = new ModelUnitDebrisData();
            debrisData.ScrapQuantity = reader.ReadInt32();
            debrisData.Expires = reader.ReadBoolean();
            debrisData.ExpiryTime = reader.ReadDouble();
            debrisData.RelatedUnitClass = (ModelUnitClass)reader.ReadInt32();
            return debrisData;
        }

        private ModelUnitCargoData ReadUnitCargoData(BinaryReader reader)
        {
            var unitCargoData = new ModelUnitCargoData();
            unitCargoData.CargoClass = (ModelCargoClass)reader.ReadInt32();
            unitCargoData.Quantity = reader.ReadInt32();
            unitCargoData.Expires = reader.ReadBoolean();
            unitCargoData.SpawnTime = reader.ReadDouble();
            return unitCargoData;
        }

        private IEnumerable<ModelFaction> ReadFactions(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                yield return ReadFaction(reader);
            }
        }

        private void ReadAllFactionAvatarProfileIds(BinaryReader reader)
        {
            var factionCount = reader.ReadInt32();
            for (int i = 0; i < factionCount; i++)
            {
                var faction = reader.ReadFaction(this.factionsById);
                var avatarCount = reader.ReadByte();
                for (int j = 0; j < avatarCount; j++)
                {
                    var avatarProfileId = reader.ReadByte();
                    if (faction != null)
                    {
                        if (faction.AvatarProfileIds == null)
                            faction.AvatarProfileIds = new List<byte>();

                        faction.AvatarProfileIds.Add(avatarProfileId);
                    }
                }
            }
        }

        private IEnumerable<ModelSector> ReadSectors(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                yield return ReadSector(reader);
            }
        }

        private IEnumerable<ModelSectorPatrolPath> ReadPatrolPaths(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                yield return ReadPatrolPath(reader);
            }
        }

        private void ReadAllFactionRelations(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var factionId = reader.ReadInt32();
                var factionRelations = ReadFactionRelationData(reader, factionId);
                var faction = this.factionsById.GetValueOrDefault(factionId);

                if (faction != null)
                {
                    faction.Relations = factionRelations;
                }
                else
                {
                    Logging.UnknownFactionMessage(factionId, "loading faction relations");
                }
            }
        }

        private void ReadAllFactionRecentDamageReceived(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var faction = reader.ReadFaction(this.factionsById);

                var damageCount = reader.ReadInt32();
                for (int j = 0; j < damageCount; j++)
                {
                    var otherFaction = reader.ReadFaction(this.factionsById);
                    var damage = reader.ReadSingle();

                    if (faction != null && otherFaction != null)
                    {
                        faction.RecentDamageItems.Add(new ModelFactionRecentDamageItem
                        {
                            OtherFaction = otherFaction,
                            RecentDamageReceived = damage
                        });
                    }
                }
            }
        }

        private ModelFactionRelationData ReadFactionRelationData(BinaryReader reader, int factionId)
        {
            var factionRelationData = new ModelFactionRelationData();

            var relationCount = reader.ReadInt32();

            for (var i = 0; i < relationCount; i++)
            {
                var item = ReadFactionRelationDataItem(reader, factionId);
                if (item.OtherFaction != null)
                {
                    factionRelationData.Items.Add(item);
                }
            }

            return factionRelationData;
        }

        private ModelFactionRelationDataItem ReadFactionRelationDataItem(BinaryReader reader, int factionId)
        {
            var relation = new ModelFactionRelationDataItem();

            var otherFactionId = reader.ReadInt32();
            relation.OtherFaction = this.factionsById.GetValueOrDefault(otherFactionId);

            if (relation.OtherFaction == null)
            {
                Logging.UnknownFactionMessage(otherFactionId, $"loading faction relation data for faction {factionId}");
            }

            relation.PermanentPeace = reader.ReadBoolean();
            relation.RestrictHostilityTimeout = reader.ReadBoolean();
            relation.Neutrality = (Neutrality)reader.ReadInt32();
            relation.HostilityEndTime = reader.ReadDouble();
            return relation;
        }

        private void ReadAllFactionOpinions(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var factionId = reader.ReadInt32();
                var otherFactionId = reader.ReadInt32();
                var opinion = reader.ReadSingle();
                var createdTime = (double)reader.ReadUInt32();

                var faction = this.factionsById.GetValueOrDefault(factionId);
                var otherFaction = this.factionsById.GetValueOrDefault(otherFactionId);

                if (faction != null)
                {
                    if (otherFaction == faction)
                    {
                        Logging.Warning($"Faction opinion for faction {factionId} is referencing itself. Item will not be loaded.");
                    }
                    else
                    {
                        if (faction.Opinions == null)
                        {
                            faction.Opinions = new ModelFactionOpinionData();
                        }

                        if (otherFaction != null)
                        {
                            faction.Opinions.Items.Add(new ModelFactionOpinionDataItem
                            {
                                OtherFaction = otherFaction,
                                Opinion = opinion,
                                CreatedTime = createdTime
                            });
                        }
                        else
                        {
                            Logging.UnknownFactionMessage(otherFactionId, "loading faction opinions");
                        }
                    }
                }
                else
                {
                    Logging.UnknownFactionMessage(factionId, "loading faction opinions");
                }
            }
        }

        private ModelSector ReadSector(BinaryReader reader)
        {
            var sector = new ModelSector();
            sector.Id = reader.ReadInt32();
            sector.Name = reader.ReadString();
            sector.MapPosition = reader.ReadVec3();
            sector.Description = reader.ReadString();
            sector.GateDistanceMultiplier = reader.ReadSingle();
            sector.RandomSeed = reader.ReadInt32();
            sector.BackgroundRotation = reader.ReadVec3();
            sector.AmbientLightColor = reader.ReadVec3();
            sector.DirectionLightColor = reader.ReadVec3();
            sector.DirectionLightRotation = reader.ReadVec3();
            sector.LastTimeChangedControl = reader.ReadDouble();
            sector.LightDirectionFudge = reader.ReadSingle();

            return sector;
        }

        private ModelFaction ReadFaction(BinaryReader reader)
        {
            var faction = new ModelFaction();
            faction.Id = reader.ReadInt32();
            faction.GeneratedNameId = reader.ReadInt32();
            faction.GeneratedSuffixId = reader.ReadInt32();
            faction.CustomName = reader.ReadString();
            faction.CustomShortName = reader.ReadString();

            faction.HomeSector = reader.ReadSector(this.sectorsById);
            faction.HomeSectorPosition = reader.ReadNullableVec3();

            faction.Credits = reader.ReadInt32();
            faction.Description = reader.ReadString();
            faction.IsCivilian = reader.ReadBoolean();
            faction.FactionType = (FactionType)reader.ReadInt32();
            faction.Aggression = reader.ReadSingle();
            faction.Virtue = reader.ReadSingle();
            faction.Greed = reader.ReadSingle();
            faction.Cooperation = reader.ReadSingle();
            faction.TradeEfficiency = reader.ReadSingle();
            faction.DynamicRelations = reader.ReadBoolean();
            faction.ShowJobBoards = reader.ReadBoolean();
            faction.CreateJobs = reader.ReadBoolean();
            faction.RequisitionPointMultiplier = reader.ReadSingle();
            faction.DestroyWhenNoUnits = reader.ReadBoolean();
            faction.MinNpcCombatEfficiency = reader.ReadSingle();
            faction.MaxNpcCombatEfficiency = reader.ReadSingle();
            faction.AdditionalRpProvision = reader.ReadInt32();
            faction.TradeIllegalGoods = reader.ReadBoolean();
            faction.SpawnTime = reader.ReadDouble();
            faction.HighestEverNetWorth = reader.ReadInt64();
            faction.RankingSystemId = reader.ReadInt32();
            faction.PreferredFormationId = reader.ReadInt32();

            bool hasAISettings = reader.ReadBoolean();

            if (hasAISettings)
            {
                faction.CustomSettings = ReadFactionCustomSettings(reader);
            }

            bool hasStats = reader.ReadBoolean();
            if (hasStats)
            {
                faction.Stats = ReadFactionStats(reader);
            }

            var excludedSectorCount = reader.ReadInt32();
            for (int i = 0; i < excludedSectorCount; i++)
            {
                var sectorId = reader.ReadInt32();
                var sector = this.sectorsById.GetValueOrDefault(sectorId);
                faction.AutopilotExcludedSectors.Add(sector);
            }

            return faction;
        }

        private ModelFactionCustomSettings ReadFactionCustomSettings(BinaryReader reader)
        {
            var settings = new ModelFactionCustomSettings();
            settings.BuildShips = reader.ReadBoolean();
            settings.RepairShips = reader.ReadBoolean();
            settings.UpgradeShips = reader.ReadBoolean();
            settings.RepairMinHullDamage = reader.ReadSingle();
            settings.RepairMinCreditsBeforeRepair = reader.ReadInt32();
            settings.PreferenceToPlaceBounty = reader.ReadSingle();
            settings.LargeShipPreference = reader.ReadSingle();
            settings.DailyIncome = reader.ReadInt32();
            settings.HostileWithAll = reader.ReadBoolean();
            settings.MinFleetUnitCount = reader.ReadInt32();
            settings.MaxFleetUnitCount = reader.ReadInt32();
            settings.OffensiveStance = reader.ReadSingle();
            settings.AllowOtherFactionToUseDocks = reader.ReadBoolean();
            settings.PreferenceToBuildTurrets = reader.ReadSingle();
            settings.PreferenceToBuildStations = reader.ReadSingle();
            settings.PreferenceToHaveAmmo = reader.ReadSingle();
            settings.IgnoreStationCreditsReserve = reader.ReadBoolean();
            settings.MaxJumpDistanceFromHomeSector = reader.ReadInt32();
            settings.MaxStationBuildDistanceFromHomeSector = reader.ReadInt32();
            settings.PilotGender = (GenderChoice)reader.ReadInt32();
            settings.FixedShipCount = reader.ReadInt32();
            settings.SectorControlLikelihood = reader.ReadSingle();

            return settings;
        }

        private ModelFactionStats ReadFactionStats(BinaryReader reader)
        {
            var stats = new ModelFactionStats();
            stats.TotalShipsClaimed = reader.ReadInt32();
            stats.UnitsDestroyedByClassId = ReadFactionStatsUnitCounts(reader);
            stats.UnitLostByClassId = ReadFactionStatsUnitCounts(reader);
            stats.ScratchcardsScratched = reader.ReadInt32();
            stats.HighestScratchcardWin = reader.ReadInt32();

            return stats;
        }

        private Dictionary<ModelUnitClass, int> ReadFactionStatsUnitCounts(BinaryReader reader)
        {
            var itemCount = reader.ReadInt32();
            var items = new Dictionary<ModelUnitClass, int>();
            for (int i = 0; i < itemCount; i++)
            {
                items.Add((ModelUnitClass)reader.ReadInt32(), reader.ReadInt32());
            }

            return items;
        }

        private ModelSectorPatrolPath ReadPatrolPath(BinaryReader reader)
        {
            var path = new ModelSectorPatrolPath();
            path.Id = reader.ReadInt32();
            path.Sector = reader.ReadSector(this.sectorsById);
            path.IsLoop = reader.ReadBoolean();

            // nodes
            int nodeCount = reader.ReadInt32();
            for (int i = 0; i < nodeCount; i++)
            {
                path.Nodes.Add(new ModelSectorPatrolPathNode
                {
                    SectorPosition = reader.ReadVec3(),
                    Order = reader.ReadInt32()
                });
            }

            return path;
        }

        public static ModelMission ReadMission(
    BinaryReader reader,
    Dictionary<int, ModelSector> sectors,
    Dictionary<int, ModelFaction> factions,
    Dictionary<int, ModelUnit> units,
    Dictionary<int, ModelFleet> fleets
    )
        {
            var jobTypeId = reader.ReadInt32();
            var jobType = (JobType)jobTypeId;
            var modelMission = CreateMissionFromJobType.Create(jobType);

            reader.ReadBoolean(); // IsDynamicGenerated
            modelMission.Id = reader.ReadInt32();
            reader.ReadInt32(); // JobDataId
            modelMission.IsActive = reader.ReadBoolean();
            modelMission.StageIndex = reader.ReadInt32();
            modelMission.IsFinished = reader.ReadBoolean();
            modelMission.CompletionSuccess = reader.ReadBoolean();
            modelMission.ShowInJournal = reader.ReadBoolean();
            modelMission.OwnerFaction = reader.ReadFaction(factions);
            modelMission.MissionGiverFaction = reader.ReadFaction(factions);
            modelMission.CompletionOpinionChange = reader.ReadSingle();
            modelMission.FailureOpinionChange = reader.ReadSingle();
            modelMission.StartTime = reader.ReadDouble();
            modelMission.RewardCredits = reader.ReadInt32();

            var objectiveCount = GetJobObjectiveCount(GetJobDataIdFromMissionType(modelMission.MissionType));
            for (var i = 0; i < objectiveCount; i++)
            {
                modelMission.Objectives.Add(ReadActiveJobObjective(reader));
            }

            switch (modelMission.MissionType)
            {
                case MissionType.Courier:
                    {
                        var courierMission = (ModelCourierMission)modelMission;
                        courierMission.PickupUnit = reader.ReadUnit(units);
                        courierMission.DestinationUnit = reader.ReadUnit(units);

                        var item = new ModelComponentUnitCargoDataItem();
                        item.CargoClass = (ModelCargoClass)reader.ReadInt32();
                        item.Quantity = reader.ReadInt32();
                        courierMission.CargoItem = item;
                        courierMission.HasPlayerPickedUpCargo = reader.ReadBoolean();
                    }
                    break;
                case MissionType.DestroyGroup:
                    {
                        var destroyUnitsMission = (ModelDestroyUnitsMission)modelMission;
                        var unitCount = reader.ReadInt32();
                        for (var i = 0; i < unitCount; i++)
                        {
                            destroyUnitsMission.TargetUnits.Add(reader.ReadUnit(units));
                        }

                        destroyUnitsMission.HasSetGroupHostileToPlayer = reader.ReadBoolean();
                        destroyUnitsMission.TargetFaction = reader.ReadFaction(factions);
                        destroyUnitsMission.TargetSector = reader.ReadSector(sectors);
                        destroyUnitsMission.TargetFleet = reader.ReadFleet(fleets);
                    }
                    break;
                case MissionType.DeliverShip:
                    {
                        var deliverShipMission = (ModelDeliverShipMission)modelMission;
                        deliverShipMission.UnitClass = (ModelUnitClass)reader.ReadInt32();
                        deliverShipMission.DestinationUnit = reader.ReadUnit(units);
                    }
                    break;
                case MissionType.Breakdown:
                    {
                        var breakdownMission = (ModelBreakdownMission)modelMission;
                        breakdownMission.BaseUnit = reader.ReadUnit(units);
                        breakdownMission.BreakdownUnit = reader.ReadUnit(units);
                    }
                    break;
            }

            return modelMission;
        }

        private static ModelMissionObjective ReadActiveJobObjective(
            BinaryReader reader)
        {
            var objective = new ModelMissionObjective();
            objective.IsActive = reader.ReadBoolean();
            objective.IsComplete = reader.ReadBoolean();
            objective.Success = reader.ReadBoolean();
            objective.ShowInJournal = reader.ReadBoolean();
            return objective;
        }

        private static JobType GetJobTypeFromJobDataId(JobDataIds jobDataId)
        {
            switch (jobDataId)
            {
                case JobDataIds.DeliverShip:
                    return JobType.DeliverShip;
                case JobDataIds.Courier:
                    return JobType.Courier;
                case JobDataIds.DestroyFleet:
                    return JobType.DestroyGroup;
                case JobDataIds.Breakdown:
                    return JobType.Breakdown;
                default:
                    throw new NotImplementedException($"Unknown job data id {(int)jobDataId}");
            }
        }

        private static int GetJobObjectiveCount(JobDataIds jobDataType)
        {
            switch (jobDataType)
            {
                case JobDataIds.DeliverShip:
                case JobDataIds.DestroyFleet:
                case JobDataIds.DestroyUnits:
                    return 1;
                case JobDataIds.Breakdown:
                case JobDataIds.Courier:
                    return 2;

            }

            return 0;
        }

        private static JobDataIds GetJobDataIdFromMissionType(MissionType missionType)
        {
            switch (missionType)
            {
                case MissionType.Breakdown:
                    return JobDataIds.Breakdown;
                case MissionType.Courier:
                    return JobDataIds.Courier;
                case MissionType.DeliverShip:
                    return JobDataIds.Courier;
                case MissionType.DestroyGroup:
                    return JobDataIds.DestroyFleet;
                default:
                    return JobDataIds.None;
            }
        }

        public static ModelFleetOrderCollection ReadOrders(
            BinaryReader reader,
            int fleetId,
            Dictionary<int, ModelFaction> factions,
            Dictionary<int, ModelSector> sectors,
            Dictionary<int, ModelUnit> units,
            Dictionary<int, ModelFleet> fleets,
            Dictionary<int, ModelSectorPatrolPath> patrolPaths,
            Dictionary<int, ModelPerson> people,
            Dictionary<int, ModelPassengerGroup> passengerGroups)
        {
            var fleetOrders = new ModelFleetOrderCollection();
            var count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                var order = ReadOrder(reader, factions, sectors, units, fleets, patrolPaths);
                fleetOrders.Orders.Add(order);
            }

            var queuedCount = reader.ReadInt32();
            for (int i = 0; i < queuedCount; i++)
            {
                var index = reader.ReadInt32();
                if (index < 0 || index >= fleetOrders.Orders.Count)
                {
                    Logging.Warning($"Fleet {fleetId} contains an invalid order index {index}");
                }
                else
                {
                    fleetOrders.QueuedOrders.Add(fleetOrders.Orders[index]);
                }
            }

            var hasCurrentOrder = reader.ReadBoolean();
            if (hasCurrentOrder)
            {
                var index = reader.ReadInt32();

                if (index < 0 || index >= fleetOrders.Orders.Count)
                {
                    throw new Exception($"Fleet {fleetId} contains an invalid order index {index}");
                }

                var currentOrder = fleetOrders.Orders[index];

                var activeOrder = ReadActiveOrder(
                    reader,
                    currentOrder,
                    fleetId,
                    factions,
                    sectors,
                    units,
                    fleets,
                    patrolPaths,
                    people,
                    passengerGroups);

                fleetOrders.CurrentOrder = activeOrder;
            }

            return fleetOrders;
        }

        public static ModelActiveFleetOrder ReadActiveOrder(
            BinaryReader reader,
            ModelFleetOrder fleetOrder,
            int fleetId,
            Dictionary<int, ModelFaction> factions,
            Dictionary<int, ModelSector> sectors,
            Dictionary<int, ModelUnit> units,
            Dictionary<int, ModelFleet> fleets,
            Dictionary<int, ModelSectorPatrolPath> patrolPaths,
            Dictionary<int, ModelPerson> people,
            Dictionary<int, ModelPassengerGroup> passengerGroups)
        {
            var fleetOrderType = fleetOrder.OrderType;
            var activeOrder = ActiveFleetOrderReader.Read(
                reader,
                fleetOrderType,
                fleetId,
                factions,
                sectors,
                units,
                fleets,
                patrolPaths,
                people,
                passengerGroups);
            activeOrder.Order = fleetOrder;

            return activeOrder;
        }

        public static ModelFleetOrder ReadOrder(
            BinaryReader reader,
            Dictionary<int, ModelFaction> factions,
            Dictionary<int, ModelSector> sectors,
            Dictionary<int, ModelUnit> units,
            Dictionary<int, ModelFleet> fleets,
            Dictionary<int, ModelSectorPatrolPath> patrolPaths)
        {
            var orderType = (FleetOrderType)reader.ReadInt32();

            var order = CreateFleetOrderFromType.Create(orderType);
            order.Id = reader.ReadInt32();
            order.CompletionMode = (FleetOrderCompletionMode)reader.ReadInt32();
            order.AllowCombatInterception = reader.ReadBoolean();
            order.CloakPreference = (FleetOrderCloakPreference)reader.ReadInt32();
            order.MaxJumpDistance = reader.ReadInt32();
            order.AllowTimeout = reader.ReadBoolean();
            order.TimeoutTime = reader.ReadSingle();
            order.MaxDuration = reader.ReadSingle();
            order.Priority = reader.ReadSingle();

            switch (order.OrderType)
            {
                case FleetOrderType.AttackGroup:
                    {
                        var o = (ModelAttackFleetOrder)order;
                        var targetFleetId = reader.ReadInt32();
                        o.Target = fleets.GetValueOrDefault(targetFleetId);
                        o.AttackPriority = reader.ReadSingle();
                    }
                    break;
                case FleetOrderType.CollectCargo:
                    {
                        var o = (ModelCollectCargoOrder)order;
                        o.TargetUnit = reader.ReadUnit(units);
                    }
                    break;

                case FleetOrderType.Scavenge:
                    {
                        var o = (ModelScavengeOrder)order;
                        o.TargetSector = reader.ReadSector(sectors);
                        o.CollectOwnerMode = (CollectCargoOwnerMode)reader.ReadInt32();
                    }
                    break;
                case FleetOrderType.Mine:
                    {
                        var o = (ModelMineOrder)order;
                        o.TargetSector = reader.ReadSector(sectors);
                        o.CollectOwnerMode = (CollectCargoOwnerMode)reader.ReadInt32();

                        var manualMineTargetUnitId = reader.ReadInt32();

                        var manualMineTargetUnit = units.GetValueOrDefault(manualMineTargetUnitId);
                        if (manualMineTargetUnit != null)
                        {
                            o.ManualMineTarget = manualMineTargetUnit;
                        }
                    }
                    break;
                case FleetOrderType.Dock:
                    {
                        var o = (ModelDockOrder)order;
                        var unitId = reader.ReadInt32();
                        o.TargetDock = units.GetValueOrDefault(unitId);
                    }
                    break;
                case FleetOrderType.Patrol:
                    {
                        var o = (ModelPatrolOrder)order;

                        o.PathDirection = reader.ReadInt32();
                        o.IsLooping = reader.ReadBoolean();

                        var nodeCount = reader.ReadInt32();
                        for (var i = 0; i < nodeCount; i++)
                        {
                            var node = new ModelPatrolPathNode();
                            var sectorId = reader.ReadInt32();
                            node.Sector = sectors.GetValueOrDefault(sectorId);
                            node.SectorPosition = reader.ReadVec3();
                            o.Nodes.Add(node);
                        }

                        o.IsLoop = reader.ReadBoolean();
                    }
                    break;
                case FleetOrderType.PatrolPath:
                    {
                        var o = (ModelPatrolPathOrder)order;

                        o.PathDirection = reader.ReadInt32();
                        o.IsLooping = reader.ReadBoolean();

                        var patrolPathId = reader.ReadInt32();
                        o.PatrolPath = patrolPaths.GetValueOrDefault(patrolPathId);
                    }
                    break;

                case FleetOrderType.Wait:
                    {
                        var o = (ModelWaitOrder)order;
                        o.WaitTime = reader.ReadSingle();
                    }
                    break;

                case FleetOrderType.AttackTarget:
                    {
                        var o = (ModelAttackTargetOrder)order;
                        var targetUnitId = reader.ReadInt32();
                        o.TargetUnit = units.GetValueOrDefault(targetUnitId);
                        o.AttackPriority = reader.ReadSingle();
                    }
                    break;
                case FleetOrderType.Trade:
                    {
                        var o = (ModelTradeOrder)order;
                        o.MinBuyQuantity = reader.ReadInt32();
                        o.MinBuyCargoPercentage = reader.ReadSingle();
                    }
                    break;
                case FleetOrderType.ManualTrade:
                    {
                        var o = (ModelManualTradeOrder)order;
                        o.MinBuyQuantity = reader.ReadInt32();
                        o.MinBuyCargoPercentage = reader.ReadSingle();

                        var hasTradeRoute = reader.ReadBoolean();
                        if (hasTradeRoute)
                        {
                            o.CustomTradeRoute = CustomTradeRouteReader.Read(reader, units);
                        }
                    }
                    break;
                case FleetOrderType.AutonomousTrade:
                    {
                        var o = (ModelUniverseTradeOrder)order;
                        o.MinBuyQuantity = reader.ReadInt32();
                        o.MinBuyCargoPercentage = reader.ReadSingle();

                        o.TradeOnlySpecificCargoClasses = reader.ReadBoolean();

                        o.TradeSpecificCargoClasses.Clear();
                        var cargoCount = reader.ReadInt32();
                        for (var i = 0; i < cargoCount; i++)
                        {
                            // TODO: Change cargo class id to enum
                            o.TradeSpecificCargoClasses.Add((ModelCargoClass)reader.ReadInt32());
                        }
                    }
                    break;
                case FleetOrderType.JoinFleet:
                    {
                        var o = (ModelJoinFleetOrder)order;
                        var fleetId = reader.ReadInt32();
                        var fleet = fleets.GetValueOrDefault(fleetId);

                        // TODO: Verify fleet valid
                        o.TargetFleet = fleet;
                    }
                    break;

                case FleetOrderType.MoveTo:
                    {
                        var o = (ModelMoveToOrder)order;
                        o.CompleteOnReachTarget = reader.ReadBoolean();
                        o.ArrivalThreshold = reader.ReadSingle();
                        o.MatchTargetOrientation = reader.ReadBoolean();
                        o.PreferredRelativeVectorFromTarget = reader.ReadNullableVec3();

                        var hasTarget = reader.ReadBoolean();
                        if (hasTarget)
                        {
                            o.Target = SectorTargetReader.Read(reader, sectors, units, fleets);
                        }
                    }
                    break;

                case FleetOrderType.Protect:
                    {
                        var o = (ModelProtectOrder)order;
                        o.CompleteOnReachTarget = reader.ReadBoolean();
                        o.ArrivalThreshold = reader.ReadSingle();
                        o.MatchTargetOrientation = reader.ReadBoolean();
                        o.PreferredRelativeVectorFromTarget = reader.ReadNullableVec3();

                        var hasTarget = reader.ReadBoolean();
                        if (hasTarget)
                        {
                            o.Target = SectorTargetReader.Read(reader, sectors, units, fleets);
                        }
                    }
                    break;

                case FleetOrderType.SellCargo:
                    {
                        var o = (ModelSellCargoOrder)order;
                        o.FreeUnitsCompleteThreshold = reader.ReadInt32();
                        o.MinBuyPriceMultiplier = reader.ReadSingle();
                        o.SellOnlyListedCargos = reader.ReadBoolean();
                        o.CompleteWhenNoBuyerFound = reader.ReadBoolean();
                        o.CompleteWhenNoCargoToSell = reader.ReadBoolean();

                        var manualBuyerUnitId = reader.ReadInt32();
                        // TODO: Validate
                        o.ManualBuyerUnit = units.GetValueOrDefault(manualBuyerUnitId);
                        o.CustomSellCargoTime = reader.ReadSingle();

                        var sellCargoClassCount = reader.ReadInt32();
                        for (var i = 0; i < sellCargoClassCount; i++)
                        {
                            // TODO: Validate
                            var cargoClassId = reader.ReadInt32();
                            o.SellCargoClasses.Add((ModelCargoClass)cargoClassId);
                        }

                        o.SellEquipment = reader.ReadBoolean();
                    }
                    break;
                case FleetOrderType.RTB:
                case FleetOrderType.DisposeCargo:
                case FleetOrderType.AutonomousTransportPassengers:
                case FleetOrderType.AutonomousBountyHunterObjective:
                case FleetOrderType.AutonomousRoamLocationsObjective:
                case FleetOrderType.Explore:
                case FleetOrderType.Undock:
                    {
                        // nothing to read/write
                    }
                    break;
                case FleetOrderType.ManualRearm:
                    {
                        var o = (ModelManualRearmFleetOrder)order;
                        o.EquipmentCargoUsage = reader.ReadSingle();
                        o.InsufficientCreditsMode = (InsufficientCreditsMode)reader.ReadInt32();

                        var rearmLocationUnitId = reader.ReadInt32();

                        // TODO: Validate
                        o.RearmLocationUnit = units.GetValueOrDefault(rearmLocationUnitId);
                    }
                    break;
                case FleetOrderType.RearmAtNearest:
                    {
                        var o = (ModelRearmAtNearestFleetOrder)order;
                        o.EquipmentCargoUsage = reader.ReadSingle();
                        o.InsufficientCreditsMode = (InsufficientCreditsMode)reader.ReadInt32();
                    }
                    break;
                case FleetOrderType.ManualRepair:
                    {
                        var o = (ModelManualRepairFleetOrder)order;
                        o.InsufficientCreditsMode = (InsufficientCreditsMode)reader.ReadInt32();

                        var repairLocationUnitId = reader.ReadInt32();

                        // TODO: Validate
                        o.RepairLocationUnit = units.GetValueOrDefault(repairLocationUnitId);
                    }
                    break;
                case FleetOrderType.RepairAtNearest:
                    {
                        var o = (ModelRepairAtNearestStationOrder)order;
                        o.InsufficientCreditsMode = (InsufficientCreditsMode)reader.ReadInt32();
                    }
                    break;
                case FleetOrderType.MoveToNearestFriendlyStation:
                    {
                        var o = (ModelMoveToNearestFriendlyStationOrder)order;
                        o.CompleteOnReachTarget = reader.ReadBoolean();
                    }
                    break;
                case FleetOrderType.EnterWormhole:
                    {
                        var o = (ModelEnterWormholeOrder)order;
                        o.TargetWormhole = reader.ReadUnit(units);
                    }
                    break;
                case FleetOrderType.ExploreSector:
                    {
                        var o = (ModelExploreSectorOrder)order;
                        o.Sector = reader.ReadSector(sectors);
                    }
                    break;
                case FleetOrderType.MoveToSector:
                    {
                        var o = (ModelMoveToSectorOrder)order;
                        o.TargetSector = reader.ReadSector(sectors);
                    }
                    break;
                case FleetOrderType.WaitForAutoRepair:
                    {
                        var o = (ModelWaitForAutoRepairOrder)order;
                        o.HullConditionThreshold = reader.ReadSingle();
                        o.ComponentsConditionThreshold = reader.ReadSingle();
                        o.ShieldConditionThreshold = reader.ReadSingle();
                    }
                    break;
                case FleetOrderType.BuildStation:
                    {
                        var o = (ModelBuildStationOrder)order;
                        o.UnitClass = (ModelUnitClass)reader.ReadInt32();
                        o.Sector = reader.ReadSector(sectors);
                        o.SectorPosition = reader.ReadVec3();
                        o.InsufficientCreditsMode = (InsufficientCreditsMode)reader.ReadInt32();
                    }
                    break;
                case FleetOrderType.ClaimUnit:
                    {
                        var o = (ModelClaimUnitOrder)order;
                        o.Unit = reader.ReadUnit(units);
                    }
                    break;
                default:
                    {
                        throw new Exception($"Unable to read data for objective of type {order.OrderType}. Unknown type");
                    }
            }

            return order;
        }

        public static ModelFleetSpawner ReadFleetSpawner(
            BinaryReader reader,
            Dictionary<int, ModelFaction> factions,
            Dictionary<int, ModelSector> sectors,
            Dictionary<int, ModelUnit> units,
            Dictionary<int, ModelFleet> fleets,
            Dictionary<int, ModelPerson> people,
            Dictionary<int, ModelSectorPatrolPath> patrolPaths)
        {
            var fleetSpawner = new ModelFleetSpawner();
            fleetSpawner.Name = reader.ReadString();

            fleetSpawner.Position = reader.ReadVec3();
            fleetSpawner.Rotation = reader.ReadVec4();

            fleetSpawner.InitialSpawnTimeRandomness = reader.ReadSingle();
            fleetSpawner.SpawnTimeRandomness = reader.ReadSingle();
            fleetSpawner.ShipDesignation = reader.ReadString();
            fleetSpawner.ShipName = reader.ReadString();
            fleetSpawner.NamePrefix = reader.ReadString();
            fleetSpawner.SpawnCounter = reader.ReadInt32();
            fleetSpawner.RespawnWhenNoObjectives = reader.ReadBoolean();
            fleetSpawner.RespawnWhenNoPilots = reader.ReadBoolean();
            fleetSpawner.AllowRespawnInActiveScene = reader.ReadBoolean();
            fleetSpawner.FleetHomeBase = reader.ReadUnit(units);
            fleetSpawner.FleetHomeSector = reader.ReadSector(sectors);
            fleetSpawner.OwnerFaction = reader.ReadFaction(factions);
            fleetSpawner.Sector = reader.ReadSector(sectors);
            fleetSpawner.SpawnDock = reader.ReadUnit(units);
            fleetSpawner.NextSpawnTime = reader.ReadDouble();
            fleetSpawner.MinTimeBeforeSpawn = reader.ReadSingle();
            fleetSpawner.MaxTimeBeforeSpawn = reader.ReadSingle();
            fleetSpawner.MinGroupUnitCount = reader.ReadInt32();
            fleetSpawner.MaxGroupUnitCount = reader.ReadInt32();
            fleetSpawner.SpawnedFleet = reader.ReadFleet(fleets);

            var unitClassCount = reader.ReadInt32();
            for (var i = 0; i < unitClassCount; i++)
            {
                fleetSpawner.UnitClasses.Add((ModelUnitClass)reader.ReadInt32());
            }

            var pilotCount = reader.ReadInt32();
            for (var i = 0; i < pilotCount; i++)
            {
                fleetSpawner.PilotResourceNames.Add(reader.ReadString());
            }

            fleetSpawner.FleetResourceName = reader.ReadString();

            var orderCount = reader.ReadInt32();
            for (var i = 0; i < orderCount; i++)
            {
                var order = ReadOrder(
                    reader,
                    factions,
                    sectors,
                    units,
                    fleets,
                    patrolPaths);

                fleetSpawner.Orders.Add(order);
            }

            return fleetSpawner;
        }
    }
}
