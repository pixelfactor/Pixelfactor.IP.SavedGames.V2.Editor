using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using Pixelfactor.IP.SavedGames.V2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools.Main.Export
{
    public class AddAmmoTool
    {
        private Dictionary<int, EditorComponentClass> componentClassesById = new Dictionary<int, EditorComponentClass>(1000);

        private Dictionary<int, List<EditorComponentBayRef>> componentBaysByUnitId = new Dictionary<int, List<EditorComponentBayRef>>(50);

        public AddAmmoTool(CustomSettings customSettings)
        {
            this.componentClassesById.Clear();
            this.LoadComponentClasses(customSettings.ComponentPrefabsPath);
            this.LoadComponentBays(customSettings.ComponentBayPrefabsPath);
        }

        private void LoadComponentBays(string bayPrefabsPath)
        {
            foreach (var componentBay in GameObjectHelper.GetPrefabsOfTypeFromPath<EditorComponentBayRef>(bayPrefabsPath))
            {
                var parts = componentBay.name.Split("_");
                var shipClassName = parts[1];
                if (System.Enum.TryParse($"Ship_{shipClassName}", out ModelUnitClass modelUnitClass) ||
                    System.Enum.TryParse($"Station_{shipClassName}", out modelUnitClass))
                {
                    if (!this.componentBaysByUnitId.TryGetValue((int)modelUnitClass, out List<EditorComponentBayRef> componentBays))
                    {
                        componentBays = new List<EditorComponentBayRef>(20);
                        this.componentBaysByUnitId.Add((int)modelUnitClass, componentBays);
                    }

                    componentBays.Add(componentBay);
                }
            }
        }

        private void LoadComponentClasses(string componentPrefabsBasePath)
        {
            foreach (var componentClass in GameObjectHelper.GetPrefabsOfTypeFromPath<EditorComponentClass>(
                componentPrefabsBasePath, 
                "*",
                new System.IO.EnumerationOptions { RecurseSubdirectories = true }))
            {
                this.componentClassesById.Add(componentClass.UniqueId, componentClass);
            }
        }

        public void AddAmmo(EditorScenario editorScenario, Model.SavedGame savedGame, bool exportByDefault)
        {
            foreach (var editorUnit in editorScenario.GetComponentsInChildren<EditorUnit>())
            {
                if (!editorUnit.ModelUnitClass.IsShipOrStation())
                    continue;

                if (!ShouldExportUnitCargo(editorUnit, exportByDefault))
                    continue;

                var modelUnit = savedGame.Units.FirstOrDefault(e => e.Id == editorUnit.Id);
                if (modelUnit != null)
                {
                    AddAmmoToUnit(editorScenario, editorUnit, savedGame, modelUnit);
                }
                
            }
        }

        private void AddAmmoToUnit(
            EditorScenario editorScenario,
            EditorUnit editorUnit, 
            SavedGame savedGame, 
            ModelUnit modelUnit)
        {
            var bays = GetUnitBays(editorUnit.ModelUnitClass);
            if (bays == null)
            {
                Debug.LogWarning("Cannot add ammo to unit as no component bays were found for type of: " + modelUnit.Class);
                return;
            }

            foreach (var bay in bays)
            { 
                var component = GetEffectiveComponentInBay(modelUnit, bay);
                if (component != null)
                {
                    var cargoItems = component.GetComponent<EditorCargoBayItems>();
                    if (cargoItems != null)
                    {
                        foreach (var cargoItem in cargoItems.Items)
                        {
                            AddCargoToUnit(modelUnit, cargoItem.CargoClass, cargoItem.Quantity);
                        }
                    }
                }
            }
        }

        private List<EditorComponentBayRef> GetUnitBays(ModelUnitClass modelUnitClass)
        {
            return this.componentBaysByUnitId.GetValueOrDefault((int)modelUnitClass);
        }

        private void AddCargoToUnit(ModelUnit modelUnit, EditorCargoClassRef cargoClass, int quantity)
        {
            if (modelUnit.ComponentUnitData == null)
                modelUnit.ComponentUnitData = new ModelComponentUnitData();

            if (modelUnit.ComponentUnitData.CargoData == null)
                modelUnit.ComponentUnitData.CargoData = new ModelComponentUnitCargoData();

            modelUnit.ComponentUnitData.CargoData.Items.Add(new ModelComponentUnitCargoDataItem
            {
                CargoClass = cargoClass.CargoClass,
                Quantity = quantity
            });
        }

        private EditorComponentClass GetEffectiveComponentInBay(ModelUnit modelUnit, EditorComponentBayRef bay)
        {
            var component = bay.DefaultComponent;

            var mod = modelUnit.ComponentUnitData?.ModData?.Items?.FirstOrDefault(e => e.BayId == bay.BayId);
            if (mod != null)
            {
                return this.componentClassesById.GetValueOrDefault((int)mod.ComponentClass);
            }

            return component;
        }

        private bool ShouldExportUnitCargo(EditorUnit editorUnit, bool exportByDefault)
        {
            switch (editorUnit.EditorAutoExportAmmo)
            {
                case EditorUnit.AutoExportAmmoOption.DontExport:
                    return false;
                case EditorUnit.AutoExportAmmoOption.Export:
                    return true;
                default:
                case EditorUnit.AutoExportAmmoOption.DontCare:
                    return exportByDefault;
            }
        }
    }
}
