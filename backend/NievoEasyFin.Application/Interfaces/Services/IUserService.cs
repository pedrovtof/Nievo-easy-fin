using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Application.Interfaces.Services;

/// <summary>
/// Interface for user services, handling user bank accounts, cards, goals, and categories.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Creates a new bank account for the authenticated user.
    /// </summary>
    /// <param name="request">PostUserBanksRequest</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the user bank account creation.</returns>
    Task<IActionResult> PostUserBanks(PostUserBanksRequest request);

    /// <summary>
    /// Get list of user bank accounts
    /// </summary>
    /// <param name="request">GetUserBanksRequest</param>
    Task<IActionResult> GetUserBanks(GetUserBanksRequest request);

    /// <summary>
    /// Get list of card registred of the user
    /// </summary>
    Task<IActionResult> GetUserCard([FromQuery] GetUserCardRequest request);

    /// <summary>
    /// Create an user card
    /// </summary>
    Task<IActionResult> PostUserCard([FromBody] PostUserCardRequest request);

    /// <summary>
    /// Create an user goal
    /// </summary>
    Task<IActionResult> PostUserGoal([FromBody] PostUserGoalRequest request);

    /// <summary>
    /// Get user goal
    /// </summary>
    Task<IActionResult> GetUserGoal([FromBody] GetUserGoalRequest request);

    /// <summary>
    /// Create an user category
    /// </summary>
    Task<IActionResult> PostUserCategory([FromBody] PostUserCategoryRequest request);

    /// <summary>
    /// Get user category
    /// </summary>
    Task<IActionResult> GetUserCategory([FromBody] GetUserCategoryRequest request);
}
