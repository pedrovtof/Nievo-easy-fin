using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace auth.Models
{
    public class UserModels
    {

        [Table("user", Schema = "user_details")]
        public class User
        {
            [Key]
            [Column("id", TypeName = "SERIAL")]
            public int Id { get; set; }

            [Column("user_type", TypeName = "INT")]
            public int UserType { get; set; }

            [Column("name", TypeName = "VARCHAR(255)")]
            public string Name { get; set; }

            [Column("email", TypeName = "VARCHAR(100)")]
            public string Email { get; set; }

            [Column("country_code", TypeName = "VARCHAR(5)")]
            public string CountryCode { get; set; }

            [Column("ddd", TypeName = "VARCHAR(5)")]
            public string Ddd { get; set; }

            [Column("phone", TypeName = "VARCHAR(15)")]
            public string Phone { get; set; }

            [Column("status_id", TypeName = "INT")]
            public int StatusId { get; set; }

            [Column("created_at", TypeName = "TIMESTAMP")]
            public DateTime CreatedAt { get; set; }

            [Column("updated_at", TypeName = "TIMESTAMP")]
            public DateTime UpdatedAt { get; set; }
        }

        [Table("user_status", Schema = "user_details")]
        public class UserStatus
        {
            [Key]
            [Column("id", TypeName = "SERIAL")]
            public int Id { get; set; }

            [Column("name", TypeName = "VARCHAR(255)")]
            public string Name { get; set; }

            [Column("description", TypeName = "VARCHAR(255)")]
            public string Description { get; set; }

            [Column("created_at", TypeName = "TIMESTAMP")]
            public DateTime CreatedAt { get; set; }

            [Column("updated_at", TypeName = "TIMESTAMP")]
            public DateTime UpdatedAt { get; set; }

            [Column("active", TypeName = "BOOLEAN")]
            public bool Active { get; set; }
        }

        [Table("user_type", Schema = "user_details")]
        public class UserType
        {
            [Key]
            [Column("id", TypeName = "SERIAL")]
            public int Id { get; set; }

            [Column("name", TypeName = "VARCHAR(255)")]
            public string Name { get; set; }

            [Column("description", TypeName = "VARCHAR(255)")]
            public string Description { get; set; }

            [Column("created_at", TypeName = "TIMESTAMP")]
            public DateTime CreatedAt { get; set; }

            [Column("updated_at", TypeName = "TIMESTAMP")]
            public DateTime UpdatedAt { get; set; }

        }

        [Table("user_password_history", Schema = "user_details")]
        public class UserPasswordHistory
        {
            [Key]
            [Column("id", TypeName = "SERIAL")]
            public int Id { get; set; }

            [Column("user_id", TypeName = "INT")]
            public int UserId { get; set; }

            [Column("active", TypeName = "BOOLEAN")]
            public bool Active { get; set; }

            [Column("value", TypeName = "TEXT")]
            public string Value { get; set; }

            [Column("created_at", TypeName = "TIMESTAMP")]
            public DateTime CreatedAt { get; set; }

            [Column("updated_at", TypeName = "TIMESTAMP")]
            public DateTime UpdatedAt { get; set; }
        }


    }
}