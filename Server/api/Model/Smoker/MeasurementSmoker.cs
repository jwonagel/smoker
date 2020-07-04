using System;

namespace api.Model.Smoker
{
    public class MeasurementSmoker
    {
        public Guid MeasurementId { get; set; }

        public double FireSensor { get; set; }

        public double ContentSensor { get; set; }

        public double Sensor1 { get; set; }

        public double Sensor2 { get; set; }

        public double Sensor3 { get; set; }

        public double Sensor4 { get; set; }

        public DateTime TimeStamp { get; set; }

        public double OpenCloseState { get; set; }

        public bool IsAutoMode { get; set; }

        public int OpenCloseTreshold { get; set;}
    }
}