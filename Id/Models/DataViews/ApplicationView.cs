namespace Id.Models.DataViews
{
	public class ApplicationView
	{
		public string ApplicationId { get; set; } = string.Empty;
		public string ApplicationName { get; set; } = string.Empty;
		public string? ApplicationDescription { get; set; }
		public string? ApplicationWebsite { get; set; }
		public string? ApplicationEmail { get; set; }
		public string ApplicationLogoUrl { get; set; } = string.Empty;
		public string? BrandName { get; set; }
	}
}