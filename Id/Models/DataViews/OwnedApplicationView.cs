namespace Id.Models.DataViews
{
	public class OwnedApplicationView
	{
		public string OwnerId { get; set; }
		public string BrandId { get; set; }
		public int ApplicationUserId { get; set; }
		public string ApplicationLogo { get; set; }
		public string ApplicationName { get; set; }
		public string ApplicationWebsite { get; set; }
		public string ApplicationEmail { get; set; }
		public string ApplicationDescription { get; set; }
	}
}