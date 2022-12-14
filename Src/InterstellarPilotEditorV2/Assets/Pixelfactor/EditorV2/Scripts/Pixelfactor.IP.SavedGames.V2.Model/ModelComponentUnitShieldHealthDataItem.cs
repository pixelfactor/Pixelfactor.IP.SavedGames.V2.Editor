namespace Pixelfactor.IP.SavedGames.V2.Model
{
    /// <summary>
    /// Data for of the hex shield points
    /// </summary>
    public class ModelComponentUnitShieldHealthDataItem
    {
        public int ShieldPointIndex { get; set; }

        /// <summary>
        /// 0 - 1
        /// </summary>
        public float Health { get; set; }
    }
}
