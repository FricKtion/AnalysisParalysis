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
    Task<GamePickingSession> StartSession(string bggUser, string ownerId);

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

    /// <summary>
    /// Reduces the number of available games in the session to <paramref name="restrictCount"/> per user.
    /// </summary>
    /// <param name="restrictCount">The number of games per user to restrict to.</param>
    /// <returns>The updated session.</returns>
    GamePickingSession RestrictSessionGameOptions(GamePickingSession session, int restrictCount);

    /// <summary>
    /// If <paramref name="user"/> is found in <paramref name="session"/>, toggle that user's ready status.
    /// </summary>
    /// <param name="session">The session to look for the provided user in.</param>
    /// <param name="user">The user to toggle the ready status of.</param>
    /// <returns>The updated session.</returns>
    GamePickingSession ToggleUserReadyStatus(GamePickingSession session, User user);

    /// <summary>
    /// Mark all users in <paramref name="session"/> as not ready.
    /// </summary>
    /// <param name="session">The session to make changes to.</param>
    /// <returns>The updated session.</returns>
    GamePickingSession UnreadyAllUsers(GamePickingSession session);

    /// <summary>
    /// Updates the session to denote which game was selected and by which user.
    /// </summary>
    /// <param name="session">The session to be updated.</param>
    /// <param name="user">The user who selected the game.</param>
    /// <param name="game">The game that was selected.</param>
    /// <returns>The updated session.</returns>
    GamePickingSession UserSelectedGame(GamePickingSession session, User user, BoardGame game);

    /// <summary>
    /// Add <paramref name="user"/> to <paramref name="session"/> if they aren't already connected.
    /// </summary>
    /// <param name="session">The session to join.</param>
    /// <param name="user">The user that's joining.</param>
    void AddUserToSession(GamePickingSession session, string userId);

    /// <summary>
    /// Remove <paramref name="user"/> from <paramref name="session"/> if they're connected.
    /// </summary>
    /// <param name="session">The session to leave.</param>
    /// <param name="user">The user leaving.</param>
    void RemoveUserFromSession(GamePickingSession session, User user);
}
