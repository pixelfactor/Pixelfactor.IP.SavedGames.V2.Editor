using Pixelfactor.IP.SavedGames.V2.Editor.Tools;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Windows
{
    public class BuildWindow
    {
        public void Draw()
        {
            var enabled = ConnectSectorsTool.CanConnectSelectedSectorsWithWormholes();

            EditorGUI.BeginDisabledGroup(!enabled);

            if (GUILayout.Button(new GUIContent(
                "Connect selected sectors",
                "Connects the currently selected sectors with wormholes")))
            {
                ConnectSectorsTool.ConnectSelectedSectorsWithWormholesMenuItem();
            }

            EditorGUI.EndDisabledGroup();
        }
    }
}
