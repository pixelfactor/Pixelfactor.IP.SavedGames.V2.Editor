using Pixelfactor.IP.Common.Factions;
using Pixelfactor.IP.SavedGames.V2.Model.Factions;
using Pixelfactor.IP.SavedGames.V2.Model.Factions.FactionAITypes;
using System;

namespace Pixelfactor.IP.SavedGames.V2.Model
{
    public static class CreateFactionAIFromType
    {
        public static ModelFactionAI Create(FactionAIType factionAIType)
        {
            switch (factionAIType)
            {
                case FactionAIType.PassengerTransport:
                    {
                        return new ModelFactionAIPassengerTransport();
                    }
                case FactionAIType.Explorer:
                    {
                        return new ModelFactionAIExplorer();
                    }
                case FactionAIType.BountyHunter:
                    {
                        return new ModelFactionAIBountyHunter();
                    }
                case FactionAIType.EquipmentDealer:
                    {
                        return new ModelFactionAIEquipmentDealer();
                    }
                case FactionAIType.StationOwner:
                    {
                        return new ModelFactionAIStationOwner();
                    }
                case FactionAIType.Bandit:
                    {
                        return new ModelFactionAIBandit();
                    }
                case FactionAIType.Scavenger:
                    {
                        return new ModelFactionAIScavenger();
                    }
                case FactionAIType.Empire:
                    {
                        return new ModelFactionAINavy();
                    }
                case FactionAIType.Trader:
                    {
                        return new ModelFactionAITrader();
                    }
                case FactionAIType.Miner:
                    {
                        return new ModelFactionAIMiner();
                    }
                case FactionAIType.Patroller:
                    {
                        return new ModelFactionAIPatroller();
                    }
                case FactionAIType.StationBuilder:
                    {
                        return new ModelFactionAIStationBuilder();
                    }
                case FactionAIType.Mercenary:
                    {
                        return new ModelFactionAIMercenary();
                    }
                case FactionAIType.Generic:
                    {
                        return new ModelFactionAI();
                    }
                case FactionAIType.Outlaw:
                    {
                        return new ModelFactionAIOutlaw();
                    }
                default:
                    {
                        throw new NotImplementedException($"Unknown faction AI type {(int)factionAIType}");
                    }
            }
        }
    }
}
