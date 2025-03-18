using Microsoft.AspNetCore.Mvc;

namespace Id.Tests.Api;

/// <summary>
/// Unit tests for the <see cref="IsApplicationNameUnique"/> class.
/// </summary>
public class IsApplicationNameUniqueTests
{
	private readonly Mock<IApplicationsManager> _applicationsManagerMock;
	private readonly IsApplicationNameUnique _controller;

	/// <summary>
	/// Initializes a new instance of the <see cref="IsApplicationNameUniqueTests"/> class.
	/// </summary>
	public IsApplicationNameUniqueTests()
	{
		_applicationsManagerMock = new Mock<IApplicationsManager>();
		_controller = new IsApplicationNameUnique(_applicationsManagerMock.Object);
	}

	/// <summary>
	/// Tests whether OnGetAsync returns true when the application name is unique.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task OnGetAsync_ShouldReturnTrue_WhenNameIsUnique()
	{
		// Arrange
		_ = _applicationsManagerMock.Setup(m => m.IsNameUnique("UniqueApp")).ReturnsAsync(true);

		// Act
		OkObjectResult? result = await _controller.OnGetAsync("UniqueApp") as OkObjectResult;

		// Assert
		_ = result.Should().NotBeNull();
		_ = result!.Value.Should().BeEquivalentTo(new { IsUnique = true });

		_applicationsManagerMock.Verify(m => m.IsNameUnique("UniqueApp"), Times.Once);
	}

	/// <summary>
	/// Tests whether OnGetAsync returns false when the application name is taken.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task OnGetAsync_ShouldReturnFalse_WhenNameIsTaken()
	{
		// Arrange
		_ = _applicationsManagerMock.Setup(m => m.IsNameUnique("TakenApp")).ReturnsAsync(false);

		// Act
		OkObjectResult? result = await _controller.OnGetAsync("TakenApp") as OkObjectResult;

		// Assert
		_ = result.Should().NotBeNull();
		_ = result!.Value.Should().BeEquivalentTo(new { IsUnique = false });

		_applicationsManagerMock.Verify(m => m.IsNameUnique("TakenApp"), Times.Once);
	}
}