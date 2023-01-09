using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using System.Collections.Generic;
using System.Linq;

namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    public class WindowHelper
    {
        public static string DescribeSectors(IEnumerable<EditorSector> sectors)
        {
            if (sectors.Count() == 0)
            {
                return "[None]";
            }

            if (sectors.Count() == 1)
            {
                return sectors.First().Name;
            }

            if (sectors.Count() < 4)
            {
                return string.Join(", ", sectors.Select(e => e.Name));
            }

            return $"{sectors.Count()} sectors";
        }
    }
}
