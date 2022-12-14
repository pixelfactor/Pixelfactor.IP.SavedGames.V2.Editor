﻿using Pixelfactor.IP.Common.Triggers;

namespace Pixelfactor.IP.SavedGames.V2.Model.Actions
{
    /// <summary>
    /// Changes the cargo on the player's current ship
    /// </summary>
    public class ModelAction_Player_ChangeCargo : ModelAction
    {
        public override ActionType Type => ActionType.Player_ChangeCargo;
    }
}