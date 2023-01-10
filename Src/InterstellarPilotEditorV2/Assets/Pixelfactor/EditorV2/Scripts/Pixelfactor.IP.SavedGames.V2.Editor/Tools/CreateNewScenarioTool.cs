using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Edit;
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
        public static EditorSavedGame CreateNew()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

                var settings = CustomSettings.GetOrCreateSettings();

                return InstantiateScenePrefab(settings);
            }

            return null;
        }

        public static EditorSavedGame InstantiateScenePrefab(CustomSettings settings)
        {
            var defaultPrefabPath = settings.DefaultScenePrefabPath;

            var templatePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(defaultPrefabPath);

            if (templatePrefab == null)
            {
                throw new System.Exception($"Unable to load template prefab. Check that the asset exists at path: \"{defaultPrefabPath}\"");
            }

            var go = (GameObject)PrefabUtility.InstantiatePrefab(templatePrefab.gameObject);
            var savedGame = go.GetComponentInChildren<EditorSavedGame>();

            EditSectorTool.RandomizeAll(savedGame);
            Selector.SelectFirstSector(savedGame);
            return savedGame;
        }
    }
}
