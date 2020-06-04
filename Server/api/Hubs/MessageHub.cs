using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;

namespace api.Hubs
{

    public interface IMessageHub
    {
        Task ReceiveMessage(string type, string message);

    }

    [Authorize]
    public class MessageHub : Hub<IMessageHub>
    {
        private static readonly string _smokerGroupName = "smoker";
        private static readonly string _userGroupName = "user";

        public async Task SendMessage(string type, string message)
        {
            await Clients.All.ReceiveMessage(type, message);

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
            return claim.Value == _smokerGroupName ? claim.Value : _userGroupName;
        }

    }
}