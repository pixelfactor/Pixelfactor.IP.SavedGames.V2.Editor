using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    public class EditorNpcPilotSettings : MonoBehaviour
    {
        public bool UsesCloak = true;

        public float UseCloakPreference = 0.5f;

        public float EnterCombatCloakedProbability = 0.5f;

        public float RestrictedWeaponPreference = 0.5f;

        /// <summary>
        /// 0 - 1, 1 = best. Not used so often in 1.6.x
        /// </summary>
        public float CombatEfficiency = 0.5f;

        /// <summary>
        /// When true, npc will not run out of ammo
        /// </summary>
        public bool CheatAmmo = false;

        /// <summary>
        /// Whether the NPC can abandon ships when heavily damaged
        /// </summary>
        public bool AllowDitchShip = true;
    }
}
