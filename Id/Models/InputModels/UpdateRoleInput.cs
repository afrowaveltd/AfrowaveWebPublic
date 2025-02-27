using System.ComponentModel.DataAnnotations;

namespace Id.Models.InputModels
{
	/// <summary>
	/// Input model for updating a role.
	/// </summary>
	public class UpdateRoleInput : IRoleInput
	{
		/// <summary>
		/// Gets or sets the role ID.
		/// </summary>
		[Required]
		public int RoleId { get; set; } = 0;

		/// <summary>
		/// Gets or sets the application ID.
		/// </summary>
		[Required]
		public string ApplicationId { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the role name.
		/// </summary>
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets a value indicating whether the role should be aligned to all users.
		/// </summary>
		public bool AllignToAll { get; set; } = false;

		/// <summary>
		/// Gets or sets a value indicating whether the role can administer roles.
		/// </summary>
		public bool CanAdministerRoles { get; set; } = false;
	}
}