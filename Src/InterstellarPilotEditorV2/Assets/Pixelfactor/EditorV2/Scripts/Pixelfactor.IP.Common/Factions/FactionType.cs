namespace Pixelfactor.IP.Common.Factions
{
    [System.Flags]
    public enum FactionType
    {
        None,
        Trader = (1 << 0),
        Scavenger = (1 << 1),
        Miner = (1 << 2),
        /// <summary>
        /// Hunts outlaws
        /// </summary>
        BountyHunter = (1 << 3),
        /// <summary>
        /// Neutral faction that creates stations
        /// </summary>
        StationBuilder = (1 << 4),
        Empire = (1 << 5),
        /// <summary>
        /// A faction that will attack anyone for any reason
        /// </summary>
        Bandit = (1 << 6),
        PassengerTransport = (1 << 7),
        Explorer = (1 << 8),
        EquipmentDealer = (1 << 9),
        Mercenary = (1 << 10),
        /// <summary>
        /// A faction that raids other factions
        /// </summary>
        Outlaw = (1 << 11),

        /// <summary>
        /// A faction that controls a sector
        /// </summary>
        Security = (1 << 12),

        /// <summary>
        /// A faction that owns a station
        /// </summary>
        Bar = (1 << 13),
        Player = (1 << 14),
        Generic = (1 << 15),
    }
}
