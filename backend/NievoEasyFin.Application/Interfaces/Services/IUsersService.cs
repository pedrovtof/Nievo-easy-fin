using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Application.Interfaces.Services;

public interface IUsersService
{
    Task<IActionResult> PostCreateUserAsync(PostCreateUserRequest request);
    Task<IActionResult> PostCreateUserSsoAsync(PostCreateUserSsoRequest request);
}
