using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using api.Model.Smoker;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SettingsController : ControllerBase
    {

        private readonly ISmokerService _smokerService;
        private readonly ILogger<SmokerController> _logger;

        public SettingsController(ILogger<SmokerController> logger, ISmokerService smokerService)
        {
            _logger = logger;
            _smokerService = smokerService;
        }

        [HttpGet]
        [Route("latest")]
        public SettingsSmoker GetCurrentSetting() 
        {
            return _smokerService.CurrentActiveSettings();
        }        

    }
}
