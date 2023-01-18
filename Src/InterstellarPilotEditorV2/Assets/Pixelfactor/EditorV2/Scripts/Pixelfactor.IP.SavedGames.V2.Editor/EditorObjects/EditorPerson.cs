using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    public class EditorPerson : MonoBehaviour
    {
        [Tooltip("Whether the person is male as opposed to female.")]
        public bool IsMale = true;

        [Tooltip("An id that is unique to all people. If the value is less than zero (the default) a unique id will be automatically assigned when exporting")]
        public int Id = -1;

        /// <summary>
        /// Use this OR the generated name ids
        /// </summary>
        [Tooltip("Optional title for the person")]
        public string CustomTitle;

        /// <summary>
        /// Use this OR the generated name ids
        /// </summary>
        [Tooltip("The name of the person. If no name is provided the engine will randomly assign one.")]
        public string CustomName;

        /// <summary>
        /// Use this OR the generated name ids
        /// </summary>
        [Tooltip("Optional custom short name of the person displayed in places where there is little room")]
        public string CustomShortName;

        [Tooltip("The mandatory faction that the person belongs to.")]
        public EditorFaction Faction;

        [Tooltip("The number of kills this person has")]
        public int Kills = 0;

        [Tooltip("The number of times the person has died")]
        public int Deaths = 0;

        [Header("Advanced")]
        [Tooltip("The ID of the person's first name from the name database")]
        public int FirstNameId = -1;

        [Tooltip("The ID of the person's last name from the name database")]
        public int LastNameId = -1;

        [Tooltip("Whether the person controlling the ship is a bot / auto-pilot. Note that IP2 no longer generates auto-pilots but has some code that considers them")]
        public bool IsAutoPilot = false;

        [Tooltip("Seed value of the person. The seed for people is used infrequently but would in future be used to generate content on the fly such as procedural avatars")]
        public int Seed = -1;

        [Tooltip("Determines what opens when the player uses comms to contact the person. Editing dialog is not currently supported")]
        public int DialogId = -1;

        [Tooltip("Determines the type of random things that the person can say. Changing value is not currently supported in editor")]
        public int DialogProfileId = -1;

        [Tooltip("Whether to destroy this person when killed rather than keep them alive but increment the deaths counter (normally true)")]
        public bool DestroyOnKill = true;

        public string GetEditorName()
        {
            if (!string.IsNullOrWhiteSpace(this.CustomName))
                return this.CustomName;

            if (!string.IsNullOrWhiteSpace(this.CustomShortName))
                return this.CustomShortName;

            return "Unnamed";
        }

        public void OnValidate()
        {
            // When a person is inside a unit, their local position should be zero
            var parentUnit = this.GetComponentInParent<EditorUnit>();
            if (parentUnit != null)
            {
                this.transform.localPosition = Vector3.zero;
            }
        }
    }
}
