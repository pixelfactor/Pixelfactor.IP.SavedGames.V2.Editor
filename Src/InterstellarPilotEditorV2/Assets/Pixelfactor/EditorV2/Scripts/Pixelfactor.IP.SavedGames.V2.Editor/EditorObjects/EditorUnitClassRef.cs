using Pixelfactor.IP.SavedGames.V2.Model;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    /// <summary>
    /// Wrapper around unit class to make it somewhat easier to assign rather than the lengthy enum
    /// </summary>
    public class EditorUnitClassRef : MonoBehaviour
    {
        public int UnitClassId = -1;

        public ModelUnitClass UnitClass => (ModelUnitClass)this.UnitClassId;
    }
}
