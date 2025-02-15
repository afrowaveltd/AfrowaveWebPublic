namespace Id.Models.ResultModels
{
	public class DeleteResult
	{
		public bool Success { get; set; } = true;
		public int RoleId { get; set; } = 0;
		public string? ErrorMessage { get; set; }
	}
}