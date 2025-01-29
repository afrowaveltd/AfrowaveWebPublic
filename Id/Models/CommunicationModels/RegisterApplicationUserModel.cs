using System.ComponentModel.DataAnnotations;

namespace Id.Models.CommunicationModels
{
	public class RegisterApplicationUserModel
	{
		public string ApplicationId { get; set; } = string.Empty;

		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; } = string.Empty;

		[Required]
		[DataType(DataType.Password)]
		public string PasswordConfirm { get; set; } = string.Empty;

		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string DisplayedName { get; set; } = string.Empty;
		public string? PhoneNumber { get; set; } = string.Empty;
		public IFormFile? ProfilePicture { get; set; }

		[Required]
		public bool AcceptTerms { get; set; } = false;
		[Required]
		public bool AcceptPrivacyPolicy { get; set; } = false;
		[Required]
		public bool AcceptCookiePolicy { get; set; } = false;
	}
}