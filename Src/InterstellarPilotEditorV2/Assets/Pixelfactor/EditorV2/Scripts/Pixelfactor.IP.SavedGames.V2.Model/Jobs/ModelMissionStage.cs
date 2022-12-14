namespace Pixelfactor.IP.SavedGames.V2.Model.Jobs
{
    public class ModelMissionStage
    {
        /// <summary>
        /// Required for linking
        /// </summary>
        public ModelMission Mission { get; set; }

        public bool CompletesMission { get; set; } = false;
        public string JournalEntry { get; set; } = null;

        public bool MissionSuccess { get; set; } = true;
    }
}
