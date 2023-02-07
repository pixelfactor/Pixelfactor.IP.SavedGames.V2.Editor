namespace Pixelfactor.IP.Common.Factions
{
    [System.Flags]
    public enum FactionFreelancerType
    {
        None = 0,
        Freelancer = (1 << 0),
        Gang = (1 << 1),
        Faction = (1 << 2)
    }
}
