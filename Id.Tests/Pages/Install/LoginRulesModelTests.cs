namespace Id.Tests.Pages.Install;

/// <summary>
/// Tests the LoginRulesModel for various scenarios including redirection on invalid install state and handling of valid
/// and invalid model states.
/// Verifies settings are correctly captured and applied.
/// </summary>
public class LoginRulesModelTests : RazorPageTestBase<LoginRulesModel>
{
	/// <summary>
	/// Configures services for dependency injection, setting up mock implementations for various services.
	/// </summary>
	/// <param name="services">Facilitates the registration of services that can be injected into other components.</param>
	protected override void ConfigureServices(IServiceCollection services)
	{
		IdentificatorSettings mockSettings = new IdentificatorSettings
		{
			ApplicationId = "app123"
		};

		ISettingsService settingsService = Substitute.For<ISettingsService>();
		_ = settingsService.GetSettingsAsync().Returns(mockSettings);

		IInstallationStatusService status = Substitute.For<IInstallationStatusService>();
		_ = status.ProperInstallState(InstalationSteps.LoginRules).Returns(true);

		_ = services.AddSingleton(settingsService);
		_ = services.AddSingleton(status);
		_ = services.AddSingleton(Substitute.For<IStringLocalizer<LoginRulesModel>>());
		_ = services.AddSingleton(Substitute.For<ILogger<LoginRulesModel>>());
	}

	/// <summary>
	/// Tests the OnGetAsync method to ensure it redirects when the installation state is invalid.
	/// </summary>
	/// <returns>Returns a RedirectToPageResult that points to the SmtpSettings page.</returns>
	[Fact]
	public async Task OnGetAsync_ShouldRedirect_WhenInstallStateInvalid()
	{
		IInstallationStatusService status = Services.GetRequiredService<IInstallationStatusService>();
		_ = status.ProperInstallState(InstalationSteps.LoginRules).Returns(false);

		LoginRulesModel page = CreatePageModel();
		IActionResult result = await page.OnGetAsync();

		RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
		Assert.Equal("/Install/SmtpSettings", redirect.PageName);
	}

	/// <summary>
	/// Handles the GET request for the page and returns the page result when the input is valid. It verifies that the
	/// input is not null and matches expected values.
	/// </summary>
	/// <returns>Returns a PageResult indicating the page was successfully retrieved.</returns>
	[Fact]
	public async Task OnGetAsync_ShouldReturnPage_WhenValid()
	{
		LoginRulesModel page = CreatePageModel();
		IActionResult result = await page.OnGetAsync();

		_ = Assert.IsType<PageResult>(result);
		Assert.NotNull(page.Input);
		Assert.Equal("app123", page.Input!.ApplicationId);
	}

	/// <summary>
	/// Tests the OnPostAsync method to ensure it returns a PageResult when the model state is invalid due to a missing
	/// required field.
	/// </summary>
	/// <returns>Returns a PageResult indicating the page should be displayed with validation errors.</returns>
	[Fact]
	public async Task OnPostAsync_ShouldReturnPage_WhenModelInvalid()
	{
		LoginRulesModel page = CreatePageModel();
		page.ModelState.AddModelError("Host", "Required");

		IActionResult result = await page.OnPostAsync();

		_ = Assert.IsType<PageResult>(result);
	}

	/// <summary>
	/// Tests the OnPostAsync method to ensure it redirects correctly when valid input is provided.
	/// </summary>
	/// <returns>Returns a RedirectToPageResult indicating the page to redirect to after successful processing.</returns>
	[Fact]
	public async Task OnPostAsync_ShouldRedirect_WhenValid()
	{
		IdentificatorSettings capturedSettings = new IdentificatorSettings();
		ISettingsService settingsService = Services.GetRequiredService<ISettingsService>();
		await settingsService.SetSettingsAsync(Arg.Do<IdentificatorSettings>(s =>
		{
			capturedSettings = s;
		}));

		LoginRulesModel page = CreatePageModel();
		page.Input = new LoginRulesModel.InputModel
		{
			ApplicationId = "app123",
			MaxFailedLoginAttempts = 4,
			LockoutTime = 10,
			PasswordResetTokenExpiration = 20,
			OTPTokenExpiration = 30,
			RequireConfirmedEmail = false
		};

		page.ModelState.Clear();

		IActionResult result = await page.OnPostAsync();

		RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
		Assert.Equal("/Install/PasswordRules", redirect.PageName);

		Assert.Equal(4, capturedSettings.LoginRules.MaxFailedLoginAttempts);
		Assert.Equal(10, capturedSettings.LoginRules.LockoutTime);
		Assert.Equal(20, capturedSettings.LoginRules.PasswordResetTokenExpiration);
		Assert.Equal(30, capturedSettings.LoginRules.OTPTokenExpiration);
		Assert.False(capturedSettings.LoginRules.RequireConfirmedEmail);
		Assert.True(capturedSettings.LoginRules.IsConfigured);
	}
}