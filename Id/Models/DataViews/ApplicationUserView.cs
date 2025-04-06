namespace Id.Models.DataViews
{
	/// <summary>
	/// Represents a user in the application with properties for ID, name, description, profile picture, gender, and roles.
	/// Includes default values for each property.
	/// </summary>
	public class ApplicationUserView
	{
		/// <summary>
		/// Represents the unique identifier for an application user. Initialized to 0 by default.
		/// </summary>
		public int ApplicationUserId { get; set; } = 0;

		/// <summary>
		/// Represents the name displayed for an entity. Initialized to an empty string by default.
		/// </summary>
		public string DisplayedName { get; set; } = string.Empty;

		/// <summary>
		/// Holds a description as a string that can be null. Initialized to an empty string by default.
		/// </summary>
		public string? Description { get; set; } = string.Empty;

		/// <summary>
		/// Represents the URL or path to a user's profile picture. Initialized to an empty string by default.
		/// </summary>
		public string ProfilePicture { get; set; } = string.Empty;

		/// <summary>
		/// Represents the gender of an individual, defaulting to 'Other' if not specified.
		/// </summary>
		public Gender Gender { get; set; } = Gender.Other;

		/// <summary>
		/// Holds a list of application roles associated with a user. It is initialized as an empty list.
		/// </summary>
		public List<ApplicationRole> Roles { get; set; } = [];
	}
}