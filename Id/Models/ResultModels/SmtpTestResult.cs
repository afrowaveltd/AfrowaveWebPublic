namespace Id.Models.ResultModels
{
	public class SmtpTestResult
	{
		public bool Success { get; set; } = false;
		public string Log { get; set; } = string.Empty;
		public string? Error { get; set; }
	}
}