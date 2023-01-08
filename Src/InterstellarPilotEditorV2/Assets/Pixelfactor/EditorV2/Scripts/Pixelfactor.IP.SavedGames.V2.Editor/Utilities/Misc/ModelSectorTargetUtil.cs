using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.SavedGames.V2.Model;
using System.Linq;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Utilities
{
    public static class ModelSectorTargetUtil
    {
        public static ModelSectorTarget FromTransform(Transform transform, SavedGame savedGame)
        {
            if (transform == null)
            {
                return null;
            }

            var editorSector = transform.GetComponentInParent<EditorSector>();
            if (editorSector == null)
            {
                Logging.LogAndThrow("Sector target should be in a sector", transform);
            }

            var modelSector = savedGame.Sectors.FirstOrDefault(e => e.Id == editorSector.Id);
            if (modelSector == null)
            {
                Logging.LogAndThrow("Expecting a sector here", transform);
            }

            var editorUnit = transform.GetComponentInParent<EditorUnit>();
            if (editorUnit != null)
            {
                var modelUnit = savedGame.Units.FirstOrDefault(e => e.Id == editorUnit.Id);
                if (modelUnit != null)
                {
                    return new ModelSectorTarget
                    {
                        HadValidTarget = true,
                        Sector = modelUnit.Sector,
                        Position = modelUnit.Position,
                        TargetUnit = modelUnit
                    };
                }
            }

            var editorFleet = transform.GetComponentInParent<EditorFleet>();
            if (editorFleet != null)
            {
                var modelFleet = savedGame.Fleets.FirstOrDefault(e => e.Id == editorFleet.Id);
                if (modelFleet != null)
                {
                    return new ModelSectorTarget
                    {
                        HadValidTarget = true,
                        Sector = modelFleet.Sector,
                        Position = modelFleet.Position,
                        TargetFleet = modelFleet
                    };
                }
            }

            return new ModelSectorTarget
            {
                Sector = modelSector,
                Position = (transform.position - editorSector.transform.position).ToVec3_ZeroY(),
            };
        }
    }
}
