using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using Pixelfactor.IP.SavedGames.V2.Model;
using System;
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

        public ImportOperation(SavedGame savedGameModel, EditorScenario editorScenario)
        {
            this.editorScenario = editorScenario;
            this.model = savedGameModel;

            this.sectorsTransform = this.editorScenario.SectorsRoot != null ? this.editorScenario.SectorsRoot : this.editorScenario.transform;

            this.settings = CustomSettings.GetOrCreateSettings();
        }

        public void Import()
        {
            this.ImportSectors();
        }

        private void ImportSectors()
        {
            var sectorPrefab = AssetDatabase.LoadAssetAtPath<EditorSector>(this.settings.SectorPrefabPath);

            foreach (var modelSector in this.model.Sectors)
            {
                var editorSector = PrefabHelper.Instantiate(sectorPrefab, this.sectorsTransform);
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
            }
        }
    }
}
