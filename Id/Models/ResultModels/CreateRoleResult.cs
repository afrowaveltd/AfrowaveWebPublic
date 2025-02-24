namespace Id.Models.ResultModels
{
	public class CreateRoleResult
	{
		public bool Success { get; set; } = false;
		public int RoleId { get; set; } = 0;
		public string RoleName { get; set; } = string.Empty;
		public string NormalizedName { get; set; } = string.Empty;
		public bool AllignToAll { get; set; } = false;
		public bool CanAdministerRoles { get; set; } = false;
		public List<string> Errors { get; set; } = [];
	}
}