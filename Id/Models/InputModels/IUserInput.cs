namespace Id.Models.InputModels
{
	public interface IUserInput
	{
		string? Email { get; set; }
		string? FirstName { get; set; }
		string? LastName { get; set; }
		string? DisplayedName { get; set; }
		DateTime? Birthdate { get; set; }
		IFormFile? ProfilePicture { get; set; }
	}
}