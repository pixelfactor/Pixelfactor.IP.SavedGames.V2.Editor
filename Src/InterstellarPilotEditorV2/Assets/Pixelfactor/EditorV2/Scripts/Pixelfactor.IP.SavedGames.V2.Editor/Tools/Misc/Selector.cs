﻿using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    public static class Selector
    {
        /// <summary>
        /// Gets all objects of the given type that are either selected or have a child that is selected
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetInParents<T>() where T : MonoBehaviour
        {
            var list = new List<T>();

            foreach (var obj in Selection.objects)
            {
                if (obj is GameObject gameObject)
                {
                    var p = gameObject.GetComponentInParent<T>();
                    if (p != null && !list.Contains(p))
                    {
                        list.Add(p);
                    }
                }
            }

            return list;
        }

        public static bool TryGetSelectedRootUnits(out List<EditorUnit> units)
        {
            units = new List<EditorUnit>();
            foreach (var obj in Selection.objects)
            {
                if (obj is GameObject gameObject)
                {
                    var p = gameObject.GetComponent<EditorUnit>();
                    if (p != null && p.IsShip())
                    {
                        units.Add(p);
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return units.Count > 0;
        }

        public static void Add(Object gameObject)
        {
            var selection = Selection.objects.ToList();
            selection.Add(gameObject);
            Selection.objects = selection.ToArray();
        }

        public static void SelectFirstSector()
        {
            var savedGame = SavedGameUtil.FindSavedGame();
            SelectFirstSector(savedGame);
        }

        public static void SelectFirstSector(EditorScenario savedGame)
        {
            if (savedGame != null)
            {
                var sector = savedGame.GetComponentInChildren<EditorSector>();
                if (sector != null)
                {
                    Selection.activeGameObject = sector.gameObject;
                }
            }
        }

        public static EditorSector GetSingleSelectedSectorOrNull()
        {
            if (Selection.activeGameObject != null)
            {
                return Selection.activeGameObject.GetComponentInParent<EditorSector>();
            }

            return null;
        }

        public static bool TryGetTwoSelectedGameObjects(out GameObject a, out GameObject b)
        {
            a = null;
            b = null;

            if (Selection.gameObjects.Length != 2)
            {
                return false;
            }

            a = Selection.gameObjects[0];
            b = Selection.gameObjects[1];

            return true;
        }
    }
}
