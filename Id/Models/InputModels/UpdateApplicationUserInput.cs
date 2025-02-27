namespace Id.Models.InputModels
{
	/// <summary>
	/// Input model for updating an application user.
	/// </summary>
	public class UpdateApplicationUserInput
	{
		/// <summary>
		/// Gets or sets the application user ID.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the user description.
		/// </summary>
		public string? UserDescription { get; set; }
	}
}