using System;
using System.Collections.Generic;
using System.Linq;

namespace api.Services
{
    public interface INotificationInfoService
    {
        void AddNotification(NotifcationInfo notifcationInfo);

        bool HasNotifcaitonInfoInTimeSpan(NotificationType type, TimeSpan timeSpan);
    }

    public class NotificationInfoService : INotificationInfoService
    {

        private List<NotifcationInfo> _notifcations = new List<NotifcationInfo>();

        public void AddNotification(NotifcationInfo notifcationInfo)
        {
            _notifcations.Add(notifcationInfo);

            var toDeleteTimeStamp = DateTime.Now - TimeSpan.FromHours(2);            
            var toDelete = _notifcations
                .Where(n => n.TimeStamp < toDeleteTimeStamp)
                .ToList();
            
            foreach(var n in toDelete) 
            {
                _notifcations.Remove(n);
            }
        }

        public bool HasNotifcaitonInfoInTimeSpan(NotificationType type, TimeSpan timeSpan)
        {
            var timeStamp = DateTime.Now - timeSpan;
            return _notifcations.Any(n => n.TimeStamp > timeStamp && n.NotificationType == type);
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