namespace Pixelfactor.IP.SavedGames.V2.Model
{
    /// <summary>
    /// Not used in game version 1.6.x
    /// </summary>
    public class ModelUnitDebrisData
    {
        public int ScrapQuantity { get; set; }
        public bool Expires { get; set; }
        public double ExpiryTime { get; set; }
        public ModelUnitClass RelatedUnitClass { get; set; }
    }
}
