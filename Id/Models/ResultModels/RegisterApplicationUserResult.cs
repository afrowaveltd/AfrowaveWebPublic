namespace Id.Models.ResultModels
{
	/// <summary>
	/// Result of registering an application user
	/// </summary>
	public class RegisterApplicationUserResult
	{
		/// <summary>
		/// Success of registering an application user
		/// </summary>
		public bool Success { get; set; }

		/// <summary>
		/// Error message
		/// </summary>
		public string? ErrorMessage { get; set; }

		/// <summary>
		/// User ID
		/// </summary>
		public string? UserId { get; set; }

		/// <summary>
		/// Application user ID
		/// </summary>
		public int ApplicationUserId { get; set; }
	}
}