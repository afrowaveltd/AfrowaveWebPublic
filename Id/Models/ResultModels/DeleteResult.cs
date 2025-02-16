namespace Id.Models.ResultModels
{
	public class DeleteResult<T>

	{
		public bool Success { get; set; } = true;
		public T? DeletedId { get; set; }
		public string? ErrorMessage { get; set; }
	}
}