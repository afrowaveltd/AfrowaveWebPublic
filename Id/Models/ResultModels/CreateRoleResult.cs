namespace Id.Models.ResultModels
{
	public class CreateRoleResult
	{
		public bool Success { get; set; } = false;
		public int RoleId { get; set; } = 0;
		public List<string> Errors { get; set; } = [];
	}
}