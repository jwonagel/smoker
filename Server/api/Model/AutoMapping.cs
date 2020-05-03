using api.Model.Client;
using api.Model.Dababase;
using api.Model.Smoker;
using AutoMapper;

namespace api.Model
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Alert, AlertSmoker>();
            CreateMap<AlertSmoker, Alert>();

            CreateMap<Settings, SettingsSmoker>();
            CreateMap<SettingsSmoker, Settings>();
            
            CreateMap<Measurement, MeasurementClient>();

        }
    }



}