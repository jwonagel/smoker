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
        private readonly ISmokerConnectionService _smokerConnectionService;


        public SmokerService(SmokerDBContext context, IMapper mapper, IHubContext<MessageHub, IMessageHub> messageHub, IUserInfoService userInfoService, ISmokerConnectionService smokerConnectionService)
        {
            _context = context;
            _mapper = mapper;
            _messageHub = messageHub;
            _userInfoService = userInfoService;
            _smokerConnectionService = smokerConnectionService;
        }

        public async Task<bool> AddMessurement(MeasurementSmoker measurement)
        {
            var dbMeasurement = MapToMeasurement(measurement);
            dbMeasurement.MeasurementId = Guid.NewGuid();
            dbMeasurement.TimeStampeReceived = DateTime.Now;

            _context.Measurements.Add(dbMeasurement);
            
            var saved = await _context.SaveChangesAsync() == 1;
            await _messageHub.Clients.Group(MessageHub.UserGroupName).ReceiveMessage("Measurement", "Update");

            return saved;            
        }

        public async Task<SettingsClient> UpdateSettings(SettingsClient settings)
        {
            var settingsDatabase = await _context.Settings.FirstAsync();
            _mapper.Map(settings, settingsDatabase);
            
            settingsDatabase.LastSettingsUpdate = DateTime.Now;
            settingsDatabase.LastSettingsUpdateUser = _userInfoService.FirstName + " " + _userInfoService.LastName;

            await _context.SaveChangesAsync();
            await _messageHub.Clients.All.ReceiveMessage("Settings", "Update").ConfigureAwait(false);      

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

            var clientMeasurement = _mapper.Map<MeasurementClient>(measurement);
            clientMeasurement.IsSmokerConnected = _smokerConnectionService.IsSmokerConnected;
            return clientMeasurement;
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