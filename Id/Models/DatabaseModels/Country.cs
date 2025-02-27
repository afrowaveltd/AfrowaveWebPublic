using System.ComponentModel.DataAnnotations;

namespace Id.Models.DatabaseModels
{
	/// <summary>
	/// Represents a country entity with its details.
	/// </summary>
	public class Country
	{
		/// <summary>
		/// Gets or sets the unique identifier of the country.
		/// </summary>
		[Key]
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the country.
		/// </summary>
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the dial code of the country.
		/// </summary>
		public string Dial_code { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the emoji of the country.
		/// </summary>
		public string Emoji { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the code of the country.
		/// </summary>
		public string Code { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the addresses associated with the country.
		/// </summary>
		public List<UserAddress> Addresses { get; set; } = new();
	}
}