using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Model.Dababase;

namespace api.Services
{
    public interface INotficationService
    {
        Task HandleMeaseurent(Settings settings, Measurement measurement);
    }

    public class NotficationService : INotficationService
    {
        
        private TimeSpan _silentTimeAfterNotification = TimeSpan.FromMinutes(30);

        private List<string> _messages = new List<string>();
        private Settings _settings;
        private Measurement _measurement;

        private readonly ISlackMessageSender _slackMessageSender;
        private readonly INotificationInfoService _notificationInfoService;

        public NotficationService(ISlackMessageSender slackMessageSender, INotificationInfoService notificationInfoService)
        {
            _slackMessageSender = slackMessageSender;
            _notificationInfoService = notificationInfoService;
        }

        public async Task HandleMeaseurent(Settings settings, Measurement measurement)
        {
            _settings = settings;
            _measurement = measurement;
            HandleFireTemperatur();

            if (_messages.Any())
            {
                var message = string.Join('\n', _messages);
                await _slackMessageSender.SendMessageOnSmokerAsync(message);
            }
        }

        private void HandleFireTemperatur()
        {
            if(_settings.FireNotifcationTemperatur > _measurement.FireSensor &&
             _notificationInfoService.HasNotifcaitonInfoInTimeSpan(NotificationType.FireTemperatur, _silentTimeAfterNotification))
            {
                var temp = _settings.FireNotifcationTemperatur;
                var message = $"Temperatur des Feuerraumes ist unter {temp:F1} C gefallen.";
                _messages.Add(message);
                _notificationInfoService.AddNotification(new NotifcationInfo
                {
                    Content = message,
                    NotificationType = NotificationType.FireTemperatur,
                    TimeStamp = DateTime.Now
                });
            }
        }
    }
}