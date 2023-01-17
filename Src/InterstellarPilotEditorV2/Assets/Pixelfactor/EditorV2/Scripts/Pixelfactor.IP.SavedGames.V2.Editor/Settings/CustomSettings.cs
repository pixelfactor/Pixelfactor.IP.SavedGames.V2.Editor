using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Settings
{
    public class CustomSettings : ScriptableObject
    {
#if UNITY_EDITOR_WIN
        private const string defaultSteamInstallationPath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Interstellar Pilot 2\\Interstellar Pilot 2.exe";
#else
        private const string defaultSteamInstallationPath = "";
#endif

        public const string k_MyCustomSettingsPath = "Assets/Pixelfactor/EditorV2/IP2EditorSettings.asset";

        [Tooltip("The full path including file name of the Interstellar Pilot 2 executable file")]
        public string GameExecutablePath;

        [Tooltip("The folder path where save games are exported to. Leave blank to export to Unity's temporary area")]
        public string DefaultExportPath;

        [Tooltip("Whether clicking the Unity play button will run the scenario")]
        public bool RunScenarioOnPlayMode = false;

        /// <summary>
        /// The width and height of each scene
        /// </summary>
        public float SectorSize = 16000;

        /// <summary>
        /// The max distance a unit can traverse from the center of a sector
        /// This should be roughly just less than SectorSize / 2
        /// </summary>
        [Tooltip("The max distance a unit can traverse from the center of a sector. This should be roughly just less than SectorSize / 2")]
        public float MaxUnitDistanceFromOriginLowerBound = 6500.0f;

        /// <summary>
        /// The max distance a unit can traverse from the center of a sector
        /// This should be roughly just less than SectorSize / 2
        /// </summary>
        [Tooltip("The max distance a unit can traverse from the center of a sector. This should be roughly just less than SectorSize / 2")]
        public float MaxUnitDistanceFromOriginUpperBound = 7000.0f;

        [Tooltip("The path to the prefab that is used to create a new scenario with no sectors")]
        public string EmptyScenePrefabPath = "Assets/Pixelfactor/EditorV2/Prefabs/ScenarioTemplates/ScenarioTemplate.prefab";

        [Tooltip("The path to the prefab that is used to create new sectors")]
        public string SectorPrefabPath = "Assets/Pixelfactor/EditorV2/Prefabs/Sectors/Sector.prefab";

        [Tooltip("The path to the text asset where sector names are defined")]
        public string SectorNamesPath = "Assets/Pixelfactor/EditorV2/Data/SectorNameList1.txt";

        [Tooltip("Whether a sector is given a unique seed automatically on export")]
        public bool Export_AutosetSectorSeed = true;

        [Tooltip("Whether a unit is given a unique seed automatically on export")]
        public bool Export_AutosetUnitSeed = true;

        [Tooltip("Whether objects are given a unique id automatically on export")]
        public bool Export_AutosetIds = true;

        [Tooltip("Whether to add ammo to ships/stations based on the weapons that they have")]
        public bool Export_AutoAddAmmo = true;

        [Tooltip("Whether to remove wormholes without a target on export")]
        public bool Export_RemoveUntargettedWormholes = true;

        [Tooltip("Determines how the spacing between sectors is converted to the spacing on the universe map")]
        public float UniverseMapScaleFactor = 0.005f;

        [Tooltip("Determines the min distance between new sectors")]
        public float MinDistanceBetweenSectors = 40000.0f;

        [Tooltip("Determines the max distance between new sectors")]
        public float MaxDistanceBetweenSectors = 125000.0f;

        [Tooltip("Determines the min angle between wormholes. Determines how far apart wormholes are. In degrees.")]
        public float MinAngleBetweenWormholes = 60.0f;

        [Tooltip("The save version that the editor works with")]
        public System.Version SaveVersion = new System.Version(2, 0, 50);

        [Tooltip("The root folder of where units are found")]
        public string UnitPrefabsPath = "Assets/Pixelfactor/EditorV2/Prefabs/Units";

        [Tooltip("The root folder of where cargo classes are found")]
        public string CargoClassPrefabsPath = "Assets/Pixelfactor/EditorV2/Prefabs/CargoClasses";

        [Tooltip("The path to the prefab used to create new factions")]
        public string FactionPrefabPath = "Assets/Pixelfactor/EditorV2/Prefabs/Factions/Faction.prefab";

        [Tooltip("The path to the prefab used to create the player faction")]
        public string PlayerFactionPrefabPath = "Assets/Pixelfactor/EditorV2/Prefabs/Factions/FactionPlayer.prefab";

        [Tooltip("The path to the prefab used for NPCs")]
        public string NpcPrefabPath = "Assets/Pixelfactor/EditorV2/Prefabs/NpcPilots/NpcPilot.prefab";

        [Tooltip("The path to the prefab used for fleets")]
        public string FleetPrefabPath = "Assets/Pixelfactor/EditorV2/Prefabs/Fleets/Fleet.prefab";

        [Tooltip("The path to ship component prefabs")]
        public string ComponentPrefabsPath = "Assets/Pixelfactor/EditorV2/Prefabs/Components";

        [Tooltip("The path to component bays")]
        public string ComponentBayPrefabsPath = "Assets/Pixelfactor/EditorV2/Prefabs/ComponentBays";

        [Tooltip("The path to asteroid types")]
        public string AsteroidTypesPath = "Assets/Pixelfactor/EditorV2/Prefabs/AsteroidTypes";

        [Tooltip("Lowest position to place a planet")]
        public float MinPlanetPositionY = -2500.0f;

        [Tooltip("Highest position to place a planet")]
        public float MaxPlanetPositionY = 1000.0f;

        [Tooltip("Minimum distance of a planet from the center of a sector")]
        public float MinPlanetDistance = 19800.0f;

        [Tooltip("Maximum distance of a planet from the center of a sector")]
        public float MaxPlanetDistance = 20000.0f;

        [Tooltip("Minimum x rotation of a planet")]
        public float MinPlanetRotationX = 5.0f;

        [Tooltip("Maximum x rotation of a planet")]
        public float MaxPlanetRotationX = 20.0f;

        [Tooltip("Maximum x rotation of a planet")]
        public int MinNumberPlanetSectors = 1;

        [Tooltip("Determines how many sectors become planetary sectors")]
        public float PlanetSectorWeighting = 1.2f;

        [Tooltip("Determines how many sectors become deep space sectors (without planets or asteroids)")]
        public float DeepSpaceSectorWeighting = 1.4f;

        [Tooltip("Determines how many sectors are given asteroids")]
        public float AsteroidSectorWeighting = 2.6f;

        [Tooltip("Minimum number of sectors that should contain asteroids")]
        public int MinNumberAsteroidSectors = 1;

        [Tooltip("Minimum distance between asteroid clusters")]
        public float MinDistanceBetweenAsteroidClusters = 400.0f;

        [Tooltip("Minimum radius of asteroid clusters")]
        public float MinAsteroidClusterRadius = 1500.0f;

        [Tooltip("Maximum radius of asteroid clusters")]
        public float MaxAsteroidClusterRadius = 8000.0f;

        [Range(0.5f, 32.0f)]
        [Tooltip("Determines the random radius given to asteroid clusters. Increase to favour lower values")]
        public float AsteroidClusterRadiusPower = 1.5f;

        [Tooltip("The inclusive minimum number of asteroid clusters to generate in a sector")]
        public int MinAsteroidClusterCount = 1;

        [Tooltip("The inclusive maximum number of asteroid clusters to generate in a sector")]
        public int MaxAsteroidClusterCount = 3;

        [Tooltip("Determines the probability that an asteroid cluster will have a gas cloud")]
        public float ProbabilityOfGeneratingGasCloud = 0.75f;

        [Tooltip("Determines the randomness of the asteroid type that is given to sectors")]
        public float AsteroidTypeWeightRandomness = 0.0f;

        public static CustomSettings GetOrCreateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<CustomSettings>(k_MyCustomSettingsPath);
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<CustomSettings>();

                if (!string.IsNullOrWhiteSpace(defaultSteamInstallationPath) && System.IO.File.Exists(defaultSteamInstallationPath))
                {
                    settings.GameExecutablePath = defaultSteamInstallationPath;
                }

                AssetDatabase.CreateAsset(settings, k_MyCustomSettingsPath);
                AssetDatabase.SaveAssets();
            }
            return settings;
        }

        internal static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
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
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.GameExecutablePath)), new GUIContent("Game executable path"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.DefaultExportPath)), new GUIContent("Export path"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.RunScenarioOnPlayMode)), new GUIContent("Use Unity play button"));
                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("Export", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.Export_AutosetSectorSeed)), new GUIContent("Autoset sector seeds"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.Export_AutosetUnitSeed)), new GUIContent("Autoset unit seeds"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.Export_AutosetIds)), new GUIContent("Autoset unique ids"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.Export_RemoveUntargettedWormholes)), new GUIContent("Remove untargetted wormholes"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.Export_AutoAddAmmo)), new GUIContent("Auto-add ammo"));
                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("Universe Build", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.MinAngleBetweenWormholes)), new GUIContent("Min wormhole angle"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.MinDistanceBetweenSectors)), new GUIContent("Min distance between sectors"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.MaxDistanceBetweenSectors)), new GUIContent("Max distance between sectors"));
                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("Sector types", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.AsteroidSectorWeighting)), new GUIContent("Asteroid sector weight"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.PlanetSectorWeighting)), new GUIContent("Planet sector weight"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.DeepSpaceSectorWeighting)), new GUIContent("Deep space sector weight"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.MinNumberAsteroidSectors)), new GUIContent("Min asteroid sectors"));
                    
                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("Asteroid clusters", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.MinDistanceBetweenAsteroidClusters)), new GUIContent("Spacing"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.MinAsteroidClusterRadius)), new GUIContent("Min radius"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.MaxAsteroidClusterRadius)), new GUIContent("Max radius"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.AsteroidClusterRadiusPower)), new GUIContent("Radius power"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.MinAsteroidClusterCount)), new GUIContent("Min cluster count"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.MaxAsteroidClusterCount)), new GUIContent("Max cluster count"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.ProbabilityOfGeneratingGasCloud)), new GUIContent("Gas cloud probability"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.AsteroidTypeWeightRandomness)), new GUIContent("Type randomness"));
                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("Planets", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.MinPlanetPositionY)), new GUIContent("Min planet Y position"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.MaxPlanetPositionY)), new GUIContent("Max planet Y position"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.MinPlanetDistance)), new GUIContent("Min planet distance"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.MaxPlanetDistance)), new GUIContent("Max planet distance"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.MinPlanetRotationX)), new GUIContent("Min planet X rotation"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.MaxPlanetRotationX)), new GUIContent("Max planet X rotation"));
                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("Advanced", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.SectorSize)), new GUIContent("Sector size"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.MaxUnitDistanceFromOriginLowerBound)), new GUIContent("Max sector distance (lower)"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.MaxUnitDistanceFromOriginUpperBound)), new GUIContent("Max sector distance (upper)"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.EmptyScenePrefabPath)), new GUIContent("Empty scenario path"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.SectorPrefabPath)), new GUIContent("Sector prefab path"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.SectorNamesPath)), new GUIContent("Sector names path"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.UniverseMapScaleFactor)), new GUIContent("Universe scale factor"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.UnitPrefabsPath)), new GUIContent("Unit prefab path"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.ComponentPrefabsPath)), new GUIContent("Component prefab path"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.ComponentBayPrefabsPath)), new GUIContent("Component bay prefabs path"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.AsteroidTypesPath)), new GUIContent("Asteroid types path"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.FactionPrefabPath)), new GUIContent("Faction prefab path"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.FleetPrefabPath)), new GUIContent("Fleet prefab path"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.NpcPrefabPath)), new GUIContent("NPC prefab path"));
                    EditorGUILayout.PropertyField(settings.FindProperty(nameof(CustomSettings.PlayerFactionPrefabPath)), new GUIContent("Player faction prefab path"));

                    settings.ApplyModifiedPropertiesWithoutUndo();
                },

                // Populate the search keywords to enable smart search filtering and label highlighting:
                keywords = new HashSet<string>(new[] { "Executable Path" })
            };

            return provider;
        }
    }
}
