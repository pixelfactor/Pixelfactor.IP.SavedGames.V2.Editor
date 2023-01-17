using Pixelfactor.IP.SavedGames.V2.Editor.Tools;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Build.Grow;
using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using System;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Pixelfactor.IP.SavedGames.V2.Editor.Settings;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Windows
{
    /// <summary>
    /// Window that allows the user to create a new scenario
    /// </summary>
    public class CreateWindow : EditorWindow
    {
        private int numSectors = 8;

        public void Foo()
        {
            CreateNewScenarioTool.CreateNewSingleSector();
            GUIUtility.ExitGUI();
        }

        void OnGUI()
        {
            GuiHelper.Subtitle("Create custom scenario", "Creates a new scenario based on paramters");

            GuiHelper.HelpPrompt("It is recommended to create smaller universes!");

            EditorGUILayout.PrefixLabel(new GUIContent("Sectors", "The number of sectors that the universe should have"));
            this.numSectors = EditorGUILayout.IntSlider(this.numSectors, 1, 256, GUILayout.ExpandWidth(false));

            if (GUILayout.Button(new GUIContent(
                "Create",
                "Creates the universe (this may take a while for a biggun)"),
                GuiHelper.ButtonLayout))
            {
                OnCreate();
            }
        }

        private void OnCreate()
        {
            Debug.Log($"Creating a new scenario with {this.numSectors} sectors...");

            var scenario = CreateNewScenarioTool.CreateNewSingleSector();

            var firstSector = scenario.GetSectors().FirstOrDefault();

            if (firstSector == null)
            {
                Debug.LogError("Expected that there would be one sector created", this);
                return;
            }

            var settings = CustomSettings.GetOrCreateSettings();
            var newSectorsToCreate = this.numSectors - 1;
            if (newSectorsToCreate > 0)
            {
                var sectors = new EditorSector[] { firstSector }.ToList();
                new BuildWindow().AutoGrow(sectors, sectors, settings, newSectorsToCreate);
            }

            Debug.Log($"Finished creating new custom scenario");

            GUIUtility.ExitGUI();
        }
        public static void ShowNew()
        {
            var window = EditorWindow.GetWindow<CreateWindow>();
            var icon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Pixelfactor/EditorV2/Textures/DevLogoCentered256.png");
            window.titleContent = new GUIContent("Create", icon);
        }
    }
}
