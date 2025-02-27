namespace Id.Models.DatabaseModels
{
	/// <summary>
	/// Represents a user role entity with its settings and policies.
	/// </summary>
	public class UserRole
	{
		/// <summary>
		/// Gets or sets the unique identifier for the user role.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the unique identifier for the application role.
		/// </summary>
		public int ApplicationRoleId { get; set; }

		/// <summary>
		/// Gets or sets the unique identifier for the user.
		/// </summary>
		public int ApplicationUserId { get; set; } = 0;

		/// <summary>
		/// Gets or sets the date and time the user was added to the role.
		/// </summary>
		public DateTime AddedToRole { get; set; } = DateTime.Now;

		/// <summary>
		/// Gets or sets the user associated with the role.
		/// </summary>
		public ApplicationUser? User { get; set; }

		/// <summary>
		/// Gets or sets the role associated with the user.
		/// </summary>
		public ApplicationRole? ApplicationRole { get; set; }
	}
}