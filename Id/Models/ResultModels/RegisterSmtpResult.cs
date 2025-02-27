namespace Id.Models.ResultModels
{
	/// <summary>
	/// Result of registering an SMTP
	/// </summary>
	public class RegisterSmtpResult
	{
		/// <summary>
		/// Success of registering an SMTP
		/// </summary>
		public bool Success { get; set; }

		/// <summary>
		/// Application SMTP settings ID
		/// </summary>
		public int ApplicationSmtpSettingsId { get; set; }

		/// <summary>
		/// Errors of registering an SMTP
		/// </summary>
		public List<string> Errors { get; set; } = [];
	}
}