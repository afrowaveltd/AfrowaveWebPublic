using System.ComponentModel.DataAnnotations;

namespace Id.Models.InputModels
{
	public class CreateRoleInput : IRoleInput
	{
		[Required]
		public string ApplicationId { get; set; } = string.Empty;

		[Required]
		public string Name { get; set; } = string.Empty;

		public bool AllignToAll { get; set; } = false;
		public bool CanAdministerRoles { get; set; } = false;
	}
}