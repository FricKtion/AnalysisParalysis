using System.Diagnostics;
using AnalysisParalysis.Data.Models;
using AnalysisParalysis.Data.Models.BoardGameGeek;
using MudBlazor;

namespace AnalysisParalysis.Mappers;

/// <summary>
/// Maps serialized BGG API responses into app models.
/// </summary>
public static class BoardGameMapper
{
    /// <summary>
    /// Maps the "Collection" object into a list of BoardGame objects.
    /// </summary>
    /// <param name="collection">Collection object serialized from BGG API.</param>
    /// <returns>List of BoardGame objects.</returns>
    public static IEnumerable<BoardGame> MapFromCollection(Collection collection)
    {
        var results = new List<BoardGame>();

        foreach(var game in collection.Items)
            results.Add(MapFromCollectionItem(game));

        return results;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static BoardGame MapFromCollectionItem(Collection.Item item)
    {
        var mappedGame = new BoardGame
        {
            Id = item.Id,
            Name = item.Name,
            TimesPlayed = item.TimesPlayed,
            YearPublished = item.YearPublished,
        };

        if(!string.IsNullOrEmpty(item.Thumbnail))
            mappedGame.Thumbnail = new Uri(item.Thumbnail);

        return mappedGame;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="details"></param>
    /// <returns></returns>
    public static IEnumerable<BoardGame> MapWithDetails(Collection collection, Thing details)
    {
        var results = new List<BoardGame>();

        foreach(var game in collection.Items)
        {
            var gameDetails = details.Items.Single(x => x.Id == game.Id);

            if(gameDetails != null)
            {
                var boardGame = MapFromCollectionItem(game);
                
                int minPlayers = 0;
                int.TryParse(gameDetails.MinPlayers.Value, out minPlayers);
                boardGame.MinimumPlayers = minPlayers;

                int maxPlayers = 0;
                int.TryParse(gameDetails.MaxPlayers.Value, out maxPlayers);
                boardGame.MaximumPlayers = maxPlayers; 

                int minTime = 0;
                int.TryParse(gameDetails.MinPlayTime.Value, out minTime);
                boardGame.MinimumPlaytime = minTime;

                int maxTime = 0;
                int.TryParse(gameDetails.MaxPlayTime.Value, out maxTime);
                boardGame.MaximumPlaytime = maxTime;

                results.Add(boardGame);
            }
            else
            {
                results.Add(MapFromCollectionItem(game));
            }
        }

        return results;
    }
}