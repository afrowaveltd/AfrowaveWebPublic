namespace Id.Models.ResultModels
{
	/// <summary>
	/// Represents the result of an update operation.
	/// </summary>
	public class UpdateResult
	{
		/// <summary>
		/// Success of the update operation
		/// </summary>
		public bool Success { get; set; } = true;

		/// <summary>
		/// Errors of the update operation
		/// </summary>
		public List<string> Errors { get; set; } = [];

		/// <summary>
		/// Dictionary of updated values
		/// </summary>
		public Dictionary<string, string> UpdatedValues { get; set; } = [];
	}
}