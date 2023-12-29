using AnalysisParalysis.Data.Definitions;
using AnalysisParalysis.Exceptions;
using AnalysisParalysis.Mappers;
using AnalysisParalysis.Services.Definitions;
using AnalysisParalysis.Services.Models;

namespace AnalysisParalysis.Services;

/// <inheritdoc />
public class SessionHostingService : ISessionHostingService
{
    private readonly List<GamePickingSession> _activeSessions = new List<GamePickingSession>();

    private readonly IBoardGameRepository _boardGameRepo;

    public SessionHostingService(IBoardGameRepository boardGameRepo)
        => (_boardGameRepo) = (boardGameRepo);

   /// <inheritdoc />
    public async Task<GamePickingSession> StartSession(string bggUser, User owner)
    {
        var rng = new Random();
        var minId = 99;
        var maxId = 1000;

        var potentialId = rng.Next(minId, maxId);
        while(_activeSessions.Select(x => x.SessionId).Contains(potentialId))
            potentialId = rng.Next(minId, maxId);

        var collection = await _boardGameRepo.GetCollection(bggUser);
        if(collection == null)
            throw new NoGamesFoundException($"No collection found for user '{bggUser}'.");

        var session = new GamePickingSession(potentialId, owner);
        session.AvailableGames = BoardGameMapper.MapFromCollection(collection).ToList();

        _activeSessions.Add(session);

        return _activeSessions.Single(x => x.SessionId == potentialId);
    }

    /// <inheritdoc />
    public GamePickingSession? GetActiveSession(int sessionId)
        => _activeSessions.SingleOrDefault(x => x.SessionIsReady && x.SessionId == sessionId);

    
    /// <inheritdoc />
    public bool SessionIsReady(int sessionId) 
        => _activeSessions.Exists(x => x.SessionId == sessionId && x.SessionIsReady);
}