using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace auth.Data
{
    [Table("user_type", Schema = "user_details")]
    public class UserTypeData
    {
        [Key]
        [Column("id", TypeName = "SERIAL")]
        public int Id { get; set; }

        [Column("name", TypeName = "VARCHAR(255)")]
        public string? Name { get; set; }

        [Column("description", TypeName = "VARCHAR(255)")]
        public string Description { get; set; } = String.Empty;

        [Column("created_at", TypeName = "TIMESTAMP")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at", TypeName = "TIMESTAMP")]
        public DateTime? UpdatedAt { get; set; }

    }


}