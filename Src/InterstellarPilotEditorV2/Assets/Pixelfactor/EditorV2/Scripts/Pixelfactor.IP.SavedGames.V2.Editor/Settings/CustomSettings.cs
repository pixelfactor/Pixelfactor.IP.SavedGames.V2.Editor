using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Settings
{
    public class CustomSettings : ScriptableObject
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

        /// <summary>
        /// The width and height of each scene
        /// </summary>
        [SerializeField]
        private float sectorSize = 16000;

        /// <summary>
        /// The max distance a unit can traverse from the center of a sector
        /// This should be roughly just less than SectorSize / 2
        /// </summary>
        [Tooltip("The max distance a unit can traverse from the center of a sector. This should be roughly just less than SectorSize / 2")]
        [SerializeField]
        private float maxUnitDistanceFromOriginLowerBound = 6500.0f;

        /// <summary>
        /// The max distance a unit can traverse from the center of a sector
        /// This should be roughly just less than SectorSize / 2
        /// </summary>
        [Tooltip("The max distance a unit can traverse from the center of a sector. This should be roughly just less than SectorSize / 2")]
        [SerializeField]
        private float maxUnitDistanceFromOriginUpperBound = 7000.0f;

        [Tooltip("The path to the prefab that is used to create a new scenario with no sectors")]
        [SerializeField]
        private string emptyScenePrefabPath = "Assets/Pixelfactor/EditorV2/Prefabs/ScenarioTemplates/ScenarioTemplate.prefab";

        [Tooltip("The path to the prefab that is used to create new sectors")]
        [SerializeField]
        private string sectorPrefabPath = "Assets/Pixelfactor/EditorV2/Prefabs/Sectors/Sector.prefab";

        [Tooltip("The path to the text asset where sector names are defined")]
        [SerializeField]
        private string sectorNamesPath = "Assets/Pixelfactor/EditorV2/Data/SectorNameList1.txt";

        [Tooltip("Whether a sector is given a unique seed automatically on export")]
        [SerializeField]
        private bool export_AutosetSectorSeed = true;

        [Tooltip("Whether a unit is given a unique seed automatically on export")]
        [SerializeField]
        private bool export_AutosetUnitSeed = true;

        [Tooltip("Whether objects are given a unique id automatically on export")]
        [SerializeField]
        private bool export_AutosetIds = true;

        [Tooltip("Whether to remove wormholes without a target on export")]
        [SerializeField]
        private bool export_RemoveUntargettedWormholes = true;

        [Tooltip("Determines how the spacing between sectors is converted to the spacing on the universe map")]
        [SerializeField]
        private float universeMapScaleFactor = 0.005f;

        [Tooltip("Determines the min distance between new sectors")]
        [SerializeField]
        private float minDistanceBetweenSectors = 40000.0f;

        [Tooltip("Determines the max distance between new sectors")]
        [SerializeField]
        private float maxDistanceBetweenSectors = 125000.0f;

        [Tooltip("Determines the min angle between wormholes. Determines how far apart wormholes are. In degrees.")]
        [SerializeField]
        private float minAngleBetweenWormholes = 60.0f;

        [Tooltip("The save version that the editor works with")]
        private System.Version saveVersion = new System.Version(2, 0, 50);

        [Tooltip("The root folder of where units are found")]
        [SerializeField]
        private string unitPrefabPath = "Assets/Pixelfactor/EditorV2/Prefabs/Units";

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

        public float SectorSize
        {
            get { return this.sectorSize; }
        }

        public float MaxUnitDistanceFromOriginLowerBound
        {
            get { return this.maxUnitDistanceFromOriginLowerBound; }
        }

        public float MaxUnitDistanceFromOriginUpperBound
        {
            get { return this.maxUnitDistanceFromOriginUpperBound; }
        }

        public string EmptyScenePrefabPath
        {
            get { return this.emptyScenePrefabPath; }
        }

        public string SectorPrefabPath
        {
            get { return this.sectorPrefabPath; }
        }

        public bool Export_AutosetSectorSeed
        {
            get { return this.export_AutosetSectorSeed; }
        }

        public bool Export_AutosetUnitSeed
        {
            get { return this.export_AutosetUnitSeed; }
        }

        public bool Export_AutosetIds
        {
            get { return export_AutosetIds; }
        }

        public string SectorNamesPath
        {
            get { return this.sectorNamesPath; }
        }

        public float UniverseMapScaleFactor
        {
            get { return this.universeMapScaleFactor; }
        }

        public float MinDistanceBetweenSectors
        {
            get { return this.minDistanceBetweenSectors; }
        }

        public float MaxDistanceBetweenSectors
        {
            get { return this.maxDistanceBetweenSectors; }
        }

        public float MinAngleBetweenWormholes
        {
            get { return this.minAngleBetweenWormholes; }
        }

        public bool Export_RemoveUntargettedWormholes
        {
            get { return this.export_RemoveUntargettedWormholes; }
        }

        public System.Version SaveVersion
        {
            get { return this.saveVersion; }
        }

        public string UnitPrefabPath
        {
            get { return this.unitPrefabPath; }
        }
    }

    // Register a SettingsProvider using IMGUI for the drawing framework:
    static class MyCustomSettingsIMGUIRegister
    {
        public const string SettingsProviderPath = "Project/IP2EditorSetings";
        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            // First parameter is the path in the Settings window.
            // Second parameter is the scope of this setting: it only appears in the Project Settings window.
            var provider = new SettingsProvider(SettingsProviderPath, SettingsScope.Project)
            {
                // By default the last token of the path is used as display name if no label is provided.
                label = "IP2 Editor",
                // Create the SettingsProvider and initialize its drawing (IMGUI) function in place:
                guiHandler = (searchContext) =>
                {
                    var settings = CustomSettings.GetSerializedSettings();
                    EditorGUILayout.LabelField("General", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(settings.FindProperty("gameExecutablePath"), new GUIContent("Game executable path"));
                    EditorGUILayout.PropertyField(settings.FindProperty("defaultExportPath"), new GUIContent("Export path"));
                    EditorGUILayout.PropertyField(settings.FindProperty("runScenarioOnPlayMode"), new GUIContent("Use Unity play button"));
                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("Export", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(settings.FindProperty("export_AutosetSectorSeed"), new GUIContent("Autoset sector seeds"));
                    EditorGUILayout.PropertyField(settings.FindProperty("export_AutosetUnitSeed"), new GUIContent("Autoset unit seeds"));
                    EditorGUILayout.PropertyField(settings.FindProperty("export_AutosetIds"), new GUIContent("Autoset unique ids"));
                    EditorGUILayout.PropertyField(settings.FindProperty("export_RemoveUntargettedWormholes"), new GUIContent("Remove untargetted wormholes"));
                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("Universe Build", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(settings.FindProperty("minAngleBetweenWormholes"), new GUIContent("Min wormhole angle"));
                    EditorGUILayout.PropertyField(settings.FindProperty("minDistanceBetweenSectors"), new GUIContent("Min distance between sectors"));
                    EditorGUILayout.PropertyField(settings.FindProperty("maxDistanceBetweenSectors"), new GUIContent("Max distance between sectors"));
                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("Advanced", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(settings.FindProperty("sectorSize"), new GUIContent("Sector size"));
                    EditorGUILayout.PropertyField(settings.FindProperty("maxUnitDistanceFromOriginLowerBound"), new GUIContent("Max sector distance (lower)"));
                    EditorGUILayout.PropertyField(settings.FindProperty("maxUnitDistanceFromOriginUpperBound"), new GUIContent("Max sector distance (upper)"));
                    EditorGUILayout.PropertyField(settings.FindProperty("emptyScenePrefabPath"), new GUIContent("Empty scenario path"));
                    EditorGUILayout.PropertyField(settings.FindProperty("sectorPrefabPath"), new GUIContent("Sector prefab path"));
                    EditorGUILayout.PropertyField(settings.FindProperty("sectorNamesPath"), new GUIContent("Sector names path"));
                    EditorGUILayout.PropertyField(settings.FindProperty("universeMapScaleFactor"), new GUIContent("Universe scale factor"));
                    EditorGUILayout.PropertyField(settings.FindProperty("unitPrefabPath"), new GUIContent("Unit prefab path"));

                    settings.ApplyModifiedPropertiesWithoutUndo();
                },

                // Populate the search keywords to enable smart search filtering and label highlighting:
                keywords = new HashSet<string>(new[] { "Executable Path" })
            };

            return provider;
        }
    }
}
