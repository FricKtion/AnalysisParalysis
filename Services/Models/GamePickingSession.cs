using AnalysisParalysis.Data.Models;
using AnalysisParalysis.Exceptions;
using MudBlazor;

namespace AnalysisParalysis.Services.Models;

public class GamePickingSession
{
    public GamePickingSession() { }

    public GamePickingSession(int sessionId)
        => (SessionId, SessionIsReady) = (sessionId, true);

    public int SessionId { get; set; } = -1;

    public bool SessionIsReady { get; set; } = false;

    public List<BoardGame> AvailableGames { get; set; } = new List<BoardGame>();

    private Dictionary<User, List<BoardGame>> _selections = new Dictionary<User, List<BoardGame>>();

    // TODO - Add an "owning" user so we know who created the session.

    private List<User> _connectedUsers = new List<User>();

    /// <summary>
    /// Chooses a random board game from the lists of user selections, as long as 
    /// there is at least one match between users.
    /// </summary>
    /// <returns>BoardGame object or null if no mathces.</returns>
    public BoardGame? ChooseFromSelections()
    {
        var allGames = new List<BoardGame>();
        foreach(var selectionList in _selections)
            allGames.AddRange(selectionList.Value);

        var matches = GameSelector.FindMatches(allGames.ToArray());

        if(!matches.Any())
            return null;

        var rng = new Random();
        return matches.ElementAt(rng.Next(0, matches.Count() - 1));
    }

    /// <summary>
    /// Limits the list of available games to a randomly selected game from
    /// each users list of selections. This will also clear all user selections. 
    /// </summary>
    public void RestrictOptions()
    {
        AvailableGames.Clear();

        var rng = new Random();
        foreach(var selectionsList in _selections.Values)
        {
            // TODO - Take into account the selections list getting smaller
            // TODO - Add more than one game from each user's list.
            AvailableGames.Add(selectionsList.ElementAt(rng.Next(0, selectionsList.Count() - 1)));
        }

        AvailableGames.ForEach(x => x.IsSelected = false);
        _selections.Clear();
    }

    /// <summary>
    /// Adds <paramref name="selectedGame"/> to the list of games selected 
    /// by <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user who selected the game.</param>
    /// <param name="selectedGame">The game selected.</param>
    /// <exception cref="UserNotConnectedException">Thrown if <paramref name="user"/> is not connected to this session.</exception>
    public void AddUserSelection(User user, BoardGame selectedGame)
        => AddUserSelections(user, new List<BoardGame> { selectedGame });

    /// <summary>
    /// Adds all games in <paramref name="selectedGames"/> to the list of 
    /// games selected by <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user who selected the games.</param>
    /// <param name="selectedGames">The games selected.</param>
    /// <exception cref="UserNotConnectedException">Thrown if <paramref name="user"/> is not connected to this session.</exception>
    public void AddUserSelections(User user, List<BoardGame> selectedGames)
    {
        if(!_connectedUsers.Select(x => x.Id).Contains(user.Id))
            throw new UserNotConnectedException($"User '{user.Id}' isn't connected to this session.");
        
        if(_selections.ContainsKey(user))
            _selections[user].AddRange(selectedGames);
        else
            _selections.Add(user, selectedGames);
    }

    // TODO - Should this be in the SessionHostingService?
    /// <summary>
    /// Adds <paramref name="user"/> as a connected user to this session.
    /// </summary>
    /// <param name="user">The user to add to this session.</param>
    public void JoinSession(User user)
    {
        if(!_connectedUsers.Select(x => x.Id).Contains(user.Id))
            _connectedUsers.Add(user);
    }
}