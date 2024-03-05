using AnalysisParalysis.Data.Models;
using AnalysisParalysis.Exceptions;
using AnalysisParalysis.Pages;
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

    public async Task StartSession(string bggUser, User user)
    {
        var newSession = await _sessionManager.StartSession(bggUser, user);
        _sessionManager.AddUserToSession(newSession, user);

        await Groups.AddToGroupAsync(Context.ConnectionId, newSession.SessionId.ToString());
        await Clients.Group(newSession.SessionId.ToString()).SendAsync(SessionEvents.StartSession.ToString(), newSession);
    }

    public async Task JoinSession(GamePickingSession session, User user)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, session.SessionId.ToString());

        var activeSession = _sessionManager.GetActiveSession(session.SessionId) 
            ?? throw new InvalidSessionIdException($"Unable to get a session with ID {session.SessionId}");
        _sessionManager.AddUserToSession(activeSession, user);

        await Clients.Group(session.SessionId.ToString()).SendAsync(SessionEvents.UserJoined.ToString(), activeSession);
    }

    public async Task LeaveSession(GamePickingSession sessionToLeave, User user)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, sessionToLeave.SessionId.ToString());

        _sessionManager.RemoveUserFromSession(sessionToLeave, user);
        
        await Clients.Group(sessionToLeave.SessionId.ToString()).SendAsync(SessionEvents.UserLeft.ToString());
    }

    public async Task UserReady(GamePickingSession session, User user)
        => await Clients.Group(session.SessionId.ToString()).SendAsync(SessionEvents.UserReady.ToString(), _sessionManager.ToggleUserReadyStatus(session, user));

    public async Task UserSelectedGame(GamePickingSession session, User user, BoardGame game)
     => await Clients.Group(session.SessionId.ToString()).SendAsync(SessionEvents.UserSelectedGame.ToString(), _sessionManager.UserSelectedGame(session, user, game));

    public async Task FindMatches(GamePickingSession session)
    {
        try
        {
            var selectedGame = _sessionManager.GetActiveSession(session.SessionId)?.ChooseFromSelections();

            if(selectedGame == null)
            {
                await Clients.Group(session.SessionId.ToString()).SendAsync(SessionEvents.NoMatches.ToString(), _sessionManager.RestrictSessionGameOptions(session, 2));
            }
            else
            {
                await Clients.Group(session.SessionId.ToString()).SendAsync(SessionEvents.GameSelected.ToString(), selectedGame);
            }
        }
        catch(NoGamesFoundException)
        {
            await Clients.Group(session.SessionId.ToString()).SendAsync(SessionEvents.NoGamesSelected.ToString(), session);
        }
    }
}
