using AnalysisParalysis.Data.Models;
using AnalysisParalysis.Services.Models;

namespace AnalysisParalysis.Services.Definitions;

public interface ISessionHostingService
{
    Task<GamePickingSession> StartSession(string bggUser);

    GamePickingSession JoinSession(int sessionId);

    GamePickingSession GetActiveSession(int sessionId);

    bool SessionIsReady(int sessionId);
}
