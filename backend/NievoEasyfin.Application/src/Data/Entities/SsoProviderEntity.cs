using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Objects.DataClasses;

namespace NievoEasyfin.Application.Data.Entities
{
    [Table("sso_provider", Schema = "journey")]
    public class SsoProviderEntity
    {
        [Key]
        [Column("id", TypeName = "SERIAL")]
        public int Id { get; set; }

        [Column("active", TypeName = "boolean")]
        public bool Active { get; set; }

        [Column("name", TypeName = "VARCHAR")]
        public string? Name { get; set; }

        [Column("description", TypeName = "VARCHAR")]
        public string? Description { get; set; }

        [Column("created_at", TypeName = "TIMESTAMP")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at", TypeName = "TIMESTAMP")]
        public DateTime? UpdatedAt { get; set; }
    }
}