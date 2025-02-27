namespace Id.Models.DataViews
{
	/// <summary>
	/// View model for brand data.
	/// </summary>
	public class BrandView
	{
		/// <summary>
		/// Gets or sets the brand ID.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the brand name.
		/// </summary>
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the brand description.
		/// </summary>
		public string? Description { get; set; }

		/// <summary>
		/// Gets or sets the path to the brand logo.
		/// </summary>
		public string? LogoPath { get; set; }

		/// <summary>
		/// Gets or sets the brand website.
		/// </summary>
		public string? Website { get; set; }

		/// <summary>
		/// Gets or sets the brand email.
		/// </summary>
		public string? Email { get; set; }

		/// <summary>
		/// Gets or sets the brand owner name.
		/// </summary>
		public string OwnerName { get; set; } = string.Empty;
	}
}