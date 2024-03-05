using AnalysisParalysis.Data.Models;
using AnalysisParalysis.Exceptions;
using MudBlazor;

namespace AnalysisParalysis.Services.Models;

// TODO - Move this to it's own project, along with the SessionHostingService and SessionHub. Then I can set the methods here to internal and force the class to be used as intended.
public class GamePickingSession
{
    public GamePickingSession()
        => (SessionId, OwningUser, SessionIsReady) = (0, new User(), false);

    public GamePickingSession(int sessionId, User owner)
        => (SessionId, OwningUser, SessionIsReady) = (sessionId, owner, true);

    public int SessionId { get; set; } = -1;

    public bool SessionIsReady { get; set; } = false;

    public List<BoardGame> AvailableGames { get; set; } = new List<BoardGame>();

    public User OwningUser { get; set; }

    public List<User> ConnectedUsers { get; set; } = new List<User>();

    private readonly Dictionary<string, List<BoardGame>> _selections = new Dictionary<string, List<BoardGame>>();

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

    /// <summary>
    /// Select a single game from one user's selections.
    /// </summary>
    /// <param name="allGames">All games selected by the user.</param>
    /// <returns>A BoardGame selected from <paramref name="allGames"/></returns>
    private BoardGame? ChooseFromSelections_SingleUser(List<BoardGame> allGames)
        => GameSelector.PickOne(allGames);

    /// <summary>
    /// If matches are found in the list, picks a single game from those matches.
    /// If no matches are found, nothing is returned.
    /// </summary>
    /// <param name="allGames">All user's selected games.</param>
    /// <returns>A singel BoardGame selected at random from the matches. If no matches, null.</returns>
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
        if(!ConnectedUsers.Select(x => x.Id).Contains(user.Id))
            throw new UserNotConnectedException($"User '{user.Id}' isn't connected to this session.");
        
        foreach(var game in selectedGames)
        {
            if(game.IsSelected)
            {
                if(_selections.ContainsKey(user.Id))
                    _selections[user.Id].AddRange(selectedGames);
                else
                    _selections.Add(user.Id, selectedGames);
            }
            else
            {
                if(_selections.ContainsKey(user.Id))
                    _selections[user.Id].RemoveAll(x => x.Id == game.Id);
            }
        }
    }

    /// <summary>
    /// Toggle the ready status of the connected user whose ID matches that of
    /// <paramref name="user"/>. An exception will be thrown if the user isn't found.
    /// </summary>
    /// <param name="user">The user to toggle the ready status of.</param>
    public void ToggleUserReadyStatus(User user)
    {
        ConnectedUsers.Single(x => x.Id == user.Id).IsReady = 
            !ConnectedUsers.Single(x => x.Id == user.Id).IsReady;
    }
}