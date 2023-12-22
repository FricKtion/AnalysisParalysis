using AnalysisParalysis.Data.Models;

namespace AnalysisParalysis.Services.Models;

public class GamePickingSession
{
    public GamePickingSession() { }

    public GamePickingSession(int sessionId)
        => SessionId = sessionId;

    public int SessionId { get; set; } = -1;

    public List<BoardGame> SelectedGames { get; set; } = new List<BoardGame>();
}