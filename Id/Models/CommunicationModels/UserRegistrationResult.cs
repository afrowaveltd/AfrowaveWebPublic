namespace Id.Models.CommunicationModels
{
	public class UserRegistrationResult
	{
		public bool Success { get; set; } = true;
		public List<string> Errors { get; set; } = new();
		public string? UserId { get; set; }
		public bool EmailSent { get; set; } = false;
		public bool PictureUploaded { get; set; } = false;
	}
}