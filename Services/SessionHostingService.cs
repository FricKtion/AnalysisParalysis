using AnalysisParalysis.Data.Definitions;
using AnalysisParalysis.Data.Models;
using AnalysisParalysis.Exceptions;
using AnalysisParalysis.Mappers;
using AnalysisParalysis.Pages;
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
        if(collection == null || collection.Count <= 0)
            throw new NoGamesFoundException($"No collection found for user '{bggUser}'.");

        var gamesList = await _boardGameRepo.GetBoardGameDetails(collection.Items.Select(x => x.Id));
        if(gamesList == null || !gamesList.Items.Any())
            throw new NoGamesFoundException($"Unable to get game details for user collection.");

        var session = new GamePickingSession(potentialId, owner);
        session.AvailableGames = BoardGameMapper.MapWithDetails(collection, gamesList).ToList();

        _activeSessions.Add(session);

        return _activeSessions.Single(x => x.SessionId == potentialId);
    }

    /// <inheritdoc />
    public GamePickingSession? GetActiveSession(int sessionId)
        => _activeSessions.SingleOrDefault(x => x.SessionIsReady && x.SessionId == sessionId);

    
    /// <inheritdoc />
    public bool SessionIsReady(int sessionId) 
        => _activeSessions.Exists(x => x.SessionId == sessionId && x.SessionIsReady);

    /// <inheritdoc />
    public GamePickingSession RestrictSessionGameOptions(GamePickingSession session, int restrictCount)
    {
        if(!SessionExists(session))
            throw new InvalidSessionIdException();

        GetActiveSession(session.SessionId)!.RestrictOptions(restrictCount);

        return GetActiveSession(session.SessionId)!;
    }

    /// <inheritdoc />
    public GamePickingSession ToggleUserReadyStatus(GamePickingSession session, User user)
    {
        if(!SessionExists(session))
            throw new InvalidSessionIdException();

        if(!UserIsInSession(session, user))
            throw new UserNotConnectedException();
        
        var userToToggle = _activeSessions.Single(x => x.SessionId == session.SessionId).ConnectedUsers.Single(x => x.Id == user.Id);
        userToToggle.IsReady = !userToToggle.IsReady;

        return GetActiveSession(session.SessionId)!;
    }

    /// <inheritdoc />
    public GamePickingSession UserSelectedGame(GamePickingSession session, User user, BoardGame game)
    {
        if(!SessionExists(session))
            throw new InvalidSessionIdException();

        if(!UserIsInSession(session, user))
            throw new UserNotConnectedException();

        _activeSessions.Single(x => x.SessionId == session.SessionId).AddUserSelection(user, game);

        return GetActiveSession(session.SessionId)!;
    }

    /// <inheritdoc />
    public void AddUserToSession(GamePickingSession session, User user)
    {
        if(!_activeSessions.Select(x => x.SessionId).Contains(session.SessionId))
            return;

        if(!_activeSessions.Single(x => x.SessionId == session.SessionId).ConnectedUsers.Select(x => x.Id).Contains(user.Id))
            _activeSessions.Single(x => x.SessionId == session.SessionId).ConnectedUsers.Add(user);
    }

    /// <inheritdoc />
    public void RemoveUserFromSession(GamePickingSession session, User user)
    {
        if(!_activeSessions.Select(x => x.SessionId).Contains(session.SessionId))
            return;

        if(_activeSessions.Single(x => x.SessionId == session.SessionId).ConnectedUsers.Select(x => x.Id).Contains(user.Id))
            _activeSessions.Single(x => x.SessionId == session.SessionId).ConnectedUsers.RemoveAll(x => x.Id == user.Id);
    }

    private bool SessionExists(GamePickingSession session)
        => _activeSessions.Select(x => x.SessionId).Contains(session.SessionId);

    private bool UserIsInSession(GamePickingSession session, User user)
        => _activeSessions.Single(x => x.SessionId == session.SessionId).ConnectedUsers.Select(x => x.Id).Contains(user.Id);
}