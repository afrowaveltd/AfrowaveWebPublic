using Microsoft.AspNetCore.Http;
using SharedTools.Models;
using SixLabors.ImageSharp;

namespace SharedTools.Services
{
	public interface IImageService
	{
		Task<List<ApiResponse<string>>> CreateApplicationIcons(IFormFile img, string applicationId);

		Task<List<ApiResponse<string>>> CreateBrandIcons(IFormFile img, int brandId);

		Task<ApiResponse<string>> CreateUserProfileImages(IFormFile img, string userId);

		bool IsImage(IFormFile img);

		Task<ApiResponse<string>> ResizeAndSaveAsync(Image img, string targetPath, int width, int height);
	}
}