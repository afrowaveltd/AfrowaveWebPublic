using System.ComponentModel.DataAnnotations;

namespace Id.Models.InputModels
{
	public class UpdateSmtpInput : ISmtpInput
	{
		[Required]
		public int Id { get; set; }

		[Required]
		public string AppliationId { get; set; } = string.Empty;

		[Required]
		public string Host { get; set; } = string.Empty;

		public int Port { get; set; } = 25;
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