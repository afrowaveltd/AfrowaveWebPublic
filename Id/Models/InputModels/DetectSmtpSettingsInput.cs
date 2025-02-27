namespace Id.Models.InputModels
{
	/// <summary>
	/// Input model for detecting SMTP settings.
	/// </summary>
	public class DetectSmtpSettingsInput
	{
		/// <summary>
		/// Gets or sets the SMTP host.
		/// </summary>
		public string Host { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the SMTP username.
		/// </summary>
		public string? Username { get; set; }

		/// <summary>
		/// Gets or sets the SMTP password.
		/// </summary>
		public string? Password { get; set; }

		/// <summary>
		/// Gets or sets the Sender email.
		/// </summary>
		public string SenderEmail { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the target email for testing message. If empty, the sender email will be used.
		/// </summary>
		public string? TargetAddress { get; set; }
	}
}