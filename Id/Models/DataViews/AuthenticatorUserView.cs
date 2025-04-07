namespace Id.Models.DataViews
{
	/// <summary>
	/// Represents a view model for an authenticator user.
	/// </summary>
	public class AuthenticatorUserView
	{
		/// <summary>
		/// Gets or sets the unique identifier for the user.
		/// </summary>
		public string AuthenticatorUserId { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the user email in the system.
		/// </summary>
		public string Email { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the phone number of the user.
		/// </summary>
		public string? PhoneNumber { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the first name of the user.
		/// </summary>
		public string FirstName { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the last name of the user.
		/// </summary>
		public string LastName { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the display name of the user.
		/// </summary>
		public string DisplayedName { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the list of brands associated with the user.
		/// </summary>
		public List<BrandView> Brands { get; set; } = new();

		/// <summary>
		/// Gets or sets the list of owned applications associated with the user.
		/// </summary>
		public List<OwnedApplicationView> OwnedApplications { get; set; } = new();

		/// <summary>
		/// Gets or sets the list of registered applications associated with the user.
		/// </summary>
		public List<UsedApplicationView> RegisteredInApplications { get; set; } = new();
	}
}