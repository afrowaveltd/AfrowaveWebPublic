using System.ComponentModel.DataAnnotations;

namespace Id.Models.InputModels
{
	public class RegisterSmtpInput : ISmtpInput
	{
		[Required]
		public string ApplicationId { get; set; } = string.Empty;

		[Required]
		public string Host { get; set; } = string.Empty;

		public int Port { get; set; }
		public bool AuthorizationRequired { get; set; } = true;
		public string? Username { get; set; }
		public string? Password { get; set; }

		[Required]
		public string SenderEmail { get; set; } = string.Empty;

		[Required]
		public string SenderName { get; set; } = string.Empty;

		public MailKit.Security.SecureSocketOptions Secure { get; set; } = MailKit.Security.SecureSocketOptions.Auto;
	}
}