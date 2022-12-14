namespace Pixelfactor.IP.Common.Factions
{
    [System.Flags]
    public enum FactionStrategy
    {
        Unspecified = 0,
        Any = 0xFFFF,
        War = 1,
        Scout = 2,
        Trade = 4,
        Scavenge = 8,
        Mine = 16,
        BountyHunt = 32,
        DealEquipment = 64,
        PassengerTransport = 128,
        /// <summary>
        /// Note: not currently used
        /// </summary>
        Explore = 256,
        Escort = 512
    }
}
