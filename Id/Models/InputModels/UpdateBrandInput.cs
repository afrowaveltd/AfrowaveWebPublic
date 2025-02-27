namespace Id.Models.InputModels
{
	/// <summary>
	/// Input model for updating a brand.
	/// </summary>
	public class UpdateBrandInput : IBrandInput
	{
		/// <summary>
		/// Gets or sets the brand ID.
		/// </summary>
		public int BrandId { get; set; } = 0;

		/// <summary>
		/// Gets or sets the brand name.
		/// </summary>
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the brand description.
		/// </summary>
		public string? Description { get; set; }

		/// <summary>
		/// Gets or sets the brand icon.
		/// </summary>
		public IFormFile? Icon { get; set; }

		/// <summary>
		/// Gets or sets the brand website.
		/// </summary>
		public string? Website { get; set; }

		/// <summary>
		/// Gets or sets the brand email.
		/// </summary>
		public string? Email { get; set; }

		/// <summary>
		/// Gets or sets the brand owner ID.
		/// </summary>
		public string OwnerId { get; set; } = string.Empty;
	}
}