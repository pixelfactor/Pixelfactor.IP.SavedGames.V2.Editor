using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    public static class Helper
    {
        public static float RandomFloatWithPower(float min, float max, float power)
        {
            return Mathf.Lerp(min, max, Mathf.Pow(Random.value, power));
        }

    }
}
