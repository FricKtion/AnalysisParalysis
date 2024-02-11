using AnalysisParalysis.Data.Models;
using AnalysisParalysis.Exceptions;
using AnalysisParalysis.Services.Definitions;
using AnalysisParalysis.Services.Enums;
using AnalysisParalysis.Services.Models;
using Microsoft.AspNetCore.SignalR;

namespace AnalysisParalysis.Hubs;

public class SessionHub : Hub
{
    private readonly ISessionHostingService _sessionManager;

    public SessionHub(ISessionHostingService sessionManager)
        => (_sessionManager) = (sessionManager);

    public async Task JoinSession(GamePickingSession session, User user)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, session.SessionId.ToString());

        var activeSession = _sessionManager.GetActiveSession(session.SessionId) 
            ?? throw new InvalidSessionIdException($"Unable to get a session with ID {session.SessionId}");
        _sessionManager.AddUserToSession(activeSession, user);

        await Clients.Group(session.SessionId.ToString())
            .SendAsync(SessionEvents.UserJoined.ToString(), activeSession);
    }

    public async Task LeaveSession(GamePickingSession sessionToLeave, User user)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, sessionToLeave.SessionId.ToString());

        _sessionManager.RemoveUserFromSession(sessionToLeave, user);
        
        await Clients.Group(sessionToLeave.SessionId.ToString())
            .SendAsync(SessionEvents.UserLeft.ToString());
    }

    public async Task UserReady(GamePickingSession usersSession)
        => await Clients.Group(usersSession.SessionId.ToString())
            .SendAsync(SessionEvents.UserReady.ToString());

    public async Task NoMatches(GamePickingSession session)
        => await Clients.Group(session.SessionId.ToString())
            .SendAsync(SessionEvents.NoMatches.ToString());

    public async Task GameSelected(GamePickingSession session, BoardGame selectedGame)
        => await Clients.Group(session.SessionId.ToString())
            .SendAsync(SessionEvents.GameSelected.ToString(), selectedGame);
}