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
    Task<IActionResult> PostCreateUserSsoAsync(PostCreateUserSsoRequest request);

    /// <summary>
    /// Initiates the password reset process by sending a reset token to the user's email.
    /// </summary>
    /// <param name="request">The request containing the user's email.</param>
    /// <returns>An IActionResult indicating if the reset process was successfully initiated.</returns>
    Task<IActionResult> PostResetPasswordAsync(PostResetPasswordRequest request);

    /// <summary>
    /// Completes the password reset process using a reset token and a new password.
    /// </summary>
    /// <param name="request">The request containing the email, reset token, and new password.</param>
    /// <returns>An IActionResult indicating the result of the password update.</returns>
    Task<IActionResult> PatchResetPasswordAsync(PatchResetPasswordRequest request);
}

