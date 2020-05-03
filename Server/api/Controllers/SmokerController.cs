using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using api.Model.Client;
using api.Model.Smoker;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SmokerController : ControllerBase
    {

        private readonly ISmokerService _smokerService;
        private readonly ILogger<SmokerController> _logger;

        public SmokerController(ILogger<SmokerController> logger, ISmokerService smokerService)
        {
            _logger = logger;
            _smokerService = smokerService;
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
        public async Task<MeasurementClient> GetLatest()
        {
            return await _smokerService.GetLatestMeasurement();
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
