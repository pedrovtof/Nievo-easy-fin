using NievoEasyFin.Application.Data.Context.Database;
using NievoEasyFin.Application.Data.Entities;
using Microsoft.EntityFrameworkCore;
using NievoEasyFin.Application.Data.Views;
using System.Text;
using Dapper;

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

        public async Task<(List<UserGoalView>, int)> GetUserGoal(int userId, bool active, int page, int pageSize)
        {
            List<UserGoalView> items = new();
            StringBuilder query = new();
            DynamicParameters param = new();

            query.Append(@"
                select 
                    g.id as Id,
                    g.name as Name,
                    g.description as Description,
                    g.active as Active,
                    g.amount as Amount,
                    g.is_percent as IsPercent,
                    g.expire_at as ExpireAt,
                    g.created_at as CreatedAt,
                    g.updated_at as UpdatedAt,
                    count(*) over() as Records
                from goals.goals g
                where g.user_id = @userId
                and g.active  = @active
            ");

            param.Add("userId", userId);
            param.Add("active", active);

            query.Append(@"
                limit @limit
                offset @offset
            ");

            param.Add("limit", pageSize);
            param.Add("offset", (page - 1) * pageSize);

            var connection = _CoreReplicaNodeDatabase.Database.GetDbConnection();
            items.AddRange(
                await connection.QueryAsync<UserGoalView>(
                    query.ToString(),
                    param
                )
            );

            return (items, items.Any() ? items.FirstOrDefault().Records : 0);
        }
    }
}
