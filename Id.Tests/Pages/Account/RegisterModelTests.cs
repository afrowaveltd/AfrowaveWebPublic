using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Id.Tests.Pages.Account;

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

		_mockApplicationsManager.ApplicationExistsAsync(Arg.Any<string>()).Returns(true);
		_mockEncryptionService.HashPasswordAsync(Arg.Any<string>()).Returns("encryptedValue");
		_mockSettingsService.GetPasswordRulesAsync().Returns(new PasswordRules());
		_mockSettingsService.GetLoginRulesAsync().Returns(new LoginRules());
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
			AcceptCookiePolicy = true
		};
	}

	[Fact]
	public async Task OnPostAsync_ShouldRedirectToAuthenticatorPage_OnSuccessfulRegistration()
	{
		WithValidModelState();
		_mockUsersManager.RegisterUserAsync(Arg.Any<RegisterUserInput>())
			.Returns(new RegisterUserResult { UserCreated = true });

		_pageModel.ApplicationId = "testAppId";

		IActionResult result = await _pageModel.OnPostAsync();

		Assert.IsType<RedirectToPageResult>(result);
	}

	[Fact]
	public async Task OnPostAsync_ShouldReturnPage_WhenPasswordConfirmationFails()
	{
		WithValidModelState();
		_pageModel.Input.PasswordConfirm = "DifferentPassword!";
		_mockUsersManager.RegisterUserAsync(Arg.Any<RegisterUserInput>())
			.Returns(new RegisterUserResult { UserCreated = false, Errors = new List<string> { "Password don't match" } });

		IActionResult result = await _pageModel.OnPostAsync();

		Assert.IsType<PageResult>(result);
	}

	[Fact]
	public async Task OnPostAsync_ShouldReturnPage_WhenPolicyAcceptanceIsMissing()
	{
		WithValidModelState();
		_pageModel.Input.AcceptTerms = false;

		_mockUsersManager.RegisterUserAsync(Arg.Any<RegisterUserInput>())
			.Returns(new RegisterUserResult { UserCreated = false, Errors = new List<string> { "Terms must be accepted" } });

		IActionResult result = await _pageModel.OnPostAsync();

		Assert.IsType<PageResult>(result);
	}

	[Fact]
	public async Task OnPostAsync_ShouldReturnError_WhenUserAlreadyExists()
	{
		WithValidModelState();
		_mockUsersManager.IsEmailFreeAsync(Arg.Any<string>()).Returns(false);

		_mockUsersManager.RegisterUserAsync(Arg.Any<RegisterUserInput>())
			.Returns(new RegisterUserResult { UserCreated = false, Errors = new List<string> { "User already exists" } });

		IActionResult result = await _pageModel.OnPostAsync();

		Assert.IsType<PageResult>(result);
	}

	[Fact]
	public async Task OnPostAsync_ShouldLogError_OnFailure()
	{
		WithValidModelState();
		_mockUsersManager.RegisterUserAsync(Arg.Any<RegisterUserInput>())
			.Returns(new RegisterUserResult { UserCreated = false, Errors = new List<string> { "Registration failed" } });

		IActionResult result = await _pageModel.OnPostAsync();

		_mockLogger.Received(1).Log(
				LogLevel.Error,
				Arg.Any<EventId>(),
				Arg.Any<object>(),
				Arg.Any<Exception>(),
				(Arg.Any<Func<object, Exception, string>>())
		);

		Assert.IsType<PageResult>(result);
	}
}