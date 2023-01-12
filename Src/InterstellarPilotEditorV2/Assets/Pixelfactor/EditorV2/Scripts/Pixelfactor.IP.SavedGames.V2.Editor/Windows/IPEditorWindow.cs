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
        public const int MiscWindowId = 6;
        public const int StatsWindowId = 5;

        private BuildWindow buildWindow = new BuildWindow();
        private SpawnWindow spawnWindow = new SpawnWindow();
        private EditWindow editWindow = new EditWindow();
        private RefineWindow refineWindow = new RefineWindow();
        private MainWindow mainWindow = new MainWindow();
        private MiscWindow miscWindow = new MiscWindow();
        private StatsWindow statsWindow = new StatsWindow();

        private Vector2 scrollPosition = Vector2.zero;   

        [MenuItem("Window/IP Editor V2")]
        static void Init()
        {
            IPEditorWindow window = EditorWindow.GetWindow<IPEditorWindow>();
            var icon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Pixelfactor/EditorV2/Textures/DevLogoCentered256.png");
            window.titleContent = new GUIContent("IP Editor V2", icon);
        }

        void OnGUI()
        {
            var icon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Pixelfactor/EditorV2/Textures/Background.png");

            var drawArea = new Rect(this.position.width - 256, this.position.height - 256, 256, 256);
            GUI.DrawTexture(drawArea, icon, ScaleMode.ScaleAndCrop, true, 0.0f, new Color(1.0f, 1.0f, 1.0f, 0.25f), 0.0f, 0.0f);

            var guiContents = new GUIContent[]
            {
                new GUIContent("Main", "File operations like import/export"),
                new GUIContent("Build", "Tools to construct or amend universe sectors"),
                new GUIContent("Spawn", "Tools to spawn objects inside sectors"),
                new GUIContent("Edit", "Tools to edit existing universe objects"),
                new GUIContent("Refine", "Tools to cleanup the current scenario"),
                new GUIContent("Stats", "Info about the current scenario"),
                new GUIContent("Misc", "Tools that don't fit in the other categories"),
            };

            currentTab = GUILayout.Toolbar(currentTab, guiContents);

            EditorGUILayout.Space();

            this.scrollPosition = EditorGUILayout.BeginScrollView(this.scrollPosition);

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
                case StatsWindowId:
                    {
                        this.statsWindow.Draw();
                    }
                    break;
                case MiscWindowId:
                    {
                        this.miscWindow.Draw();
                    }
                    break;
            }

            GUILayout.FlexibleSpace();

            EditorGUILayout.EndScrollView();
        }
    }
}
