namespace Id.Models.DatabaseModels
{
	public class ApplicationSmtpSettings
	{
		public int Id { get; set; }
		public string ApplicationId { get; set; } = string.Empty;
		public string Host { get; set; } = string.Empty;
		public int Port { get; set; } = 0;
		public string Username { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public string SenderEmail { get; set; } = string.Empty;
		public string SenderName { get; set; } = string.Empty;
		public MailKit.Security.SecureSocketOptions Secure { get; set; } = MailKit.Security.SecureSocketOptions.Auto;
		public bool AuthorizationRequired { get; set; } = true;
		public Application? Application { get; set; }
	}
}