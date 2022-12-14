using Pixelfactor.IP.Common.Factions;

namespace Pixelfactor.IP.Common
{
    public static class FactionStrategyExtensions
    {
        public static bool IsPeaceful(this FactionStrategy factionStrategy)
        {
            switch (factionStrategy)
            {
                case FactionStrategy.War:
                case FactionStrategy.Escort:
                case FactionStrategy.Scout:
                case FactionStrategy.BountyHunt:
                    return false;
            }

            return true;
        }
    }
}
