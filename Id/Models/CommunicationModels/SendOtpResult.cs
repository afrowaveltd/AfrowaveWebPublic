namespace Id.Models.CommunicationModels
{
	public class SendOtpResult
	{
		public bool Success { get; set; }
		public string OtpCode { get; set; } = string.Empty;
	}
}