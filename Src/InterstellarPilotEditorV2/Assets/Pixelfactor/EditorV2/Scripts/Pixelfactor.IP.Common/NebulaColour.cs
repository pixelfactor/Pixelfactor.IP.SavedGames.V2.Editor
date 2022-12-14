namespace Pixelfactor.IP.Common
{
    [System.Flags]
    public enum NebulaColour
    {
        NONE = 0,
        BLUE = 1,
        PINK = 2,
        PURPLE = 4,
        GREEN = 8,
        YELLOW = 16,
        ORANGE = 32,
        RED = 64,
        CYAN = 128
    }

    public static class NebulaColours
    {
        public static NebulaColour All
        {
            get
            {
                return NebulaColour.BLUE | NebulaColour.PINK | NebulaColour.PURPLE | NebulaColour.GREEN | NebulaColour.YELLOW | NebulaColour.ORANGE | NebulaColour.RED | NebulaColour.CYAN;
            }
        }
    }
}
