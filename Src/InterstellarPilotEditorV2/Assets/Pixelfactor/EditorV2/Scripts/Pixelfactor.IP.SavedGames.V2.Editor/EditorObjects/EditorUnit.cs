﻿using Pixelfactor.IP.SavedGames.V2.Model;
using System;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    /// <summary>
    /// The base world object type for IP - Could be a ship, stations, asteroid, projectile, wormhole or others
    /// </summary>
    public class EditorUnit : MonoBehaviour
    {
        [Tooltip("An id that is unique to all units. If the value is less than zero (the default) a unique id will be automatically assigned when exporting")]
        public int Id = -1;

        /// <summary>
        /// Where a unit is randomized at runtime, you can use this value to get guaranteed data.<br />
        /// Used for: asteroid cluster appearance, gas cloud appearance, unit "designations" e.g. Rations Factory BX-1
        /// </summary>
        [Tooltip("Where a unit is randomized at runtime, you can use this value to get guaranteed data. Used for: asteroid cluster appearance, gas cloud appearance, unit 'designations' e.g. Rations Factory BX-1")]
        public int Seed = -1;

        /// <summary>
        /// Obsolete - use <see cref="EditorCargoClassRef"/> now
        /// </summary>
        [UnityEngine.Serialization.FormerlySerializedAs("Class")]
        [Tooltip("The old way of assigning a unit's class. This will soon be removed. Use 'UnitClass' instead.")]
        public ModelUnitClass LegacyUnitClass = ModelUnitClass.None;

        /// <summary>
        /// The class/type of this unit
        /// </summary>
        [UnityEngine.Serialization.FormerlySerializedAs("EditorUnitClassRef")]
        [Tooltip("The mandatory class/type of this unit")]
        public EditorUnitClassRef UnitClass = null;

        [Tooltip("The faction that this unit belongs to. Only applicable to some units like ships/stations/cargo")]
        public EditorFaction Faction;

        /// <summary>
        /// A custom name for this unit. If null/empty the game displays the unit name based on other rules.
        /// </summary>
        [Tooltip("A optional custom name given to the unit. E.g. ship name or bar name. Note that if left blank for ships, the engine will assign a ship name randomly")]
        public string Name;

        [Tooltip("An optional custom short name given to the unit. Currently only used by stations")]
        public string ShortName;

        /// <summary>
        /// Optional custom radius. Only relevant to gas clouds and asteroid clusters
        [Tooltip("Defines the radius of gas clouds and asteroid clusters. Ignored when less than zero")]
        /// </summary>
        public float Radius = -1.0f;

        [Tooltip("Optional name of this unit's custom variant. Currently only applies to ships. Likely to be used for stations in the future")]
        public string VariantName = null;

        [Tooltip("Optional mass of the unit. Leave at -1 to use the default. Currently only applies to ships")]
        public float Mass = -1.0f;

        [Tooltip("When false the engine will not allow the unit to be killed during normal gameplay. The unit could still be damaged")]
        public bool AllowDestruction = true;

        [Tooltip("Whether the unit can be damaged during normal gameplay")]
        public bool IsInvulnerable = false;

        [Tooltip("The current health of the unit. When the value is < 0, the unit has full health")]
        public float Health = -1.0f;

        [Header("Advanced")]
        [Tooltip("Whether the unit has already been destroyed. Generally this will not be the case.")]
        public bool IsDestroyed = false;

        /// <summary>
        /// A name that is displayed in the editor
        /// </summary>
        [Tooltip("A name for the unit displayed in the editor and not exported")]
        public string EditorName;

        [Tooltip("Helps determine what type of ship this is so that only certain ships can be spawned when needed")]
        public EditorFactionStrategy EditorShipPurpose = EditorFactionStrategy.None;

        [Tooltip("Whether the editor should automatically add ammo to this ship when exporting")]
        public AutoExportAmmoOption EditorAutoExportAmmo = AutoExportAmmoOption.DontCare;

        public enum AutoExportAmmoOption
        {
            DontCare = 0,
            DontExport = 1,
            Export = 2
        }

        public string GetEditorName()
        {
            if (!string.IsNullOrWhiteSpace(this.EditorName))
                return this.EditorName;

            return this.name;
        }

        void OnDrawGizmos()
        {
            if (SelectionHelper.IsSelected(this) || SceneView.lastActiveSceneView.size < GetDrawLabelSceneViewSize())
            {
                if (this.transform.FindFirstParentOfType<EditorUnit>() != null)
                    return;

                var dockedShipCount = 0;
                if (this.IsShip() || this.IsStation())
                {
                    dockedShipCount = this.transform.CountRootChildrenOfType<EditorUnit>();
                }

                var name = this.gameObject.name;
                if (dockedShipCount > 0)
                {
                    name += $"\n{dockedShipCount} docked ships";
                }

                DrawString.Draw(name, this.transform.position, Color.white);
            }
        }

        public float GetDrawLabelSceneViewSize()
        {
            switch (this.ModelUnitClass)
            {
                case ModelUnitClass.Wormhole_Default:
                case ModelUnitClass.Wormhole_Unstable:
                    return 20000.0f;
                case ModelUnitClass.AsteroidCluster_TypeB:
                case ModelUnitClass.AsteroidCluster_TypeH:
                    return 10000.0f;
                default:
                    {
                        if (this.ModelUnitClass.IsStation())
                        {
                            return 5000.0f;
                        }

                        return 1000.0f;
                    }
            }
        }

        public ModelUnitClass ModelUnitClass
        {
            get
            {
                if (System.Enum.IsDefined(typeof(ModelUnitClass), this.LegacyUnitClass) && this.LegacyUnitClass != ModelUnitClass.None)
                {
                    return this.LegacyUnitClass;
                }

                if (this.UnitClass != null)
                {
                    return this.UnitClass.UnitClass;
                }

                return ModelUnitClass.None;
            }
        }

        public float GetCollisionRadius()
        {
            var sphereCollider = this.GetComponent<SphereCollider>();
            if (sphereCollider != null)
                return sphereCollider.radius;

            return 1.0f;
        }

        /// <summary>
        /// Gets a massive estimate of the ships size compared to other ships
        /// </summary>
        /// <returns></returns>
        public float GetRelativeSize()
        {
            // HACK: Hard-coded max radius is equivalent to the magnus
            return Mathf.Lerp(0.0f, 1.0f, (this.GetCollisionRadius() - 3.0f) / 25f);
        }

        public void OnValidate()
        {
            // Ensure that docked ships are always positioned in the same place as the parent dock
            if (this.IsShip() || this.IsStation())
            {
                if (this.transform.FindFirstParentOfType<EditorUnit>() != null)
                {
                    this.transform.localPosition = Vector3.zero;
                }
            }
        }
    }
}
