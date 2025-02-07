using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SharedTools.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace SharedTools.Services
{
	public class ImageService(ILogger<ImageService> logger) : IImageService
	{
		private readonly ILogger<ImageService> _logger = logger;
		private List<int> DefaultSizes = [16, 32, 76, 120, 152, 180, 192, 512];
		private readonly string[] permittedExtensions = { ".jpeg", ".jpg", ".gif", ".png" };

		private string baseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory
			 .Substring(0, AppDomain.CurrentDomain.BaseDirectory
			 .IndexOf("bin")), "wwwroot");

		public async Task<ApiResponse<string>> ResizeAndSaveAsync(Image img, string targetPath, int width, int height)
		{
			ApiResponse<string> returnValue = new ApiResponse<string>();
			try
			{
				using Image image = img;
				image.Mutate(x => x.Resize(width, height));

				if(Directory.Exists(Path.GetDirectoryName(targetPath)) == false)
				{
					try
					{
						if(Path.GetDirectoryName(targetPath) != null && Path.GetDirectoryName(targetPath) != string.Empty)
						{
							_ = Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
						}
					}
					catch(Exception ex)
					{
						_logger.LogError("Error creating directory for image in the path {targetPath}, {ex}", targetPath, ex);
					}

					if(File.Exists(targetPath))
					{
						File.Delete(targetPath);
					}
				}

				await image.SaveAsync(targetPath);

				returnValue.Successful = true;
				returnValue.Data = targetPath;
				_logger.LogInformation("Resized and saved image to {targetPath}", targetPath);
			}
			catch(Exception ex)
			{
				returnValue.Successful = false;
				returnValue.Message = ex.Message;
				_logger.LogError(ex, "Error resizing and saving image");
			}
			return returnValue;
		}

		public async Task<List<ApiResponse<string>>> CreateBrandIcons(IFormFile img, int brandId)
		{
			string targetPath = Path.Combine(baseDirectory, "brands", brandId.ToString(), "icons");
			return await CreateIcons(img, targetPath);
		}

		public async Task<List<ApiResponse<string>>> CreateApplicationIcons(IFormFile img, string applicationId)
		{
			string targetPath = Path.Combine(baseDirectory, "applications", applicationId, "icons");
			return await CreateIcons(img, targetPath);
		}

		public async Task<ApiResponse<string>> CreateUserProfileImages(IFormFile img, string userId)
		{
			ApiResponse<string> result = new();

			return result;
		}

		private async Task<List<ApiResponse<string>>> CreateIcons(IFormFile img, string targetPath)
		{
			List<ApiResponse<string>> response = new();

			using Image image = await GetImage(img); // Load the original image
			{
				// Store the original image
				string fileExtension = Path.GetExtension(img.FileName);
				string target = Path.Combine(targetPath, $"original-icon-{image.Height}x{image.Width}{fileExtension}");

				try
				{
					// Ensure the directory exists
					if(!Directory.Exists(Path.GetDirectoryName(target)))
					{
						_ = Directory.CreateDirectory(Path.GetDirectoryName(target) ?? string.Empty);
					}

					// Save the original image
					if(File.Exists(target))
					{
						File.Delete(target);
					}

					await image.SaveAsync(target);
					_logger.LogInformation("Saved original image to {targetPath}", targetPath);

					response.Add(new ApiResponse<string>
					{
						Data = target,
						Successful = true
					});
				}
				catch(Exception ex)
				{
					_logger.LogError("Error saving original image to {target}, {ex}", target, ex);
					response.Add(new ApiResponse<string>
					{
						Successful = false,
						Message = ex.Message
					});
				}

				// Check if the image is square and crop it
				if(image.Height != image.Width)
				{
					_logger.LogWarning("Image is not square, resizing to square");
					int size = Math.Min(image.Height, image.Width);
					image.Mutate(x => x.Crop(size, size));
				}

				// Create PNG icons of different sizes
				foreach(int size in DefaultSizes)
				{
					target = Path.Combine(targetPath, $"icon-{size}x{size}{fileExtension}");

					try
					{
						using Image<Rgba32> clonedImage = image.CloneAs<Rgba32>();
						ApiResponse<string> responseIcon = await ResizeAndSaveAsync(clonedImage, target, size, size);
						response.Add(responseIcon);
					}
					catch(Exception ex)
					{
						_logger.LogError("Error resizing and saving image for size {size}, {ex}", size, ex);
						response.Add(new ApiResponse<string>
						{
							Successful = false,
							Message = ex.Message
						});
					}
				}

				// Create a single 32x32 .ico file
				string icoTarget = Path.Combine(targetPath, "icon-32x32.ico");
				try
				{
					using Image<Rgba32> image32x32 = image.CloneAs<Rgba32>();
					image32x32.Mutate(x => x.Resize(32, 32)); // Ensure the image is resized to 32x32

					using FileStream icoStream = new FileStream(icoTarget, FileMode.Create);
					CreateIcoFile(image32x32, icoStream);
					_logger.LogInformation("Saved .ico file to {icoTarget}", icoTarget);

					response.Add(new ApiResponse<string>
					{
						Data = icoTarget,
						Successful = true
					});
				}
				catch(Exception ex)
				{
					_logger.LogError("Error creating .ico file, {ex}", ex);
					response.Add(new ApiResponse<string>
					{
						Successful = false,
						Message = ex.Message
					});
				}
			}

			return response;
		}

		private void CreateIcoFile(Image<Rgba32> image, Stream outputStream)
		{
			using BinaryWriter writer = new BinaryWriter(outputStream);

			// Save the image as PNG to a memory stream
			using MemoryStream memoryStream = new MemoryStream();
			image.SaveAsPng(memoryStream);
			byte[] pngBytes = memoryStream.ToArray();

			// Write ICO header
			writer.Write((ushort)0);        // Reserved
			writer.Write((ushort)1);        // ICO type
			writer.Write((ushort)1);        // Number of images (1)

			// Write directory entry
			writer.Write((byte)image.Width);   // Width
			writer.Write((byte)image.Height);  // Height
			writer.Write((byte)0);             // Number of colors (0 = no palette)
			writer.Write((byte)0);             // Reserved
			writer.Write((ushort)1);           // Color planes
			writer.Write((ushort)32);          // Bits per pixel
			writer.Write(pngBytes.Length);     // Image size
			writer.Write(6 + 16);              // Offset to image data

			// Write image data
			writer.Write(pngBytes);
		}

		public bool IsImage(IFormFile img)
		{
			string fileExtension = Path.GetExtension(img.FileName).ToLower();
			return permittedExtensions.Contains(fileExtension);
		}

		private async Task<Image> GetImage(IFormFile img)
		{
			using MemoryStream ms = new();
			await img.CopyToAsync(ms);
			return Image.Load(ms.ToArray());
		}
	}
}