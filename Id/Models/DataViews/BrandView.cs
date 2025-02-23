namespace Id.Models.DataViews
{
	public class BrandView
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string? Description { get; set; }
		public string? LogoPath { get; set; }
		public string? Website { get; set; }
		public string? Email { get; set; }
		public string OwnerName { get; set; } = string.Empty;
	}
}