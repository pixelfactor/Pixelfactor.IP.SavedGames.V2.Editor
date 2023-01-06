using Pixelfactor.IP.Common.Triggers;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.Scripting
{
    /// <summary>
    /// One of more triggers that execute one or more actions when their condition is true.
    /// Only the AND operator is currently supported i.e. all triggers in the group must evalute true for the actions to activate
    /// </summary>
    public class EditorTriggerGroup : MonoBehaviour
    {
        /// <summary>
        /// Whether triggers are evaluated when the game is fired up and a cinematic plays. Generally this is not wanted.
        /// </summary>
        public bool EvaluateDuringIntroState = false;

        /// <summary>
        /// The interval in seconds between evaluations of the trigger group. <br />
        /// If the trigger group is very complex, it may help performance to increase this value 
        /// </summary>
        public float EvaluateFrequency = 2.0f;

        /// <summary>
        /// Whether the trigger group will be disabled 
        /// </summary>
        public bool FireAndDisable = true;

        /// <summary>
        /// 
        /// </summary>
        public int Id = -1;

        /// <summary>
        /// The maximum number of times the trigger group can fire before the max fire action occurs<br />
        /// By default a trigger group fires once and then is destroyed
        /// </summary>
        public int MaxFireCount = 1;

        /// <summary>
        /// Determines what to do when the trigger group has fired the maximum number of times. <br />
        /// The default action is to destroy the trigger group
        /// </summary>
        public TriggerMaxFiredAction MaxFiredAction = TriggerMaxFiredAction.Destroy;

        /// <summary>
        /// Whether the trigger group is active.<br />
        /// NOTE: This is not the same as disabling this game object. If the game object is disabled the trigger group won't be exported at all.<br />
        /// When this is false, the trigger group will be imported into the game but won't evaluate until manually enabled.
        /// </summary>
        public bool IsActive = true;
    }
}
