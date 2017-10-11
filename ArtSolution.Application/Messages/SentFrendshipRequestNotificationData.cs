using Abp.Notifications;
using System;

namespace ArtSolution.Messages
{
    [Serializable]
    public class SentFrendshipRequestNotificationData : NotificationData
    {
        public string SenderUserName { get; set; }

        public string FriendshipMessage { get; set; }

        public SentFrendshipRequestNotificationData(string senderUserName, string friendshipMessage)
        {
            SenderUserName = senderUserName;
            FriendshipMessage = friendshipMessage;
        }
    }
}
