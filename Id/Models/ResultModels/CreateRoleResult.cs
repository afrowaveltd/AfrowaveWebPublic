namespace Id.Models.ResultModels
{
	/// <summary>
	/// Result model for creating a role.
	/// </summary>
	public class CreateRoleResult
	{
		/// <summary>
		/// Gets or sets a value indicating whether the operation was successful.
		/// </summary>
		public bool Success { get; set; } = false;

		/// <summary>
		/// Gets or sets the role ID.
		/// </summary>
		public int RoleId { get; set; } = 0;

		/// <summary>
		/// Gets or sets the role name.
		/// </summary>
		public string RoleName { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the normalized role name.
		/// </summary>
		public string NormalizedName { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets a value indicating whether the role should be aligned to all users.
		/// </summary>
		public bool AllignToAll { get; set; } = false;

		/// <summary>
		/// Gets or sets a value indicating whether the role can administer roles of application users.
		/// </summary>
		public bool CanAdministerRoles { get; set; } = false;

		/// <summary>
		/// List of errors found
		/// </summary>
		public List<string> Errors { get; set; } = [];
	}
}