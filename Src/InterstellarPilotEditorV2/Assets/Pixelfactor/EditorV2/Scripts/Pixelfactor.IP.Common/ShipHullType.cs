namespace Pixelfactor.IP
{
    [System.Flags]
    public enum ShipHullType
    {
        None = 0,
        Any = 0xFFFF,
        Scout = 1,
        Fighter = 2,
        Frigate = 4,
        Destroyer = 8,
        Cruiser = 16,
        Battleship = 32,
        Station = 64,
        Other = 128
    }
}
