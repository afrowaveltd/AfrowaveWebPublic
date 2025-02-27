namespace Id.Models.DataViews
{
	/// <summary>
	/// View model for application data.
	/// </summary>
	public class ApplicationView
	{
		/// <summary>
		/// Gets or sets the application ID.
		/// </summary>
		public string ApplicationId { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the application name.
		/// </summary>
		public string ApplicationName { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the application description.
		/// </summary>
		public string? ApplicationDescription { get; set; }

		/// <summary>
		/// Gets or sets the application website.
		/// </summary>
		public string? ApplicationWebsite { get; set; }

		/// <summary>
		/// Gets or sets the application email.
		/// </summary>
		public string? ApplicationEmail { get; set; }

		/// <summary>
		/// Gets or sets the application logo URL.
		/// </summary>
		public string ApplicationLogoUrl { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the Brand under which is the application published.
		/// </summary>
		public string? BrandName { get; set; }
	}
}