using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    public class EditorNpcPilotSettings : MonoBehaviour
    {
        [Tooltip("Determines the likelihood that this NPC will use missiles and mines")]
        [Range(0f, 1f)]
        public float RestrictedWeaponPreference = 0.5f;

        /// <summary>
        /// 0 - 1, 1 = best. Not used so often in 1.6.x
        /// </summary>
        [Tooltip("Determines how good this NPC is in combat")]
        [Range(0f, 1f)]
        public float CombatEfficiency = 0.5f;

        /// <summary>
        /// When true, npc will not run out of ammo
        /// </summary>
        [Tooltip("Determines whether this NPC can create ammo out of thin air")]
        public bool CheatAmmo = false;

        /// <summary>
        /// Whether the NPC can abandon ships when heavily damaged
        /// </summary>
        [Tooltip("Determines whether the NPC can abandon their ship. Note that ship abandoned can be turned off globally in scenario options")]
        public bool AllowDitchShip = true;
    }
}
