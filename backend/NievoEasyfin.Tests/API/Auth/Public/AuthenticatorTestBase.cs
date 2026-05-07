using Microsoft.AspNetCore.Mvc;
using NievoEasyfin.Application.Extensions.Enum;
using NievoEasyfin.Application.Interfaces.Enum;
using NievoEasyfin.Application.Interfaces.Response;
using NievoEasyfin.Application.Interfaces.Services;
using NievoEasyfin.Auth.Controllers.Public;
using NSubstitute;
using Xunit.Abstractions;

namespace NievoEasyfin.Tests.API.Auth.Public;

/// <summary>
/// Base class for all AuthenticatorController tests.
/// Provides shared mock service, controller instance, and helper methods
/// to reduce boilerplate across test classes.
/// </summary>
public abstract class AuthenticatorTestBase
{
    protected readonly IAuthenticatorService MockService;
    protected readonly AuthenticatorController Controller;
    protected readonly ITestOutputHelper Output;

    protected AuthenticatorTestBase(ITestOutputHelper output)
    {
        Output = output;
        MockService = Substitute.For<IAuthenticatorService>();
        Controller = new AuthenticatorController(MockService);
    }

    /// <summary>
    /// Creates a BadRequestObjectResult containing a ResponseApiError with the given enum error description.
    /// </summary>
    protected static BadRequestObjectResult BuildBadRequest(EnumErrosApi enumError)
    {
        var response = new ResponseApiError(new List<string> { enumError.GetDescription() });
        return new BadRequestObjectResult(response);
    }

    /// <summary>
    /// Creates a NotFoundObjectResult containing a ResponseApiError with the given enum error descriptions.
    /// </summary>
    protected static NotFoundObjectResult BuildNotFound(params EnumErrosApi[] enumErrors)
    {
        var messages = enumErrors.Select(e => e.GetDescription()).ToList();
        var response = new ResponseApiError(messages);
        return new NotFoundObjectResult(response);
    }

    /// <summary>
    /// Creates an OkObjectResult containing a ResponseApiSucess with the given data.
    /// </summary>
    protected static OkObjectResult BuildOk(object data)
    {
        var response = new ResponseApiSucess(data);
        return new OkObjectResult(response);
    }
}
