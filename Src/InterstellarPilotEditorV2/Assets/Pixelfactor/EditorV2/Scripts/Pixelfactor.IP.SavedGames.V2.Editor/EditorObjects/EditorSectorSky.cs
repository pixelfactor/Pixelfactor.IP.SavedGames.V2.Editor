using Pixelfactor.IP.Common;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    /// <summary>
    /// Defines the appearance of the sector backdrop
    /// </summary>
    public class EditorSectorSky : MonoBehaviour
    {
        public NebulaColour NebulaColors = NebulaColour.BLUE | NebulaColour.CYAN | NebulaColour.GREEN | NebulaColour.ORANGE | NebulaColour.PINK | NebulaColour.PURPLE | NebulaColour.RED | NebulaColour.YELLOW;
        [Range(0, 128)]
        public int NebulaCount = 28;
        [Range(0.0f, 2.0f)]
        public float StarsIntensity = 1.0f;
    }
}
