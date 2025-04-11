namespace Id.Models.DataViews
{
	/// <summary>
	/// Represents a view of an owned application.
	/// </summary>
	public class OwnedApplicationView
	{
		/// <summary>
		/// Gets or sets the unique identifier of the owned application.
		/// </summary>
		public string OwnerId { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the unique identifier of the brand associated with the owned application.
		/// </summary>
		public string BrandId { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the unique identifier of the application user associated with the owned application.
		/// </summary>
		public int ApplicationUserId { get; set; } = 0;

		/// <summary>
		/// Gets or sets the ApplicationLogo URL of the owned application.
		/// </summary>
		public string ApplicationLogo { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the ApplicationName of the owned application.
		/// </summary>
		public string ApplicationName { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the ApplicationWebsite URL of the owned application.
		/// </summary>
		public string? ApplicationWebsite { get; set; }

		/// <summary>
		/// Gets or sets the ApplicationEmail of the owned application.
		/// </summary>
		public string ApplicationEmail { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the ApplicationPhone of the owned application.
		/// </summary>
		public string? ApplicationDescription { get; set; }
	}
}