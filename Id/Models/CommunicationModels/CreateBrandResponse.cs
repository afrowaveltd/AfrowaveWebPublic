namespace Id.Models.CommunicationModels
{
   public class CreateBrandResponse
   {
      public bool CanCreate { get; set; } = false;
      public string WhyCantCreate { get; set; } = string.Empty;
      public bool CanUploadImage { get; set; } = false;
      public string WhyCantUploadImage { get; set; } = string.Empty;
   }
}