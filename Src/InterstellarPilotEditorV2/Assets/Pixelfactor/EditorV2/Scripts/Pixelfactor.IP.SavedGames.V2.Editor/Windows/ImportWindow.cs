using Pixelfactor.IP.SavedGames.V2.Editor.Tools;
using Pixelfactor.IP.SavedGames.V2.Editor.Tools.Main.Import;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Windows
{
    public class ImportWindow : EditorWindow
    {
        static void Init()
        {
            ShowNew();
        }

        public static void ShowNew()
        {
            ImportWindow window = EditorWindow.GetWindow<ImportWindow>();
            var icon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Pixelfactor/EditorV2/Textures/DevLogoCentered256.png");
            window.titleContent = new GUIContent("IP Editor V2", icon);
        }

        void OnGUI()
        {
            GuiHelper.Subtitle("Import", "Import scenario data from an existing file");

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
                            ImportTool.ImportFromFile(filePath, savedGame);
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
