namespace Id.Models.InputModels
{
	/// <summary>
	/// Input model for creating a brand.
	/// </summary>
	public interface IBrandInput
	{
		/// <summary>
		/// Gets or sets the brand name.
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Gets or sets the brand description.
		/// </summary>
		string? Description { get; set; }

		/// <summary>
		/// Gets or sets the brand icon.
		/// </summary>
		IFormFile? Icon { get; set; }

		/// <summary>
		/// Gets or sets the brand website.
		/// </summary>
		string? Website { get; set; }

		/// <summary>
		/// Gets or sets the brand email.
		/// </summary>
		string? Email { get; set; }

		/// <summary>
		/// Gets or sets the brand owner ID.
		/// </summary>
		string OwnerId { get; set; }
	}
}