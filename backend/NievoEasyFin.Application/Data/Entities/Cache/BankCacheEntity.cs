using System.Text.Json.Serialization;

namespace NievoEasyFin.Application.Data.Entities.Cache
{
    public class BankCacheEntity : BankEntity
    {
        public BankCacheEntity(BankEntity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            BankType = entity.BankType;
            Active = entity.Active;
            CreatedAt = entity.CreatedAt;
            UpdatedAt = entity.UpdatedAt;
        }

        public BankCacheEntity() { }
    }
}
