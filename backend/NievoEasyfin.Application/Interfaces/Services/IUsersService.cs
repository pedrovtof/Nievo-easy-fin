using Microsoft.AspNetCore.Mvc;
using NievoEasyfin.Application.Interfaces.Request;

namespace NievoEasyfin.Application.Interfaces.Services;

public interface IUsersService
{
    Task<IActionResult> PostCreateUserAsync(PostCreateUserRequest request);
    Task<IActionResult> PostCreateUserSsoAsync(PostCreateUserSsoRequest request);
}
