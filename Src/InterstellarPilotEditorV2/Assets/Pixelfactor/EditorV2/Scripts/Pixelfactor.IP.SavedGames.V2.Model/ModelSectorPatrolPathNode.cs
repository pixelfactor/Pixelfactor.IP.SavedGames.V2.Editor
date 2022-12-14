namespace Pixelfactor.IP.SavedGames.V2.Model
{
    /// <summary>
    /// A node on a <see cref="ModelSectorPatrolPath" />
    /// </summary>
    public class ModelSectorPatrolPathNode
    {
        public Vec3 SectorPosition { get; set; }
        public int Order { get; set; }
    }
}
