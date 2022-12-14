namespace Pixelfactor.IP.Common.FleetOrders
{
    public enum FleetOrderType
    {
        None = 0,
        CollectCargo = 1,
        Dock = 2,

        /// <summary>
        /// Manual Path
        /// </summary>
        Patrol = 3,
        PatrolPath = 4,
        /// <summary>
        /// Return to base
        /// </summary>
        RTB = 5,
        Scavenge = 6,
        Wait = 7,
        AttackTarget = 8,
        AttackGroup = 9,
        AutonomousTrade = 10,
        ManualTrade = 11,
        Mine = 12,
        MoveTo = 13,
        SellCargo = 14,
        Trade = 15,
        JoinFleet = 16,
        DisposeCargo = 17,
        Protect = 18,
        AutonomousTransportPassengers = 19,
        AutonomousRoamLocationsObjective = 20,
        AutonomousBountyHunterObjective = 21,
        ManualRepair = 22,
        Explore = 23,
        RepairAtNearest = 24,
        MoveToNearestFriendlyStation = 25,
        EnterWormhole = 26,
        ExploreSector = 27,
        Undock = 28,
        MoveToSector = 29,
        WaitForAutoRepair = 30,
        Rearm = 31,
        /// <summary>
        /// Ordering a fleet to rearm itself at a specific location
        /// </summary>
        ManualRearm = 32,
        RearmAtNearest = 33,
        BuildStation = 34,
        ClaimUnit = 35
    }
}