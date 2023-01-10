using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using Pixelfactor.IP.SavedGames.V2.Model;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools.Main.Import
{
    public class ImportOperation
    {
        private EditorScenario editorScenario;
        private SavedGame model;
        private Transform sectorsTransform = null;
        private CustomSettings settings = null;
        private bool exitOnError = false;
        private Dictionary<ModelUnitClass, EditorUnit> cachedUnitPrefabs = new Dictionary<ModelUnitClass, EditorUnit>(200);
        private Dictionary<int, EditorSector> editorSectorsById = new Dictionary<int, EditorSector>(64);

        public ImportOperation(SavedGame savedGameModel, EditorScenario editorScenario, bool exitOnError)
        {
            this.editorScenario = editorScenario;
            this.model = savedGameModel;

            this.exitOnError = exitOnError;

            this.sectorsTransform = this.editorScenario.SectorsRoot != null ? this.editorScenario.SectorsRoot : this.editorScenario.transform;

            this.settings = CustomSettings.GetOrCreateSettings();

            this.CacheUnitPrefabs(settings.UnitPrefabPath.TrimEnd('/') + "/Asteroids");
            this.CacheUnitPrefabs(settings.UnitPrefabPath.TrimEnd('/') + "/AsteroidClusters");
            this.CacheUnitPrefabs(settings.UnitPrefabPath.TrimEnd('/') + "/Cargo");
            this.CacheUnitPrefabs(settings.UnitPrefabPath.TrimEnd('/') + "/GasClouds");
            this.CacheUnitPrefabs(settings.UnitPrefabPath.TrimEnd('/') + "/Misc");
            this.CacheUnitPrefabs(settings.UnitPrefabPath.TrimEnd('/') + "/Planets");
            this.CacheUnitPrefabs(settings.UnitPrefabPath.TrimEnd('/') + "/Ships");
            this.CacheUnitPrefabs(settings.UnitPrefabPath.TrimEnd('/') + "/Stations");
            this.CacheUnitPrefabs(settings.UnitPrefabPath.TrimEnd('/') + "/Wormholes");

            this.editorSectorsById.Clear();
        }

        public void Import()
        {
            this.ImportSectors();
            this.ImportsUnits();
        }

        private void CacheUnitPrefabs(string path)
        {
            var units = TryGetUnityObjectsOfTypeFromPath<EditorUnit>(path);
            foreach (var unit in units)
            {
                if (this.cachedUnitPrefabs.TryGetValue(unit.ModelUnitClass, out EditorUnit existingUnit))
                {
                    Debug.LogError($"Found a duplicate prefab unit \"{existingUnit.name}\". All prefabs in these folders should have a unique class ID", existingUnit);
                    continue;
                }
                this.cachedUnitPrefabs.Add(unit.ModelUnitClass, unit);
            }
        }

        /// <summary>
        /// Adds newly (if not already in the list) found assets.
        /// Returns how many found (not how many added)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="assetsFound">Adds to this list if it is not already there</param>
        /// <returns></returns>
        public static IEnumerable<T> TryGetUnityObjectsOfTypeFromPath<T>(string path) where T : UnityEngine.Object
        {
            string[] filePaths = System.IO.Directory.GetFiles(path);

            if (filePaths != null && filePaths.Length > 0)
            {
                for (int i = 0; i < filePaths.Length; i++)
                {
                    UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(filePaths[i], typeof(T));
                    if (obj is T asset)
                    {
                        yield return asset;
                    }
                }
            }
        }

        private void ImportsUnits()
        {
            foreach (var modelUnit in this.model.Units)
            {
                ImportUnit(modelUnit);
            }
        }

        private void ImportUnit(ModelUnit modelUnit)
        {
            var prefab = this.cachedUnitPrefabs.GetValueOrDefault(modelUnit.Class);
            if (prefab == null)
            {
                var message = $"Importer couldn't find a prefab to create unit \"{modelUnit.Class}\". Check that there is a prefab inside the prefabs folder that matches the same unit class";
                Debug.LogError(message);
                OnError(message);

                return;
            }

            var editorSector = GetEditorSector(modelUnit.Sector);
            var editorUnit = PrefabHelper.Instantiate(prefab, editorSector != null ? editorSector.transform : this.editorScenario.transform);

            editorUnit.Seed = modelUnit.Seed;
            editorUnit.transform.localPosition = modelUnit.Position.ToVector3();
            editorUnit.transform.localRotation = modelUnit.Rotation.ToQuaternion();

            if (modelUnit.Radius.HasValue)
            {
                editorUnit.Radius = modelUnit.Radius.Value;
            }

            if (modelUnit.Mass.HasValue)
            {
                editorUnit.Mass = modelUnit.Mass.Value;
            }

            if (!string.IsNullOrEmpty(modelUnit.CustomClassName))
            {
                editorUnit.VariantName = modelUnit.CustomClassName;
            }

            if (!string.IsNullOrWhiteSpace(modelUnit.Name))
            {
                editorUnit.Name = modelUnit.Name;
            }

            if (!string.IsNullOrWhiteSpace(modelUnit.CustomClassName))
            {
                editorUnit.ShortName = modelUnit.ShortName;
            }

            // TODO: Faction

            editorUnit.RpProvision = modelUnit.RpProvision;

            // TODO: Cargo data

            // TODO: debris data (obsolete)

            // TODO: Asteroid data

            // TODO: Ship trader data

            // TODO: Projectile data
        }

        private EditorSector GetEditorSector(ModelSector modelSector)
        {
            if (modelSector == null)
                return null;

            return GetEditorSector(modelSector.Id);
        }

        private EditorSector GetEditorSector(int id)
        {
            if (id < 0)
                return null;

            var sector = this.editorSectorsById.GetValueOrDefault(id);
            if (sector == null)
            {
                var message = $"Missing sector with id: {id}";
                Debug.LogError(message);
                OnError(message);
            }

            return sector;
        }

        private void OnError(string message)
        {
            if (this.exitOnError)
                throw new System.Exception(message);
        }

        private void ImportSectors()
        {
            var sectorPrefab = AssetDatabase.LoadAssetAtPath<EditorSector>(this.settings.SectorPrefabPath);

            foreach (var modelSector in this.model.Sectors)
            {
                var editorSector = PrefabHelper.Instantiate(sectorPrefab, this.sectorsTransform);
                editorSector.Id = modelSector.Id;
                editorSector.Name = modelSector.Name;
                editorSector.WormholeDistanceMultiplier = modelSector.GateDistanceMultiplier;
                editorSector.BackgroundRotation = modelSector.BackgroundRotation.ToVector3();
                editorSector.Seed = modelSector.RandomSeed;
                editorSector.AmbientLightColor = modelSector.AmbientLightColor.ToColor();
                editorSector.DirectionLightColor = modelSector.DirectionLightColor.ToColor();
                editorSector.LightDirectionFudge = modelSector.LightDirectionFudge;

                editorSector.transform.localPosition = new Vector3(
                    modelSector.MapPosition.X / settings.UniverseMapScaleFactor,
                    0.0f,
                    modelSector.MapPosition.Z / settings.UniverseMapScaleFactor);

                this.editorSectorsById.Add(modelSector.Id, editorSector);

                // Sky
                if (modelSector.CustomAppearance != null)
                {
                    var editorSky = editorSector.gameObject.AddComponent<EditorSectorSky>();
                    editorSky.NebulaColors = modelSector.CustomAppearance.NebulaColors;
                    editorSky.NebulaCount = modelSector.CustomAppearance.NebulaCount;
                    editorSky.NebulaComplexity = modelSector.CustomAppearance.NebulaComplexity;
                    editorSky.NebulaBrightness = modelSector.CustomAppearance.NebulaBrightness;
                    editorSky.NebulaStyles = modelSector.CustomAppearance.NebulaStyles;
                    editorSky.NebulaTextureCount = modelSector.CustomAppearance.NebulaTextureCount;

                    editorSky.StarsCount = modelSector.CustomAppearance.StarsCount;
                    editorSky.StarsIntensity = modelSector.CustomAppearance.StarsIntensity;
                }
            }
        }
    }
}
