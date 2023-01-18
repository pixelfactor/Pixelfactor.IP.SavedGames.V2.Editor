using System.Collections.Generic;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    public static class TransformExtensions
    {
        /// <summary>
        /// This method will additionally work on prefabs
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static List<T> GetComponentsInImmediateChildren<T>(this Transform transform) where T : Component
        {
            var list = new List<T>();
            for (int i = 0; i < transform.childCount; i++)
            {
                var childTransform = transform.GetChild(i);
                var component = childTransform.GetComponent<T>();
                if (component != null)
                {
                    list.Add(component);
                }
            }

            return list;
        }

        /// <summary>
        /// This method will additionally work on prefabs
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static T GetComponentInImmediateChildren<T>(this Transform transform) where T : Component
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var childTransform = transform.GetChild(i);
                var component = childTransform.GetComponent<T>();
                if (component != null)
                {
                    return component;
                }
            }

            return null;
        }

        public static T GetComponentInSelfOrImmediateChildren<T>(this Transform transform) where T : Component
        {
            var component = transform.GetComponent<T>();
            if (component != null)
                return component;

            return GetComponentInImmediateChildren<T>(transform);
        }

        public static T GetComponentInImmediateParent<T>(this Transform transform) where T : Component
        {
            if (transform.parent != null)
            {
                return transform.parent.GetComponent<T>();
            }

            return null;
        }

        public static int CountRootChildrenOfType<T>(this Transform transform) where T : Component
        {
            var count = 0;
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var isMatch = child.GetComponent<T>() != null;
                if (isMatch)
                {
                    count++;
                }
                else
                {
                    count += CountRootChildrenOfType<T>(child);
                }
            }

            return count;
        }

        public static T FindFirstParentOfType<T>(this Transform transform) where T : Component
        {
            var t = transform.parent;
            while (t != null)
            {
                var c = t.GetComponent<T>();
                if (c != null)
                    return c;

                t = t.parent;
            }

            return null;
        }
    }
}
