using System.Diagnostics;
using AnalysisParalysis.Data.Models;
using AnalysisParalysis.Data.Models.BoardGameGeek;

namespace AnalysisParalysis.Mappers;

/// <summary>
/// Maps serialized BGG API responses into app models.
/// </summary>
public static class BoardGameMapper
{
    /// <summary>
    /// Maps the "Thing" object into a BoardGame.
    /// </summary>
    /// <param name="thing">Thing object serialized from BGG API.</param>
    /// <returns>BoardGame object.</returns>
    public static IEnumerable<BoardGame> MapFromThing(Thing thing)
    {
        var results = new List<BoardGame>();

        foreach(var item in thing.Items)
        {
            var boardGame = new BoardGame
            {
                Id = item.Id,
                Name = item.Names?.First(x => x.Type == "primary").Value
                    ?? "NO PRIMARY NAME FOUND",
                TimesPlayed = -1
            };

            int yearPublished = 0;
            int.TryParse(item.YearPublished.Value, out yearPublished);
            boardGame.YearPublished = yearPublished;

            if(!string.IsNullOrEmpty(item.Thumbnail))
                boardGame.Thumbnail = new Uri(item.Thumbnail);

            int minPlayers = 0;
            int.TryParse(item.MinPlayers.Value, out minPlayers);
            boardGame.MinimumPlayers = minPlayers;

            int maxPlayers = 0;
            int.TryParse(item.MaxPlayers.Value, out maxPlayers);
            boardGame.MaximumPlayers = maxPlayers; 

            results.Add(boardGame);
        }
    
        return results;
    }

    /// <summary>
    /// Maps the "Collection" object into a list of BoardGame objects.
    /// </summary>
    /// <param name="collection">Collection object serialized from BGG API.</param>
    /// <returns>List of BoardGame objects.</returns>
    public static IEnumerable<BoardGame> MapFromCollection(Collection collection)
    {
        var results = new List<BoardGame>();

        foreach(var game in collection.Items)
        {
            var addition = new BoardGame
            {
                Id = game.Id,
                Name = game.Name,
                TimesPlayed = game.TimesPlayed,
                YearPublished = game.YearPublished,
            };

            if(!string.IsNullOrEmpty(game.Thumbnail))
                addition.Thumbnail = new Uri(game.Thumbnail);

            results.Add(addition);
        }

        return results;
    }
}