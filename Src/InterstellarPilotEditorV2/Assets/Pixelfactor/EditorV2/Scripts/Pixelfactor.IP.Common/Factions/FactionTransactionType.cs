namespace Pixelfactor.IP.Common.Factions
{
    public enum FactionTransactionType
    {
        Unknown,
        ShipRepairs,
        ShipPurchase,
        EquipmentPurchase,
        Trade,
        Tax,
        Gift,
        Salvage,
        Mission,
        MiscPurchase,
        PassengerFare,
        Bounty,
        Tribute,
        Scratchcard,
        /// <summary>
        /// Not used
        /// </summary>
        SectorIncome,
        StationBuild,
        /// <summary>
        /// Unit dismantled
        /// </summary>
        UnitDismantled,
        /// <summary>
        /// Giving/receiving money from fleet
        /// </summary>
        FleetTransfer
    }
}
