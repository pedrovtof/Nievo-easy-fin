using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NievoEasyfin.Application.Data.Entities
{
    [Table("user_password_history", Schema = "user_details")]
    public class UserPasswordHistoryData
    {
        [Key]
        [Column("id", TypeName = "SERIAL")]
        public int Id { get; set; }

        [Column("user_id", TypeName = "INT")]
        public int? UserId { get; set; }

        [Column("active", TypeName = "BOOLEAN")]
        public bool? Active { get; set; }

        [Column("value", TypeName = "TEXT")]
        public string? Value { get; set; }

        [Column("created_at", TypeName = "TIMESTAMP")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at", TypeName = "TIMESTAMP")]
        public DateTime? UpdatedAt { get; set; }
    }
}