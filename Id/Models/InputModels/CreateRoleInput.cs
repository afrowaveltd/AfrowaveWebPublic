using System.ComponentModel.DataAnnotations;

namespace Id.Models.InputModels
{
	/// <summary>
	/// Input model for creating a role.
	/// </summary>
	public class CreateRoleInput : IRoleInput
	{
		/// <summary>
		/// Gets or sets the application ID.
		/// </summary>
		[Required]
		public string ApplicationId { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the role name.
		/// </summary>
		[Required]
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