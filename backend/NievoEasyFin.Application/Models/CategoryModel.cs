using NievoEasyFin.Application.Data.Context.Database;
using NievoEasyFin.Application.Data.Entities;
using Microsoft.EntityFrameworkCore;
using NievoEasyFin.Application.Data.Views;
using System.Text;
using Dapper;

namespace NievoEasyFin.Application.Models
{
    public class CategoryModel : CategoryEntity
    {
        private readonly CoreOrigin _CoreMainNodeDatabase;

        private readonly CoreReplica? _CoreReplicaNodeDatabase;

        public CategoryModel(CoreOrigin coreMainNodeDatabase, CoreReplica? coreReplicaNodeDatabase)
        {
            _CoreMainNodeDatabase = coreMainNodeDatabase;
            _CoreReplicaNodeDatabase = coreReplicaNodeDatabase;
        }

        /// <summary>
        /// Find a valid category by id
        /// </summary>
        /// <param name="categoryId">int</param>
        /// <returns>CategoryEntity</returns>
        public async Task<CategoryEntity> GetCategoryValidById(int categoryId)
            => await _CoreReplicaNodeDatabase.Category.FirstOrDefaultAsync(x => x.Id == categoryId && x.Active == true);

        /// <summary>
        /// Find a valid category by user and name
        /// </summary>
        /// <param name="name">int</param>
        /// <param name="userId">int</param>
        /// <returns>CategoryEntity</returns>
        public async Task<CategoryEntity> GetCategoryValidByNameAndUserId(string name, int userId)
            => await _CoreReplicaNodeDatabase.Category.FirstOrDefaultAsync(x => x.UserId == userId && x.Active == true && x.Name == name);

        /// <summary>
        /// Create a category
        /// </summary>
        /// <param name="name">str</param>
        /// <param name="description">str</param>
        /// <param name="userId">int</param>
        /// <param name="parentCategory">int</param>
        /// <param name="goal">int</param>
        /// <returns>CategoryEntity</returns>
        public async Task<CategoryEntity> CreateCategory(
            string name,
            string description,
            int userId,
            int? parentCategory,
            int? goal
        )
        {
            CategoryEntity category = new()
            {
                Name = name,
                Description = description,
                UserId = userId,
                ParentCategory = parentCategory,
                GoalId = goal,
                Active = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _CoreMainNodeDatabase.Category.AddAsync(category);
            await _CoreMainNodeDatabase.SaveChangesAsync();

            return category;
        }

        public async Task<(List<UserCategoryView>, int)> GetUserCategory(int userId, bool active, int page, int pageSize)
        {
            List<UserCategoryView> items = new();
            StringBuilder query = new();
            DynamicParameters param = new();

            query.Append(@"
                select 
                    c.id as Id,
                    c.name as Name,
                    c.description as Description,
                    c.active as Active,
                    c2.name as ParentCategoryName,
                    c2.description as ParentCategoryDescription,
                    c2.active as ParentCategoryActive,
                    g.name as GoalName,
                    g.description as GoalDescription,
                    c.created_at as CreatedAt,
                    c.updated_at as UpdatedAt,
                    count(*) over() as Records
                from goals.category c
                    inner join goals.goals g
                        on c.goal_id = g.id
                    left join goals.category c2
                        on c.id = c2.parent_category
                where g.user_id = @userId
                and g.active = @active
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
                await connection.QueryAsync<UserCategoryView>(
                    query.ToString(),
                    param
                )
            );

            return (items, items.Any() ? items.FirstOrDefault().Records : 0);
        }
    }
}
