using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools.Edit
{
    public static class EditFactionTool
    {
        public static void RandomizeEditorColor(EditorFaction faction)
        {
            faction.EditorColor = Random.ColorHSV(0.0f, 1.0f, 1.0f, 1.0f, 0.5f, 1.0f);
            EditorUtility.SetDirty(faction);
        }

        public static void Randomize(EditorFaction faction)
        {
            RandomizeEditorColor(faction);

            EditorUtility.SetDirty(faction);
        }
    }
}
