using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Application.Interfaces.Services;

/// <summary>
/// Interface for authentication services, handling login, SSO, and password management.
/// </summary>
public interface IAuthenticatorService
{

    /// <summary>
    /// Authenticates a user using email and password.
    /// </summary>
    /// <param name="request">The login request containing email and password.</param>
    /// <returns>An IActionResult containing the authentication result (JWT token on success).</returns>
    Task<IActionResult> PostLoginUserAsync(PostLoginUserRequest request);

    /// <summary>
    /// Authenticates a user using a Single Sign-On (SSO) provider.
    /// </summary>
    /// <param name="request">The SSO login request containing provider information and access token.</param>
    /// <returns>An IActionResult containing the authentication result (JWT token on success).</returns>
    Task<IActionResult> PostLoginUserSsoAsync(PostLogiPostLoginUserSsoRequest request);

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

    /// <summary>
    /// Completes the verification of the user email.
    /// </summary>
    /// <param name="request">The request containing the email and token.</param>
    /// <returns>An IActionResult indicating the result of the validation.</returns>
    Task<IActionResult> PostValidateEmailAsync([FromBody] PostValidateEmailRequest request);

    /// <summary>
    /// Sends a new email validation token to the user.
    /// </summary>
    /// <param name="request">The request containing the user email.</param>
    /// <returns>An IActionResult indicating the result of the send operation.</returns>
    Task<IActionResult> PostValidateEmailSendAsync([FromBody] PostValidateEmailSendRequest request);
}
