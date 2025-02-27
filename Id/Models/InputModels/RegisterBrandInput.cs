using System.ComponentModel.DataAnnotations;

namespace Id.Models.InputModels
{
	/// <summary>
	/// Input model for registering a brand.
	/// </summary>
	public class RegisterBrandInput : IBrandInput
	{
		/// <summary>
		/// Gets or sets the brand name.
		/// </summary>
		[Required]
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
		[Required]
		public string OwnerId { get; set; } = string.Empty;
	}
}