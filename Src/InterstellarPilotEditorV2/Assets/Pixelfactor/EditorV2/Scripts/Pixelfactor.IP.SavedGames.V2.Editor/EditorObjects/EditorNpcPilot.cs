using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    /// <summary>
    /// Represents a person in the game that is controlled by an NPC/AI
    /// </summary>
    public class EditorNpcPilot : MonoBehaviour
    {
        [Tooltip("The fleet that the pilot belongs to. This can be left null and by convention the fleet will be found if the pilot is nested underneath it")]
        public EditorFleet Fleet = null;
    }
}
