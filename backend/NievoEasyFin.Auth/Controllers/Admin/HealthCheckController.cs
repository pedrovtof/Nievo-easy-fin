using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Data.Context.Database;
using Microsoft.EntityFrameworkCore;
using Dapper;
using NievoEasyFin.Application.Models;
using NievoEasyFin.Application.Services.Cache;

namespace NievoEasyFin.Auth.Controllers.Admin;

/// <summary>
/// Class created for validate status from the server
/// </summary>
[ApiController]
[Route("api/private/v1/[controller]")]
public class HealthCheckController : ControllerBase
{
    private readonly AuthOrigin _AuthMainNodeDatabase;

    private readonly AuthReplica _AuthReplicaNodeDatabase;

    private readonly AuthDbCacheService _AuthMainCacheNodeDatabase;

    public HealthCheckController(
        AuthOrigin AuthMainNodeDatabase,
        AuthReplica AuthReplicaNodeDatabase,
        AuthDbCacheService AuthMainCacheNodeDatabase
    )
    {
        _AuthMainNodeDatabase = AuthMainNodeDatabase;
        _AuthReplicaNodeDatabase = AuthReplicaNodeDatabase;
        _AuthMainCacheNodeDatabase = AuthMainCacheNodeDatabase;
    }

    /// <summary>
    /// Method created for validate status server it is alive
    /// </summary>
    /// <response code = "200">Return ALIVE message</response>
    [HttpGet("check")]
    [AllowAnonymous]
    public IActionResult HealthCheck()
    {
        var _response = new { message = "Alive" };
        return Ok(_response);
    }

    /// <summary>
    /// Method created for validate status server postgres Main Node it is alive
    /// </summary>
    /// <response code = "200">1</response>
    /// <response code = "400">BadRequest</response>
    [HttpGet("database/pgsql/main-node")]
    [Authorize]
    public async Task<IActionResult> HealthCheckDatabasePgsqlMainNodeAsync()
    {
        var connection = _AuthMainNodeDatabase.Database.GetDbConnection();
        var dataFrame = await connection.QueryFirstOrDefaultAsync(
            "select 1 as test_mainNode"
        );

        if (dataFrame == null)
            BadRequest();

        return Ok(dataFrame);
    }

    /// <summary>
    /// Method created for validate status server postgres Replica Node it is alive
    /// </summary>
    /// <response code = "200">1</response>
    /// <response code = "400">BadRequest</response>
    [HttpGet("database/pgsql/replica-node")]
    [Authorize]
    public async Task<IActionResult> HealthCheckDatabasePgsqlReplicaNodeAsync()
    {
        var connection = _AuthReplicaNodeDatabase.Database.GetDbConnection();
        var dataFrame = await connection.QueryFirstOrDefaultAsync(
            "select 1 as test_replicaNode"
        );

        if (dataFrame == null)
            BadRequest();

        return Ok(dataFrame);
    }

    /// <summary>
    /// Method created for validate status server Cache main node
    /// </summary>
    /// <response code = "200">"example"</response>
    /// <response code = "400">BadRequest</response>
    [HttpGet("database/cache/main-node")]
    [Authorize]
    public async Task<IActionResult> HealthCheckDatabaseCacheMainNodeAsync()
    {
        var CreateTest = _AuthMainCacheNodeDatabase.TestCacheServiceAsync("test", "example");
        if (CreateTest == null)
            return BadRequest();
        return Ok();
    }


    /// <summary>
    /// Method created for validate smtp email send
    /// Always return 200, you have to validate if you receive it.
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST v1/HealthCheck/smtp
    ///     Body =>  "Joe.Black@example.com"
    ///     
    /// </remarks>
    /// <response code = "200">Return ok</response>
    [HttpPost("smtp")]
    [Authorize]
    public async Task<IActionResult> HealthCheckSmtpSendEmailAsync([FromBody]
        string email
    )
    {
        SmtpModel test = new SmtpModel();
        await test.TestSendEmailAsync(email);
        return Ok();
    }


}
