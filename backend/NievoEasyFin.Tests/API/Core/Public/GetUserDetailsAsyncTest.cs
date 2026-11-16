using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Data.Views;
using NievoEasyFin.Application.Extensions.Enum;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Tests.Build.Request;
using NSubstitute;
using Xunit.Abstractions;

namespace NievoEasyFin.Tests.API.Core.Public;

/// <summary>
/// Unit tests for the GET user details endpoint (<c>GET /api/public/v1/User/details</c>) of <see cref="NievoEasyFin.Core.Controllers.Public.UserController"/>.
/// </summary>
public class GetUserDetailsAsyncTest : AccountsTestBase
{
    /// <summary>
    /// Initializes a new instance of <see cref="GetUserDetailsAsyncTest"/>.
    /// </summary>
    /// <param name="output">Test output helper.</param>
    public GetUserDetailsAsyncTest(ITestOutputHelper output) : base(output) { }

    #region Success

    /// <summary>
    /// Verifies that GetUserDetails returns 200 OK with user details when the request is valid.
    /// </summary>
    [Fact(DisplayName = "GetUserDetails with valid request returns Ok with user details")]
    public async Task GetUserDetailsAsync_ValidRequest_ReturnsOk()
    {
        // Arrange
        var view = new UserDetailsViews
        {
            Name = "Alex Morgan",
            Email = "alex.morgan@nievo.com",
            Phone = 119888877,
            UserStatus = "ACTIVE"
        };
        var okResult = BuildOk(new GetUserDetailsResponse(view));

        MockService.GetUserDetails(Arg.Any<GetUserDetailsRequest>())
                   .Returns(Task.FromResult<IActionResult>(okResult));

        var fakeToken = "fake-token";

        // Act
        var request = new GetUserDetailsRequestBuilder();
        var result = await Controller.GetUserDetails(fakeToken, request);

        // Assert
        var objectResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var apiSuccess = objectResult.Value.Should().BeOfType<ResponseApiSucess>().Subject;
        var data = apiSuccess.Data.Should().BeOfType<GetUserDetailsResponse>().Subject;
        data.Name.Should().Be(view.Name);
        data.Email.Should().Be(view.Email);
        data.Phone.Should().Be(view.Phone);
        data.UserStatus.Should().Be(view.UserStatus);
        Output.WriteLine("\n Validated success on GetUserDetails \n");
    }

    #endregion

    #region Error Scenarios

    public static IEnumerable<object[]> BadRequestErrors => new List<object[]>
    {
        new object[] { EnumErrosApi.GETUSERDETAILSYASYNC_CORESERVICE_400_EMPTY_EMAIL, "Empty email" },
        new object[] { EnumErrosApi.GETUSERDETAILSYASYNC_CORESERVICE_400_INVALID_EMAIL, "Invalid email" },
    };

    /// <summary>
    /// Verifies that GetUserDetails returns 400 BadRequest when validation errors occur.
    /// </summary>
    [Theory(DisplayName = "GetUserDetails returns BadRequest for validation error scenarios")]
    [MemberData(nameof(BadRequestErrors))]
    public async Task GetUserDetailsAsync_ErrorScenario_ReturnsBadRequest(EnumErrosApi enumError, string scenario)
    {
        // Arrange
        var badRequestResult = BuildBadRequest(enumError);

        MockService.GetUserDetails(Arg.Any<GetUserDetailsRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        var fakeToken = "fake-token";

        // Act
        var request = new GetUserDetailsRequestBuilder();
        var result = await Controller.GetUserDetails(fakeToken, request);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(enumError.GetDescription());
        Output.WriteLine($"\n Validated error: {scenario} ({enumError}) \n");
    }

    /// <summary>
    /// Verifies that GetUserDetails returns 404 NotFound when the user is not found.
    /// </summary>
    [Fact(DisplayName = "GetUserDetails when user not found returns NotFound")]
    public async Task GetUserDetailsAsync_UserNotFound_ReturnsNotFound()
    {
        // Arrange
        var notFoundResult = BuildNotFound(EnumErrosApi.GETUSERDETAILSYASYNC_CORESERVICE_404_USER_NOT_FOUND);

        MockService.GetUserDetails(Arg.Any<GetUserDetailsRequest>())
                   .Returns(Task.FromResult<IActionResult>(notFoundResult));

        var fakeToken = "fake-token";

        // Act
        var request = new GetUserDetailsRequestBuilder();
        var result = await Controller.GetUserDetails(fakeToken, request);

        // Assert
        var objectResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(EnumErrosApi.GETUSERDETAILSYASYNC_CORESERVICE_404_USER_NOT_FOUND.GetDescription());
        Output.WriteLine("\n Validated 404 NotFound on GetUserDetails \n");
    }

    #endregion

    #region Service Delegation

    /// <summary>
    /// Verifies that GetUserDetails delegates the call to ICoreUserService exactly once.
    /// </summary>
    [Fact(DisplayName = "GetUserDetails delegates call to service exactly once")]
    public async Task GetUserDetailsAsync_WhenCalled_DelegatesToService()
    {
        // Arrange
        var okResult = BuildOk(new { });

        MockService.GetUserDetails(Arg.Any<GetUserDetailsRequest>())
                   .Returns(Task.FromResult<IActionResult>(okResult));

        var fakeToken = "fake-token";

        // Act
        var request = new GetUserDetailsRequestBuilder();
        await Controller.GetUserDetails(fakeToken, request);

        // Assert
        await MockService.Received(1).GetUserDetails(Arg.Any<GetUserDetailsRequest>());
        Output.WriteLine("\n Validated that service was called once. \n");
    }

    #endregion
}
