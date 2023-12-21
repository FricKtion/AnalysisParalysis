using AnalysisParalysis.Data.Models;

namespace AnalysisParalysis.Services.Definitions;

public interface IGameSelectionService
{
    IEnumerable<BoardGame> FindMatches(params BoardGame[] games);

    BoardGame PickOne(IEnumerable<BoardGame> games);
}
