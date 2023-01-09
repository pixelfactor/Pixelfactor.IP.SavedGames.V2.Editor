using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools.Wormholes
{
    public static class RemoveUntargettedWormholesTool
    {
        public static int Remove(EditorSavedGame editorSavedGame)
        {
            var count = 0;
            foreach (var sector in editorSavedGame.GetComponentsInChildren<EditorSector>())
            {
                foreach (var wormhole in sector.GetComponentsInChildren<EditorUnitWormholeData>())
                {
                    if ((wormhole.IsUnstable && wormhole.UnstableTarget == null) ||
                        (!wormhole.IsUnstable && wormhole.TargetWormholeUnit == null))
                    {
                        count++;
                        Object.DestroyImmediate(wormhole.gameObject);

                        EditorUtility.SetDirty(sector.gameObject);
                    }
                }
            }

            return count;
        }
    }
}
