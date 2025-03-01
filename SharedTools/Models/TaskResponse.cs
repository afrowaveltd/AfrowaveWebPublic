namespace SharedTools.Models
{
	/// <summary>
	/// TaskResponse class is a class that is used to return a response from a task.
	/// </summary>
	public class TaskResponse
	{
		/// <summary>
		/// Successful is a boolean that indicates if the operation was successful.
		/// </summary>
		public bool Successful { get; set; } = true;

		/// <summary>
		/// List of errors that occurred during the operation.
		/// </summary>
		public List<string>? Errors { get; set; }
	}
}