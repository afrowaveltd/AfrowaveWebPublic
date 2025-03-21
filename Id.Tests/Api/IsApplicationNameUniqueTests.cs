namespace Id.Tests.Api;

/// <summary>
/// Unit tests for the <see cref="IsApplicationNameUnique"/> class.
/// </summary>
public class IsApplicationNameUniqueTests
{
	private readonly IApplicationsManager _applicationsManagerMock;
	private readonly IsApplicationNameUnique _controller;

	/// <summary>
	/// Initializes a new instance of the <see cref="IsApplicationNameUniqueTests"/> class.
	/// </summary>
	public IsApplicationNameUniqueTests()
	{
		_applicationsManagerMock = Substitute.For<IApplicationsManager>();
		_controller = new IsApplicationNameUnique(_applicationsManagerMock);
	}

	/// <summary>
	/// Tests whether OnGetAsync returns true when the application name is unique.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task OnGetAsync_ShouldReturnTrue_WhenNameIsUnique()
	{
		// Arrange
		_ = _applicationsManagerMock.IsNameUnique("UniqueApp").Returns(true);

		// Act
		OkObjectResult? result = await _controller.OnGetAsync("UniqueApp") as OkObjectResult;

		// Assert
		_ = result.Should().NotBeNull();
		_ = result!.Value.Should().BeEquivalentTo(new { IsUnique = true });

		await _applicationsManagerMock.Received(1).IsNameUnique("UniqueApp");
	}

	/// <summary>
	/// Tests whether OnGetAsync returns false when the application name is taken.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task OnGetAsync_ShouldReturnFalse_WhenNameIsTaken()
	{
		// Arrange
		_ = _applicationsManagerMock.IsNameUnique("TakenApp").Returns(false);

		// Act
		OkObjectResult? result = await _controller.OnGetAsync("TakenApp") as OkObjectResult;

		// Assert
		_ = result.Should().NotBeNull();
		_ = result!.Value.Should().BeEquivalentTo(new { IsUnique = false });

		await _applicationsManagerMock.Received(1).IsNameUnique("TakenApp");
	}
}