namespace Id.Tests.Api
{
	/// <summary>
	/// Unit tests for the <see cref="PasswordRequirements"/> class.
	/// </summary>
	public class PasswordRequirementsTests
	{
		private readonly ISettingsService _mockSettingsService;
		private readonly PasswordRequirements _controller;

		/// <summary>
		/// Initializes a new instance of the <see cref="PasswordRequirementsTests"/> class.
		/// </summary>
		public PasswordRequirementsTests()
		{
			_mockSettingsService = Substitute.For<ISettingsService>();

			// Create an instance of the PasswordRequirements controller with the mocked service
			_controller = new PasswordRequirements(_mockSettingsService);
		}

		/// <summary>
		/// Tests whether Get returns the password rules.
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task Get_ShouldReturnPasswordRules()
		{
			// Arrange
			PasswordRules expectedRules = new PasswordRules
			{
				MinimumLength = 10,
				MaximumLength = 128,
				RequireDigit = true,
				RequireUppercase = true,
				RequireLowercase = true,
				RequireNonAlphanumeric = false
			};

			IdentificatorSettings settings = new IdentificatorSettings { PasswordRules = expectedRules };
			_ = _mockSettingsService.GetSettingsAsync().Returns(settings);

			// Act
			IActionResult result = await _controller.Get();

			// Assert
			OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
			PasswordRules returnedRules = Assert.IsType<PasswordRules>(okResult.Value);
			Assert.Equal(expectedRules.MinimumLength, returnedRules.MinimumLength);
			Assert.Equal(expectedRules.MaximumLength, returnedRules.MaximumLength);
			Assert.Equal(expectedRules.RequireDigit, returnedRules.RequireDigit);
			Assert.Equal(expectedRules.RequireUppercase, returnedRules.RequireUppercase);
			Assert.Equal(expectedRules.RequireLowercase, returnedRules.RequireLowercase);
			Assert.Equal(expectedRules.RequireNonAlphanumeric, returnedRules.RequireNonAlphanumeric);
		}

		/// <summary>
		/// Tests whether Get returns the default password rules when the service fails.
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task Get_ShouldReturnDefaultRules_WhenServiceFails()
		{
			// Arrange: Simulate a failure by returning null
			_ = _mockSettingsService.GetSettingsAsync().Returns((IdentificatorSettings)null);

			// Act
			IActionResult result = await _controller.Get();

			// Assert
			OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
			PasswordRules returnedRules = Assert.IsType<PasswordRules>(okResult.Value);
			Assert.NotNull(returnedRules);
			Assert.Equal(8, returnedRules.MinimumLength); // Default value from IdentificatorSettings
			Assert.Equal(128, returnedRules.MaximumLength);
		}
	}
}