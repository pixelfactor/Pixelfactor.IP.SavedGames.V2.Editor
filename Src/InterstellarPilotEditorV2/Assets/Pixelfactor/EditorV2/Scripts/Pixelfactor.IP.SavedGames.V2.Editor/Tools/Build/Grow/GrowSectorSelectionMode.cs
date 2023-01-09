namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools.Build.Grow
{
    /// <summary>
    /// Determines what to select after using the grow tool
    /// </summary>
    public enum GrowSectorSelectionMode
    {
        /// <summary>
        /// The selection will remain the same
        /// </summary>
        Keep = 0,
        /// <summary>
        /// The new sectors will be selected
        /// </summary>
        New = 1,
        /// <summary>
        /// The existing and new sectors will be selected
        /// </summary>
        Combined = 2
    }
}
