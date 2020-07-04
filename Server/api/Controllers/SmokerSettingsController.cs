using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Hubs;
using api.Model.Client;
using api.Model.Smoker;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [ApiController]
    [Route("/Smoker/Settings")]
    [Authorize]
    public class SmokerSettingsController : ControllerBase
    {

        private readonly ISmokerService _smokerService;
        private readonly ILogger<SmokerController> _logger;

        public SmokerSettingsController(ILogger<SmokerController> logger, ISmokerService smokerService)
        {
            _logger = logger;
            _smokerService = smokerService;
        }

        [HttpGet]
        [Route("latest")]
        public async Task<SettingsSmoker> GetCurrentSettingSmoker() 
        {
            return await _smokerService.CurrentActiveSettings();
        }        
    }
}
