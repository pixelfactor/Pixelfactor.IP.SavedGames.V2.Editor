﻿using Pixelfactor.IP.SavedGames.V2.Model;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    /// <summary>
    /// Defines the ID of a component bay and meta, unique to a specific ship/station<br />
    /// Data in this class should not be changed and won't be exported
    /// </summary>
    public class EditorComponentBayRef : MonoBehaviour
    {
        [Tooltip("The ID of this component bay, unique to the ship/station. Do not change this from the prefab value")]
        public int BayId = -1;
        [Tooltip("The name of the bay - for info only")]
        public string Name;
        [Tooltip("The component type of the bay - for info only")]
        public ModelComponentBayType BayType;
        /// <summary>
        /// The component that is installed in this bay before any mods
        /// </summary>
        [Tooltip("The component that is installed in this bay before any mods")]
        public EditorComponentClass DefaultComponent;
    }
}
