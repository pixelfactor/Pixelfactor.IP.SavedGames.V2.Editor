namespace Pixelfactor.IP.SavedGames.V2.Model
{
    /// <summary>
    /// A message that shows up after a certain amount of time
    /// </summary>
    public class ModelPlayerDelayedMessage
    {
        public ModelPlayerMessage Message { get; set; }
        /// <summary>
        /// The scenario time in seconds when to show the message
        /// </summary>
        public double ShowTime { get; set; }
    }
}
