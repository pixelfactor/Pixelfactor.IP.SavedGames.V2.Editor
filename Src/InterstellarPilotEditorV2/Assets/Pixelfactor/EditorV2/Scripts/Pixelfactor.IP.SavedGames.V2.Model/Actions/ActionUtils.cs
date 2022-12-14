using Pixelfactor.IP.Common.Triggers;

namespace Pixelfactor.IP.SavedGames.V2.Model.Actions
{
    public class ActionUtils
    {
        public static ModelAction CreateModelAction(ActionType actionType)
        {
            switch (actionType)
            {
                case ActionType.Dialog_Activate:
                    return new ModelAction_Dialog_Activate();
                case ActionType.Dialog_ChangeStage:
                    return new ModelAction_Dialog_ChangeStage();
                case ActionType.Dialog_SetInitialStage:
                    return new ModelAction_Dialog_SetInitialStage();
                case ActionType.Faction_SetNeutralityWith:
                    return new ModelAction_Faction_SetNeutralityWith();
                case ActionType.Faction_SetOpinionWith:
                    return new ModelAction_Faction_SetOpinionWith();
                case ActionType.Faction_DiscoverFactionUnits:
                    return new ModelAction_Faction_DiscoverFactionUnits();
                case ActionType.Faction_SetRpMultiplier:
                    return new ModelAction_Faction_SetRpMultiplier();
                case ActionType.Fleet_Activate:
                    return new ModelAction_Fleet_Activate();
                case ActionType.Fleet_EnqueueOrder:
                    return new ModelAction_Fleet_EnqueueOrder();
                case ActionType.Fleet_SetOrder:
                    return new ModelAction_Fleet_SetOrder();
                case ActionType.Mission_Activate:
                    return new ModelAction_Mission_Activate();
                case ActionType.Mission_ActivateObjective:
                    return new ModelAction_Mission_ActivateObjective();
                case ActionType.Mission_ChangeStage:
                    return new ModelAction_Mission_ChangeStage();
                case ActionType.Mission_CompleteObjective:
                    return new ModelAction_Mission_CompleteObjective();
                case ActionType.Player_ChangeCargo:
                    return new ModelAction_Player_ChangeCargo();
                case ActionType.Player_ChangeCredits:
                    return new ModelAction_Player_ChangeCredits();
                case ActionType.Player_DiscoverFactionUnits:
                    return new ModelAction_Player_DiscoverFactionUnits();
                case ActionType.Player_DiscoverStaticUnitsInSectors:
                    return new ModelAction_Player_DiscoverStaticUnitsInSectors();
                case ActionType.Player_NewMessage:
                    return new ModelAction_Player_NewMessage();
                case ActionType.Player_NewMessageSimple:
                    return new ModelAction_Player_NewMessageSimple();
                case ActionType.TriggerGroup_Activate:
                    return new ModelAction_TriggerGroup_Activate();
                case ActionType.Unit_Activate:
                    return new ModelAction_Unit_Activate();
                case ActionType.Unit_AllowDestruction:
                    return new ModelAction_Unit_AllowDestruction();
                case ActionType.Unit_ChangeCargo:
                    return new ModelAction_Unit_ChangeCargo();
                case ActionType.Unit_ChangeCargos:
                    return new ModelAction_Unit_ChangeCargos();
                case ActionType.Unit_ChangeFleet:
                    return new ModelAction_Unit_ChangeFleet();
                case ActionType.Unit_Dock:
                    return new ModelAction_Unit_Dock();
                case ActionType.Unit_LegacyInstallComponent:
                    return new ModelAction_Unit_LegacyInstallComponent();
                case ActionType.Unit_LegacyUninstallComponent:
                    return new ModelAction_Unit_LegacyUninstallComponent();
                case ActionType.Unit_PerformScan:
                    return new ModelAction_Unit_PerformScan();
                case ActionType.Unit_PositionRelativeToUnit:
                    return new ModelAction_Unit_PositionRelativeToUnit();
                case ActionType.Spawn_BanditHorde:
                    return new ModelAction_Spawn_BanditHorde();
                default:
                    {
                        throw new System.Exception($"Unknown action type: {actionType}");
                    }
            }
        }
    }
}
