namespace Pixelfactor.IP.SavedGames.V2.Model
{
    /// <summary>
    /// Defines a unit that is pulling another one with a tractor beam
    /// </summary>
    public class ModelTractorerDataItem
    {
        public ModelUnit TractoredUnit { get; set; }
        public ModelUnit TractoringUnit { get; set; }
    }
}
