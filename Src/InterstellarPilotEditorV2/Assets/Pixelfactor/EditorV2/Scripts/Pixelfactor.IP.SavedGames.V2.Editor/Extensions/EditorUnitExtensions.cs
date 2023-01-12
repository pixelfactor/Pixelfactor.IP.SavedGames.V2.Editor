using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;

namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    public static class EditorUnitExtensions
    {
        public static bool IsStableWormhole(this EditorUnit unit)
        {
            var wormholeData = unit.GetComponent<EditorUnitWormholeData>();
            return wormholeData != null && !wormholeData.IsUnstable;
        }

        public static bool IsUnstableWormhole(this EditorUnit unit)
        {
            var wormholeData = unit.GetComponent<EditorUnitWormholeData>();
            return wormholeData != null && wormholeData.IsUnstable;
        }

        public static bool IsStation(this EditorUnit unit)
        {
            return unit.ModelUnitClass.IsStation();
        }

        public static bool IsShip(this EditorUnit unit)
        {
            return unit.ModelUnitClass.IsShip();
        }

        public static bool IsAbandonedShip(this EditorUnit unit)
        {
            return unit.IsShip() && unit.Faction == null;
        }

        public static bool IsAbandonedStation(this EditorUnit unit)
        {
            return unit.IsStation() && unit.Faction == null;
        }

        public static bool CanHaveFaction(this EditorUnit unit)
        {
            return unit.ModelUnitClass.IsShipOrStation() || unit.ModelUnitClass.IsCargo();
        }

        public static bool IsMajorStation(this EditorUnit unit)
        {
            return unit.ModelUnitClass.IsMajorStation();
        }

        public static bool IsMinorStation(this EditorUnit unit)
        {
            return unit.ModelUnitClass.IsMinorStation();
        }

        public static bool IsAsteroidCluster(this EditorUnit unit)
        {
            return unit.ModelUnitClass.IsAsteroidCluster();
        }

        public static bool IsAsteroid(this EditorUnit unit)
        {
            return unit.ModelUnitClass.IsAsteroid();
        }

        public static bool IsCargoContainer(this EditorUnit unit)
        {
            return unit.ModelUnitClass.IsCargo();
        }

        public static bool IsPlanet(this EditorUnit unit)
        {
            return unit.ModelUnitClass.IsPlanet();
        }

        public static bool IsGasCloud(this EditorUnit unit)
        {
            return unit.ModelUnitClass.IsGasCloud();
        }
    }
}
