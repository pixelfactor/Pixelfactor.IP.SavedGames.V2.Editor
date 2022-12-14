namespace Pixelfactor.IP.SavedGames.V2.Model
{
    /// <summary>
    /// A single item with a <see cref="ModelFleetSpawnParams"/>
    /// </summary>
    public class ModelFleetSpawnParamsItem
    {
        public string PilotResourceName { get; set; }
        public string ShipName { get; set; }
        public ModelUnitClass UnitClass { get; set; }
        public bool AddCargoLoadout { get; set; } = true;
    }
}
