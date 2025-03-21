namespace Id.Tests.Api
{
	/// <summary>
	/// Unit tests for the <see cref="IsEmailUnique"/> class.
	/// </summary>
	public class IsEmailUniqueTests
	{
		private readonly IUsersManager _usersManagerMock;
		private readonly IStringLocalizer<IsEmailUnique> _localizerMock;
		private readonly IsEmailUnique _controller;

		/// <summary>
		/// Initializes a new instance of the <see cref="IsEmailUniqueTests"/> class.
		/// </summary>
		public IsEmailUniqueTests()
		{
			_usersManagerMock = Substitute.For<IUsersManager>();
			_localizerMock = Substitute.For<IStringLocalizer<IsEmailUnique>>();

			_controller = new IsEmailUnique(_usersManagerMock, _localizerMock);
		}

		/// <summary>
		/// Tests whether OnGetAsync returns true when the email is unique.
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task OnGetAsync_ShouldReturnTrue_WhenEmailIsUnique()
		{
			// Arrange
			_ = _usersManagerMock.IsEmailFreeAsync("unique@example.com").Returns(true);

			// Act
			OkObjectResult? result = await _controller.OnGetAsync("unique@example.com") as OkObjectResult;

			// Assert
			_ = result.Should().NotBeNull();
			_ = result!.Value.Should().BeEquivalentTo(new { isUnique = true });

			await _usersManagerMock.Received(1).IsEmailFreeAsync("unique@example.com");
		}

		/// <summary>
		/// Tests whether OnGetAsync returns false when the email is taken.
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task OnGetAsync_ShouldReturnFalse_WhenEmailIsTaken()
		{
			// Arrange
			_ = _usersManagerMock.IsEmailFreeAsync("taken@example.com").Returns(false);

			// Act
			OkObjectResult? result = await _controller.OnGetAsync("taken@example.com") as OkObjectResult;

			// Assert
			_ = result.Should().NotBeNull();
			_ = result!.Value.Should().BeEquivalentTo(new { isUnique = false });

			await _usersManagerMock.Received(1).IsEmailFreeAsync("taken@example.com");
		}

		/// <summary>
		/// Tests whether OnGetAsync returns bad request when the email is invalid.
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task OnGetAsync_ShouldReturnBadRequest_WhenEmailIsInvalid()
		{
			// Arrange
			_ = _localizerMock["Invalid email address"].Returns(new LocalizedString("Invalid email address", "Invalid email address"));

			// Act
			BadRequestObjectResult? result = await _controller.OnGetAsync("invalid-email") as BadRequestObjectResult;

			// Assert
			_ = result.Should().NotBeNull();
			_ = result!.Value.Should().Be("Invalid email address");
		}
	}
}