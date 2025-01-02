namespace Id.Models.DatabaseModels
{
	public class RefreshToken
	{
		public int Id { get; set; }
		public string UserId { get; set; } = string.Empty;

		public string Token { get; set; } = string.Empty;
		public DateTime Expires { get; set; } = DateTime.Now;
		public DateTime Created { get; set; } = DateTime.Now;
		public string CreatedByIp { get; set; } = string.Empty;
		public DateTime? Revoked { get; set; }
		public string? RevokedByIp { get; set; }
		public string? ReasonRevoked { get; set; }
		public string? ReplacedByToken { get; set; }
		public bool IsActive => Revoked == null && DateTime.UtcNow < Expires;
		public User? User { get; set; }
	}
}