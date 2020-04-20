using System;
using System.Threading.Tasks;
using api.Model.Dababase;
using api.Model.Smoker;

namespace api.Services
{

    public interface ISmokerService
    {
         Task<bool> AddMessurement(MeasurementSmoker measurement);
    }

    public class SmokerService : ISmokerService
    {

        private SmokerDBContext _context;

        public SmokerService(SmokerDBContext context)
        {
            _context = context;
        }

        public async Task<bool> AddMessurement(MeasurementSmoker measurement)
        {
            var dbMeasurement = MapToMeasurement(measurement);
            dbMeasurement.MeasurementId = Guid.NewGuid();
            dbMeasurement.TimeStampeReceived = DateTime.Now;

            _context.Measurements.Add(dbMeasurement);
            return await _context.SaveChangesAsync() == 1;            
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