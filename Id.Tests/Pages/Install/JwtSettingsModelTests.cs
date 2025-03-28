namespace Id.Tests.Pages.Install;

/// <summary>
/// JWT settings model tests for various scenarios including redirection on invalid install state and handling of valid
/// </summary>
public class JwtSettingsModelTests : RazorPageTestBase<JwtSettingsModel>
{
	/// <summary>
	/// Configures services for dependency injection, setting up mock implementations for various services.
	/// </summary>
	/// <param name="services">Services mocked for the test</param>
	protected override void ConfigureServices(IServiceCollection services)
	{
		ISettingsService settings = Substitute.For<ISettingsService>();
		_ = settings.GetSettingsAsync().Returns(new IdentificatorSettings());

		IInstallationStatusService status = Substitute.For<IInstallationStatusService>();
		_ = status.ProperInstallState(InstalationSteps.JwtSettings).Returns(true);

		_ = services.AddSingleton(settings);
		_ = services.AddSingleton(status);
		_ = services.AddSingleton(Substitute.For<IStringLocalizer<JwtSettingsModel>>());
		_ = services.AddSingleton(Substitute.For<ILogger<JwtSettingsModel>>());
	}

	/// <summary>
	/// Tests the OnGetAsync method to ensure it redirects when the installation state is invalid.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task OnGetAsync_ShouldRedirect_WhenInstallStateInvalid()
	{
		IInstallationStatusService status = Services.GetRequiredService<IInstallationStatusService>();
		_ = status.ProperInstallState(InstalationSteps.JwtSettings).Returns(false);

		JwtSettingsModel page = CreatePageModel();
		IActionResult result = await page.OnGetAsync();

		RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
		Assert.Equal("/", redirect.PageName);
	}

	/// <summary>
	/// Handles the GET request for the page and returns the page result when the input is valid.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task OnGetAsync_ShouldReturnPage_WhenValid()
	{
		JwtSettingsModel page = CreatePageModel();
		IActionResult result = await page.OnGetAsync();

		_ = Assert.IsType<PageResult>(result);
	}

	/// <summary>
	/// Handles the POST request for the page and returns the page result when the model is invalid.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task OnPostAsync_ShouldReturnPage_WhenModelInvalid()
	{
		JwtSettingsModel page = CreatePageModel();
		page.ModelState.AddModelError("Issuer", "Required");

		IActionResult result = await page.OnPostAsync();

		_ = Assert.IsType<PageResult>(result);
	}

	/// <summary>
	/// Handles the POST request for the page and saves the settings when the model is valid.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task OnPostAsync_ShouldSaveSettings_AndRedirect()
	{
		IdentificatorSettings captured = new IdentificatorSettings();
		ISettingsService settingsService = Services.GetRequiredService<ISettingsService>();
		settingsService
			.When(x => x.SetSettingsAsync(Arg.Any<IdentificatorSettings>()))
			.Do(ctx => captured = ctx.Arg<IdentificatorSettings>());

		JwtSettingsModel page = CreatePageModel();
		page.Input = new JwtSettingsModel.InputModel
		{
			Issuer = "Afrowave",
			Audience = "Users",
			AccessTokenExpiration = 30,
			RefreshTokenExpiration = 14
		};

		page.ModelState.Clear();

		IActionResult result = await page.OnPostAsync();

		RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
		Assert.Equal("/Install/CorsSettings", redirect.PageName);

		Assert.True(captured.JwtSettings.IsConfigured);
		Assert.Equal("Afrowave", captured.JwtSettings.Issuer);
		Assert.Equal("Users", captured.JwtSettings.Audience);
		Assert.Equal(30, captured.JwtSettings.AccessTokenExpiration);
		Assert.Equal(14, captured.JwtSettings.RefreshTokenExpiration);
	}
}