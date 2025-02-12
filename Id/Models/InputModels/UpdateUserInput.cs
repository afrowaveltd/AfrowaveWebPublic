using System.ComponentModel.DataAnnotations;

namespace Id.Models.InputModels
{
	public class UpdateUserInput : IUserInput
	{
		[Required]
		public string UserId { get; set; } = string.Empty;

		public string? Email { get; set; }
		public string? Password { get; set; }
		public string? PasswordConfirm { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? DisplayedName { get; set; }
		public DateTime? Birthdate { get; set; }
		public IFormFile? ProfilePicture { get; set; }
		public bool? AcceptTerms { get; set; }
		public bool? AcceptPrivacyPolicy { get; set; }
		public bool? AcceptCookiePolicy { get; set; }
	}
}