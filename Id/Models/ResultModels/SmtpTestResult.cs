namespace Id.Models.ResultModels
{
	/// <summary>
	/// Represents the result of an SMTP test operation.
	/// </summary>
	public class SmtpTestResult
	{
		/// <summary>
		/// Success of the SMTP test
		/// </summary>
		public bool Success { get; set; } = false;

		/// <summary>
		/// Log of the SMTP test
		/// </summary>
		public string Log { get; set; } = string.Empty;

		/// <summary>
		/// Error message
		/// </summary>
		public string? Error { get; set; }
	}
}