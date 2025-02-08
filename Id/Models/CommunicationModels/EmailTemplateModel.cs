namespace Id.Models.CommunicationModels
{
	public class EmailTemplateModel
	{
		public string HeaderText { get; set; } = string.Empty;
		public string ActionUrl { get; set; } = string.Empty; // Pro reset hesla
		public string OtpCode { get; set; } = string.Empty;  // Pro OTP
		public string Subject { get; set; } = string.Empty;
	}
}