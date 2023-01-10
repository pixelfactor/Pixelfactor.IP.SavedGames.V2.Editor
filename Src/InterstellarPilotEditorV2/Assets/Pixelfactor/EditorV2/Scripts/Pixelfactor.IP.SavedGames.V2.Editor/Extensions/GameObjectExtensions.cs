using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    public static class GameObjectExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            var component = gameObject.GetComponent<T>();
            if (component != null)
                return component;

            return gameObject.AddComponent<T>();
        }
    }
}
