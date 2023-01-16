using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    public static class GameObjectHelper
    {
        public static T Instantiate<T>(Component parent = null, string name = null) where T : Component
        {
            var g = new GameObject();
            var c = g.AddComponent<T>();

            if (parent != null)
            {
                g.transform.SetParent(parent.transform, false);
                g.transform.localPosition = Vector3.zero;
            }

            if (name != null)
            {
                g.name = name;
            }

            return c;
        }

        /// <summary>
        /// Adds newly (if not already in the list) found assets.
        /// Returns how many found (not how many added)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="assetsFound">Adds to this list if it is not already there</param>
        /// <returns></returns>
        public static IEnumerable<T> GetPrefabsOfTypeFromPath<T>(
            string path, 
            string searchPattern = "*", 
            System.IO.EnumerationOptions enumerationOptions = null) where T : UnityEngine.Object
        {
            string[] filePaths = null;
            if (enumerationOptions != null)
                filePaths = System.IO.Directory.GetFiles(path, searchPattern, enumerationOptions);
            else
                filePaths = System.IO.Directory.GetFiles(path, searchPattern);

            if (filePaths != null && filePaths.Length > 0)
            {
                for (int i = 0; i < filePaths.Length; i++)
                {
                    if (System.IO.Path.GetExtension(filePaths[i]) != ".prefab")
                        continue;

                    UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(filePaths[i], typeof(T));
                    if (obj is T asset)
                    {
                        yield return asset;
                    }
                }
            }
        }
    }
}
