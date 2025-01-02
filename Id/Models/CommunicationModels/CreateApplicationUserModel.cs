namespace Id.Models.CommunicationModels
{
   public class CreateApplicationUserModel
   {
      public string ApplicationId { get; set; } = string.Empty;
      public string UserId { get; set; } = string.Empty;
      public bool AgreedToTerms { get; set; }
      public bool AgreedToCookies { get; set; }
      public bool AgreedSharingUserDetails { get; set; }
   }
}