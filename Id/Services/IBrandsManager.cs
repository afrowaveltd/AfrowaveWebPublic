using Id.Models.InputModels;
using Id.Models.ResultModels;

namespace Id.Services
{
	public interface IBrandsManager
	{
		string GetFullsizeLogoPath(int brandId);
		string GetIconPath(int brandId);
		string GetLogoPath(int brandId, LogoSize size);
		Task<bool> IsNameUnique(string name);
		Task<RegisterBrandResult> RegisterBrandAsync(RegisterBrandInput input);
		Task<UpdateResult> UpdateBrandAsync(UpdateBrandInput input);
		Task<bool> ValidBrandAndOwner(int brandId, string ownerId);
	}
}