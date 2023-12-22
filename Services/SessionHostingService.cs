using AnalysisParalysis.Exceptions;
using AnalysisParalysis.Services.Definitions;
using AnalysisParalysis.Services.Models;

namespace AnalysisParalysis.Services;

public class SessionHostingService : ISessionHostingService
{
    private readonly List<GamePickingSession> _activeSessions = new List<GamePickingSession>();

    // TODO - Lock this so that multiple sessions can't be created at the exact same time.
    public GamePickingSession StartSession()
    {
        var rng = new Random();
        var minId = 99;
        var maxId = 1000;

        var potentialId = rng.Next(minId, maxId);
        while(_activeSessions.Select(x => x.SessionId).Contains(potentialId))
            potentialId = rng.Next(minId, maxId);

        _activeSessions.Add(new GamePickingSession(potentialId));

        return _activeSessions.Single(x => x.SessionId == potentialId);
    }

    public GamePickingSession JoinSession(int sessionId)
    {
        if(_activeSessions.Select(x => x.SessionId).Contains(sessionId))
            return _activeSessions.Single(x => x.SessionId == sessionId);
        else
            throw new InvalidSessionIdException($"Could not find a session with ID: {sessionId}.");            
    }
}