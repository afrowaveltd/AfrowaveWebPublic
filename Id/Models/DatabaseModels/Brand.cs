namespace Id.Models.DatabaseModels
{
	/// <summary>
	/// Represents a brand entity with its applications and owner.
	/// </summary>
	public class Brand
	{
		/// <summary>
		/// Gets or sets the unique identifier of the brand.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the brand.
		/// </summary>
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets a value indicating whether the brand has a logo.
		/// </summary>
		public bool Logo { get; set; } = false;

		/// <summary>
		/// Gets or sets the website URL of the brand.
		/// </summary>
		public string? Website { get; set; }

		/// <summary>
		/// Gets or sets a brief description of the brand.
		/// </summary>
		public string? Description { get; set; }

		/// <summary>
		/// Gets or sets the contact email for the brand.
		/// </summary>
		public string? Email { get; set; }

		/// <summary>
		/// Gets or sets the identifier of the owner of the brand.
		/// </summary>
		public string OwnerId { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the owner of the brand.
		/// </summary>
		public User? Owner { get; set; }

		/// <summary>
		/// Gets or sets the applications associated with the brand.
		/// </summary>
		public List<Application> Applications { get; set; } = [];
	}
}