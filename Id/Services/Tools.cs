using System.ComponentModel.DataAnnotations;

namespace Id.Services
{
   public class Tools
   {
      private static readonly string[] permittedExtensions = { ".jpeg", ".jpg", ".gif", ".png" };

      public static bool IsEmailValid(string email)
      {
         return new EmailAddressAttribute().IsValid(email);
      }

      public static bool IsValidUrl(string url)
      {
         // Check if the input is null or empty
         if(string.IsNullOrWhiteSpace(url))
            return false;

         // Try to create a Uri object
         if(Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult))
         {
            // Check if the Uri has a valid scheme (e.g., http, https, ftp)
            return uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps;
         }

         return false;
      }

      public static bool IsImage(IFormFile file)
      {
         // Check if the file is null
         if(file == null)
            return false;

         // Check if the file is less than 1MB
         if(file.Length > 1048576)
            return false;
         // Check if the file has a valid extension
         return permittedExtensions.Contains(Path.GetExtension(file.FileName).ToLower());
      }
   }
}