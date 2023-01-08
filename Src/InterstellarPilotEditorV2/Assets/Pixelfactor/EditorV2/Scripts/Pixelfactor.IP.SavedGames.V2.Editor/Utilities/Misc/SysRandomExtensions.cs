using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    public static class SysRandomExtensions
    {
        public static float NextFloat(this System.Random random)
        {
            return (float)random.NextDouble();
        }

        public static float NextFloat(this System.Random random, float minValue, float maxValue)
        {
            return minValue + ((float)random.NextDouble() * (maxValue - minValue));
        }

        public static Quaternion RandomQuaternion(this System.Random random)
        {
            return Quaternion.Euler(
                NextFloat(random) * 360.0f,
                NextFloat(random) * 360.0f,
                NextFloat(random) * 360.0f);
        }
    }
}
