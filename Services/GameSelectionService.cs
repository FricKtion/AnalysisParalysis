using AnalysisParalysis.Data.Models;
using AnalysisParalysis.Services.Definitions;

namespace AnalysisParalysis.Services;

public class GameSelectionService : IGameSelectionService
{
    public IEnumerable<BoardGame> FindMatches(params BoardGame[] games)
        => games.GroupBy(x => x.Name).Where(x => x.Count() > 1).SelectMany(x => x);

    public BoardGame PickOne(IEnumerable<BoardGame> games)
        => games.ElementAt(new Random().Next(0, games.Count()));
}
