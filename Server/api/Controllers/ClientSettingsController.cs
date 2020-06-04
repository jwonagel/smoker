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
    [Route("/Client/Settings")]
    [Authorize]
    public class ClientSettingsController : ControllerBase
    {

        private readonly ISmokerService _smokerService;
        private readonly ILogger<SmokerController> _logger;

        public ClientSettingsController(ILogger<SmokerController> logger, ISmokerService smokerService)
        {
            _logger = logger;
            _smokerService = smokerService;
        }

        [HttpGet]
        [Route("current")]
        public async Task<SettingsClient> GetCurrentSettingClient()
        {
            return await _smokerService.GetCurrentClientSettings();
        }


        [HttpPost]
        [Route("current")]
        public async Task<IActionResult> PostCurrentSetting(SettingsClient settings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var updatedSetting = await _smokerService.UpdateSettings(settings);

            return Ok(updatedSetting);
        }


        // [HttpGet]
        // [Route("signalr")]
        // public async Task<ActionResult> SignalRTest() 
        // {
        //     try 
        //     {
        //         await _messageHub.Clients.Group("smoker").ReceiveMessage("Test", "Bar");
        //         return Ok();
        //     }
        //     catch(Exception e) 
        //     {
        //         return BadRequest(e.Message);
        //     }
            
        // } 

    }
}
