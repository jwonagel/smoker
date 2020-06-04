using System;
using System.Collections.Generic;
using api.Model.Dababase;

namespace api.Model.Client
{
    public class SettingsClient
    {
        public Guid SettingsId { get; set;}

        public int OpenCloseTreshold { get; set; }

        public bool IsAutoMode { get; set; }

        public int? FireNotifcationTemperatur { get; set; }

        public int TemperaturUpdateCycleSeconds { get; set; }

        public DateTime LastSettingsUpdate { get; set; }

        public string LastSettingsUpdateUser { get; set; }

        public DateTime LastSettingsActivation { get; set; }        

        public List<AlertClient> Alerts { get; set; }

    }

}
