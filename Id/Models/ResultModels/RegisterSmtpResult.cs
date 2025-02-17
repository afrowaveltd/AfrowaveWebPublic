namespace Id.Models.ResultModels
{
	public class RegisterSmtpResult
	{
		public bool Success { get; set; }
		public int ApplicationSmtpSettingsId { get; set; }
		public List<string> Errors { get; set; } = [];
	}
}