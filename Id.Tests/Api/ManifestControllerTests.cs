using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;

namespace Id.Tests.Api;

/// <summary>
/// Unit tests for the <see cref="ManifestController"/> class.
/// </summary>
public class ManifestControllerTests
{
	private readonly IApplicationsManager _applicationsManagerMock;
	private readonly ManifestController _controller;

	/// <summary>
	/// Initializes a new instance of the <see cref="ManifestControllerTests"/> class.
	/// </summary>
	public ManifestControllerTests()
	{
		// Arrange
		_applicationsManagerMock = Substitute.For<IApplicationsManager>();
		_controller = new ManifestController(_applicationsManagerMock);

		// Set up HttpContext and UrlHelper
		var httpContext = new DefaultHttpContext();
		var actionContext = new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor());

		// Mock UrlHelper
		var urlHelperMock = Substitute.For<IUrlHelper>();
		urlHelperMock.Content(Arg.Any<string>()).Returns(x => x.Arg<string>()); // Return the input string as-is
		_controller.Url = urlHelperMock;

		_controller.ControllerContext = new ControllerContext(actionContext);
	}

	/// <summary>
	/// Tests whether GetManifestAsync returns a manifest JSON object.
	/// </summary>
	[Fact]
	public async Task GetManifestAsync_ShouldReturnManifestJson_WhenIconsExist()
	{
		// Arrange
		var mockIcons = new ImageLinksResult
		{
			Png16 = "/icons/icon-16.png",
			Png32 = "/icons/icon-32.png",
			Png76 = "/icons/icon-76.png",
			Png120 = "/icons/icon-120.png",
			Png152 = "/icons/icon-152.png",
			Png180 = "/icons/icon-180.png",
			Png192 = "/icons/icon-192.png",
			Png512 = "/icons/icon-512.png"
		};

		_applicationsManagerMock.GetAuthenticatorImagesLinksAsync().Returns(Task.FromResult(mockIcons));

		// Act
		var result = await _controller.GetManifestAsync() as JsonResult;

		// Assert
		result.Should().NotBeNull();
		result!.Value.Should().NotBeNull();

		var manifest = result.Value as ManifestController.Manifest;
		manifest.Should().NotBeNull();

		// Assert properties
		manifest!.Name.Should().Be("Afrowave");
		manifest.ShortName.Should().Be("Afrowave");
		manifest.ThemeColor.Should().Be("#000000");
		manifest.BackgroundColor.Should().Be("#ffffff");
		manifest.Display.Should().Be("standalone");

		// Ensure icons are correctly mapped
		manifest.Icons.Should().NotBeEmpty();
	}
}