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
        [Tooltip("The name of the person. If no name is provided the engine will randomly assign one.")]
        public string CustomName;

        [Tooltip("The mandatory faction that the person belongs to.")]
        public EditorFaction Faction;

        [Tooltip("The number of kills this person has")]
        public int Kills;
    }
}
