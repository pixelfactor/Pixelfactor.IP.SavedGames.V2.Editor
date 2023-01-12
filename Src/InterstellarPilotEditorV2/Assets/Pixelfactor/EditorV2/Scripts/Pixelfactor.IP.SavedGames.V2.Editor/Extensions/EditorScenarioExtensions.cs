using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using System.Collections.Generic;
using System.Linq;

namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    public static class EditorSavedGameExtensions
    {
        public static EditorUnit[] GetStableWormholes(this EditorScenario editorScenario)
        {
            return editorScenario.GetComponentsInChildren<EditorUnit>().Where(e => e.IsStableWormhole()).ToArray();
        }

        public static int GetStableWormholeCount(this EditorScenario editorScenario)
        {
            return GetStableWormholes(editorScenario).Length;
        }

        public static EditorUnit[] GetUnstableWormholes(this EditorScenario editorScenario)
        {
            return editorScenario.GetComponentsInChildren<EditorUnit>().Where(e => e.IsUnstableWormhole()).ToArray();
        }

        public static int GetUnstableWormholeCount(this EditorScenario editorScenario)
        {
            return GetUnstableWormholes(editorScenario).Length;
        }

        public static EditorPerson[] GetPeople(this EditorScenario editorScenario)
        {
            return editorScenario.GetComponentsInChildren<EditorPerson>();
        }

        public static int GetPersonCount(this EditorScenario editorScenario)
        {
            return GetPeople(editorScenario).Length;
        }

        public static EditorFleet[] GetFleets(this EditorScenario editorScenario)
        {
            return editorScenario.GetComponentsInChildren<EditorFleet>();
        }

        public static int GetFleetCount(this EditorScenario editorScenario)
        {
            return GetFleets(editorScenario).Length;
        }

        public static EditorUnit[] GetAbandonedShips(this EditorScenario editorScenario)
        {
            return editorScenario.GetComponentsInChildren<EditorUnit>().Where(e => e.IsAbandonedShip()).ToArray();
        }

        public static int GetAbandonedShipCount(this EditorScenario editorScenario)
        {
            return GetAbandonedShips(editorScenario).Length;
        }

        public static EditorUnit[] GetAbandonedStations(this EditorScenario editorScenario)
        {
            return editorScenario.GetComponentsInChildren<EditorUnit>().Where(e => e.IsAbandonedStation()).ToArray();
        }

        public static int GetAbandonedStationCount(this EditorScenario editorScenario)
        {
            return GetAbandonedStations(editorScenario).Length;
        }

        public static EditorFaction[] GetFactions(this EditorScenario editorScenario)
        {
            return editorScenario.GetComponentsInChildren<EditorFaction>();
        }

        public static int GetFactionCount(this EditorScenario editorScenario)
        {
            return GetFactions(editorScenario).Length;
        }

        public static int GetSectorCount(this EditorScenario editorScenario)
        {
            return editorScenario.GetSectors().Length;
        }

        public static EditorSector[] GetSectors(this EditorScenario editorScenario)
        {
            if (editorScenario == null) 
                return new EditorSector[0];

            return editorScenario.GetSectorsRoot().GetComponentsInChildren<EditorSector>();
        }

        public static int GetMajorStationCount(this EditorScenario editorScenario)
        {
            if (editorScenario == null)
                return 0;

            return editorScenario.GetComponentsInChildren<EditorUnit>().Count(e => e.IsMajorStation());
        }

        public static int GetMinorStationCount(this EditorScenario editorScenario)
        {
            if (editorScenario == null)
                return 0;

            return editorScenario.GetComponentsInChildren<EditorUnit>().Count(e => e.IsMinorStation());
        }

        public static int GetAsteroidClusterCount(this EditorScenario editorScenario)
        {
            if (editorScenario == null)
                return 0;

            return editorScenario.GetComponentsInChildren<EditorUnit>().Count(e => e.IsAsteroidCluster());
        }

        public static int GetAsteroidCount(this EditorScenario editorScenario)
        {
            if (editorScenario == null)
                return 0;

            return editorScenario.GetComponentsInChildren<EditorUnit>().Count(e => e.IsAsteroid());
        }

        public static int GetCargoContainerCount(this EditorScenario editorScenario)
        {
            if (editorScenario == null)
                return 0;

            return editorScenario.GetComponentsInChildren<EditorUnit>().Count(e => e.IsCargoContainer());
        }

        public static int GetPlanetCount(this EditorScenario editorScenario)
        {
            if (editorScenario == null)
                return 0;

            return editorScenario.GetComponentsInChildren<EditorUnit>().Count(e => e.IsPlanet());
        }

        public static int GetGasCloudCount(this EditorScenario editorScenario)
        {
            if (editorScenario == null)
                return 0;

            return editorScenario.GetComponentsInChildren<EditorUnit>().Count(e => e.IsGasCloud());
        }
    }
}
