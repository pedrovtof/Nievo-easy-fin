using NievoEasyFin.Application.Data.Context.Database;
using NievoEasyFin.Application.Data.Entities;
using Microsoft.EntityFrameworkCore;

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
            int goal
        )
        {
            CategoryEntity category = new()
            {
                Name = name,
                Description = description,
                UserId = userId,
                ParrentCategory = parentCategory,
                GoalId = goal,
                Active = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _CoreMainNodeDatabase.Category.AddAsync(category);
            await _CoreMainNodeDatabase.SaveChangesAsync();

            return category;
        }
    }
}
