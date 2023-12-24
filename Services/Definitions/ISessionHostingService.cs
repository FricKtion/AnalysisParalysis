using AnalysisParalysis.Data.Models;
using AnalysisParalysis.Services.Models;

namespace AnalysisParalysis.Services.Definitions;

public interface ISessionHostingService
{
    GamePickingSession StartSession();

    GamePickingSession JoinSession(int sessionId);

    bool SessionIsReady(int sessionId);
}
