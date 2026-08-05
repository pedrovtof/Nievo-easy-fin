using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NievoEasyFin.Application.Data.Entities;

[Table("users_accepted_terms", Schema = "journey")]
public class UsersAcceptedTermsEntity
{
	[Key]
	[Column("id", TypeName = "SERIAL")]
	public int Id { get; set; }

	[Column("user_id", TypeName = "INT")]
	public int? UserId { get; set; }

	[Column("accept_id", TypeName = "INT")]
	public int? AcceptId { get; set; }

	[Column("accepted", TypeName = "BOOL")]
	public bool? Accepted { get; set; }

	[Column("request_details", TypeName = "json")]
	public object RequestDetails { get; set; }

	[Column("created_at", TypeName = "TIMESTAMP")]
	public DateTime? CreatedAt { get; set; }

	[Column("updated_at", TypeName = "TIMESTAMP")]
	public DateTime? UpdatedAt { get; set; }
}
