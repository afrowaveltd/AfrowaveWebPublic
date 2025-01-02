namespace Id.Models.DatabaseModels
{
	public class UserRole
	{
		public int Id { get; set; }
		public int ApplicationRoleId { get; set; }
		public string UserId { get; set; } = string.Empty;
		public DateTime AddedToRole { get; set; } = DateTime.Now;
		public User? User { get; set; }
		public ApplicationRole? ApplicationRole { get; set; }
	}
}