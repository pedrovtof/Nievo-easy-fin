using NievoEasyFin.Application.Data.Context.Database;
using NievoEasyFin.Application.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Text;
using NievoEasyFin.Application.Data.Views;

namespace NievoEasyFin.Application.Models
{
    public class GoalModel : GoalEntity
    {
        private readonly CoreOrigin _CoreMainNodeDatabase;

        private readonly CoreReplica? _CoreReplicaNodeDatabase;

        public GoalModel(CoreOrigin coreMainNodeDatabase, CoreReplica? coreReplicaNodeDatabase)
        {
            _CoreMainNodeDatabase = coreMainNodeDatabase;
            _CoreReplicaNodeDatabase = coreReplicaNodeDatabase;
        }

        /// <summary>
        /// Get a valid goal
        /// </summary>
        /// <param name="userId">User</param>
        /// <param name="goalName">Name of the goal</param>
        /// <returns>GoalEntity</returns>
        public async Task<GoalEntity> GetGoalValidByUserAndName(int userId, string goalName)
            => await _CoreReplicaNodeDatabase.Goal.FirstOrDefaultAsync(x => x.UserId == userId && x.Active == true && x.Name == goalName);

        /// <summary>
        /// Create new goal for a user
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="description">Description</param>
        /// <param name="userId">User Id</param>
        /// <param name="amount">Amount</param>
        /// <param name="isPercent">Is percent</param>
        /// <param name="expiredAt">expired at</param>
        /// <returns>GoalEntity</returns>
        public async Task<GoalEntity> CreateGoal(
            string name,
            string description,
            int userId,
            int amount,
            bool isPercent,
            DateTime expiredAt
        )
        {
            GoalEntity goal = new GoalEntity()
            {
                Name = name,
                Description = description,
                UserId = userId,
                Amount = amount,
                IsPercent = isPercent,
                Active = true,
                ExpireAt = expiredAt,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _CoreMainNodeDatabase.Goal.AddAsync(goal);
            await _CoreMainNodeDatabase.SaveChangesAsync();

            return goal;
        }
    }
}
