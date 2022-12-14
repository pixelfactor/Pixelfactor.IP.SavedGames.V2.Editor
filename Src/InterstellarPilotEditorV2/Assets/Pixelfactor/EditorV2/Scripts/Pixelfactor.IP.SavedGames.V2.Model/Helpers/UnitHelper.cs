using Pixelfactor.IP.SavedGames.V2.Model;

namespace Pixelfactor.IP.SavedGames.V2.Model.Helpers
{
    public static class UnitHelper
    {
        /// <summary>
        /// HACK due to bad save format
        /// </summary>
        /// <param name="unitClass"></param>
        /// <returns></returns>
        public static bool IsProjectile(ModelUnitClass unitClass)
        {
            return unitClass.ToString().Contains("Projectile");
        }
    }
}
