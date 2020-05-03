using System;

namespace api.Model.Client
{
    public class MeasurementClient
    {
        public Guid MeasurementId { get; set; } 

        public double FireSensor { get; set; }

        public double ContentSensor { get; set; }

        public double Sensor1 { get; set; }

        public double Sensor2 { get; set; }

        public double Sensor3 { get; set; }

        public double Sensor4 { get; set; }

        public DateTime TimeStampSmoker { get; set; }

    }
}