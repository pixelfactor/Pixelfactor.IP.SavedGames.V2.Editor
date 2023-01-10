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
    }
}
