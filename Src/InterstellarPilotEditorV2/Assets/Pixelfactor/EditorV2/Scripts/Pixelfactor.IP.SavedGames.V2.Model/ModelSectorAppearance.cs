using Pixelfactor.IP.Common;

namespace Pixelfactor.IP.SavedGames.V2.Model
{
    public class ModelSectorAppearance
    {
        public NebulaBrightness NebulaBrightness = NebulaBrightness.BRIGHT | NebulaBrightness.DARK | NebulaBrightness.MEDIUM | NebulaBrightness.VERY_BRIGHT | NebulaBrightness.VERY_DARK;
        public NebulaColour NebulaColors = NebulaColour.BLUE | NebulaColour.CYAN | NebulaColour.GREEN | NebulaColour.ORANGE | NebulaColour.PINK | NebulaColour.PURPLE | NebulaColour.RED | NebulaColour.YELLOW;
        public NebulaStyle NebulaStyles = NebulaStyle.CLOUDY | NebulaStyle.DARKMATTER | NebulaStyle.GLITTERY | NebulaStyle.STREAKY;
        public float NebulaComplexity = 0.6f;
        public int NebulaCount = 28;
        public int NebulaTextureCount = 10;
        public float StarsIntensity = 1.0f;
        public StarsCount StarsCount = StarsCount.MEDIUM | StarsCount.HIGH;
    }
}
