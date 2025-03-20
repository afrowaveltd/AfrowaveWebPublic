using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Id.Tests.Pages.Account;

/// <summary>
/// Contains tests for the RegisterUserModel.
/// </summary>
public class RegisterModelTests
{
	private readonly IUsersManager _mockUsersManager;
	private readonly IApplicationUsersManager _mockApplicationUsersManager;
	private readonly IApplicationsManager _mockApplicationsManager;
	private readonly IRolesManager _mockRolesManager;
	private readonly ISelectOptionsServices _mockSelectOptionsServices;
	private readonly ISettingsService _mockSettingsService;
	private readonly ILogger<RegisterUserModel> _mockLogger;
	private readonly IEncryptionService _mockEncryptionService;
	private readonly IStringLocalizer<RegisterUserModel> _mockLocalizer;
	private readonly RegisterUserModel _pageModel;

	/// <summary>
	/// Initializes a new instance of the <see cref="RegisterModelTests"/> class.
	/// </summary>
	public RegisterModelTests()
	{
		_mockUsersManager = Substitute.For<IUsersManager>();
		_mockApplicationUsersManager = Substitute.For<IApplicationUsersManager>();
		_mockApplicationsManager = Substitute.For<IApplicationsManager>();
		_mockRolesManager = Substitute.For<IRolesManager>();
		_mockSelectOptionsServices = Substitute.For<ISelectOptionsServices>();
		_mockSettingsService = Substitute.For<ISettingsService>();
		_mockLogger = Substitute.For<ILogger<RegisterUserModel>>();
		_mockEncryptionService = Substitute.For<IEncryptionService>();
		_mockLocalizer = Substitute.For<IStringLocalizer<RegisterUserModel>>();

		_pageModel = new RegisterUserModel(
			_mockLogger,
			_mockUsersManager,
			_mockApplicationUsersManager,
			_mockApplicationsManager,
			_mockEncryptionService,
			_mockRolesManager,
			_mockSelectOptionsServices,
			_mockSettingsService,
			_mockLocalizer
		);

		_ = _mockApplicationsManager.ApplicationExistsAsync(Arg.Any<string>()).Returns(true);
		_ = _mockEncryptionService.HashPasswordAsync("StrongPassword123!").Returns("encryptedValue");
		_ = _mockEncryptionService.HashPasswordAsync("DifferentPassword!").Returns("differentValue");
		_ = _mockSettingsService.GetPasswordRulesAsync().Returns(new PasswordRules());
		_ = _mockSettingsService.GetLoginRulesAsync().Returns(new LoginRules());
	}

	private void WithValidModelState()
	{
		_pageModel.ModelState.Clear();
		_pageModel.Input = new RegisterUserInput
		{
			Email = "test@example.com",
			Password = "StrongPassword123!",
			PasswordConfirm = "StrongPassword123!",
			FirstName = "John",
			LastName = "Doe",
			DisplayedName = "JohnDoe",
			AcceptTerms = true,
			AcceptPrivacyPolicy = true,
			AcceptCookiePolicy = true,
			ApplicationId = "TestApp123"
		};
	}

	/// <summary>
	/// Tests the OnPostAsync method to ensure it redirects to the authenticator page on successful registration.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task OnPostAsync_ShouldRedirectToAuthenticatorPage_OnSuccessfulRegistration()
	{
		WithValidModelState();
		_ = _mockUsersManager.RegisterUserAsync(Arg.Any<RegisterUserInput>())
			.Returns(new RegisterUserResult { UserCreated = true });

		_pageModel.ApplicationId = "testAppId";

		IActionResult result = await _pageModel.OnPostAsync();

		_ = Assert.IsType<RedirectToPageResult>(result);
	}

	/// <summary>
	/// Tests the OnPostAsync method to ensure it returns the page on password confirmation failure.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task OnPostAsync_ShouldReturnPage_WhenPasswordConfirmationFails()
	{
		WithValidModelState();
		_pageModel.Input.PasswordConfirm = "DifferentPassword!";
		_ = _mockUsersManager.RegisterUserAsync(Arg.Any<RegisterUserInput>())
			.Returns(new RegisterUserResult { UserCreated = false, Errors = new List<string> { "Password don't match" } });

		IActionResult result = await _pageModel.OnPostAsync();

		_ = Assert.IsType<PageResult>(result);
	}

	/// <summary>
	/// Tests the OnPostAsync method to ensure it returns the page on policy acceptance failure.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task OnPostAsync_ShouldReturnPage_WhenPolicyAcceptanceIsMissing()
	{
		WithValidModelState();
		_pageModel.Input.AcceptTerms = false;

		_ = _mockUsersManager.RegisterUserAsync(Arg.Any<RegisterUserInput>())
			.Returns(new RegisterUserResult { UserCreated = false, Errors = new List<string> { "Terms must be accepted" } });

		IActionResult result = await _pageModel.OnPostAsync();

		_ = Assert.IsType<PageResult>(result);
	}

	/// <summary>
	/// Tests the OnPostAsync method to ensure it returns the page on user creation failure.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task OnPostAsync_ShouldReturnError_WhenUserAlreadyExists()
	{
		WithValidModelState();
		_ = _mockUsersManager.IsEmailFreeAsync(Arg.Any<string>()).Returns(false);

		_ = _mockUsersManager.RegisterUserAsync(Arg.Any<RegisterUserInput>())
			.Returns(new RegisterUserResult { UserCreated = false, Errors = new List<string> { "User already exists" } });

		IActionResult result = await _pageModel.OnPostAsync();

		_ = Assert.IsType<PageResult>(result);
	}

	/// <summary>
	/// Tests the OnPostAsync method to ensure it logs an error on failure.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task OnPostAsync_ShouldLogError_OnFailure()
	{
		WithValidModelState();
		_ = _mockUsersManager.RegisterUserAsync(Arg.Any<RegisterUserInput>())
			.Returns(new RegisterUserResult { UserCreated = false, Errors = new List<string> { "Registration failed" } });

		IActionResult result = await _pageModel.OnPostAsync();

		_mockLogger.Received(1).Log(
				LogLevel.Error,
				Arg.Any<EventId>(),
				Arg.Any<object>(),
				Arg.Any<Exception>(),
				Arg.Any<Func<object, Exception, string>>()
		);

		_ = Assert.IsType<PageResult>(result);
	}
}