using System.Text.Json.Serialization;

namespace NievoEasyFin.Application.Data.Entities.Cache
{
    /// <summary>
    /// Bank Type cache entity
    /// </summary>
    public class BankTypeCacheEntity : BankTypeEntity
    {
        /// <summary>
        /// Main constructor
        /// </summary>
        /// <param name="entity"></param>
        public BankTypeCacheEntity(BankTypeEntity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Description = entity.Description;
            Active = entity.Active;
            CreatedAt = entity.CreatedAt;
            UpdatedAt = entity.UpdatedAt;
        }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public BankTypeCacheEntity() { }
    }
}
