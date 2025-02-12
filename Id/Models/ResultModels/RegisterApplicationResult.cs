namespace Id.Models.ResultModels
{
	public class RegisterApplicationResult
	{
		public bool ApplicationCreated { get; set; } = true;
		public string ApplicationId { get; set; } = string.Empty;
		public bool LogoUploaded { get; set; } = true;
		public string ErrorMessage { get; set; } = string.Empty;
	}
}