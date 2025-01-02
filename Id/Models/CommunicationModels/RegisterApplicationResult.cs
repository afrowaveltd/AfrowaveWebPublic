namespace Id.Models.CommunicationModels
{
    public class RegisterApplicationResult
    {
        public string OwnerId { get; set; } = string.Empty;
        public int BrandId { get; set; } = 0;
        public string ApplicationId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public bool Enabled { get; set; } = false;
        public bool Success { get; set; } = false;
        public bool LogoCreated { get; set; } = false;
        public List<string>? Error { get; set; }
    }
}