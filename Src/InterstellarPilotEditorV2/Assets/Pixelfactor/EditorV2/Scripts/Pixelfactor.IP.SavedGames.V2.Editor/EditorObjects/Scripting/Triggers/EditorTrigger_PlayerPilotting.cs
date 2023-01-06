using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.Scripting.Triggers
{
    /// <summary>
    /// Trigger fired when the player is pilotting a ship
    /// </summary>
    public class EditorTrigger_PlayerPilotting : EditorTrigger_Base
    {
        /// <summary>
        /// When true, the trigger only fires when the HUD is active
        /// </summary>
        public bool WaitForHud = true;
    }
}
