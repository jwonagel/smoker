using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Model.Dababase
{
    [Table("SETTING")]
    public class Settings
    {
        [Key]
        [Column("SETTINGS_ID")]
        public Guid SettingsId { get; set;}

        [Column("OPEN_CLOSE_TRESHOLD")]
        public int OpenCloseTreshold { get; set; }

        [Column("IS_AUTO_MODE")]
        public bool IsAutoMode { get; set; }

        [Column("FIRE_NOTIFCATION_TEMPERATUR")]
        public int? FireNotifcationTemperatur { get; set; }

        [Column("TEMPERATUR_UPDATE_CYCLE_SECONDS")]
        public int TemperaturUpdateCycleSeconds { get; set; }

        [Column("LAST_SETTINGS_UPDATE")]
        public DateTime LastSettingsUpdate { get; set; }

        [Column("LASAT_SETTINGS_UPDATE_USER")]
        public string LastSettingsUpdateUser { get; set; }

        [Column("LAST_SETTINGS_ACTIVATION")]
        public DateTime LastSettingsActivation { get; set; }        

        public List<Alert> Alerts { get; set; }

    }
}