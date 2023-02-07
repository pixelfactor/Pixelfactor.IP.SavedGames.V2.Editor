using Pixelfactor.IP.Common;
using System;

namespace Pixelfactor.IP.SavedGames.V2.Model
{
    public class ModelHeader : ISavedGameHeader
    {
        /// <summary>
        /// The "save" version. Not guaranteed to be the same as the game version but normally close.
        /// </summary>
        public Version SaveVersion { get; set; }
        
        /// <summary>
        /// Gets or sets the version of the game engine that the save was created with
        /// </summary>
        public Version GameVersion { get; set; }

        /// <summary>
        /// The version of the game that the save file was created in
        /// </summary>
        public Version CreatedVersion { get; set; }

        public bool IsAutoSave { get; set; }

        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Determines what unity scene to load up - don't change
        /// </summary>
        public int ScenarioInfoId { get; set; }

        /// <summary>
        /// The number of times saved since the game was installed
        /// </summary>
        public int GlobalSaveNumber { get; set; }

        /// <summary>
        /// The number of times saved within this specific game
        /// </summary>
        public int SaveNumber { get; set; }

        /// <summary>
        /// Whether the game has a player. Almost always yes. If so, additional info is saved
        /// </summary>
        public bool HavePlayer { get; set; }

        /// <summary>
        /// The sector the player is in<br />
        /// Header info only - does not affect the loaded game
        /// </summary>
        public string PlayerSectorName { get; set; }

        /// <summary>
        /// Header info only - does not affect the loaded game
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// Header info only - does not affect the loaded game
        /// </summary>
        public long Credits { get; set; } = 0;

        public long NetWorth { get; set; } = 0;
        /// <summary>
        /// Header info only - does not affect the loaded game
        /// </summary>
        public string FactionName { get; set; }

        public bool Permadeath { get; set; } = false;

        public DateTime GameStartDate { get; set; }

        /// <summary>
        /// Seconds since start of game
        /// </summary>
        public double SecondsElapsed { get; set; }

        /// <summary>
        /// Optional custom name of the scenario
        /// </summary>
        public string ScenarioTitle { get; set; }

        /// <summary>
        /// Optional author of the scenario
        /// </summary>
        public string ScenarioAuthor { get; set; }

        /// <summary>
        /// Optional tool that authored the scenario
        /// </summary>
        public string ScenarioAuthoringTool { get; set; }

        public string ScenarioDescription { get; set; }
    }
}
