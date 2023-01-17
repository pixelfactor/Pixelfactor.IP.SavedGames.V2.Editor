using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using System.Collections.Generic;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools.Sectors
{
    public static class SectorFinder
    {
        static Queue<SectorBlueprintNode> nodeQueue = new Queue<SectorBlueprintNode>(1000);
        static HashSet<int> visitedNodes = new HashSet<int>(20);
        struct SectorBlueprintNode
        {
            public EditorSector Sector;
            public int JumpDistance;

            public SectorBlueprintNode(EditorSector sector, int jumpDistance) : this()
            {
                this.Sector = sector;
                this.JumpDistance = jumpDistance;
            }
        }

        public static (EditorSector sector, int? jumpDistance)? Find(
            EditorSector startSector,
            IEnumerable<EditorSector> sectors,
            System.Func<EditorSector, bool> predicate,
            int maxJumpDistance)
        {
            if (predicate(startSector))
            {
                return (startSector, 0);
            }

            visitedNodes.Clear();
            nodeQueue.Clear();

            nodeQueue.Enqueue(new SectorBlueprintNode(startSector, 0));

            while (nodeQueue.Count > 0)
            {
                var currentNode = nodeQueue.Dequeue();
                var currentSector = currentNode.Sector;

                if (predicate(currentSector))
                {
                    return (currentSector, currentNode.JumpDistance);
                }

                if (currentNode.JumpDistance < maxJumpDistance)
                {
                    var connections = currentSector.GetValidStableWormholes();
                    foreach (var connection in connections)
                    {
                        var targetSector = connection.GetActualTargetSector();

                        if (!visitedNodes.Contains(targetSector.GetInstanceID()))
                        {
                            visitedNodes.Add(targetSector.GetInstanceID());

                            nodeQueue.Enqueue(new SectorBlueprintNode(targetSector, currentNode.JumpDistance + 1));
                        }
                    }
                }
            }

            return null;
        }
    }
}
