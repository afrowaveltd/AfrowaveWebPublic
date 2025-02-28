using Id.Models.DataViews;
using Id.Models.InputModels;
using Id.Models.ResultModels;

namespace Id.Services
{
	/// <summary>
	/// Interface for managing applications.
	/// </summary>
	public interface IBrandsManager
	{
		/// <summary>
		/// Gets the list of application views for all applications registered with the brand.
		/// </summary>
		/// <param name="brandId">BrandID</param>
		/// <returns>List of ApplicationViews</returns>
		Task<List<ApplicationView>> GetBrandApplicationsAsync(int brandId);

		/// <summary>
		/// Gets the brand information.
		/// </summary>
		/// <param name="brandId">Brand ID</param>
		/// <returns>BrandView</returns>
		Task<BrandView?> GetBrandInfoAsync(int brandId);

		/// <summary>
		/// Gets the full size logo path.
		/// </summary>
		/// <param name="brandId">BrandID</param>
		/// <returns>Returns the relative URL for the full size brand logo</returns>
		string GetFullsizeLogoPath(int brandId);

		/// <summary>
		/// Gets the icon path.
		/// </summary>
		/// <param name="brandId">Brand ID</param>
		/// <returns>Returns the relative URL for the icon of the brand</returns>
		string GetIconPath(int brandId);

		/// <summary>
		/// Gets the logo path.
		/// </summary>
		/// <param name="brandId">Brand ID</param>
		/// <param name="size">Size of the logo from enum</param>
		/// <returns>relative path for the logo of the brand</returns>
		string GetLogoPath(int brandId, LogoSize size);

		/// <summary>
		/// Checks if the brand name is unique.
		/// </summary>
		/// <param name="name">Brand name</param>
		/// <returns>True if the brand name is not registered yet</returns>
		Task<bool> IsNameUnique(string name);

		/// <summary>
		/// Registers a brand.
		/// </summary>
		/// <param name="input">RegisterBrandInput</param>
		/// <returns>RegisterBrandResult</returns>
		Task<RegisterBrandResult> RegisterBrandAsync(RegisterBrandInput input);

		/// <summary>
		/// Updates a brand.
		/// </summary>
		/// <param name="input">UpdateBrandInput</param>
		/// <returns>UpdateResult</returns>
		Task<UpdateResult> UpdateBrandAsync(UpdateBrandInput input);

		/// <summary>
		/// Validates the brand and owner.
		/// </summary>
		/// <param name="brandId">BrandId</param>
		/// <param name="ownerId">OwnerId</param>
		/// <returns>True, if user is the owner of the brand</returns>
		Task<bool> ValidBrandAndOwner(int brandId, string ownerId);
	}
}