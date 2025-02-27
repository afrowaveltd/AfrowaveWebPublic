namespace Id.Models.ResultModels
{
	/// <summary>
	/// Result of checking input
	/// </summary>
	public class CheckInputResult
	{
		/// <summary>
		/// Success of checking input
		/// </summary>
		public bool Success { get; set; } = true;

		/// <summary>
		/// Errors of checking input
		/// </summary>
		public List<string> Errors { get; set; } = [];
	}
}