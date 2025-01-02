using Id.Models.CommunicationModels;

namespace Id.Services
{
   public interface IBrandService
   {
      Task<bool> IsBrandNameUnique(string name);

      Task<CreateBrandResponse> RegisterBrandAsync(CreateBrandModel model);
   }
}