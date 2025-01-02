namespace Id.Models.CommunicationModels
{
	public class SmtpTestResponse
	{
		public bool Success { get; set; }
		public SmtpTestErrorType ErrorType { get; set; }
		public string? ErrorText { get; set; }
		public string Log { get; set; } = string.Empty;
	}

	public enum SmtpTestErrorType
	{
		None,
		AuthenticationFailed,
		ConnectionFailed,
		Timeout,
		InvalidRecipient,
		Other
	}
}