namespace Pixelfactor.IP.SavedGames.V2.Model
{
    public class ModelNpcPilotSettings
    {
        public float RestrictedWeaponPreference { get; set; }

        /// <summary>
        /// 0 - 1, 1 = best. Not used so often in 1.6.x
        /// </summary>
        public float CombatEfficiency { get; set; }

        /// <summary>
        /// When true, npc will not run out of ammo
        /// </summary>
        public bool CheatAmmo { get; set; }

        public bool AllowDitchShip { get; set; } = true;
    }
}
