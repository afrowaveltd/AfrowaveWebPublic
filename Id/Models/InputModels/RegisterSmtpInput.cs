using System.ComponentModel.DataAnnotations;

namespace Id.Models.InputModels
{
	/// <summary>
	/// Input model for registering SMTP settings.
	/// </summary>
	public class RegisterSmtpInput : ISmtpInput
	{
		/// <summary>
		/// Gets or sets the application ID.
		/// </summary>
		[Required]
		public string ApplicationId { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the SMTP host.
		/// </summary>
		[Required]
		public string Host { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the SMTP port.
		/// </summary>
		public int Port { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the SMTP server requires authorization.
		/// </summary>
		public bool AuthorizationRequired { get; set; } = true;

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

		[Required]
		public string SenderEmail { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the Sender name.
		/// </summary>

		[Required]
		public string SenderName { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the Secure Socket Layer option.
		/// </summary>
		public MailKit.Security.SecureSocketOptions Secure { get; set; } = MailKit.Security.SecureSocketOptions.Auto;
	}
}