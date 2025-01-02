using MailKit.Security;
using System.ComponentModel.DataAnnotations;

namespace Id.Models.CommunicationModels
{
	public class SmtpSenderModel
	{
		public string Host { get; set; } = string.Empty;

		public int? Port { get; set; } = 0;

		public string? Username { get; set; }

		public string? Password { get; set; }

		[Required]
		public string SenderEmail { get; set; } = string.Empty;

		[Required]
		public string SenderName { get; set; } = string.Empty;

		public SecureSocketOptions Secure { get; set; } = SecureSocketOptions.Auto;
		public bool AuthorizationRequired { get; set; } = true;
	}
}