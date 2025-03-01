using Microsoft.AspNetCore.Http;
using SharedTools.Models;
using SixLabors.ImageSharp;

namespace SharedTools.Services
{
	/// <summary>
	/// Interface for the ImageService class.
	/// </summary>
	public interface IImageService
	{
		/// <summary>
		/// Creates an application icons.
		/// </summary>
		/// <param name="img">IFormFile image file</param>
		/// <param name="applicationId">Application Id</param>
		/// <returns>List of names of newly created files.</returns>
		Task<List<ApiResponse<string>>> CreateApplicationIcons(IFormFile img, string applicationId);

		/// <summary>
		/// Creates a brand icons.
		/// </summary>
		/// <param name="img">IFormFile image file</param>
		/// <param name="brandId">Brand Id</param>
		/// <returns>List of names of newly created files</returns>
		Task<List<ApiResponse<string>>> CreateBrandIcons(IFormFile img, int brandId);

		/// <summary>
		/// Creates a user profile images.
		/// </summary>
		/// <param name="img">IFormFile image file</param>
		/// <param name="userId">User Id</param>
		/// <returns>List of names of newly created files</returns>
		Task<ApiResponse<string>> CreateUserProfileImages(IFormFile img, string userId);

		/// <summary>
		/// Checks if the file is an image.
		/// </summary>
		/// <param name="img">IFormFile image file</param>
		/// <returns></returns>
		bool IsImage(IFormFile img);

		/// <summary>
		/// Resizes an image and saves it to the target path.
		/// </summary>
		/// <param name="img">IFormFile image file</param>
		/// <param name="targetPath">Target path for the image</param>
		/// <param name="width">Width in pixels</param>
		/// <param name="height">Height in pixels</param>
		/// <returns>Path of the newly created file</returns>
		Task<ApiResponse<string>> ResizeAndSaveAsync(Image img, string targetPath, int width, int height);
	}
}