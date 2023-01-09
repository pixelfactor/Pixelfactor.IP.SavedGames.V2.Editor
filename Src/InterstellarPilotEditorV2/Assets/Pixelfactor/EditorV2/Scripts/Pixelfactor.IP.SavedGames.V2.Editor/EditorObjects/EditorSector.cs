using Pixelfactor.IP.SavedGames.V2.Model;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    /// <summary>
    /// Sector/room within the universe
    /// </summary>
    public class EditorSector : MonoBehaviour
    {
        [Tooltip("A unique sector Id. If the value is less than zero (the default) a unique id will be automatically assigned when exporting")]
        public int Id = -1;

        [Tooltip("The name of the sector as shown in the engine")]
        public string Name;

        /// <summary>
        /// Reserved for future use - A description of the sector as shown to the player.
        /// </summary>
        [HideInInspector]
        [Tooltip("Reserved for future use - A description of the sector as shown to the player.")]
        public string Description;

        /// <summary>
        /// Determines the distance of the wormholes from the sector origin. Should be ~1.0
        /// </summary>
        [Tooltip("Determines the size of the playing area of the sector. This value is used extensively by the engine when determining where to place units. Use 0 to make the smallest sector possible. This value is respected when using the 'connect tool' to create wormholes.")]
        [Range(0f, 1f)]
        public float WormholeDistanceMultiplier = 1.0f;

        /// <summary>
        /// Something to do with the appearance of asteroid clusters
        /// </summary>
        [Tooltip("A seed used to determine the appearance of the sector e.g. procedural sky. When unset, a seed will be assigned randomly at runtime. Set a value to have a consistent appearance on each play.")]
        [UnityEngine.Serialization.FormerlySerializedAs("RandomSeed")]
        public int Seed = -1;

        /// <summary>
        /// Skybox rotation
        /// </summary>
        [Tooltip("An optional rotation (fudge) applied to the sector sky. Use this to tweak the sky background.")]
        public Vector3 BackgroundRotation;

        /// <summary>
        /// Reserved for future use - the rotation of the sectors directional light.
        /// </summary>
        [Tooltip("Reserved for future use - the rotation of the sectors directional light.")]
        [HideInInspector]
        public Vector3 DirectionLightRotation;

        /// <summary>
        /// Alpha ignored
        /// </summary>
        [Tooltip("The colour of the sectors ambient light. The alpha component is not used")]
        public Color AmbientLightColor = new Color(0.3f, 0.3f, 0.3f, 1.0f);

        /// <summary>
        /// Alpha ignored
        /// </summary>
        [Tooltip("The colour of the sectors directional light. The alpha component is not used")]
        public Color DirectionLightColor = new Color(0.8f, 0.8f, 0.8f, 1.0f);

        /// <summary>
        /// Changes the direction light direction at the start of the game
        /// </summary>
        [Range(0.0f, 1.0f)]
        [Tooltip("The direction of the sector's directional light at the start of the game")]
        public float LightDirectionFudge = 0.0f;

        void OnDrawGizmosSelected()
        {
            if (SelectionHelper.IsSelected(this) || SelectionHelper.IsSelected(this.transform.parent))
            {
                GUIStyle style = new GUIStyle();
                style.fontSize = 18;
                style.alignment = TextAnchor.MiddleCenter;
                DrawString.Draw(this.gameObject.name, this.transform.position, new Color(0.3f, 0.3f, 1.0f), style, new Vector3(0.0f, -100.0f, 0.0f));
            }
        }
    }
}
