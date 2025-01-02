namespace Id.Models.CommunicationModels
{
    public class RoleAssignResult
    {
        public int RoleId { get; set; } = 0;
        public string UserId { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string NormalizedName { get; set; } = string.Empty;
        public bool Successful { get; set; } = true;
        public string Message { get; set; } = string.Empty;
    }
}