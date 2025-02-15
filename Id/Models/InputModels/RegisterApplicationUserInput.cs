using System.ComponentModel.DataAnnotations;

namespace Id.Models.InputModels
{
	public class RegisterApplicationUserInput
	{
		[Required]
		public string UserId { get; set; } = string.Empty;

		[Required]
		public string ApplicationId { get; set; } = string.Empty;

		public string? UserDescription { get; set; }
		public bool AgreedToTerms { get; set; } = false;
		public bool AgreedSharingUserDetails { get; set; } = false;
		public bool AgreedToCookies { get; set; } = false;
	}
}