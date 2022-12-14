using Pixelfactor.IP.Common.Triggers;

namespace Pixelfactor.IP.SavedGames.V2.Model.Triggers
{
    public static class TriggerUtils
    {
        public static ModelTrigger CreateModelTrigger(TriggerType triggerType)
        {
            switch (triggerType)
            {
                case TriggerType.Always:
                    return new ModelTrigger_Always();
                case TriggerType.Dialog_MessageConfirmed:
                    return new ModelTrigger_Dialog_MessageConfirmed();
                case TriggerType.Dialog_NumTimesStageShown:
                    return new ModelTrigger_Dialog_NumTimesStageShown();
                case TriggerType.Dialog_OptionConfirmed:
                    return new ModelTrigger_Dialog_OptionConfirmed();
                case TriggerType.Dialog_StageFinished:
                    return new ModelTrigger_Dialog_StageFinished();
                case TriggerType.Fleet_DestroyedOrNoPilots:
                    return new ModelTrigger_Fleet_DestroyedOrNoPilots();
                case TriggerType.Fleet_HostileTargets:
                    return new ModelTrigger_Fleet_HostileTargets();
                case TriggerType.Fleet_InSector:
                    return new ModelTrigger_Fleet_InSector();
                case TriggerType.Fleet_InState:
                    return new ModelTrigger_Fleet_InState();
                case TriggerType.Player_CurrentHudTarget:
                    return new ModelTrigger_Player_CurrentHudTarget();
                case TriggerType.Player_DockedAtUnit:
                    return new ModelTrigger_Player_DockedAtUnit();
                case TriggerType.Player_HasMissileLock:
                    return new ModelTrigger_Player_HasMissileLock();
                case TriggerType.Player_HasOpenedMessage:
                    return new ModelTrigger_Player_HasOpenedMessage();
                case TriggerType.Player_InSector:
                    return new ModelTrigger_Player_InSector();
                case TriggerType.Player_IsPilotting:
                    return new ModelTrigger_Player_IsPilotting();
                case TriggerType.Player_NearSectorTarget:
                    return new ModelTrigger_Player_NearSectorTarget();
                case TriggerType.Player_NearUnit:
                    return new ModelTrigger_Player_NearUnit();
                case TriggerType.Player_NoHostileFactions:
                    return new ModelTrigger_Player_NoHostileFactions();
                case TriggerType.Player_TimePilotting:
                    return new ModelTrigger_Player_TimePilotting();
                case TriggerType.Scenario_AllowDialog:
                    return new ModelTrigger_Scenario_AllowDialog();
                case TriggerType.Scenario_TimeElapsed:
                    return new ModelTrigger_Scenario_TimeElapsed();
                case TriggerType.Scenario_TimeSinceDialogShown:
                    return new ModelTrigger_Scenario_TimeSinceDialogShown();
                case TriggerType.Unit_HasCargo:
                    return new ModelTrigger_Unit_HasCargo();
                case TriggerType.Unit_NearSectorTarget:
                    return new ModelTrigger_Unit_NearSectorTarget();
                case TriggerType.Unit_ReceivedDamage:
                    return new ModelTrigger_Unit_ReceivedDamage();
                case TriggerType.Unit_TotalDamageReceived:
                    return new ModelTrigger_Unit_TotalDamageReceivedd();
                case TriggerType.Units_Destroyed:
                    return new ModelTrigger_Units_Destroyed();
                default:
                    {
                        throw new System.Exception($"Unknown trigger type: {triggerType}");
                    }
            }
        }
    }
}
