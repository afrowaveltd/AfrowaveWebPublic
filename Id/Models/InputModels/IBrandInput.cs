namespace Id.Models.InputModels
{
	public interface IBrandInput
	{
		string Name { get; set; } = string.Empty;
		string? Description { get; set; }
		IFormFile? Icon { get; set; }
		string? Website { get; set; }
		string? Email { get; set; }
		string OwnerId { get; set; } = string.Empty;
	}
}