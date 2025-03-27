namespace Id.Tests.Pages.Install;

public class LoginRulesModelTests : RazorPageTestBase<LoginRulesModel>
{
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

	[Fact]
	public async Task OnGetAsync_ShouldReturnPage_WhenValid()
	{
		LoginRulesModel page = CreatePageModel();
		IActionResult result = await page.OnGetAsync();

		_ = Assert.IsType<PageResult>(result);
		Assert.NotNull(page.Input);
		Assert.Equal("app123", page.Input!.ApplicationId);
	}

	[Fact]
	public async Task OnPostAsync_ShouldReturnPage_WhenModelInvalid()
	{
		LoginRulesModel page = CreatePageModel();
		page.ModelState.AddModelError("Host", "Required");

		IActionResult result = await page.OnPostAsync();

		_ = Assert.IsType<PageResult>(result);
	}

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