using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    public static class PrefabHelper
    {
        public static T Instantiate<T>(string path) where T : Component
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);

            return Instantiate<T>(asset);
        }

        public static T Instantiate<T>(string path, Transform transform) where T : Component
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);

            return Instantiate<T>(asset, transform);
        }

        public static T Instantiate<T>(T asset) where T : Component
        {
            var gameObject = (GameObject)PrefabUtility.InstantiatePrefab(asset.gameObject);

            return gameObject.GetComponent<T>();
        }

        public static T Instantiate<T>(T asset, Transform transform) where T : Component
        {
            var gameObject = (GameObject)PrefabUtility.InstantiatePrefab(asset.gameObject, transform);

            return gameObject.GetComponent<T>();
        }
    }
}
