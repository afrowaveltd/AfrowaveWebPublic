using System.ComponentModel.DataAnnotations;

namespace Id.Models.InputModels
{
	public class RegisterBrandInput
	{
		[Required]
		public string Name { get; set; } = string.Empty;

		public string? Description { get; set; }
		public IFormFile? Icon { get; set; }
		public string? Website { get; set; }
		public string? Email { get; set; }

		[Required]
		public string OwnerId { get; set; } = string.Empty;
	}
}