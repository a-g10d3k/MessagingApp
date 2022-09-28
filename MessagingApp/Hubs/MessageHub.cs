using Microsoft.AspNetCore.SignalR;
using MessagingApp.Models;

namespace MessagingApp.Hubs
{
    public class MessageHub : Hub
    {
        public async Task Connect(string username)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, username);
        }
    }
}
