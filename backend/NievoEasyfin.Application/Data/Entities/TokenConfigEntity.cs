using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NievoEasyfin.Application.Data.Entities
{
    [Table("token_config", Schema = "token_config")]
    public class TokenConfigEntity
    {
        [Key]
        [Column("id", TypeName = "SERIAL")]
        public int Id { get; set; }

        [Column("html_format", TypeName = "TEXT")]
        public string? HtmlFormat { get; set; }

        [Column("created_at", TypeName = "TIMESTAMP")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at", TypeName = "TIMESTAMP")]
        public DateTime? UpdatedAt { get; set; }
    }
}
