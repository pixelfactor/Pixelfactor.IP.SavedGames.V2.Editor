using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    public class EditorPlayerMessage : MonoBehaviour
    {
        public int Id = -1;
        public double EngineTimeStamp;
        public bool AllowDelete;
        public bool Opened;
        public EditorUnit SenderUnit;
        public EditorUnit SubjectUnit;
        /// <summary>
        /// This is only used for 'special messages' where the msg data is held in engine assets. Should generally be left alone
        /// </summary>
        public int MessageTemplateId = -1;
        public string ToText;
        public string FromText;
        public string MessageText;
        public string SubjectText;
        public float ShowTime = -1.0f;

        /// <summary>
        /// Determines whether player is notified when message received<br />
        /// Applies to delayed messages where <see cref="ShowTime"/> greater than zero
        /// </summary>
        public bool Notifications = true;

        /// <summary>
        /// Determines whether player gets alert when message is received
        /// Applies to delayed messages where <see cref="ShowTime"/> greater than zero
        /// </summary>
        public bool Important = true;
    }
}
