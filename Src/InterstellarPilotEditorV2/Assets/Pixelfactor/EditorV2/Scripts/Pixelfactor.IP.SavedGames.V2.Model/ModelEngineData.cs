﻿namespace Pixelfactor.IP.SavedGames.V2.Model
{
    /// <summary>
    /// Holds stats on unique ID counters. Although not essential, preserving these ensures the engine never hands out the same idea twice for the life of a game
    /// </summary>
    public class ModelEngineData
    {
        public int UnitIdCounter { get; set; } = 100000;
        public int PlayerMessageIdCounter { get; set; } = 100000;
        public int PersonIdCounter { get; set; } = 100000;
        public int FactionIdCounter { get; set; } = 100000;
        public int FleetOrderIdCounter { get; set; } = 100000;
        public int PassengerGroupIdCounter { get; set; } = 100000;
        public int JobIdCounter { get; set; } = 100000;
        public int FleetIdCounter { get; set; } = 100000;
        public int SectorIdCounter { get; set; } = 100000;
        public int MissionIdCounter { get; set; } = 100000;
        public int PatrolPathIdCounter { get; set; } = 100000;
        public int MissionObjectiveIdCounter { get; set; } = 100000;
    }
}
