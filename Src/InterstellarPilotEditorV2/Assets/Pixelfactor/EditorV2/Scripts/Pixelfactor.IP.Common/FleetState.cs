namespace Pixelfactor.IP.Common
{
    public enum FleetState
    {
        Idle,
        MoveToTarget,
        Regrouping,
        EnteringGate,

        /// <summary>
        /// Not currently implemented
        /// </summary>
        Docking,
        CombatInterception
    }
}
