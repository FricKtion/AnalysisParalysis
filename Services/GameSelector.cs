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
    /// </summary>
    /// <param name="games">The arrays to look for matches within.</param>
    /// <returns>Any games that are shared between multiple arrays.</returns>
    public static IEnumerable<BoardGame> FindMatches(params BoardGame[] games)
        => games.GroupBy(x => x.Name).Where(x => x.Count() > 1).SelectMany(x => x);

    /// <summary>
    /// Picks one game at random from the provided list.
    /// </summary>
    /// <param name="games">The list of games to choose from.</param>
    /// <returns>A random BoardGame object from the provided list.</returns>
    public static BoardGame PickOne(IEnumerable<BoardGame> games)
        => games.ElementAt(new Random().Next(0, games.Count()));
}
