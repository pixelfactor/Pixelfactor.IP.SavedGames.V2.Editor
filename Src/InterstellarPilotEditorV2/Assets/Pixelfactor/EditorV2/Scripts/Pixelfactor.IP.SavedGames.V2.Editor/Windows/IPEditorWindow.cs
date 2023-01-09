using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Windows
{
    public class IPEditorWindow : EditorWindow
    {
        private int currentTab = MainWindowId;

        public const int MainWindowId = 0;
        public const int BuildWindowId = 1;
        public const int SpawnWindowId = 2;
        public const int EditWindowId = 3;
        public const int RefineWindowId = 4;

        private BuildWindow buildWindow = new BuildWindow();
        private SpawnWindow spawnWindow = new SpawnWindow();
        private EditWindow editWindow = new EditWindow();
        private RefineWindow refineWindow = new RefineWindow();
        private MainWindow mainWindow = new MainWindow();

        [MenuItem("Window/IP Editor V2")]
        static void Init()
        {
            IPEditorWindow window = EditorWindow.GetWindow<IPEditorWindow>();
            window.minSize = new Vector2(600, 400);
            var icon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Pixelfactor/EditorV2/Textures/DevLogoCentered256.png");
            window.titleContent = new GUIContent("IP Editor V2", icon);
        }

        void OnGUI()
        {
            currentTab = GUILayout.Toolbar(currentTab, new string[] { "Main", "Build", "Spawn", "Edit", "Refine" });

            EditorGUILayout.Space();

            switch (currentTab)
            {
                case MainWindowId:
                    {
                        this.mainWindow.Draw();
                    }
                    break;
                case BuildWindowId:
                    {
                        this.buildWindow.Draw();
                    }
                    break;
                case SpawnWindowId:
                    {
                        this.spawnWindow.Draw();
                    }
                    break;
                case EditWindowId:
                    {
                        this.editWindow.Draw();
                    }
                    break;
                case RefineWindowId:
                    {
                        this.refineWindow.Draw();
                    }
                    break;
            }

            GUILayout.FlexibleSpace();

            GUIStyle bImageStyle = new GUIStyle(EditorStyles.label);
            bImageStyle.alignment = TextAnchor.LowerRight;
            var icon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Pixelfactor/EditorV2/Textures/Background.png");
            GUILayout.Button(icon, bImageStyle);
        }
    }
}
