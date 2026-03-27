using Microsoft.AspNetCore.Mvc;
using NievoEasyfin.Application.Data.Context.Database;
using Microsoft.EntityFrameworkCore;

namespace NievoEasyfin.Auth.Controllers
{
    /// <summary>
    /// Class created for validate status from the server
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class HealthCheckController : ControllerBase
    {
        private static AuthOrigin _AuthMainNodeDatabase;
        private static AuthReplica _AuthReplicaNodeDatabase;

        public HealthCheckController(AuthOrigin AuthMainNodeDatabase, AuthReplica AuthReplicaNodeDatabase)
        {
            _AuthMainNodeDatabase = AuthMainNodeDatabase;
            _AuthReplicaNodeDatabase = AuthReplicaNodeDatabase;
        }

        /// <summary>
        /// Class created for validate status server it is alive
        /// </summary>
        /// <response code = "200">Return ALIVE message</response>
        [HttpGet("check")]
        public IActionResult HealthCheck()
        {
            var _response = new { message = "Alive" };
            return Ok(_response);
        }

        /// <summary>
        /// Class created for validate status server postgres Main Node it is alive
        /// </summary>
        /// <response code = "200">Return Query check message and a dataframe with random number search in Database</response>
        [HttpGet("database/pgsql/main-node")]
        public async Task<IActionResult> HealthCheckDatabasePgsqlMainNodeAsync()
        {
            string query = "select 10";

            var dataframe = await _AuthMainNodeDatabase.Database.ExecuteSqlRawAsync(query);

            return Ok(dataframe);
        }

        /// <summary>
        /// Class created for validate status server postgres Replica Node it is alive
        /// </summary>
        /// <response code = "200">Return Query check message and a dataframe with random number search in Database</response>
        [HttpGet("database/pgsql/replica-node")]
        public async Task<IActionResult> HealthCheckDatabasePgsqlReplicaNodeAsync()
        {
            string query = "select 10";

            var dataframe = await _AuthReplicaNodeDatabase.Database.ExecuteSqlRawAsync(query);

            return Ok(dataframe);
        }
    }
}