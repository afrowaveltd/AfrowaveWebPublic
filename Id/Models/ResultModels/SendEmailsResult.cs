namespace Id.Models.ResultModels
{
	public class SendEmailsResult
	{
		public int SenderApplicationId { get; set; } = 0;
		public string Subject { get; set; } = string.Empty;
		public List<EmailResult> Results { get; set; } = new List<EmailResult>();
	}
}