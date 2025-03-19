using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Id.Tests.SharedTools.Tests;

/// <summary>
/// Unit tests for the <see cref="ImageService"/> class.
/// These tests validate image processing, resizing, and file validation.
/// </summary>
public class ImageServiceTests
{
	private readonly Mock<ILogger<ImageService>> _mockLogger;
	private readonly ImageService _imageService;

	/// <summary>
	/// Initializes an instance of ImageServiceTests. Sets up a mock logger and an instance of ImageService for testing.
	/// </summary>
	public ImageServiceTests()
	{
		_mockLogger = new Mock<ILogger<ImageService>>();
		_imageService = new ImageService(_mockLogger.Object);
	}

	/// <summary>
	/// Tests whether the IsImage method correctly validates image files.
	/// </summary>
	[Fact]
	public void IsImage_ValidExtensions_ReturnsTrue()
	{
		// Arrange
		Mock<IFormFile> mockFile = new Mock<IFormFile>();
		_ = mockFile.Setup(f => f.FileName).Returns("image.jpg");

		// Act
		bool result = _imageService.IsImage(mockFile.Object);

		// Assert
		Assert.True(result);
	}

	/// <summary>
	/// Tests whether the ResizeAndSaveAsync method resizes and saves an image correctly.
	/// </summary>
	[Fact]
	public async Task ResizeAndSaveAsync_ResizesImageAndSaves()
	{
		// Arrange
		using Image<Rgba32> image = new Image<Rgba32>(100, 100);
		Image<Rgba32> clonedImage = image.Clone(); // Avoid disposing issues

		string tempPath = Path.Combine(Path.GetTempPath(), "test-image.png");
		_ = Directory.CreateDirectory(Path.GetDirectoryName(tempPath) ?? ".");

		// Act
		ApiResponse<string> result = await _imageService.ResizeAndSaveAsync(clonedImage, tempPath, 50, 50);

		// Assert
		Assert.True(result.Successful, "ResizeAndSaveAsync returned false.");
		Assert.True(File.Exists(tempPath), "The resized image was not saved.");
	}
}