using System.ComponentModel.DataAnnotations;

namespace Id.Models.DatabaseModels
{
	public class UserAddress
	{
		public int Id { get; set; }
		public string UserId { get; set; } = string.Empty;

		[Required]
		public string StreetAddress { get; set; } = string.Empty;

		public string? StreetAddress2 { get; set; } = string.Empty;

		[Required]
		public string City { get; set; } = string.Empty;

		[Required]
		public string PostCode { get; set; } = string.Empty;

		public int? CountryId { get; set; }

		public User? User { get; set; }
		public Country? Country { get; set; }
	}
}