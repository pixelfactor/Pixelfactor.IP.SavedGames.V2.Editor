using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Functionality
{
    [InitializeOnLoad]
    public static class HeirachyDragDropHandler
    {
        static HeirachyDragDropHandler()
        {
            DragAndDrop.AddDropHandler(HierarchyDropHandler);
        }

        private static EditorFaction GetDraggedFaction()
        {
            var draggedObjects = DragAndDrop.objectReferences;
            if (draggedObjects != null &&
                draggedObjects.Length == 1 &&
                draggedObjects[0] is GameObject draggedGameObject)
            {
                return draggedGameObject.GetComponent<EditorFaction>();
            }

            return null;
        }

        private static DragAndDropVisualMode
            HierarchyDropHandler(int dropTargetInstanceID, HierarchyDropFlags dropMode, Transform parentForDraggedObjects, bool perform)
        {
            var draggedFaction = GetDraggedFaction();
            if (draggedFaction == null)
                return DragAndDropVisualMode.None;

            if ((dropMode & HierarchyDropFlags.DropUpon) == HierarchyDropFlags.None)
                return DragAndDropVisualMode.None;

            var target = EditorUtility.InstanceIDToObject(dropTargetInstanceID) as GameObject;
            if (target == null)
                return DragAndDropVisualMode.None;

            var targetUnit = target.GetComponent<EditorUnit>();
            if (targetUnit == null)
                return DragAndDropVisualMode.None;

            if (PrefabUtility.IsPartOfPrefabAsset(draggedFaction))
                return DragAndDropVisualMode.Rejected;

            if (!targetUnit.CanHaveFaction())
                return DragAndDropVisualMode.Rejected;

            if (perform)
            {
                if (targetUnit.Faction != draggedFaction)
                {
                    targetUnit.Faction = draggedFaction;
                    EditorUtility.SetDirty(targetUnit);
                }
            }

            return DragAndDropVisualMode.Link;
        }
    }
}
