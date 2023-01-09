using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    public static class Geometry
    {
        public static Quaternion RandomYRotation()
        {
            return Quaternion.Euler(0.0f, Random.value * 360.0f, 0.0f);
        }

        public static Vector3 RandomXZUnitVector()
        {
            return Quaternion.Euler(0.0f, Random.value * 360.0f, 0.0f) * Vector3.forward;
        }

        public static Vector3 RandomXZUnitVector(System.Random random)
        {
            return Quaternion.Euler(0.0f, random.NextFloat() * 360.0f, 0.0f) * Vector3.forward;
        }
    }
}
