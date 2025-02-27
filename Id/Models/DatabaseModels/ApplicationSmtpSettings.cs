namespace Id.Models.DatabaseModels
{
	/// <summary>
	/// Represents the SMTP settings for an application.
	/// </summary>
	public class ApplicationSmtpSettings
	{
		/// <summary>
		/// Gets or sets the unique identifier for the SMTP settings.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the unique identifier for the application.
		/// </summary>
		public string ApplicationId { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the host name of the SMTP server.
		/// </summary>
		public string Host { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the port number of the SMTP server.
		/// </summary>
		public int Port { get; set; } = 0;

		/// <summary>
		/// Gets or sets the username for the SMTP server.
		/// </summary>
		public string? Username { get; set; }

		/// <summary>
		/// Gets or sets the password for the SMTP server.
		/// </summary>
		public string? Password { get; set; }

		/// <summary>
		/// Gets or sets the email address of the sender.
		/// </summary>
		public string SenderEmail { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the name of the sender.
		/// </summary>
		public string SenderName { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the secure socket options for the SMTP server.
		/// </summary>
		public MailKit.Security.SecureSocketOptions Secure { get; set; } = MailKit.Security.SecureSocketOptions.Auto;

		/// <summary>
		/// Gets or sets a value indicating whether authorization is required for the SMTP server.
		/// </summary>
		public bool AuthorizationRequired { get; set; } = true;

		/// <summary>
		/// Gets or sets the application associated with the SMTP settings.
		/// </summary>
		public Application? Application { get; set; }
	}
}