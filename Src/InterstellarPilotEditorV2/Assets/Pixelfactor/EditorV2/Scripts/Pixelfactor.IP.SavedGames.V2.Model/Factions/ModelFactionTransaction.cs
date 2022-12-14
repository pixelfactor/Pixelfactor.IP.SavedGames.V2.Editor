﻿using Pixelfactor.IP.Common.Factions;

namespace Pixelfactor.IP.SavedGames.V2.Model.Factions
{
    /// <summary>
    /// Represents a recent change in credits. Should only be required by player faction
    /// </summary>
    public class ModelFactionTransaction
    {
        public FactionTransactionType TransactionType { get; set; }
        public FactionTransactionTaxType TaxType { get; set; }
        public int Value { get; set; }
        public int CurrentBalance { get; set; }
        public ModelUnit LocationUnit { get; set; }
        public ModelFaction OtherFaction { get; set; }
        public ModelCargoClass RelatedCargoClass { get; set; }
        public ModelUnitClass RelatedUnitClass { get; set; }
        public double GameWorldTime { get; set; }
        public int? RelatedCount { get; set; }
    }
}
