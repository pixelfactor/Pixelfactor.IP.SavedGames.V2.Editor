namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    using UnityEngine;

    public class EditorShieldComponentClass : EditorComponentClass
    {
        /// <summary>
        /// The max capacity of the shield
        /// </summary>
        public float[] Capacities = new float[6];

        /// <summary>
        /// The amount of energy required to regenerate 1 shield point
        /// </summary>
        public float RegenEnergyCost = 1.5f;

        /// <summary>
        /// The maximum regeneration of the shields capacity in units per second
        /// </summary>
        public float RegenRateMultiplier = 10.0f;

        /// <summary>
        /// How much capacity a shield has when it is restored (as a multiplier of the value calculated by the engine)
        /// </summary>
        public float ShieldRestoreMultiplier = 1.0f;
        public float TotalCapacity = -1f;
    }
}