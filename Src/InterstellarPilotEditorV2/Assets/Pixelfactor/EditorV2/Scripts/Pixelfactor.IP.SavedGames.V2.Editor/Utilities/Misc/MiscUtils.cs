namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    public static class MiscUtils
    {
        /// <summary>
        /// Wraps a value between the exlusive max value and the exclusive min value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float WrapValue(float value, float min, float max)
        {
            if (min >= max)
            {
                throw new System.ArgumentException("Minimum value should be smaller than the maximum");
            }
            else
            {
                if (value < min || value >= max)
                {
                    var range = max - min;
                    var wrap = value - min;
                    var mod = wrap % range;
                    if (mod < 0.0f)
                    {
                        return mod + max;
                    }
                    else
                    {
                        return mod + min;
                    }
                }
            }
            return value;
        }
    }
}
