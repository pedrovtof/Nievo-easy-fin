using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Application.Interfaces.Services;

/// <summary>
/// Interface for user management services, handling user registration and SSO linking.
/// </summary>
public interface IUsersService
{
    /// <summary>
    /// Creates a new user account with email and password.
    /// </summary>
    /// <param name="request">The user creation request data.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the user creation.</returns>
    Task<IActionResult> PostCreateUserAsync(PostCreateUserRequest request);

    /// <summary>
    /// Creates a new user account or links an existing one via an SSO provider.
    /// </summary>
    /// <param name="request">The SSO user creation request data.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the SSO user creation.</returns>
public interface IUsersService
{
    Task<IActionResult> PostCreateUserAsync(PostCreateUserRequest request);
    Task<IActionResult> PostCreateUserSsoAsync(PostCreateUserSsoRequest request);
}
