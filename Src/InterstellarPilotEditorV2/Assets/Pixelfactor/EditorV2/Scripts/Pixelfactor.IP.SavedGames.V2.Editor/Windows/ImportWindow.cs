using Pixelfactor.IP.SavedGames.V2.Editor.Tools;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Main.Import;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Windows
{
    public class ImportWindow : EditorWindow
    {
        private bool exitOnFirstError = true;

        static void Init()
        {
            ShowNew();
        }

        public static void ShowNew()
        {
            ImportWindow window = EditorWindow.GetWindow<ImportWindow>();
            var icon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Pixelfactor/EditorV2/Textures/DevLogoCentered256.png");
            window.titleContent = new GUIContent("Import", icon);
        }

        void OnGUI()
        {
            GuiHelper.Subtitle("Import (Experimental)", "Import scenario data from an existing file");

            GuiHelper.HelpPrompt("Importing scenarios is currently experimental. It works well for importing station universe objects e.g. sectors, wormholes, gas clouds, asteroids. All other objects are likely to have data missing.");

            EditorGUILayout.PrefixLabel(new GUIContent("Exit on error", "Whether to abort the import operation on the first error encountered"));
            this.exitOnFirstError = EditorGUILayout.Toggle(this.exitOnFirstError, GUILayout.ExpandWidth(false));

            if (GUILayout.Button(
            new GUIContent(
                "Import",
                "Pick a file to import"),
            GuiHelper.ButtonLayout))
            {
                var filePath = EditorUtility.OpenFilePanel("Import", "", "dat");

                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    try
                    {
                        var savedGame = CreateNewScenarioTool.CreateNewEmpty();
                        if (savedGame != null)
                        {
                            ImportTool.ImportFromFile(filePath, savedGame, this.exitOnFirstError);

                            EditorUtility.DisplayDialog("Import", "Scenario was imported succcesfully", "OK");
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogException(ex);
                        EditorUtility.DisplayDialog("Import", "An error occured during import. Check the console for further details.", "OK");
                    }
                }
            }
        }
    }
}
