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

        public static void Add(Object gameObject)
        {
            var selection = Selection.objects.ToList();
            selection.Add(gameObject);
            Selection.objects = selection.ToArray();
        }
    }
}
