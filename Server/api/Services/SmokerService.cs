using System;
using System.Linq;
using System.Threading.Tasks;
using api.Hubs;
using api.Model.Client;
using api.Model.Dababase;
using api.Model.Smoker;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{

    public interface ISmokerService
    {
        Task<bool> AddMessurement(MeasurementSmoker measurement);
        SettingsSmoker CurrentActiveSettings();
        Task<MeasurementClient> GetLatestMeasurement();
        Task<SettingsClient> UpdateSettings(SettingsClient settings);
        Task<SettingsClient> GetCurrentClientSettings();
    }

    public class SmokerService : ISmokerService
    {

        private readonly SmokerDBContext _context;
        private readonly IMapper _mapper;
        private readonly IHubContext<MessageHub, IMessageHub> _messageHub;
        private IUserInfoService _userInfoService;

        public SmokerService(SmokerDBContext context, IMapper mapper, IHubContext<MessageHub, IMessageHub> messageHub, IUserInfoService userInfoService)
        {
            _context = context;
            _mapper = mapper;
            _messageHub = messageHub;
            _userInfoService = userInfoService;
        }

        public async Task<bool> AddMessurement(MeasurementSmoker measurement)
        {
            var dbMeasurement = MapToMeasurement(measurement);
            dbMeasurement.MeasurementId = Guid.NewGuid();
            dbMeasurement.TimeStampeReceived = DateTime.Now;

            _context.Measurements.Add(dbMeasurement);
            return await _context.SaveChangesAsync() == 1;            
        }

        public async Task<SettingsClient> UpdateSettings(SettingsClient settings)
        {
            var settingsDatabase = await _context.Settings.FirstAsync();
            _mapper.Map(settings, settingsDatabase);
            
            settings.LastSettingsUpdate = DateTime.Now;
            settings.LastSettingsUpdateUser = _userInfoService.FirstName + " " + _userInfoService.LastName;

            var t = _messageHub.Clients.All.ReceiveMessage("Settings", "Update");
            await _context.SaveChangesAsync();
            await t;            

            return _mapper.Map<SettingsClient>(settingsDatabase);
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


        public async Task<SettingsClient> GetCurrentClientSettings()
        {
            var settings = await _context.Settings.FirstAsync();
            return _mapper.Map<SettingsClient>(settings);
        }

        public async Task<MeasurementClient> GetLatestMeasurement()
        {
            var measurement = await _context.Measurements
                .OrderByDescending(m => m.TimeStampeReceived)
                .FirstOrDefaultAsync();

            return _mapper.Map<MeasurementClient>(measurement);
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