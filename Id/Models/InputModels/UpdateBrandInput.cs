namespace Id.Models.InputModels
{
	public class UpdateBrandInput : IBrandInput
	{
		public int BrandId { get; set; } = 0;
		public string Name { get; set; } = string.Empty;

		public string? Description { get; set; }
		public IFormFile? Icon { get; set; }
		public string? Website { get; set; }
		public string? Email { get; set; }
		public string OwnerId { get; set; } = string.Empty;
	}
}