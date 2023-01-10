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
        private Dictionary<int, EditorFaction> editorFactionsById = new Dictionary<int, EditorFaction>(64);
        private Dictionary<int, EditorUnit> editorUnitsById = new Dictionary<int, EditorUnit>(64);
        private EditorFaction factionPrefab = null;

        public ImportOperation(SavedGame savedGameModel, EditorScenario editorScenario, bool exitOnError)
        {
            this.editorScenario = editorScenario;
            this.model = savedGameModel;

            this.exitOnError = exitOnError;

            this.sectorsTransform = this.editorScenario.SectorsRoot != null ? this.editorScenario.SectorsRoot : this.editorScenario.transform;

            this.settings = CustomSettings.GetOrCreateSettings();

            this.CacheUnitPrefabs(settings.UnitPrefabsPath.TrimEnd('/') + "/Asteroids");
            this.CacheUnitPrefabs(settings.UnitPrefabsPath.TrimEnd('/') + "/AsteroidClusters");
            this.CacheUnitPrefabs(settings.UnitPrefabsPath.TrimEnd('/') + "/Cargo");
            this.CacheUnitPrefabs(settings.UnitPrefabsPath.TrimEnd('/') + "/GasClouds");
            this.CacheUnitPrefabs(settings.UnitPrefabsPath.TrimEnd('/') + "/Misc");
            this.CacheUnitPrefabs(settings.UnitPrefabsPath.TrimEnd('/') + "/Planets");
            this.CacheUnitPrefabs(settings.UnitPrefabsPath.TrimEnd('/') + "/Ships");
            this.CacheUnitPrefabs(settings.UnitPrefabsPath.TrimEnd('/') + "/Stations");
            this.CacheUnitPrefabs(settings.UnitPrefabsPath.TrimEnd('/') + "/Wormholes");

            this.editorSectorsById.Clear();
            this.editorFactionsById.Clear();
            this.editorUnitsById.Clear();

            this.factionPrefab = AssetDatabase.LoadAssetAtPath<EditorFaction>(settings.FactionPrefabPath);
        }

        public void Import()
        {
            this.ImportSectors();
            this.ImportFactions();
            this.ImportUnits();
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

        private void ImportFactions()
        {
            foreach (var modelFaction in this.model.Factions)
            {
                var editorFaction = PrefabHelper.Instantiate(this.factionPrefab, this.editorScenario.GetFactionsRoot());

                this.editorFactionsById.Add(modelFaction.Id, editorFaction);
                editorFaction.Id = modelFaction.Id;
                editorFaction.CustomName = modelFaction.CustomName;
                editorFaction.CustomShortName = modelFaction.CustomShortName;

                editorFaction.GeneratedNameId = modelFaction.GeneratedNameId;
                editorFaction.GeneratedSuffixId = modelFaction.GeneratedSuffixId;

                ImportFactionHomeSector(modelFaction, editorFaction);

                editorFaction.Credits = modelFaction.Credits;
                editorFaction.Description = modelFaction.Description;
                editorFaction.IsCivilian = modelFaction.IsCivilian;
                editorFaction.FactionType = modelFaction.FactionType;
                editorFaction.Aggression = modelFaction.Aggression;
                editorFaction.Virtue = modelFaction.Virtue;
                editorFaction.Greed = modelFaction.Greed;
                editorFaction.Cooperation = modelFaction.Cooperation;
                editorFaction.TradeEfficiency = modelFaction.TradeEfficiency;
                editorFaction.DynamicRelations = modelFaction.DynamicRelations;
                editorFaction.ShowJobBoards = modelFaction.ShowJobBoards;
                editorFaction.CreateJobs = modelFaction.CreateJobs;
                editorFaction.RequisitionPointMultiplier = modelFaction.RequisitionPointMultiplier;
                editorFaction.DestroyWhenNoUnits = modelFaction.DestroyWhenNoUnits;
                editorFaction.MinNpcCombatEfficiency = modelFaction.MinNpcCombatEfficiency;
                editorFaction.MaxNpcCombatEfficiency = modelFaction.MaxNpcCombatEfficiency;
                editorFaction.AdditionalRpProvision = modelFaction.AdditionalRpProvision;
                editorFaction.TradeIllegalGoods = modelFaction.TradeIllegalGoods;
                editorFaction.SpawnTime = modelFaction.SpawnTime;
                editorFaction.HighestEverNetWorth = modelFaction.HighestEverNetWorth;
                editorFaction.PilotRankingSystemId = modelFaction.RankingSystemId;
                editorFaction.PreferredFormationId = modelFaction.PreferredFormationId;
            }
        }

        private void ImportFactionHomeSector(ModelFaction modelFaction, EditorFaction editorFaction)
        {
            var editorHomeSector = GetEditorSector(modelFaction.HomeSector);
            if (editorHomeSector != null)
            {
                if (modelFaction.HomeSectorPosition.HasValue)
                {
                    var homeSector = new GameObject($"{editorFaction.CustomName}_HomeSector");
                    homeSector.transform.SetParent(editorHomeSector.transform);
                    homeSector.transform.localPosition = modelFaction.HomeSectorPosition.Value.ToVector3();
                }
                else
                {
                    editorFaction.HomeSectorTransform = editorHomeSector.transform;
                }
            }
        }

        private void ImportUnits()
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

            this.editorUnitsById.Add(modelUnit.Id, editorUnit);
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
    }
}
