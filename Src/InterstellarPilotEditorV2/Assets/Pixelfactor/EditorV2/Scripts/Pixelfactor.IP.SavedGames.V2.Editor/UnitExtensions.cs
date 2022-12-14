using Pixelfactor.IP.SavedGames.V2.Model;

namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    public static class UnitExtensions
    {
        public static bool IsStation(this ModelUnit unit)
        {
            return unit.Class.IsStation();
        }

        public static bool IsShip(this ModelUnit unit)
        {
            return unit.Class.IsShip();
        }

        public static bool IsShipOrStation(this ModelUnit unit)
        {
            return unit.Class.IsShipOrStation();
        }

        public static bool IsWormhole(this ModelUnit unit)
        {
            return unit.Class.IsWormhole();
        }

        public static bool IsCargo(this ModelUnit unit)
        {
            return unit.Class.IsCargo();
        }
    }
}
