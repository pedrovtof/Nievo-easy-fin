using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Application.Interfaces.Services;

public interface IAuthenticatorService
{
    Task<IActionResult> PostLoginUserAsync(PostLoginUserRequest request);
    Task<IActionResult> PostLoginUserSsoAsync(PostLogiPostLoginUserSsoRequest request);
    Task<IActionResult> PostResetPasswordAsync(PostResetPasswordRequest request);
    Task<IActionResult> PatchResetPasswordAsync(PatchResetPasswordRequest request);
}
