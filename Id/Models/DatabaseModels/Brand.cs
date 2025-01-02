namespace Id.Models.DatabaseModels
{
	public class Brand
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public bool Logo { get; set; } = false;
		public string? Website { get; set; }
		public string? Description { get; set; }
		public string? Email { get; set; }
		public string OwnerId { get; set; } = string.Empty;
		public User? Owner { get; set; }
		public List<Application> Applications { get; set; } = new();
	}
}