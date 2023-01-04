using Pixelfactor.IP.SavedGames.V2.Model;

namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    public static class UnitClassExtensions
    {
        public static bool IsStation(this ModelUnitClass unitClass)
        {
            return unitClass.ToString().StartsWith("Station");
        }

        public static bool IsShip(this ModelUnitClass unitClass)
        {
            return unitClass.ToString().StartsWith("Ship");
        }

        public static bool IsShipOrStation(this ModelUnitClass unitClass)
        {
            return IsStation(unitClass) || IsShip(unitClass);
        }

        public static bool IsTurret(this ModelUnitClass unitClass)
        {
            return unitClass == ModelUnitClass.Station_MediumWeaponsPlatform || unitClass == ModelUnitClass.Station_LightWeaponsPlatform;
        }

        public static bool IsWormhole(this ModelUnitClass unitClass)
        {
            return unitClass.ToString().StartsWith("Wormhole");
        }

        public static bool IsCargo(this ModelUnitClass unitClass)
        {
            return unitClass.ToString().StartsWith("Cargo");
        }
    }
}
