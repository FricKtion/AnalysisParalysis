using AnalysisParalysis.Data.Models.BoardGameGeek;

namespace AnalysisParalysis.Data.Definitions;

/// <summary>
/// Acts as a wrapper for the BGG API, providing methods to interact and pull data from said API.
/// </summary>
public interface IBoardGameRepository
{
    /// <summary>
    /// Gets a user's collection from the data source.
    /// </summary>
    /// <param name="bggUserName">Username of the collection to find.</param>
    /// <returns>Serialized response from BGG API if found, otherwise null.</returns>
    Task<Collection?> GetCollection(string bggUserName);

    /// <summary>
    /// Get details for the boare game with an ID matching <paramref name="boardGameId"/>.
    /// </summary>
    /// <param name="boardGameId">The ID of the game to find details for.</param>
    /// <returns>Serialized response from BGG API if found, otherwise null.</returns>
    Task<Thing?> GetBoardGameDetails(int boardGameId);
}