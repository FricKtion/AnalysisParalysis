using AnalysisParalysis.Services.Models;
using Microsoft.AspNetCore.SignalR;

namespace AnalysisParalysis.Hubs;

// TODO - Replace string method names with a static class or something?
public class SessionHub : Hub
{
    public async Task JoinSession(GamePickingSession sessionToJoin)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, sessionToJoin.SessionId.ToString());
        await Clients.Group(sessionToJoin.SessionId.ToString()).SendAsync("UserJoined");
    }

    public async Task LeaveSession(GamePickingSession sessionToLeave)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, sessionToLeave.SessionId.ToString());
        await Clients.Group(sessionToLeave.SessionId.ToString()).SendAsync("UserLeft");
    }

    public async Task UserReady(GamePickingSession usersSession)
        => await Clients.Group(usersSession.SessionId.ToString()).SendAsync("UserReady");
}