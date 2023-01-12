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

        public static bool IsMinorStation(this ModelUnitClass unitClass)
        {
            switch (unitClass)
            {
                case ModelUnitClass.Station_LightWeaponsPlatform:
                case ModelUnitClass.Station_MediumWeaponsPlatform:
                case ModelUnitClass.Station_SmallSatellite:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsMajorStation(this ModelUnitClass unitClass)
        {
            return IsStation(unitClass) && !IsMinorStation(unitClass);
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

        public static bool IsPlanet(this ModelUnitClass unitClass)
        {
            return unitClass.ToString().StartsWith("Planet");
        }

        public static bool IsGasCloud(this ModelUnitClass unitClass)
        {
            return unitClass.ToString().StartsWith("GasCloud");
        }

        public static bool IsAsteroid(this ModelUnitClass unitClass)
        {
            switch (unitClass)
            {
                case ModelUnitClass.Asteroid_TypeB:
                case ModelUnitClass.Asteroid_TypeH:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsAsteroidCluster(this ModelUnitClass unitClass)
        {
            switch (unitClass)
            {
                case ModelUnitClass.AsteroidCluster_TypeB:
                case ModelUnitClass.AsteroidCluster_TypeH:
                    return true;
                default:
                    return false;
            }
        }
    }
}
