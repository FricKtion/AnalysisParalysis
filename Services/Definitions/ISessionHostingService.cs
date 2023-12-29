using AnalysisParalysis.Data.Models;
using AnalysisParalysis.Services.Models;

namespace AnalysisParalysis.Services.Definitions;

/// <summary>
/// Starts and manages GamePickingSessions.
/// </summary>
public interface ISessionHostingService
{
    /// <summary>
    /// Start a new session within the site. This will assign a randomized ID to the 
    /// session, assign the available board game based on <paramref name="bggUser"/>,
    /// and mark the session as active.
    /// </summary>
    /// <param name="bggUser">The BGG collection to allow users to pick games from.</param>
    /// <returns>The newly created GamePickingSession object.</returns>
    /// <exception cref="NoGamesFoundException">Thrown if no games are found in the provided user's collection.</exception>
    Task<GamePickingSession> StartSession(string bggUser, User owner);

    /// <summary>
    /// Looks for an acive session with an ID matching <paramref name="sessionId"/>.
    /// </summary>
    /// <param name="sessionId">The ID of the session to look for.</param>
    /// <returns>The session if an active session was found with matching ID, othwerwise null.</returns>
    GamePickingSession? GetActiveSession(int sessionId);

    /// <summary>
    /// Checks if a sesion with ID <paramref name="sessionId"/> exists and is ready.
    /// </summary>
    /// <param name="sessionId">The ID of the session to check.</param>
    /// <returns>True if the session is found and is ready, otherwise false.</returns>
    bool SessionIsReady(int sessionId);
}
