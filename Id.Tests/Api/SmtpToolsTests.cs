namespace Id.Tests.Api
{
	/// <summary>
	/// Unit tests for the <see cref="SmtpTools"/> class.
	/// </summary>
	public class SmtpToolsTests
	{
		private readonly Mock<IEmailManager> _mockEmailManager;
		private readonly Mock<IStringLocalizer<SmtpTools>> _mockLocalizer;
		private readonly SmtpTools _controller;

		/// <summary>
		/// Initializes a new instance of the <see cref="SmtpToolsTests"/> class.
		/// </summary>
		public SmtpToolsTests()
		{
			_mockEmailManager = new Mock<IEmailManager>();
			_mockLocalizer = new Mock<IStringLocalizer<SmtpTools>>();
			_controller = new SmtpTools(_mockLocalizer.Object, _mockEmailManager.Object);
		}

		/// <summary>
		/// Tests whether AutodetectSettings returns detected settings.
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task AutodetectSettings_ShouldReturnDetectedSettings_WhenHostIsValid()
		{
			// Arrange
			DetectSmtpSettingsInput input = new DetectSmtpSettingsInput { Host = "smtp.example.com" };
			SmtpDetectionResult expectedResult = new SmtpDetectionResult { Successful = true, Port = 587, Secure = MailKit.Security.SecureSocketOptions.StartTls };

			_ = _mockEmailManager
				 .Setup(e => e.AutodetectSmtpSettingsAsync(It.IsAny<DetectSmtpSettingsInput>()))
				 .ReturnsAsync(expectedResult);

			// Act
			SmtpDetectionResult result = await _controller.AutodetectSettings(input);

			// Assert
			Assert.NotNull(result);
			Assert.True(result.Successful);
			Assert.Equal(587, result.Port);
		}

		/// <summary>
		/// Tests whether AutodetectSettings returns an error when the host is missing.
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task AutodetectSettings_ShouldReturnError_WhenHostIsMissing()
		{
			// Arrange
			DetectSmtpSettingsInput input = new DetectSmtpSettingsInput { Host = "" };

			// Act
			SmtpDetectionResult result = await _controller.AutodetectSettings(input);

			// Assert
			Assert.False(result.Successful);
			Assert.Equal("Host is required", result.Message);
		}

		/// <summary>
		/// Tests whether TestSettings returns a success result when the SMTP settings are valid.
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task TestSettings_ShouldReturnSuccess_WhenSmtpIsValid()
		{
			// Arrange
			SmtpSenderModel input = new SmtpSenderModel
			{
				Host = "smtp.example.com",
				Port = 587,
				Username = "user@example.com",
				Password = "password"
			};
			SmtpTestResult expectedResult = new SmtpTestResult { Success = true };

			_ = _mockEmailManager
				 .Setup(e => e.TestSmtpSettingsAsync(It.IsAny<SmtpSenderModel>()))
				 .ReturnsAsync(expectedResult);

			// Act
			SmtpTestResult result = await _controller.TestSettings(input);

			// Assert
			Assert.True(result.Success);
		}

		/// <summary>
		/// Tests whether TestSettings returns an error when the SMTP settings are invalid
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task TestSettings_ShouldReturnError_WhenAuthenticationFails()
		{
			// Arrange
			SmtpSenderModel input = new SmtpSenderModel
			{
				Host = "smtp.example.com",
				Port = 587,
				Username = "wrong-user@example.com",
				Password = "wrong-password"
			};
			SmtpTestResult expectedResult = new SmtpTestResult { Success = false, Error = "Authentication failed" };

			_ = _mockEmailManager
				 .Setup(e => e.TestSmtpSettingsAsync(It.IsAny<SmtpSenderModel>()))
				 .ReturnsAsync(expectedResult);

			// Act
			SmtpTestResult result = await _controller.TestSettings(input);

			// Assert
			Assert.False(result.Success);
			Assert.Equal("Authentication failed", result.Error);
		}
	}
}