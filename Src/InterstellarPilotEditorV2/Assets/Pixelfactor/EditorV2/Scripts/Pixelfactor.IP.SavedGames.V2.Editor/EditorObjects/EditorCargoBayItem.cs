namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    /// <summary>
    /// Defines a quantity of cargo
    /// </summary>
    [System.Serializable]
    public class EditorCargoBayItem
    {
        public EditorCargoClassRef CargoClass = null;

        public int Quantity = 0;

        public EditorCargoBayItem Clone()
        {
            return new EditorCargoBayItem()
            {
                CargoClass = this.CargoClass,
                Quantity = this.Quantity
            };
        }
    }
}