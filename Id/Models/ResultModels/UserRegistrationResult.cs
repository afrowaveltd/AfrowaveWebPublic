namespace Id.Models.ResultModels
{
	/// <summary>
	/// Represents the result of a user registration operation.
	/// </summary>
	public class UserRegistrationResult
	{
		/// <summary>
		/// Success of the user registration operation
		/// </summary>
		public bool Success { get; set; } = true;

		/// <summary>
		/// Errors of the user registration operation
		/// </summary>
		public List<string> Errors { get; set; } = [];

		/// <summary>
		/// User ID of the user created
		/// </summary>
		public string? UserId { get; set; }

		/// <summary>
		/// True if the confirmation email was sent, false if not
		/// </summary>
		public bool EmailSent { get; set; } = false;

		/// <summary>
		/// True if the profile picture was uploaded, false if not
		/// </summary>
		public bool PictureUploaded { get; set; } = false;
	}
}