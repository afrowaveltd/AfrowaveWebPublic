namespace Id.Models.ResultModels
{
	/// <summary>
	/// Result of registering an application
	/// </summary>
	public class RegisterApplicationResult
	{
		/// <summary>
		/// Success of registering an application
		/// </summary>
		public bool ApplicationCreated { get; set; } = true;

		/// <summary>
		/// Application ID
		/// </summary>
		public string ApplicationId { get; set; } = string.Empty;

		/// <summary>
		/// Logo uploaded - true if logo uploaded, false if not
		/// </summary>
		public bool LogoUploaded { get; set; } = true;

		/// <summary>
		/// Error message
		/// </summary>
		public string ErrorMessage { get; set; } = string.Empty;
	}
}