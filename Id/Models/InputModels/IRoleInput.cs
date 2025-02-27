namespace Id.Models.InputModels
{
	/// <summary>
	/// Input model for creating a role.
	/// </summary>
	public interface IRoleInput
	{
		/// <summary>
		/// Gets or sets the application ID.
		/// </summary>
		string ApplicationId { get; set; }

		/// <summary>
		/// Gets or sets the role name.
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the role should be aligned to all users.
		/// </summary>
		bool AllignToAll { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the role can administer roles.
		/// </summary>
		bool CanAdministerRoles { get; set; }
	}
}