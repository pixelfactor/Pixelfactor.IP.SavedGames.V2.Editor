﻿using Pixelfactor.IP.SavedGames.V2.Model;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    /// <summary>
    /// Defines an item of cargo on a ship/station
    /// </summary>
    public class EditorCargo : MonoBehaviour
    {
        [Tooltip("The amount of this cargo")]
        public int Quantity = 1;

        [Tooltip("The type of the cargo")]
        public EditorCargoClassRef Cargo = null;

        [Header("Advanced")]
        [UnityEngine.Serialization.FormerlySerializedAs("CargoClass")]
        [Tooltip("The type of the cargo. Obsolete - assign CargoClass instead")]
        public ModelCargoClass LegacyCargoClass;

        public ModelCargoClass ModelCargoClass
        {
            get
            {
                if (this.Cargo != null)
                {
                    return this.Cargo.CargoClass;
                }

                return this.LegacyCargoClass;
            }
        }
    }
}
