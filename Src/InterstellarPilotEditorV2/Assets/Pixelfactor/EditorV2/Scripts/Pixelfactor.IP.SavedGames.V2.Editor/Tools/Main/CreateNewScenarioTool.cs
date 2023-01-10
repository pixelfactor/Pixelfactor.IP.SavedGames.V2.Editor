using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Edit;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Spawning;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools
{
    public static class CreateNewScenarioTool
    {
        /// <summary>
        /// Creates a new unity scene with a heirachy that creates the basics
        /// </summary>
        public static EditorScenario CreateNewSingleSector()
        {
            var savedGame = CreateNewEmpty();
            if (savedGame != null)
            {
                SetupDefaultScenario(savedGame);
            }

            return savedGame;
        }

        public static void SetupDefaultScenario(EditorScenario savedGame)
        {
            var settings = CustomSettings.GetOrCreateSettings();
            var sector = Spawn.Sector(savedGame, settings.SectorPrefabPath);
            var playerFaction = Spawn.Faction(savedGame, settings.PlayerFactionPrefabPath);
            var playerUnit = Spawn.Unit(sector, Model.ModelUnitClass.Ship_ShuttleA, settings.UnitPrefabsPath);

            playerUnit.Faction = playerFaction;

            var playerPerson = GameObjectHelper.Instantiate<EditorPerson>(playerUnit.transform);
            playerPerson.Faction = playerFaction;
            playerPerson.CustomName = "Player";

            var player = GameObjectHelper.Instantiate<EditorPlayer>(savedGame, "Player");
            player.Person = playerPerson;

            Selector.SelectFirstSector(savedGame);

            AutoNameObjects.AutoNameAllObjects(savedGame);
        }

        /// <summary>
        /// Creates a new unity scene with a heirachy that creates the basics
        /// </summary>
        public static EditorScenario CreateNewEmpty()
        {
            var settings = CustomSettings.GetOrCreateSettings();

            return CreateNew(settings.EmptyScenePrefabPath);
        }

        public static EditorScenario CreateNew(string prefabPath)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

                return InstantiateSavedGame(prefabPath);
            }

            return null;
        }

        public static EditorScenario InstantiateSavedGame(string defaultPrefabPath)
        {
            var templatePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(defaultPrefabPath);

            if (templatePrefab == null)
            {
                throw new System.Exception($"Unable to load template prefab. Check that the asset exists at path: \"{defaultPrefabPath}\"");
            }

            var go = (GameObject)PrefabUtility.InstantiatePrefab(templatePrefab.gameObject);
            var savedGame = go.GetComponentInChildren<EditorScenario>();

            EditSectorTool.RandomizeAllWithoutDirty(savedGame);

            return savedGame;
        }
    }
}
