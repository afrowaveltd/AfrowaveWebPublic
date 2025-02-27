namespace Id.Models.InputModels
{
	/// <summary>
	/// Input model for SMTP settings.
	/// </summary>
	public interface ISmtpInput
	{
		/// <summary>
		/// Gets or sets the application ID.
		/// </summary>
		string ApplicationId { get; set; }

		/// <summary>
		/// Gets or sets the SMTP host.
		/// </summary>
		string Host { get; set; }

		/// <summary>
		/// Gets or sets the SMTP port.
		/// </summary>
		int Port { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the SMTP server requires authorization.
		/// </summary>
		bool AuthorizationRequired { get; set; }

		/// <summary>
		/// Gets or sets the SMTP username.
		/// </summary>
		string? Username { get; set; }

		/// <summary>
		/// Gets or sets the SMTP password.
		/// </summary>
		string? Password { get; set; }

		/// <summary>
		/// Gets or sets the Sender email.
		/// </summary>
		string SenderEmail { get; set; }

		/// <summary>
		/// Gets or sets the Sender name.
		/// </summary>
		string SenderName { get; set; }

		/// <summary>
		/// Gets or sets the Secure Socket Layer option.
		/// </summary>
		MailKit.Security.SecureSocketOptions Secure { get; set; }
	}
}