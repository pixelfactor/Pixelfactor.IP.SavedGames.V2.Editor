using Pixelfactor.IP.SavedGames.V2.Model;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    /// <summary>
    /// Represents a possible ship type that the <see cref="EditorFleetSpawnerItem" may spawn/>
    /// </summary>
    public class EditorFleetSpawnerItem : MonoBehaviour
    {
        public ModelUnitClass UnitClass;
    }
}
