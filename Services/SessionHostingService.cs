using AnalysisParalysis.Data.Definitions;
using AnalysisParalysis.Exceptions;
using AnalysisParalysis.Mappers;
using AnalysisParalysis.Services.Definitions;
using AnalysisParalysis.Services.Models;

namespace AnalysisParalysis.Services;

public class SessionHostingService : ISessionHostingService
{
    private readonly List<GamePickingSession> _activeSessions = new List<GamePickingSession>();

    private readonly IBoardGameRepository _boardGameRepo;

    public SessionHostingService(IBoardGameRepository boardGameRepo)
        => (_boardGameRepo) = (boardGameRepo);

    // TODO - Lock this so that multiple sessions can't be created at the exact same time.
    public async Task<GamePickingSession> StartSession(string bggUser)
    {
        var rng = new Random();
        var minId = 99;
        var maxId = 1000;

        var potentialId = rng.Next(minId, maxId);
        while(_activeSessions.Select(x => x.SessionId).Contains(potentialId))
            potentialId = rng.Next(minId, maxId);

        var collection = await _boardGameRepo.GetCollection(bggUser);

        var session = new GamePickingSession(potentialId);
        session.AvailableGames = BoardGameMapper.MapFromCollection(collection).ToList();

        _activeSessions.Add(session);

        return _activeSessions.Single(x => x.SessionId == potentialId);
    }

    // TODO - Do I need this?
    public GamePickingSession JoinSession(int sessionId)
    {
        if(_activeSessions.Select(x => x.SessionId).Contains(sessionId))
            return _activeSessions.Single(x => x.SessionId == sessionId);
        else
            throw new InvalidSessionIdException($"Could not find a session with ID: {sessionId}.");            
    }

    public GamePickingSession GetActiveSession(int sessionId)
        => _activeSessions.Single(x => x.SessionIsReady && x.SessionId == sessionId);

    public bool SessionIsReady(int sessionId) 
        => _activeSessions.Exists(x => x.SessionId == sessionId && x.SessionIsReady);
}