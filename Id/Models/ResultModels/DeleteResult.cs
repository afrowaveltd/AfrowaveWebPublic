namespace Id.Models.ResultModels
{
	/// <summary>
	/// Represents the result of a delete operation.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class DeleteResult<T>

	{
		/// <summary>
		/// Indicates whether the delete operation was successful.
		/// </summary>
		public bool Success { get; set; } = true;

		/// <summary>
		/// The ID of the item that was deleted.
		/// </summary>
		public T? DeletedId { get; set; }

		/// <summary>
		/// The error message if the delete operation was not successful.
		/// </summary>
		public string? ErrorMessage { get; set; }
	}
}