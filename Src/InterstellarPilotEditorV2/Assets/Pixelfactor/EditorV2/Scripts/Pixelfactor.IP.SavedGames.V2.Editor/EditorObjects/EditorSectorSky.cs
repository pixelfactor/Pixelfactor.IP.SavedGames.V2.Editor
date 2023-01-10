using Pixelfactor.IP.Common;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    /// <summary>
    /// Defines the appearance of the sector backdrop
    /// </summary>
    public class EditorSectorSky : MonoBehaviour
    {
        [Tooltip("Collection of possible colours for the nebulae")]
        public NebulaColour NebulaColors = NebulaColour.BLUE | NebulaColour.CYAN | NebulaColour.GREEN | NebulaColour.ORANGE | NebulaColour.PINK | NebulaColour.PURPLE | NebulaColour.RED | NebulaColour.YELLOW;

        [Tooltip("The max number of nebulae generated. Generally should keep below 64 for performance")]
        [Range(0, 128)]
        public int NebulaCount = 28;

        [Tooltip("Determines brightness of the stars")]
        [Range(0.0f, 2.0f)]
        public float StarsIntensity = 1.0f;

        [Tooltip("Determines the collection of textures used for the stars")]
        public StarsCount StarsCount = StarsCount.MEDIUM | StarsCount.HIGH;

        [Tooltip("Determines the range of different textures used for the nebulae")]
        [Range(1, 20)]
        public int NebulaTextureCount = 10;

        [Tooltip("Determines the range of different textures used for the nebulae")]
        public NebulaBrightness NebulaBrightness = NebulaBrightness.BRIGHT | NebulaBrightness.DARK | NebulaBrightness.MEDIUM | NebulaBrightness.VERY_BRIGHT | NebulaBrightness.VERY_DARK;

        /// <summary>
        /// Not currently used
        /// </summary>
        [HideInInspector]
        public float NebulaComplexity = 0.6f;

        [Tooltip("Determines the range of different textures used for the nebulae")]
        public NebulaStyle NebulaStyles = NebulaStyle.CLOUDY | NebulaStyle.DARKMATTER | NebulaStyle.GLITTERY | NebulaStyle.STREAKY;
    }
}
