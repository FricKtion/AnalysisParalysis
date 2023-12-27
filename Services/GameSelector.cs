using AnalysisParalysis.Data.Models;
using AnalysisParalysis.Services.Definitions;

namespace AnalysisParalysis.Services;

// TODO - Where to put this class?
public static class GameSelector
{
    public static IEnumerable<BoardGame> FindMatches(params BoardGame[] games)
        => games.GroupBy(x => x.Name).Where(x => x.Count() > 1).SelectMany(x => x);

    public static BoardGame PickOne(IEnumerable<BoardGame> games)
        => games.ElementAt(new Random().Next(0, games.Count()));
}
