using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NievoEasyfin.Application.Data.Entities
{
    [Table("user", Schema = "user_details")]
    public class UserEntity
    {
        [Key]
        [Column("id", TypeName = "SERIAL")]
        public int Id { get; set; }

        [Column("name", TypeName = "VARCHAR(255)")]
        public string? Name { get; set; }

        [Column("email", TypeName = "VARCHAR(100)")]
        public string? Email { get; set; }

        [Column("phone", TypeName = "INT")]
        public int? Phone { get; set; }

        [Column("status_id", TypeName = "INT")]
        public int? StatusId { get; set; }

        [Column("password", TypeName = "TEXT")]
        public string? Password { get; set; }

        [Column("created_at", TypeName = "TIMESTAMP")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at", TypeName = "TIMESTAMP")]
        public DateTime? UpdatedAt { get; set; }
    }
}