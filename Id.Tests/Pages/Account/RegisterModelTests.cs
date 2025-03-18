using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Id.Tests.Pages.Account;

public class RegisterModelTests
{
	private readonly Mock<IUsersManager> _mockUsersManager;
	private readonly Mock<IApplicationUsersManager> _mockApplicationUsersManager;
	private readonly Mock<IApplicationsManager> _mockApplicationsManager;
	private readonly Mock<IRolesManager> _mockRolesManager;
	private readonly Mock<ISelectOptionsServices> _mockSelectOptionsServices;
	private readonly Mock<ISettingsService> _mockSettingsService;
	private readonly Mock<ILogger<RegisterUserModel>> _mockLogger;
	private readonly Mock<IEncryptionService> _mockEncryptionService;
	private readonly Mock<IStringLocalizer<RegisterUserModel>> _mockLocalizer;
	private readonly RegisterUserModel _pageModel;

	public RegisterModelTests()
	{
		_mockUsersManager = new Mock<IUsersManager>();
		_mockApplicationUsersManager = new Mock<IApplicationUsersManager>();
		_mockApplicationsManager = new Mock<IApplicationsManager>();
		_mockRolesManager = new Mock<IRolesManager>();
		_mockSelectOptionsServices = new Mock<ISelectOptionsServices>();
		_mockSettingsService = new Mock<ISettingsService>();
		_mockLogger = new Mock<ILogger<RegisterUserModel>>();
		_mockEncryptionService = new Mock<IEncryptionService>();
		_mockLocalizer = new Mock<IStringLocalizer<RegisterUserModel>>();

		_pageModel = new RegisterUserModel(
			_mockLogger.Object,
			_mockUsersManager.Object,
			_mockApplicationUsersManager.Object,
			_mockApplicationsManager.Object,
			_mockEncryptionService.Object,
			_mockRolesManager.Object,
			_mockSelectOptionsServices.Object,
			_mockSettingsService.Object,
			_mockLocalizer.Object
		);
	}

	[Fact]
	public async Task OnPostAsync_ShouldRedirectToAuthenticatorPage_OnSuccessfulRegistration()
	{
		// Arrange
		_pageModel.Input = new RegisterUserInput
		{
			Email = "test@example.com",
			Password = "StrongPassword123!",
			PasswordConfirm = "StrongPassword123!",
			AcceptTerms = true,
			AcceptPrivacyPolicy = true,
			AcceptCookiePolicy = true
		};

		_ = _mockUsersManager.Setup(um => um.RegisterUserAsync(It.IsAny<RegisterUserInput>()))
						  .ReturnsAsync(new RegisterUserResult { UserCreated = true });

		// Act
		IActionResult result = await _pageModel.OnPostAsync();

		// Assert
		RedirectToPageResult redirectResult = Assert.IsType<RedirectToPageResult>(result);
		Assert.Equal("/Account/AuthenticatorUserRegistration", redirectResult.PageName);  // ✅ Fixed expected URL
	}

	[Fact]
	public async Task OnPostAsync_ShouldReturnPage_WhenPasswordConfirmationFails()
	{
		// Arrange
		_pageModel.Input = new RegisterUserInput
		{
			Email = "test@example.com",
			Password = "Password123!",
			PasswordConfirm = "DifferentPassword!"
		};

		// Act
		IActionResult result = await _pageModel.OnPostAsync();

		// Assert
		_ = Assert.IsType<PageResult>(result);
		Assert.Contains(_pageModel.ModelState, e => e.Value.Errors.Count > 0);
	}

	[Fact]
	public async Task OnPostAsync_ShouldReturnPage_WhenPolicyAcceptanceIsMissing()
	{
		// Arrange
		_pageModel.Input = new RegisterUserInput
		{
			Email = "test@example.com",
			Password = "Password123!",
			PasswordConfirm = "Password123!",
			AcceptTerms = false, // ❌ User did not accept terms
			AcceptPrivacyPolicy = true,
			AcceptCookiePolicy = true
		};

		// Act
		IActionResult result = await _pageModel.OnPostAsync();

		// Assert
		_ = Assert.IsType<PageResult>(result);
		Assert.Contains(_pageModel.ModelState, e => e.Value.Errors.Count > 0);
	}

	[Fact]
	public async Task OnPostAsync_ShouldReturnError_WhenUserAlreadyExists()
	{
		// Arrange
		_pageModel.Input = new RegisterUserInput
		{
			Email = "existing@example.com",
			Password = "Password123!",
			PasswordConfirm = "Password123!",
			AcceptTerms = true,
			AcceptPrivacyPolicy = true,
			AcceptCookiePolicy = true
		};

		_ = _mockUsersManager.Setup(um => um.IsEmailFreeAsync(It.IsAny<string>()))
						  .ReturnsAsync(false); // ❌ Email is already taken

		// Act
		IActionResult result = await _pageModel.OnPostAsync();

		// Assert
		_ = Assert.IsType<PageResult>(result);
		Assert.Contains(_pageModel.ModelState, e => e.Value.Errors.Count > 0);
	}

	[Fact]
	public async Task OnPostAsync_ShouldLogError_OnFailure()
	{
		// Arrange
		_pageModel.Input = new RegisterUserInput
		{
			Email = "test@example.com",
			Password = "StrongPassword123!",
			PasswordConfirm = "StrongPassword123!",
			AcceptTerms = true,
			AcceptPrivacyPolicy = true,
			AcceptCookiePolicy = true
		};

		_ = _mockUsersManager.Setup(um => um.RegisterUserAsync(It.IsAny<RegisterUserInput>()))
						  .ReturnsAsync(new RegisterUserResult { UserCreated = false, Errors = new List<string> { "Registration failed" } });

		List<string> logMessages = new List<string>();

		_ = _mockLogger.Setup(x => x.Log(
				  It.IsAny<LogLevel>(),
				  It.IsAny<EventId>(),
				  It.IsAny<object>(),
				  It.IsAny<Exception>(),
				  It.IsAny<Func<object, Exception, string>>()))
			 .Callback<LogLevel, EventId, object, Exception, Func<object, Exception, string>>((level, eventId, state, exception, formatter) =>
			 {
				 logMessages.Add(formatter(state, exception));
			 });

		// Act
		IActionResult result = await _pageModel.OnPostAsync();

		// Assert
		Assert.Contains(logMessages, msg => msg.Contains("Registration failed"));
		_ = Assert.IsType<PageResult>(result);
	}
}