namespace Pixelfactor.IP.SavedGames.V2.Model
{
    public class ModelComponentBayMapItem
    {
        /// <summary>
        /// The id local to the ship this bay is placed on
        /// </summary>
        public int Id { get; set; }
        public ModelComponentBayType Type { get; set; }
        public string Name { get; set; }
    }
}
