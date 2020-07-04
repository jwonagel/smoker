using System;

namespace api.Services
{
    public interface INotificationInfoService
    {
        void AddNotification(NotifcationInfo notifcationInfo);

        bool HasNotifcaitonInfoInTimeSpan(NotificationType type, TimeSpan timeSpan);
    }

    public class NotificationInfoService : INotificationInfoService
    {
        public void AddNotification(NotifcationInfo notifcationInfo)
        {
            throw new NotImplementedException();
        }

        public bool HasNotifcaitonInfoInTimeSpan(NotificationType type, TimeSpan timeSpan)
        {
            throw new NotImplementedException();
        }
    }


    public class NotifcationInfo
    {
        public string Content { get; set; }

        public DateTime TimeStamp { get; set; }

        public NotificationType NotificationType {get;set;}
    }

    public enum NotificationType{
        FireTemperatur
        
    }
}