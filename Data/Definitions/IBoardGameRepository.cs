using AnalysisParalysis.Data.Models;

namespace AnalysisParalysis.Data.Definitions;

public interface IBoardGameRepository
{
    Task<Collection?> GetCollection(string bggUserName);

    Task<BoardGame?> GetBoardGameDetails(int boardGameId);
}