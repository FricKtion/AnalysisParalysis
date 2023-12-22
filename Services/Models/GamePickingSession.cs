using AnalysisParalysis.Data.Models;

namespace AnalysisParalysis.Services.Models;

public class GamePickingSession
{
    public GamePickingSession() { }

    public GamePickingSession(int sessionId)
        => (SessionId, SessionIsReady) = (sessionId, true);

    public int SessionId { get; set; } = -1;

    public bool SessionIsReady { get; set; } = false;

    public List<BoardGame> SelectedGames { get; set; } = new List<BoardGame>();
}