namespace Id.Models.ResultModels
{
	public class RegisterUserResult
	{
		public bool UserCreated { get; set; } = true;
		public string UserId { get; set; } = string.Empty;
		public bool ProfilePictureUploaded { get; set; } = true;
		public List<string> Errors { get; set; } = [];
	}
}