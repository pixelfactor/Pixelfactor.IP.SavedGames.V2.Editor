using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    /// <summary>
    /// Defines a type of asteroid (e.g. Ice/Rock)
    /// </summary>
    public class EditorAsteroidType : MonoBehaviour
    {
        [Tooltip("Name of this asteroid type")]
        public string Name;

        /// <summary>
        /// Determines the frequency of generating this asteroid type
        /// </summary>
        [Tooltip("Determines the frequency of generating this asteroid type")]
        public float PreferredWeighting = 1.0f;

        [Tooltip("The asteroid cluster that is associated with this asteroid type")]
        public EditorUnit AsteroidClusterPrefab = null;

        public EditorUnit AsteroidPrefab = null;
    }
}
