namespace Pixelfactor.IP.Common.Triggers
{
    public enum TriggerType
    {
        Unspecified = -1,
        /// <summary>
        /// Trigger that fires always when active
        /// </summary>
        Always = 0,
        /// <summary>
        /// When the current player unit is in a sector
        /// </summary>
        Player_InSector = 10000,
        /// <summary>
        /// When the current player unit has an incoming missile
        /// </summary>
        Player_HasMissileLock = 11000,
        /// <summary>
        /// When the position of the current player unit is near something
        /// </summary>
        Player_NearSectorTarget = 12000,
        /// <summary>
        /// When the position of the current player unit is near something
        /// </summary>
        Player_NearUnit = 12100,
        /// <summary>
        /// When the player is pilotting a ship
        /// </summary>
        Player_IsPilotting = 13000,
        /// <summary>
        /// When the current player is docked at a unit
        /// </summary>
        Player_DockedAtUnit = 14000,
        /// <summary>
        /// Number of real seconds that the player has been pilotting
        /// </summary>
        Player_TimePilotting = 15000,
        /// <summary>
        /// When the current hud target is a certain unit
        /// </summary>
        Player_CurrentHudTarget = 16000,
        /// <summary>
        /// Player faction has no active hostile factions
        /// </summary>
        Player_NoHostileFactions = 17000,
        /// <summary>
        /// When the player has read a messsage
        /// </summary>
        Player_HasOpenedMessage = 18000,

        /// <summary>
        /// When a fleet is in a sector
        /// </summary>
        Fleet_InSector = 20000,
        /// <summary>
        /// When a fleet is in a specific state
        /// </summary>
        Fleet_InState = 21000,
        /// <summary>
        /// When a fleet has detected hostile targets
        /// </summary>
        Fleet_HostileTargets = 22000,
        /// <summary>
        /// When the fleet has been destroyed or has no pilots
        /// </summary>
        Fleet_DestroyedOrNoPilots = 23000,

        /// <summary>
        /// Real seconds since start of the scenario
        /// </summary>
        Scenario_TimeElapsed = 100,
        /// <summary>
        /// Real seconds since the last dialog (NPC conversation) was shown to the player
        /// </summary>
        Scenario_TimeSinceDialogShown = 200,
        /// <summary>
        /// When the scenario is in a state where dialog (NPC conversation) can be shown to the player
        /// </summary>
        Scenario_AllowDialog = 300,

        /// <summary>
        /// Unit has just received damage
        /// </summary>
        Unit_ReceivedDamage = 30000,
        /// <summary>
        /// Unit total damage received is below or above a certain amount
        /// </summary>
        Unit_TotalDamageReceived = 31000,
        /// <summary>
        /// When a unit is docked at another unit
        /// </summary>
        Unit_DockedAtUnit = 32000,
        /// <summary>
        /// When a unit is in a sector
        /// </summary>
        Unit_InSector = 33000,
        /// <summary>
        /// When a unit is near something in a sector
        /// </summary>
        Unit_NearSectorTarget = 34000,
        /// <summary>
        /// When a unit has a certain number of cargo
        /// </summary>
        Unit_HasCargo = 35000,

        /// <summary>
        /// When all the units are destroyed
        /// </summary>
        Units_Destroyed = 36000,

        /// <summary>
        /// When a specific dialog message has been shown to the player
        /// </summary>
        Dialog_MessageConfirmed = 50000,
        /// <summary>
        /// When the player has been at a certain dialog stage
        /// </summary>
        Dialog_NumTimesStageShown = 51000,
        /// <summary>
        /// The number of times player has confirmed a dialog option
        /// </summary>
        Dialog_OptionConfirmed = 52000,
        /// <summary>
        /// When a dialog state has finished showing to the player
        /// </summary>
        Dialog_StageFinished = 53000,
        
    }
}
