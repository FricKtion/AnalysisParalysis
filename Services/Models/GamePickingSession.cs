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

    private readonly Dictionary<User, List<BoardGame>> _selections = new Dictionary<User, List<BoardGame>>();

    // TODO - Add an "owning" user so we know who created the session.

    private readonly List<User> _connectedUsers = new List<User>();

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

        if(_selections.Keys.Count > 1)
            return ChooseFromSelections_MultipleUsers(allGames);
        else if(_selections.Keys.Count == 1)
            return ChooseFromSelections_SingleUser(allGames);
        else 
            throw new NoGamesFoundException("No users have selected games.");
    }

    private BoardGame? ChooseFromSelections_SingleUser(List<BoardGame> allGames)
        => GameSelector.PickOne(allGames);

    private BoardGame? ChooseFromSelections_MultipleUsers(List<BoardGame> allGames)
    {
        var matches = GameSelector.FindMatches(allGames.ToArray());

        if(!matches.Any())
            return null;

        return GameSelector.PickOne(matches);
    }

    /// <summary>
    /// Limits the list of available games to a number of randomly selected games from
    /// each users list of selections. This will also clear all user selections. 
    /// </summary>
    /// <param name="countFromEachUser">The number of games from each user's selected list to include.</param>
    public void RestrictOptions(int countFromEachUser)
    {
        AvailableGames.Clear();

        var rng = new Random();

        foreach(var selectionsList in _selections.Values)
        {
            if(selectionsList.Count < countFromEachUser)
            {
                AvailableGames.AddRange(selectionsList);
            }
            else
            {
                int i = 0;
                while(i < countFromEachUser)
                {
                    var selectedIndex = rng.Next(0, selectionsList.Count - 1);
                    AvailableGames.Add(selectionsList[selectedIndex]);
                    selectionsList.RemoveAt(selectedIndex);
                    i++;
                }
            }
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
        
        foreach(var game in selectedGames)
        {
            if(game.IsSelected)
            {
                if(_selections.ContainsKey(user))
                    _selections[user].AddRange(selectedGames);
                else
                    _selections.Add(user, selectedGames);
            }
            else
            {
                if(_selections.ContainsKey(user))
                    _selections[user].Remove(game);
            }
        }
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