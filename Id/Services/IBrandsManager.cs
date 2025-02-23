using Id.Models.DataViews;
using Id.Models.InputModels;
using Id.Models.ResultModels;

namespace Id.Services
{
	public interface IBrandsManager
	{
		Task<List<ApplicationView>> GetBrandApplicationsAsync(int brandId);

		Task<BrandView?> GetBrandInfoAsync(int brandId);

		string GetFullsizeLogoPath(int brandId);

		string GetIconPath(int brandId);

		string GetLogoPath(int brandId, LogoSize size);

		Task<bool> IsNameUnique(string name);

		Task<RegisterBrandResult> RegisterBrandAsync(RegisterBrandInput input);

		Task<UpdateResult> UpdateBrandAsync(UpdateBrandInput input);

		Task<bool> ValidBrandAndOwner(int brandId, string ownerId);
	}
}