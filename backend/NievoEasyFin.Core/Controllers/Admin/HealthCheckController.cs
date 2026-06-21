using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Data.Context.Database;
using Microsoft.EntityFrameworkCore;
using Dapper;

namespace NievoEasyFin.Core.Controllers.Admin;

/// <summary>
/// Class created for validate status from the server
/// </summary>
[ApiController]
[Route("api/admin/v1/[controller]")]
public class HealthCheckController : ControllerBase
{
    private readonly CoreOrigin _CoreMainNodeDatabase;

    private readonly CoreReplica _CoreReplicaNodeDatabase;

    public HealthCheckController(
        CoreOrigin CoreMainNodeDatabase,
        CoreReplica CoreReplicaNodeDatabase
    )
    {
        _CoreMainNodeDatabase = CoreMainNodeDatabase;
        _CoreReplicaNodeDatabase = CoreReplicaNodeDatabase;
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
        var connection = _CoreMainNodeDatabase.Database.GetDbConnection();
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
        var connection = _CoreReplicaNodeDatabase.Database.GetDbConnection();
        var dataFrame = await connection.QueryFirstOrDefaultAsync(
            "select 1 as test_replicaNode"
        );

        if (dataFrame == null)
            BadRequest();

        return Ok(dataFrame);
    }
}
