namespace Id.Models.CommunicationModels
{
	public class ApplicationUserDetails
	{
		public string UserId { get; set; } = string.Empty;
		public string ApplicationId { get; set; } = string.Empty;
		public int ApplicationUserId { get; set; } = 0;
		public List<int> UserRoles { get; set; } = [];
	}
}