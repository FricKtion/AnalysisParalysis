namespace AnalysisParalysis.Data.Models;

public class Collection
{
    public int Count { get; set; } = 0;

    public IEnumerable<BoardGame> Games { get; set; } = Enumerable.Empty<BoardGame>();
}
