using System.ComponentModel.DataAnnotations;

namespace Id.Models.DatabaseModels
{
	/// <summary>
	/// Represents a user address entity with its details.
	/// </summary>
	public class UserAddress
	{
		/// <summary>
		/// Gets or sets the unique identifier of the user address.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the identifier of the user.
		/// </summary>
		public string UserId { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the street address of the user.
		/// </summary>
		[Required]
		public string StreetAddress { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the second street address of the user.
		/// </summary>
		public string? StreetAddress2 { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the city of the user.
		/// </summary>
		[Required]
		public string City { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets a postal code of the user.
		/// </summary>
		[Required]
		public string PostCode { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the identifier of the country.
		/// </summary>
		public int? CountryId { get; set; }

		/// <summary>
		/// Gets or sets the user associated with the address.
		/// </summary>
		public User? User { get; set; }

		/// <summary>
		/// Gets or sets the country associated with the address.
		/// </summary>
		public Country? Country { get; set; }
	}
}