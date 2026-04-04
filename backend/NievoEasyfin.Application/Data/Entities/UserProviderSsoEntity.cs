using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NievoEasyfin.Application.Data.Entities
{
    [Table("user_provider_sso", Schema = "journey")]
    public class UserProviderSsoEntity
    {
        [Key]
        [Column("id", TypeName = "SERIAL")]
        public int? Id { get; set; }

        [Column("sso_provider_id", TypeName = "INT")]
        public int? SsoProviderId { get; set; }

        [Column("user_id", TypeName = "INT")]
        public int? UserId { get; set; }

        [Column("sub", TypeName = "VARCHAR(250)")]
        public string? Sub { get; set; }

        [Column("created_at", TypeName = "TIMESTAMP")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at", TypeName = "TIMESTAMP")]
        public DateTime? UpdatedAt { get; set; }
    }
}
