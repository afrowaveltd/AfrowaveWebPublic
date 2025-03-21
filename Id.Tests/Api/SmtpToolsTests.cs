namespace Id.Tests.Api
{
	/// <summary>
	/// Unit tests for the <see cref="SmtpTools"/> class.
	/// </summary>
	public class SmtpToolsTests
	{
		private readonly IEmailManager _mockEmailManager;
		private readonly IStringLocalizer<SmtpTools> _mockLocalizer;
		private readonly SmtpTools _controller;

		/// <summary>
		/// Initializes a new instance of the <see cref="SmtpToolsTests"/> class.
		/// </summary>
		public SmtpToolsTests()
		{
			_mockEmailManager = Substitute.For<IEmailManager>();
			_mockLocalizer = Substitute.For<IStringLocalizer<SmtpTools>>();
			_controller = new SmtpTools(_mockLocalizer, _mockEmailManager);
		}

		/// <summary>
		/// Tests whether AutodetectSettings returns detected settings.
		/// </summary>
		[Fact]
		public async Task AutodetectSettings_ShouldReturnDetectedSettings_WhenHostIsValid()
		{
			// Arrange
			var input = new DetectSmtpSettingsInput { Host = "smtp.example.com" };
			var expectedResult = new SmtpDetectionResult { Successful = true, Port = 587, Secure = MailKit.Security.SecureSocketOptions.StartTls };

			_mockEmailManager.AutodetectSmtpSettingsAsync(Arg.Any<DetectSmtpSettingsInput>()).Returns(Task.FromResult(expectedResult));

			// Act
			var result = await _controller.AutodetectSettings(input);

			// Assert
			Assert.NotNull(result);
			Assert.True(result.Successful);
			Assert.Equal(587, result.Port);
		}

		/// <summary>
		/// Tests whether AutodetectSettings returns an error when the host is missing.
		/// </summary>
		[Fact]
		public async Task AutodetectSettings_ShouldReturnError_WhenHostIsMissing()
		{
			// Arrange
			var input = new DetectSmtpSettingsInput { Host = "" };

			// Act
			var result = await _controller.AutodetectSettings(input);

			// Assert
			Assert.False(result.Successful);
			Assert.Equal("Host is required", result.Message);
		}

		/// <summary>
		/// Tests whether TestSettings returns a success result when the SMTP settings are valid.
		/// </summary>
		[Fact]
		public async Task TestSettings_ShouldReturnSuccess_WhenSmtpIsValid()
		{
			// Arrange
			var input = new SmtpSenderModel
			{
				Host = "smtp.example.com",
				Port = 587,
				Username = "user@example.com",
				Password = "password"
			};
			var expectedResult = new SmtpTestResult { Success = true };

			_mockEmailManager.TestSmtpSettingsAsync(Arg.Any<SmtpSenderModel>()).Returns(Task.FromResult(expectedResult));

			// Act
			var result = await _controller.TestSettings(input);

			// Assert
			Assert.True(result.Success);
		}

		/// <summary>
		/// Tests whether TestSettings returns an error when the SMTP settings are invalid.
		/// </summary>
		[Fact]
		public async Task TestSettings_ShouldReturnError_WhenAuthenticationFails()
		{
			// Arrange
			var input = new SmtpSenderModel
			{
				Host = "smtp.example.com",
				Port = 587,
				Username = "wrong-user@example.com",
				Password = "wrong-password"
			};
			var expectedResult = new SmtpTestResult { Success = false, Error = "Authentication failed" };

			_mockEmailManager.TestSmtpSettingsAsync(Arg.Any<SmtpSenderModel>()).Returns(Task.FromResult(expectedResult));

			// Act
			var result = await _controller.TestSettings(input);

			// Assert
			Assert.False(result.Success);
			Assert.Equal("Authentication failed", result.Error);
		}
	}
}