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
/// Base class for all UsersController tests.
/// </summary>
public abstract class UsersTestBase
{
    protected readonly IUsersService MockService;
    protected readonly UsersController Controller;
    protected readonly ITestOutputHelper Output;

    protected UsersTestBase(ITestOutputHelper output)
    {
        Output = output;
        MockService = Substitute.For<IUsersService>();
        Controller = new UsersController(MockService);
    }

    protected static BadRequestObjectResult BuildBadRequest(EnumErrosApi enumError)
    {
        var response = new ResponseApiError(new List<string> { enumError.GetDescription() });
        return new BadRequestObjectResult(response);
    }

    protected static NotFoundObjectResult BuildNotFound(params EnumErrosApi[] enumErrors)
    {
        var messages = enumErrors.Select(e => e.GetDescription()).ToList();
        var response = new ResponseApiError(messages);
        return new NotFoundObjectResult(response);
    }

    protected static OkObjectResult BuildOk(object data)
    {
        var response = new ResponseApiSucess(data);
        return new OkObjectResult(response);
    }
}
