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
        private readonly INotficationService _notificationService;

        private Settings _settings;

        public SmokerService(SmokerDBContext context, IMapper mapper, IHubContext<MessageHub, IMessageHub> messageHub, IUserInfoService userInfoService, ISmokerConnectionService smokerConnectionService, INotficationService notificationService = null)
        {
            _context = context;
            _mapper = mapper;
            _messageHub = messageHub;
            _userInfoService = userInfoService;
            _smokerConnectionService = smokerConnectionService;
            _notificationService = notificationService;
        }

        public async Task<bool> AddMessurement(MeasurementSmoker measurement)
        {
            var dbMeasurement = MapToMeasurement(measurement);
            dbMeasurement.MeasurementId = Guid.NewGuid();
            dbMeasurement.TimeStampeReceived = DateTime.Now;
            var t = CheckSettingsForNull();

            _context.Measurements.Add(dbMeasurement);
            var saved = await _context.SaveChangesAsync() == 1;
            var t1  = _messageHub.Clients.Group(MessageHub.UserGroupName).ReceiveMessage("Measurement", "Update");
            await t;
            await _notificationService.HandleMeaseurent(_settings, dbMeasurement);
            await t1;
            return saved;            
        }

        private async Task CheckSettingsForNull()
        {
            if(_settings == null)
            {
                _settings = await GetActiveSettings();
            }
        }


        public async Task<SettingsClient> UpdateSettings(SettingsClient settings)
        {
            var settingsDatabase = await _context.Settings.FirstAsync();
            _mapper.Map(settings, settingsDatabase);
            
            settingsDatabase.LastSettingsUpdate = DateTime.Now;
            settingsDatabase.LastSettingsUpdateUser = _userInfoService.FirstName + " " + _userInfoService.LastName;
            _settings = settingsDatabase;

            await _context.SaveChangesAsync();
            await _messageHub.Clients.All.ReceiveMessage("Settings", "Update").ConfigureAwait(false);      


            return _mapper.Map<SettingsClient>(settingsDatabase);
        }

        public SettingsSmoker CurrentActiveSettings()
        {
            CheckSettingsForNull().RunSynchronously();
            var res = _mapper.Map<SettingsSmoker>(_settings);
            return res;
        }

        private Task<Settings> GetActiveSettings()
        {
            return _context.Settings
                .Include(s => s.Alerts)
                .OrderByDescending(s => s.LastSettingsActivation)
                .FirstOrDefaultAsync();
        }


        public async Task<SettingsClient> GetCurrentClientSettings()
        {
            await CheckSettingsForNull();
            return _mapper.Map<SettingsClient>(_settings);
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
                OpenCloseState = measurement.OpenCloseState,
                IsAutoMode = measurement.IsAutoMode
            };
        }

    }
}