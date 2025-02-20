namespace Id.Models.InputModels
{
	public class DetectSmtpSettingsInput
	{
		public string Host { get; set; } = string.Empty;
		public string? Username { get; set; }
		public string? Password { get; set; }
		public string SenderEmail { get; set; } = string.Empty;
		public string? TargetAddress { get; set; }
	}
}