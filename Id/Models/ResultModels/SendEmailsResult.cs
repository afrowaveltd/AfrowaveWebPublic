namespace Id.Models.ResultModels
{
	/// <summary>
	/// Represents the result of an email sending operation.
	/// </summary>
	public class SendEmailsResult
	{
		/// <summary>
		/// The ID of the sender application.
		/// </summary>
		public int SenderApplicationId { get; set; } = 0;

		/// <summary>
		/// The subject of the email.
		/// </summary>
		public string Subject { get; set; } = string.Empty;

		/// <summary>
		/// List of email results.
		/// </summary>
		/// <completionlist cref="EmailResult"/>
		public List<EmailResult> Results { get; set; } = [];
	}
}