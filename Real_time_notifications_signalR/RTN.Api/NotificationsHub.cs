using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace RTN.Api;

[Authorize]
public class NotificationsHub : Hub<INotificationClient>
{
    public override async Task OnConnectedAsync()
    {
        // Context.ConnectionId is id of the client that is connected now
        await Clients
            // .Client(Context.ConnectionId)
            .User("344BF590-0079-490E-8D74-95397874E125") // do not hardcode but take it from db or wherever you store your users
            // this will send the message to user's all connections whether there are 3 tabs opened or connected by desktop and mobile apps etc.
            .ReceiveNotification($"Thanks for connecting {Context.User?.Identity?.Name}");
        
        await base.OnConnectedAsync();
    }
}

public interface INotificationClient
{
    Task ReceiveNotification(string message);
}