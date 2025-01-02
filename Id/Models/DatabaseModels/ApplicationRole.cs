namespace Id.Models.DatabaseModels
{
	public class ApplicationRole
	{
		public int Id { get; set; }
		public string ApplicationId { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public string NormalizedName { get; set; } = string.Empty;
		public bool DefaultForNewUsers { get; set; } = false;
		public bool CanAsignOrRemoveRoles { get; set; } = false;
		public bool IsEnabled { get; set; } = true;

		public Application? Application { get; set; }
		public List<UserRole> Roles { get; set; } = [];
	}
}