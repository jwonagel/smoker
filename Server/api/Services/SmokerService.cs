using System;
using System.Linq;
using System.Threading.Tasks;
using api.Model.Dababase;
using api.Model.Smoker;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{

    public interface ISmokerService
    {
         Task<bool> AddMessurement(MeasurementSmoker measurement);


         SettingsSmoker CurrentActiveSettings();

    }

    public class SmokerService : ISmokerService
    {

        private readonly SmokerDBContext _context;
        private readonly IMapper _mapper;

        public SmokerService(SmokerDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddMessurement(MeasurementSmoker measurement)
        {
            var dbMeasurement = MapToMeasurement(measurement);
            dbMeasurement.MeasurementId = Guid.NewGuid();
            dbMeasurement.TimeStampeReceived = DateTime.Now;

            _context.Measurements.Add(dbMeasurement);
            return await _context.SaveChangesAsync() == 1;            
        }

        public SettingsSmoker CurrentActiveSettings()
        {
            var currentSetting = _context.Settings
                .Include(s => s.Alerts)
                .OrderByDescending(s => s.LastSettingsActivation)
                .FirstOrDefault();

            var res = _mapper.Map<SettingsSmoker>(currentSetting);

            return res;
        }

        private Measurement MapToMeasurement(MeasurementSmoker measurement)
        {
            return new Measurement 
            {
                ContentSensor = measurement.ContentSensor,
                FireSensor = measurement.FireSensor,
                MeasurementId = measurement.MeasurementId,
                Sensor1 = measurement.Sensor1,
                Sensor2 = measurement.Sensor2,
                Sensor3 = measurement.Sensor3,
                Sensor4 = measurement.Sensor4,
                TimeStampSmoker = measurement.TimeStamp,
            };
        }
    }
}