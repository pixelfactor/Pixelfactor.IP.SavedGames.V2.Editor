namespace Pixelfactor.IP.SavedGames.V2.Model.Factions.Bounty
{
    public class ModelFactionBountyBoardItem
    {
        public ModelPerson TargetPerson { get; set; }
        public int Reward { get; set; }
        public ModelUnit LastKnownTargetUnit { get; set; }
        public ModelSector LastKnownTargetSector { get; set; }
        /// <summary>
        /// Local to sector
        /// </summary>
        public Vec3? LastKnownTargetPosition { get; set; }
        public double? TimeOfLastSighting { get; set; }
        /// <summary>
        /// The faction that added the bounty
        /// </summary>
        public ModelFaction SourceFaction { get; set; }
    }
}
