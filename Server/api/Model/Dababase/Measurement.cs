using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Model.Dababase
{
    [Table("MEASUREMENT")]
    public class Measurement
    {

        [Key]
        [Column("MEASUREMENT_ID")]
        public Guid MeasurementId { get; set; } = Guid.NewGuid();

        [Column("FIRE_SENSOR")]
        public double FireSensor { get; set; }

        [Column("CONTENT_SENSOR")]
        public double ContentSensor { get; set; }

        [Column("SENSOR_1")]
        public double Sensor1 { get; set; }

        [Column("SENSOR_2")]
        public double Sensor2 { get; set; }

        [Column("SENSOR_3")]
        public double Sensor3 { get; set; }

        [Column("SENSOR_4")]
        public double Sensor4 { get; set; }

        [Column("TIME_STAMP_SMOKER")]
        public DateTime TimeStampSmoker { get; set; }

        [Column("TIME_STAMP_RECEIVED")]
        public DateTime TimeStampeReceived { get; set; }

        [Column("OPEN_CLOSE_STATE")]
        public double OpenCloseState { get; set; }

        [Column("IS_AUTO_MODE")]
        public bool IsAutoMode { get; set; }

    }
}