using AnalysisParalysis.Data.Models;
using AnalysisParalysis.Exceptions;

namespace AnalysisParalysis.Services.Models;

public class GamePickingSession
{
    public GamePickingSession() { }

    public GamePickingSession(int sessionId)
        => (SessionId, SessionIsReady) = (sessionId, true);

    public int SessionId { get; set; } = -1;

    public bool SessionIsReady { get; set; } = false;

    public List<BoardGame> AvailableGames { get; set; } = new List<BoardGame>();

    private Dictionary<User, List<BoardGame>> _selections;

    private List<User> _connectedUsers = new List<User>();

    public void AddUserSelections(User user, List<BoardGame> selectedGames)
    {
        if(_connectedUsers.Select(x => x.Id).Contains(user.Id))
            _selections.Add(user, selectedGames);
        else
            throw new UserNotConnectedException($"User '{user.Id}' isn't connected to this session.");
    }

    public void JoinSession(User user)
    {
        if(!_connectedUsers.Select(x => x.Id).Contains(user.Id))
            _connectedUsers.Add(user);
    }
}