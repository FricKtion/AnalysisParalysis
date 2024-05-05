using AnalysisParalysis.Data.Models;
using AnalysisParalysis.Services.Definitions;

namespace AnalysisParalysis.Services;

/// <summary>
/// Provides logic for selecting a game given some options.
/// </summary>
public static class GameSelector
{
    /// <summary>
    /// Finds any matches amongst the provided arrays. 
    /// Only games that show up <paramref name="matchesRequired"/> times will count as having matched.
    /// </summary>
    /// <param name="games">The arrays to look for matches within.</param>
    /// <param name="matchesRequired"> The number of times a game must exist to coutn as a match.
    /// <returns>Any games that are shared between multiple arrays.</returns>
    public static IEnumerable<BoardGame> FindMatches(int matchesRequired, params BoardGame[] games)
        => games.GroupBy(x => x.Name).Where(x => x.Count() == matchesRequired).SelectMany(x => x);

    /// <summary>
    /// Returns the number of times <paramref name="gameToCount"/> is included in <paramref name="games"/>.
    /// </summary>
    /// <param name="games">The list of games to search through.</param>
    /// <param name="gameToCount">The game to count.</param>
    /// <returns>The number of times a game shows up in a list of games.</returns>
    public static int GetSelectionCount(BoardGame gameToCount, params BoardGame[] games)
        => games.GroupBy(x => x.Name).Count(x => x.Key == gameToCount.Name);

    /// <summary>
    /// Picks one game at random from the provided list.
    /// </summary>
    /// <param name="games">The list of games to choose from.</param>
    /// <returns>A random BoardGame object from the provided list.</returns>
    public static BoardGame PickOne(IEnumerable<BoardGame> games)
        => games.ElementAt(new Random().Next(0, games.Count()));
}
