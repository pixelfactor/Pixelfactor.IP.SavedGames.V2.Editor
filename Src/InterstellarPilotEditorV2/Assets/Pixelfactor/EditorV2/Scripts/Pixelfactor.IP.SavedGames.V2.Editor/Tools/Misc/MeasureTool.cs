using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools.Misc
{
    /// <summary>
    /// Measures the distance between two selected options
    /// </summary>
    public static class MeasureTool
    {
        public static float MeasureSelected()
        {
            if (Selector.TryGetTwoSelectedGameObjects(out GameObject a, out GameObject b))
            {
                return Vector3.Distance(a.transform.position, b.transform.position);
            }

            return 0.0f;
        }
    }
}
