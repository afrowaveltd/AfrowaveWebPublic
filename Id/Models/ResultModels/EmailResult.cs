namespace Id.Models.ResultModels
{
	public class EmailResult
	{
		public string TargetEmail { get; set; } = string.Empty;
		public bool Success { get; set; } = false;
		public string Subject { get; set; } = string.Empty;
		public string? ErrorMessage { get; set; }
	}
}