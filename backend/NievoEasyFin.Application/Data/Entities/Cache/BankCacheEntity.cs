using System.Text.Json.Serialization;

namespace NievoEasyFin.Application.Data.Entities.Cache
{
    /// <summary>
    /// Bank cache entity
    /// </summary>
    public class BankCacheEntity : BankEntity
    {
        /// <summary>
        /// Main constructor
        /// </summary>
        /// <param name="entity"></param>
        public BankCacheEntity(BankEntity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            BankType = entity.BankType;
            Active = entity.Active;
            CreatedAt = entity.CreatedAt;
            UpdatedAt = entity.UpdatedAt;
        }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public BankCacheEntity() { }
    }
}
