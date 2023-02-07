﻿using Pixelfactor.IP.Common;
using Pixelfactor.IP.SavedGames.V2.Model.Jobs;
using Pixelfactor.IP.SavedGames.V2.Model.Triggers;
using System.Collections.Generic;

namespace Pixelfactor.IP.SavedGames.V2.Model
{
    public class SavedGame : ISavedGame
    {
        public ModelHeader Header { get; set; }

        /// <summary>
        /// Optional long description of the scenario that is displayed in the mission log part of the scenarios list
        /// </summary>
        public string MissionLog { get; set; } = null;

        public List<ModelPerson> People { get; } = new List<ModelPerson>(1000);
        public List<ModelSector> Sectors { get; } = new List<ModelSector>(128);

        public List<ModelFaction> Factions { get; } = new List<ModelFaction>(1000);

        /// <summary>
        /// Not used in custom universe
        /// </summary>
        public List<ModelSectorPatrolPath> PatrolPaths { get; } = new List<ModelSectorPatrolPath>(20);

        public List<ModelUnit> Units { get; } = new List<ModelUnit>(2000);

        public List<ModelFleet> Fleets { get; } = new List<ModelFleet>(1000);

        /// <summary>
        /// These were mainly used by scenarios to artificially inject some ships on a timer. The sandbox is now able to spawn fleets via the faction AI/>
        /// </summary>
        public List<ModelFleetSpawner> FleetSpawners { get; } = new List<ModelFleetSpawner>(10);

        public ModelPlayer Player { get; set; }

        public List<ModelMission> Missions { get; } = new List<ModelMission>(200);
        public ModelUnit CurrentHudTarget { get; set; }
        public ModelScenarioData ScenarioData { get; set; }
        public List<ModelMoon> Moons { get; } = new List<ModelMoon>(40);
        public ModelSeedOptions SeedOptions { get; set; }

        public List<ModelTriggerGroup> TriggerGroups { get; set; } = new List<ModelTriggerGroup>();

        public ModelEngineData EngineData { get; set; }

        /// <summary>
        /// When NPC bails out of ship and leaves it, eventually the unit is destroyed
        /// </summary>
        public List<ModelDitchedUnit> DitchedUnitsToBeCleanedUp = new List<ModelDitchedUnit>(10);

        public List<ModelPlayerUnitFleetSettings> PlayerUnitFleetSettingItems { get; set; } = new List<ModelPlayerUnitFleetSettings>(8);

        public ModelPlayerFleetSettingsCombined PlayerDefaultFleetSettings { get; set; }

        public List<ModelTractorerDataItem> TractorerDataItems { get; set; } = new List<ModelTractorerDataItem>();
    }
}
