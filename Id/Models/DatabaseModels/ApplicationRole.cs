namespace Id.Models.DatabaseModels
{
	/// <summary>
	/// Represents an application role entity with its settings and policies.
	/// </summary>
	public class ApplicationRole
	{
		/// <summary>
		/// Gets or sets the unique identifier for the application role.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		///	Gets or sets the unique identifier for the application.
		/// </summary>
		public string ApplicationId { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the name of the application role.
		/// </summary>
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the normalized name of the application role.
		/// </summary>
		public string NormalizedName { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the assigning of the application role to all users.
		/// </summary>
		public bool AllignToAll { get; set; } = false;

		/// <summary>
		/// Gets or sets a value indicating whether the application role can administer roles.
		/// </summary>
		public bool CanAdministerRoles { get; set; } = false;

		/// <summary>
		/// Gets or sets the application associated with the application role.
		/// </summary>
		public Application? Application { get; set; }

		/// <summary>
		/// Gets or sets roles associated with the application role.
		/// </summary>
		public List<UserRole> Roles { get; set; } = [];
	}
}