namespace Pixelfactor.IP.SavedGames.V2.Model
{
    public class ModelUnitWormholeData
    {
        /// <summary>
        /// For stable wormholes, points to the other wormhole that this connects to.
        /// </summary>
        public ModelUnit TargetWormholeUnit { get; set; }

        /// <summary>
        /// Don't change for existing wormholes - changing this will cause carnage
        /// </summary>
        public bool IsUnstable { get; set; }

        public double UnstableNextChangeTargetTime { get; set; }

        /// <summary>
        /// Local to sector
        /// </summary>
        public Vec3 UnstableTargetPosition { get; set; }

        public Vec3 UnstableTargetRotation { get; set; }

        public ModelSector UnstableTargetSector { get; set; }
    }
}
