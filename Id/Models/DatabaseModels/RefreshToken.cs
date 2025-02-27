namespace Id.Models.DatabaseModels
{
	/// <summary>
	/// Represents a refresh token entity with its details.
	/// </summary>
	public class RefreshToken
	{
		/// <summary>
		/// Gets or sets the unique identifier of the refresh token.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the identifier of the user.
		/// </summary>
		public string UserId { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the token value.
		/// </summary>
		public string Token { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the expiration date of the token.
		/// </summary>
		public DateTime Expires { get; set; } = DateTime.Now;

		/// <summary>
		/// Gets or sets the date the token was created.
		/// </summary>
		public DateTime Created { get; set; } = DateTime.Now;

		/// <summary>
		/// Gets or sets the IP address the token was created from.
		/// </summary>
		public string CreatedByIp { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the date the token was revoked.
		/// </summary>
		public DateTime? Revoked { get; set; }

		/// <summary>
		/// Gets or sets the IP address the token was revoked from.
		/// </summary>
		public string? RevokedByIp { get; set; }

		/// <summary>
		/// Gets or sets the reason the token was revoked.
		/// </summary>
		public string? ReasonRevoked { get; set; }

		/// <summary>
		/// Gets or sets the token that replaced this token.
		/// </summary>
		public string? ReplacedByToken { get; set; }

		/// <summary>
		/// Gets a value indicating whether the token is active.
		/// </summary>
		public bool IsActive => Revoked == null && DateTime.UtcNow < Expires;

		/// <summary>
		/// Gets or sets the user associated with the refresh token.
		/// </summary>
		public User? User { get; set; }
	}
}