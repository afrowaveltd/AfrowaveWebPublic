using MailKit.Security;
using System.ComponentModel.DataAnnotations;

namespace Id.Models.CommunicationModels
{
	/// <summary>
	/// Smtp sender model.
	/// </summary>
	public class SmtpSenderModel
	{
		/// <summary>
		/// Gets or sets the host.
		/// </summary>
		public string Host { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the port.
		/// </summary>
		public int? Port { get; set; } = 0;

		/// <summary>
		/// Gets or sets the username.
		/// </summary>
		public string? Username { get; set; }

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		public string? Password { get; set; }

		/// <summary>
		/// Gets or sets the sender email.
		/// </summary>
		[Required]
		public string SenderEmail { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the sender name.
		/// </summary>
		[Required]
		public string SenderName { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the secure socket options.
		/// </summary>
		public SecureSocketOptions Secure { get; set; } = SecureSocketOptions.Auto;

		/// <summary>
		/// Gets or sets a value indicating whether authorization required.
		/// </summary>
		public bool AuthorizationRequired { get; set; } = true;

		/// <summary>
		/// Gets or sets the target for testing.
		/// </summary>
		public string? TargetForTesting { get; set; }
	}
}