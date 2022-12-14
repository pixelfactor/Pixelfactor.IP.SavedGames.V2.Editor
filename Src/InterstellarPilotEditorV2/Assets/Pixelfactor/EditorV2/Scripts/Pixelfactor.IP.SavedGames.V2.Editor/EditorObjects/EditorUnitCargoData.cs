using Pixelfactor.IP.SavedGames.V2.Model;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    public class EditorUnitCargoData : MonoBehaviour
    {
        public ModelCargoClass CargoClass;
        public int Quantity = 1;
        public bool Expires = false;
        public double SpawnTime = 0d;
    }
}
