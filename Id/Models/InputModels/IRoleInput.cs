namespace Id.Models.InputModels
{
	public interface IRoleInput
	{
		string ApplicationId { get; set; }
		string Name { get; set; }
		bool AllignToAll { get; set; }
		bool CanAdministerRoles { get; set; }
	}
}