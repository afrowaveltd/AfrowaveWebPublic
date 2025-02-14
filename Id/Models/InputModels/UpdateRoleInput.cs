using System.ComponentModel.DataAnnotations;

namespace Id.Models.InputModels
{
	public class UpdateRoleInput : IRoleInput
	{
		[Required]
		public int RoleId { get; set; } = 0;

		[Required]
		public string ApplicationId { get; set; } = string.Empty;

		public string Name { get; set; } = string.Empty;
		public bool AllignToAll { get; set; } = false;
		public bool CanAdministerRoles { get; set; } = false;
	}
}