using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.Scripting.Triggers
{
    public class EditorTrigger_Base : MonoBehaviour
    {
        /// <summary>
        /// Whether to invert the trigger. i.e. it will fire when the conditions are NOT true
        /// </summary>
        public bool Invert = false;
    }
}
