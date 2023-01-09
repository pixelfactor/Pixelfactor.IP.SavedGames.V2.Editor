﻿using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Edit;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools.Build.Grow
{
    /// <summary>
    /// Adds sectors to an existing one
    /// </summary>
    public static class GrowTool
    {
        /// <summary>
        /// Adds a new connection to the existing sector. Returns null if there's no space
        /// </summary>
        /// <param name="editorSector"></param>
        /// <returns></returns>
        public static EditorSector GrowOnce(EditorSector existingSector, EditorSector sectorPrefab)
        {
            var newInstance = (GameObject)PrefabUtility.InstantiatePrefab(sectorPrefab.gameObject, existingSector.transform.parent);
            var sector = newInstance.GetComponent<EditorSector>();

            EditSectorTool.Randomize(sector);

            return sector;
        }

        public static EditorSector GrowOnceAndConnect()
        {
            return null;
        }
    }
}
