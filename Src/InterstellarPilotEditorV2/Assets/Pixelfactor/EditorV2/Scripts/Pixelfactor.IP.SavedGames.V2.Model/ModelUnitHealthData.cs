namespace Pixelfactor.IP.SavedGames.V2.Model
{
    public class ModelUnitHealthData
    {
        /// <summary>
        /// Do not use!
        /// </summary>
        public bool IsDestroyed { get; set; }

        /// <summary>
        /// Current health of the unit. Any value up to the max health defined by the unit class
        /// </summary>
        public float Health { get; set; }
    }
}
