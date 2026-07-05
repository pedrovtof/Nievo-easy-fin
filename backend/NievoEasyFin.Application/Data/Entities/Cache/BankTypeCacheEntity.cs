using System.Text.Json.Serialization;

namespace NievoEasyFin.Application.Data.Entities.Cache
{
    public class BankTypeCacheEntity : BankTypeEntity
    {
        public BankTypeCacheEntity(BankTypeEntity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Description = entity.Description;
            Active = entity.Active;
            CreatedAt = entity.CreatedAt;
            UpdatedAt = entity.UpdatedAt;
        }

        public BankTypeCacheEntity() { }
    }
}
