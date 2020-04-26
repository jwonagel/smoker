using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Model.Dababase
{
    [Table("ALERT")]
    public class Alert
    {
        [Key]
        [Column("ALLERT_ID")]
        public Guid AlertId { get; set;}

        [Column("SENSOR_ID")]
        public int SensorId { get; set; }

        [Column("TEMPERATUR")]
        public int Temperatur { get; set; }

        [Column("ALERT_TYPE")]
        public AlertType AlertType { get; set; }

        public Settings Settings { get;set;}

        [Column("FK_SETTING_ID")]
        public Guid SettingsId { get; set; }
    }

}