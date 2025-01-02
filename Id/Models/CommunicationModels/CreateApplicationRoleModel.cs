namespace Id.Models.CommunicationModels
{
    public class CreateApplicationRoleModel
    {
        public string ApplicationId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool DefaultForNewUsers { get; set; } = false;
        public bool CanAsignOrRemoveRoles { get; set; } = false;
    }
}