using api.Model.Hub;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;

namespace api.Hubs
{

    public interface IMessageHub
    {
        Task ReceiveMessage(string type, string message);

        Task ReceiveUpdateOpenCloseState(OpenCloseModel model);

    }

    [Authorize]
    public class MessageHub : Hub<IMessageHub>
    {
        public const string UserGroupName = "user";
        private const string SmokerGroupName = "smoker";
        private readonly ISmokerConnectionService _smokerConnectionService;

        public MessageHub(ISmokerConnectionService smokerConnectionService)
        {
            _smokerConnectionService = smokerConnectionService;
        }

        public async Task SendMessage(string type, string message)
        {
            await Clients.All.ReceiveMessage(type, message);

        }

        public async Task SendUpdateCloseState(OpenCloseModel model)
        {
            await Clients.Group(SmokerGroupName).ReceiveUpdateOpenCloseState(model);
        }

        public override async Task OnDisconnectedAsync(System.Exception exception)
        {
            var groupName = GetGroupOfUserAsync();
            if (groupName == SmokerGroupName && _smokerConnectionService is SmokerConnectionService smokerConnectionService)
            {
                smokerConnectionService.IsSmokerConnected = false;
            }
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public override async Task OnConnectedAsync()
        {
            var groupName = GetGroupOfUserAsync();
            if (groupName == SmokerGroupName && _smokerConnectionService is SmokerConnectionService smokerConnectionService)
            {
                smokerConnectionService.IsSmokerConnected = true;
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await base.OnConnectedAsync();
        }


        private string GetGroupOfUserAsync()
        {
            var claims = Context.User.Claims;
            var claim = claims.FirstOrDefault(c => c.Type == "client_id");
            return claim.Value == SmokerGroupName ? claim.Value : UserGroupName;
        }

    }
}