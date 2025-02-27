namespace Id.Models.ResultModels
{
	/// <summary>
	/// Result of registering a user
	/// </summary>
	public class RegisterUserResult
	{
		/// <summary>
		/// User created - true if user created, false if not
		/// </summary>
		public bool UserCreated { get; set; } = true;

		/// <summary>
		/// User ID of the user created
		/// </summary>
		public string UserId { get; set; } = string.Empty;

		/// <summary>
		/// Profile picture uploaded - true if profile picture uploaded, false if not
		/// </summary>
		public bool ProfilePictureUploaded { get; set; } = true;

		/// <summary>
		/// List of errors
		/// </summary>
		public List<string> Errors { get; set; } = [];
	}
}