using System.Collections.Generic;
using System.Threading.Tasks;
using api.Model.Client;
using api.Model.Smoker;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class SmokerController : ControllerBase
    {

        private readonly ISmokerService _smokerService;
        private readonly IUserInfoService _userInfoService;
        private readonly ILogger<SmokerController> _logger;

        public SmokerController(ILogger<SmokerController> logger, ISmokerService smokerService, IUserInfoService userInfoService)
        {
            _logger = logger;
            _smokerService = smokerService;
            _userInfoService = userInfoService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]MeasurementSmoker measurement) 
        {
            var ok = await _smokerService.AddMessurement(measurement);
            if(ok)
            {
                return Ok();
            }
            return BadRequest("Error in Measurement");
        }        


        [HttpGet]
        [Route("latest")]
        public async Task<IActionResult> GetLatest()
        {
            var role = _userInfoService.Role;
            if (role == string.Empty)
            {
                return BadRequest();
            }

            var latestMeasurents = await _smokerService.GetLatestMeasurement();
            return Ok(latestMeasurents);
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string [] {
                "Test",
                "bar"
            };
        }
    }
}
