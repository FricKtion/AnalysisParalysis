using AnalysisParalysis.Data.Models.BoardGameGeek;

namespace AnalysisParalysis.Data.Definitions;

public interface IBoardGameRepository
{
    Task<Collection?> GetCollection(string bggUserName);

    Task<Thing?> GetBoardGameDetails(int boardGameId);
}