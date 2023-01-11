using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;

namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    public static class EditorUnitExtensions
    {
        public static bool CanHaveFaction(this EditorUnit unit)
        {
            return unit.ModelUnitClass.IsShipOrStation() || unit.ModelUnitClass.IsCargo();
        }
    }
}
