namespace Id.Models.DataViews
{
	public class AuthenticatorUserView
	{
		public string AuthenticatorUserId { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string? PhoneNumber { get; set; } = string.Empty;
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string DisplayedName { get; set; } = string.Empty;
		public List<BrandView> Brands { get; set; } = new();
		public List<ApplicationView> OwnedApplications { get; set; } = new();
		public List<ApplicationView> RegisteredInApplications { get; set; } = new();
	}
}