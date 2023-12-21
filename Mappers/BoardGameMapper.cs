using AnalysisParalysis.Data.Models;
using AnalysisParalysis.Data.Models.BoardGameGeek;

namespace AnalysisParalysis.Mappers;

public static class BoardGameMapper
{
    public static BoardGame MapFromThing(Thing thing)
    {
        var boardGame = new BoardGame
        {
            Id = thing.Item.Id,
            Name = thing.Item.Names?.First(x => x.Type == "primary").Value
                ?? "NO PRIMARY NAME FOUND",
            TimesPlayed = -1
        };

        int yearPublished = 0;
        int.TryParse(thing.Item.YearPublished.Value, out yearPublished);
        boardGame.YearPublished = yearPublished;

        if(!string.IsNullOrEmpty(thing.Item.Thumbnail))
            boardGame.Thumbnail = new Uri(thing.Item.Thumbnail);

        return boardGame;
    }

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