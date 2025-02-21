namespace Id.Models.DatabaseModels
{
	public class UserRole
	{
		public int Id { get; set; }
		public int ApplicationRoleId { get; set; }
		public int ApplicationUserId { get; set; } = 0;
		public DateTime AddedToRole { get; set; } = DateTime.Now;
		public ApplicationUser? User { get; set; }
		public ApplicationRole? ApplicationRole { get; set; }
	}
}