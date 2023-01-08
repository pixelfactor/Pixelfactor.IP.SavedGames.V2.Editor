﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Settings
{
    class CustomSettings : ScriptableObject
    {
        public const string k_MyCustomSettingsPath = "Assets/Pixelfactor/EditorV2/IP2EditorSettings.asset";

        [Tooltip("The full path including file name of the Interstellar Pilot 2 executable file")]
        [SerializeField]
        private string gameExecutablePath;

        [Tooltip("The folder path where save games are exported to. Leave blank to export to Unity's temporary area")]
        [SerializeField]
        private string defaultExportPath;

        [Tooltip("Whether clicking the Unity play button will run the scenario")]
        [SerializeField]
        private bool runScenarioOnPlayMode = true;

        internal static CustomSettings GetOrCreateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<CustomSettings>(k_MyCustomSettingsPath);
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<CustomSettings>();
                settings.gameExecutablePath = "";
                settings.defaultExportPath = "";
                settings.runScenarioOnPlayMode = true;
                AssetDatabase.CreateAsset(settings, k_MyCustomSettingsPath);
                AssetDatabase.SaveAssets();
            }
            return settings;
        }

        internal static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }

        public string DefaultExportPath
        {
            get { return this.defaultExportPath; }
        }

        public string GameExecutablePath
        {
            get { return this.gameExecutablePath; }
        }

        public bool RunScenarioOnPlayMode
        {
            get { return this.runScenarioOnPlayMode; }
        }
    }

    // Register a SettingsProvider using IMGUI for the drawing framework:
    static class MyCustomSettingsIMGUIRegister
    {
        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            // First parameter is the path in the Settings window.
            // Second parameter is the scope of this setting: it only appears in the Project Settings window.
            var provider = new SettingsProvider("Project/IP2EditorSetings", SettingsScope.Project)
            {
                // By default the last token of the path is used as display name if no label is provided.
                label = "IP2 Editor",
                // Create the SettingsProvider and initialize its drawing (IMGUI) function in place:
                guiHandler = (searchContext) =>
                {
                    var settings = CustomSettings.GetSerializedSettings();
                    EditorGUILayout.PropertyField(settings.FindProperty("gameExecutablePath"), new GUIContent("Game executable path"));
                    EditorGUILayout.PropertyField(settings.FindProperty("defaultExportPath"), new GUIContent("Export path"));
                    EditorGUILayout.PropertyField(settings.FindProperty("runScenarioOnPlayMode"), new GUIContent("Use Unity play button"));
                    settings.ApplyModifiedPropertiesWithoutUndo();
                },

                // Populate the search keywords to enable smart search filtering and label highlighting:
                keywords = new HashSet<string>(new[] { "Executable Path" })
            };

            return provider;
        }
    }
}
