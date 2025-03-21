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

		_ = _mockApplicationsManager.Setup(am => am.ApplicationExistsAsync(It.IsAny<string>()))
										.ReturnsAsync(true);
		_ = _mockEncryptionService.Setup(x => x.EncryptTextAsync(It.IsAny<string>(), It.IsAny<string>()))
									  .Returns("encryptedValue");
		_ = _mockSettingsService.Setup(x => x.GetPasswordRulesAsync()).ReturnsAsync(new PasswordRules());
		_ = _mockSettingsService.Setup(x => x.GetLoginRulesAsync()).ReturnsAsync(new LoginRules());
	}

	private void InitializeValidInput()
	{
		if(string.IsNullOrEmpty(_pageModel.ApplicationId))
		{
			_pageModel.ApplicationId = "testAppId";
		}
		_pageModel.ModelState.Clear();
		if(_pageModel.Input == null)
		{
			_pageModel.Input = new RegisterUserInput
			{
				Email = "test@example.com",
				Password = "StrongPassword123!",
				PasswordConfirm = "StrongPassword123!",
				AcceptTerms = true,
				AcceptPrivacyPolicy = true,
				AcceptCookiePolicy = true
			};
		}
		_ = _mockUsersManager.Setup(um => um.IsEmailFreeAsync(It.IsAny<string>()))
							  .ReturnsAsync(true);
	}

	[Fact]
	public async Task OnPostAsync_ShouldRedirectToAuthenticatorPage_OnSuccessfulRegistration()
	{
		InitializeValidInput();
		_ = _mockUsersManager.Setup(um => um.RegisterUserAsync(It.IsAny<RegisterUserInput>()))
							  .ReturnsAsync(new RegisterUserResult { UserCreated = true });

		if(_pageModel.Input == null)
		{
			InitializeValidInput();
		}
		_pageModel.ApplicationId = "testAppId";
		_pageModel.ModelState.Clear();

		IActionResult result = await _pageModel.OnPostAsync();
		_ = Assert.IsType<RedirectToPageResult>(result);
	}

	[Fact]
	public async Task OnPostAsync_ShouldReturnPage_WhenPasswordConfirmationFails()
	{
		// Arrange
		WithValidModelState();
		_pageModel.ApplicationId = "testAppId";

		// Ensure Input is set
		if(_pageModel.Input == null)
		{
			WithValidModelState();
		}

		_pageModel.Input.PasswordConfirm = "DifferentPassword!";

		// Act
		IActionResult result = await _pageModel.OnPostAsync();

		// Assert
		_ = Assert.IsType<PageResult>(result);
	}

	[Fact]
	public async Task OnPostAsync_ShouldReturnPage_WhenPolicyAcceptanceIsMissing()
	{
		// Arrange
		WithValidModelState();
		_pageModel.ApplicationId = "testAppId";

		// Ensure Input is set
		if(_pageModel.Input == null)
		{
			WithValidModelState();
		}

		_pageModel.Input.AcceptTerms = false;

		// Act
		IActionResult result = await _pageModel.OnPostAsync();

		// Assert
		_ = Assert.IsType<PageResult>(result);
	}

	[Fact]
	public async Task OnPostAsync_ShouldReturnError_WhenUserAlreadyExists()
	{
		// Arrange
		WithValidModelState();
		_pageModel.ApplicationId = "testAppId";

		_mockUsersManager.IsEmailFreeAsync(Arg.Any<string>()).Returns(false);

		// Act
		IActionResult result = await _pageModel.OnPostAsync();

		// Assert
		_ = Assert.IsType<PageResult>(result);
	}

	[Fact]
	public async Task OnPostAsync_ShouldLogError_OnFailure()
	{
		InitializeValidInput();
		_ = _mockUsersManager.Setup(um => um.RegisterUserAsync(It.IsAny<RegisterUserInput>()))
							  .ReturnsAsync(new RegisterUserResult { UserCreated = false, Errors = new List<string> { "Registration failed" } });

		IActionResult result = await _pageModel.OnPostAsync();

		_mockLogger.Verify(
			 x => x.Log(
				  LogLevel.Error,
				  It.IsAny<EventId>(),
				  It.IsAny<object>(),
				  It.IsAny<Exception>(),
				  (Func<object, Exception?, string>)It.IsAny<object>()
			 ),
			 Times.AtLeastOnce()
		);

		_ = Assert.IsType<PageResult>(result);
	}
}