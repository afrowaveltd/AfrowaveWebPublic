namespace Id.Models.ResultModels
{
	public class RegisterApplicationUserResult
	{
		public bool Success { get; set; }
		public string? ErrorMessage { get; set; }
		public string? UserId { get; set; }
		public int ApplicationUserId { get; set; }
	}
}