using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NievoEasyFin.Application.Data.Entities
{
    /// <summary>
    /// Class data bank
    /// </summary>
    [Table("user_bank_card", Schema = "accounts")]
    public class UserBankCardEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        [Key]
        [Column("id", TypeName = "SERIAL")]
        public int Id { get; set; }

        /// <summary>
        /// Bank id
        /// </summary>
        [JsonPropertyName("bank_id")]
        [Column("bank_id", TypeName = "INT")]
        public int? BankId { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [JsonPropertyName("name")]
        [Column("name", TypeName = "VARCHAR(150)")]
        public string? Name { get; set; }

        /// <summary>
        /// Card id
        /// </summary>
        [JsonPropertyName("card_id")]
        [Column("card_id", TypeName = "INT")]
        public int? CardId { get; set; }

        /// <summary>
        /// Active
        /// </summary>
        [JsonPropertyName("active")]
        [Column("active", TypeName = "BOOLEAN")]
        public bool Active { get; set; }

        /// <summary>
        /// User id
        /// </summary>
        [JsonPropertyName("user_id")]
        [Column("user_id", TypeName = "INT")]
        public int? UserId { get; set; }

        /// <summary>
        /// Expired at
        /// </summary>
        [JsonPropertyName("expired_at")]
        [Column("expired_at", TypeName = "TIMESTAMP")]
        public DateTime? ExpiredAt { get; set; }

        /// <summary>
        /// Created at
        /// </summary>
        [JsonPropertyName("created_at")]
        [Column("created_at", TypeName = "TIMESTAMP")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Updated at
        /// </summary>
        [JsonPropertyName("updated_at")]
        [Column("updated_at", TypeName = "TIMESTAMP")]
        public DateTime? UpdatedAt { get; set; }
    }
}
