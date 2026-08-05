using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Extensions.Enum;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Application.Interfaces.Services;
using NievoEasyFin.Application.Services.Security;
using NievoEasyFin.Core.Controllers.Public;
using NSubstitute;
using Xunit.Abstractions;

namespace NievoEasyFin.Tests.API.Core.Public;

/// <summary>
/// Base class for all AccountsController tests.
/// Provides shared mock service, controller instance, and helper methods
/// to reduce boilerplate across test classes.
/// </summary>
public abstract class AccountsTestBase
{
    protected readonly IAccountsService MockService;
    protected readonly AccountsController Controller;
    protected readonly ITestOutputHelper Output;
    protected readonly JsonWebTokenService JwtService;

    protected AccountsTestBase(ITestOutputHelper output)
    {
        Output = output;
        MockService = Substitute.For<IAccountsService>();
        
        JwtService = new JsonWebTokenService(null!);
        Controller = new AccountsController(MockService, JwtService);
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
