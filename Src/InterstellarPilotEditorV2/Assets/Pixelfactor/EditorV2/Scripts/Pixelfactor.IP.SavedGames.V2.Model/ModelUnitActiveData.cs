namespace Pixelfactor.IP.SavedGames.V2.Model
{
    /// <summary>
    /// Some data about a ship when it is in the "active" sector.<br />
    /// </summary>
    public class ModelUnitActiveData
    {
        public Vec3 Velocity { get; set; }
        public float CurrentTurn { get; set; }
    }
}
