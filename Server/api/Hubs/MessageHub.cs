using api.Model.Hub;
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
        private static readonly string _smokerGroupName = "smoker";

        public async Task SendMessage(string type, string message)
        {
            await Clients.All.ReceiveMessage(type, message);

        }

        public async Task SendUpdateCloseState(OpenCloseModel model)
        {
            await Clients.Group(_smokerGroupName).ReceiveUpdateOpenCloseState(model);
        }

        public override async Task OnDisconnectedAsync(System.Exception exception)
        {
            var groupName = GetGroupOfUserAsync();
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public override async Task OnConnectedAsync()
        {
            var groupName = GetGroupOfUserAsync();
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await base.OnConnectedAsync();
        }


        private string GetGroupOfUserAsync()
        {
            var claims = Context.User.Claims;
            var claim = claims.FirstOrDefault(c => c.Type == "client_id");
            return claim.Value == _smokerGroupName ? claim.Value : UserGroupName;
        }

    }
}