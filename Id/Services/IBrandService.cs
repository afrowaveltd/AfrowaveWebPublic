using Id.Models.CommunicationModels;

namespace Id.Services
{
   public interface IBrandService
   {
		string GetBrandIconPath(int brandId, LogoSize size);
		string GetBrandIconPath(int brandId);
		Task<bool> IsBrandNameUnique(string name);

      Task<CreateBrandResponse> RegisterBrandAsync(CreateBrandModel model);
   }
}