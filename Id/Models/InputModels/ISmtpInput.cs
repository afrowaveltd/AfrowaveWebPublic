namespace Id.Models.InputModels
{
	public interface ISmtpInput
	{
		string AppliationId { get; set; }
		string Host { get; set; }
		int Port { get; set; }
		bool AuthorizationRequired { get; set; }
		string? Username { get; set; }
		string? Password { get; set; }
		string SenderEmail { get; set; }
		string SenderName { get; set; }
		MailKit.Security.SecureSocketOptions Secure { get; set; }
	}
}