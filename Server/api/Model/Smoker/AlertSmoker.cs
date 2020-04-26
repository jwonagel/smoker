using System;

namespace api.Model.Smoker
{
    public class AlertSmoker
    {
        public Guid AlertId { get; set;}

        public int SensorId { get; set; }

        public int Temperatur { get; set; }

        public string AlertType { get; set; }

    }
}