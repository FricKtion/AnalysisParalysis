using AnalysisParalysis.Data.Models;
using AnalysisParalysis.Exceptions;
using AnalysisParalysis.Pages;
using AnalysisParalysis.Services.Definitions;
using AnalysisParalysis.Services.Models;
using Microsoft.AspNetCore.SignalR;

namespace AnalysisParalysis.Hubs;

// TODO - Replace string method names with a static class or something?
public class SessionHub : Hub
{
    private readonly ISessionHostingService _sessionManager;

    public SessionHub(ISessionHostingService sessionManager)
        => (_sessionManager) = (sessionManager);

    public async Task JoinSession(GamePickingSession session, User user)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, session.SessionId.ToString());

        var activeSession = _sessionManager.GetActiveSession(session.SessionId) ?? throw new InvalidSessionIdException($"Unable to get a session with ID {session.SessionId}");
        _sessionManager.AddUserToSession(activeSession, user);

        await Clients.Group(session.SessionId.ToString()).SendAsync("UserJoined", activeSession);
    }

    public async Task LeaveSession(GamePickingSession sessionToLeave)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, sessionToLeave.SessionId.ToString());
        await Clients.Group(sessionToLeave.SessionId.ToString()).SendAsync("UserLeft");
    }

    public async Task UserReady(GamePickingSession usersSession)
        => await Clients.Group(usersSession.SessionId.ToString()).SendAsync("UserReady");

    public async Task NoMatches(GamePickingSession session)
        => await Clients.Group(session.SessionId.ToString()).SendAsync("NoMatches");

    public async Task GameSelected(GamePickingSession session, BoardGame selectedGame)
        => await Clients.Group(session.SessionId.ToString()).SendAsync("GameSelected", selectedGame);
}