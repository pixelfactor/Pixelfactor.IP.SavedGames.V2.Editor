using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    public static class EditorUnitExtensions
    {
        public static EditorFleet GetFleet(this EditorUnit unit)
        {
            var npcPilot = unit.transform.GetComponentInImmediateChildren<EditorNpcPilot>();
            if (npcPilot != null && npcPilot.Fleet != null)
            {
                return npcPilot.Fleet;
            }

            return unit.FindFleetInParent();
        }

        public static EditorFleet FindFleetInParent(this EditorUnit unit)
        {
            var transform = unit.transform.parent;
            while (transform != null)
            {
                if (transform.GetComponent<EditorUnit>() != null)
                    return null;

                var fleet = transform.GetComponent<EditorFleet>();
                if (fleet != null)
                    return fleet;

                transform = transform.parent;
            }

            return null;
        }

        public static bool IsStableWormhole(this EditorUnit unit)
        {
            var wormholeData = unit.GetComponent<EditorWormholeUnit>();
            return wormholeData != null && !wormholeData.IsUnstable;
        }

        public static bool IsUnstableWormhole(this EditorUnit unit)
        {
            var wormholeData = unit.GetComponent<EditorWormholeUnit>();
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

        public static EditorSector GetSector(this EditorUnit unit)
        {
            return unit.GetComponentInParent<EditorSector>();
        }

        public static List<T> FindChildrenExcludingUnits<T>(this EditorUnit unit) where T : Component
        {
            var list = new List<T>();

            FindChildrenExcludingUnitsInternal<T>(unit.transform, list);

            return list;
        }

        public static void FindChildrenExcludingUnitsInternal<T>(Transform transform, List<T> list) where T : Component
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);

                var unit = child.GetComponent<EditorUnit>();
                if (unit == null)
                {
                    var t = child.GetComponent<T>();
                    if (t != null)
                        list.Add(t);

                    FindChildrenExcludingUnitsInternal<T>(child.transform, list);
                }

            }
        }
    }
}
