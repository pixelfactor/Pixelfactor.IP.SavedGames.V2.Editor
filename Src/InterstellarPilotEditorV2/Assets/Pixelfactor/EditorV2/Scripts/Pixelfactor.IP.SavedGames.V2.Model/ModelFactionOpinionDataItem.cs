namespace Pixelfactor.IP.SavedGames.V2.Model
{
    /// <summary>
    /// Determines what a faction thinks of another
    /// </summary>
    public class ModelFactionOpinionDataItem
    {
        public ModelFaction OtherFaction { get; set; }

        /// <summary>
        /// 0 - 1, 0 = worst, 1 = best
        /// </summary>
        public float Opinion { get; set; }

        public double CreatedTime { get; set; } = -1d;
    }
}
