using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using auth.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace auth.Controllers
{
    /// <summary>
    /// Class created for validate status from the server
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class HealthCheckController : ControllerBase
    {
        
        private readonly AuthOrigin _AuthMainNodeDatabase;
        private readonly AuthReplica _AuthReplicaNodeDatabase;

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
        [HttpGet("database/pgsql/main/node")]
        public async Task<IActionResult> HealthCheckDatabasePgsqlMainNode()
        {
            Random rnd = new Random();
            int randomValue = rnd.Next(1,10);
            randomValue = Math.Abs(randomValue);

            string query = "select {0};";

            var dataframe = await _AuthMainNodeDatabase.Database
                .ExecuteSqlRawAsync(query, randomValue);


            var _response = new {message="Query check", rows=dataframe};

            return Ok(_response);
        }

        /// <summary>
        /// Class created for validate status server postgres Replica Node it is alive
        /// </summary>
        /// <response code = "200">Return Query check message and a dataframe with random number search in Database</response>
        [HttpGet("database/pgsql/replica/node")]
        public async Task<IActionResult> HealthCheckDatabasePgsqlReplicaNode()
        {
            Random rnd = new Random();
            int randomValue = rnd.Next(1,10);
            randomValue = Math.Abs(randomValue);

            string query = "select {0};";

            var dataframe = await _AuthReplicaNodeDatabase.Database
                .ExecuteSqlRawAsync(query, randomValue);

            var _response = new {message="Query check", rows=dataframe};

            return Ok(_response);

        }
    }
}