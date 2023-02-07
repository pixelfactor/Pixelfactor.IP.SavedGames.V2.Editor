﻿namespace Pixelfactor.IP.Common.Triggers
{
    public enum ActionType
    {
        Unspecified = -1,

        /// <summary>
        /// Add or remove player credits
        /// </summary>
        Player_ChangeCredits = 10000,
        Player_ChangeCargo = 10100,
        Player_DiscoverStaticUnitsInSectors = 10200,
        Player_DiscoverFactionUnits = 10300,
        Player_NewMessage = 10400,
        Player_NewMessageSimple = 10401,

        /// <summary>
        /// Add or remove faction credits
        /// </summary>
        Faction_Credits = 40000,
        Faction_SetNeutralityWith = 40100,
        Faction_SetOpinionWith = 40200,
        Faction_SetRpMultiplier = 40300,
        Faction_DiscoverFactionUnits = 41000,
        Faction_DiscoverUnits = 42000,

        Unit_ChangeFleet = 30000,
        Unit_ChangeCargo = 30100,
        Unit_ChangeCargos = 30200,
        Unit_Dock = 30201,
        Unit_LegacyInstallComponent = 30300,
        Unit_LegacyUninstallComponent = 30301,
        Unit_Activate = 30400,
        Unit_AllowDestruction = 30500,
        Unit_PerformScan = 30600,
        Unit_PositionRelativeToUnit = 30700,

        Fleet_EnqueueOrder = 20000,
        Fleet_Activate = 20100,
        Fleet_SetOrder = 20200,
        Fleet_ActivateOrder = 20300,

        Dialog_ChangeStage = 50000,
        Dialog_Activate = 50100,
        Dialog_SetInitialStage = 50300,

        Spawn_BanditHorde = 60000,

        Mission_Activate = 70000,
        Mission_ChangeStage = 70100,
        Mission_ActivateObjective = 70010,
        Mission_CompleteObjective = 70005,

        Hud_ActivateComponents = 80000,
        Hud_ActivateComponent = 80001,
        Hud_FlashAndEnableComponent = 80002,

        Hud_AllowSteering = 80100,
        Hud_AllowThrottle = 80200,
        Hud_AllowTargetting = 80300,
        Hud_ClearSpeech = 80400,
        Hud_ShowSpeech = 80500,
        Hud_SetTarget = 80600,
        
        Misc_ActivateGameObject = 90000,

        TriggerGroup_Activate = 95000,

    }
}
