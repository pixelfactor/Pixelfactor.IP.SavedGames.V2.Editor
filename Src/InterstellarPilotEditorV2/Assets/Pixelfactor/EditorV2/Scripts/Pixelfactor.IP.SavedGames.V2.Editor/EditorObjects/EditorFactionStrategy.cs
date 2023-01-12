namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    /// <summary>
    /// Defines the type of things that a faction or ship can do
    /// </summary>
    public enum EditorFactionStrategy
    {
        None = 0,
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
