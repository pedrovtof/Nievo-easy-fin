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
    /// Completes the verification of the user email.
    /// </summary>
    /// <param name="request">The request containing the email and token.</param>
    /// <returns>An IActionResult indicating the result of the password update.</returns>
    Task<IActionResult> PostValidateEmailAsync([FromBody] PostValidateEmailRequest request);

    /// <summary>
    /// Send the validate email token to the user.
    /// </summary>
    /// <param name="request">The request containing the email.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the process.</returns>
    Task<IActionResult> PostValidateEmailSendAsync([FromBody] PostValidateEmailSendRequest request);

    /// <summary>
    /// Endpoint to get accept terms
    /// </summary>
    /// <returns>Terms accept entity</returns>
    Task<IActionResult> GetAcceptTermsSingupAsync();
}
