﻿using Pixelfactor.IP.SavedGames.V2.Model;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    /// <summary>
    /// Wrapper around cargo class to make it somewhat easier to assign rather than the lengthy enum
    /// </summary>
    public class EditorCargoClassRef : MonoBehaviour
    {
        [Tooltip("Unique ID of this cargo class. Should not be changed from the prefab")]
        public int CargoClassId = -1;

        public ModelCargoClass CargoClass => (ModelCargoClass)this.CargoClassId;
    }
}
