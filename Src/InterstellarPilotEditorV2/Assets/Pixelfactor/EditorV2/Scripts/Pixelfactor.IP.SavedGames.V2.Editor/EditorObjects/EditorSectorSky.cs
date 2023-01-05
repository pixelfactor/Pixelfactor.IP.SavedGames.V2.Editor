using Pixelfactor.IP.Common;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    /// <summary>
    /// Defines the appearance of the sector backdrop
    /// </summary>
    public class EditorSectorSky : MonoBehaviour
    {
        public NebulaBrightness NebulaBrightness = NebulaBrightness.BRIGHT | NebulaBrightness.DARK | NebulaBrightness.MEDIUM | NebulaBrightness.VERY_BRIGHT | NebulaBrightness.VERY_DARK;
        public NebulaColour NebulaColors = NebulaColour.BLUE | NebulaColour.CYAN | NebulaColour.GREEN | NebulaColour.ORANGE | NebulaColour.PINK | NebulaColour.PURPLE | NebulaColour.RED | NebulaColour.YELLOW;
        public NebulaStyle NebulaStyles = NebulaStyle.CLOUDY | NebulaStyle.DARKMATTER | NebulaStyle.GLITTERY | NebulaStyle.STREAKY;
        [Range(0, 128)]
        public int NebulaCount = 28;
        [Range(0, 32)]
        public int NebulaTextureCount = 10;
        [Range(0.0f, 2.0f)]
        public float StarsIntensity = 1.0f;
        public StarsCount StarsCount = StarsCount.MEDIUM | StarsCount.HIGH;
    }
}
