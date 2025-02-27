namespace Id.Models.ResultModels
{
	/// <summary>
	/// Result of assigning a role
	/// </summary>
	public class RoleAssignResult
	{
		/// <summary>
		/// Role ID
		/// </summary>
		public int RoleId { get; set; } = 0;

		/// <summary>
		/// User ID
		/// </summary>
		public string UserId { get; set; } = string.Empty;

		/// <summary>
		/// Application user ID
		/// </summary>
		public int ApplicationUserId { get; set; } = 0;

		/// <summary>
		/// Application ID
		/// </summary>
		public string ApplicationId { get; set; } = string.Empty;

		/// <summary>
		/// Role name
		/// </summary>
		public string RoleName { get; set; } = string.Empty;

		/// <summary>
		/// Normalized role name
		/// </summary>
		public string NormalizedName { get; set; } = string.Empty;

		/// <summary>
		/// Success of assigning a role
		/// </summary>
		public bool Successful { get; set; } = true;

		/// <summary>
		/// Error message
		/// </summary>
		public string Message { get; set; } = string.Empty;
	}
}