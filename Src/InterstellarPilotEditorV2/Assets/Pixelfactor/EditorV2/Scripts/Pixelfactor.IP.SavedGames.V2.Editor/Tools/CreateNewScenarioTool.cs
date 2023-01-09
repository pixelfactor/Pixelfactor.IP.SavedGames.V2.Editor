using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
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
        public static void CreateNew()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

                var defaultPrefabPath = CustomSettings.GetOrCreateSettings().DefaultScenePrefabPath;

                var templatePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(defaultPrefabPath);

                if (templatePrefab == null)
                {
                    throw new System.Exception($"Unable to load template prefab. Check that the asset exists at path: \"{defaultPrefabPath}\"");
                }

                PrefabUtility.InstantiatePrefab(templatePrefab.gameObject);
            }
        }
    }
}
