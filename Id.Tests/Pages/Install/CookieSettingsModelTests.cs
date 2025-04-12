using Microsoft.AspNetCore.Mvc.Rendering;

namespace Id.Tests.Pages.Install;

/// <summary>
/// Tests the CookieSettingsModel class, verifying redirection and page loading based on installation state and model
/// validity.
/// Also checks settings saving functionality.
/// </summary>
public class CookieSettingsModelTests : RazorPageTestBase<CookieSettingsModel>
{
	/// <summary>
	/// Configures services for dependency injection, setting up various service mocks for testing purposes.
	/// </summary>
	/// <param name="services">Facilitates the registration of service instances within the application's service container.</param>
	protected override void ConfigureServices(IServiceCollection services)
	{
		var settings = Substitute.For<ISettingsService>();
		settings.GetSettingsAsync().Returns(new IdentificatorSettings());

		var status = Substitute.For<IInstallationStatusService>();
		status.ProperInstallState(InstalationSteps.CookieSettings).Returns(true);

		var select = Substitute.For<ISelectOptionsServices>();
		select.GetBinaryOptionsAsync(true).Returns(Task.FromResult(new List<SelectListItem>
		{
			new() { Value = "true", Text = "Yes" },
			new() { Value = "false", Text = "No" }
		}));
		select.GetSameSiteModeOptionsAsync(SameSiteMode.Lax).Returns(Task.FromResult(new List<SelectListItem>
		{
			new() { Value = "Lax", Text = "Lax" }
		}));

		services.AddSingleton(settings);
		services.AddSingleton(status);
		services.AddSingleton(select);
		services.AddSingleton(Substitute.For<IStringLocalizer<CookieSettingsModel>>());
		services.AddSingleton(Substitute.For<ILogger<CookieSettingsModel>>());
	}

	/// <summary>
	/// Tests the OnGetAsync method to ensure it redirects when the installation state is invalid.
	/// </summary>
	/// <returns>Returns a RedirectToPageResult that redirects to the home page.</returns>
	[Fact]
	public async Task OnGetAsync_ShouldRedirect_WhenInstallStateInvalid()
	{
		var status = Services.GetRequiredService<IInstallationStatusService>();
		status.ProperInstallState(InstalationSteps.CookieSettings).Returns(false);

		var page = CreatePageModel();
		var result = await page.OnGetAsync();

		var redirect = Assert.IsType<RedirectToPageResult>(result);
		Assert.Equal("/", redirect.PageName);
	}

	/// <summary>
	/// Handles the GET request asynchronously, returning a page and loading various cookie options.
	/// </summary>
	/// <returns>Returns a PageResult indicating the success of the page load.</returns>
	[Fact]
	public async Task OnGetAsync_ShouldReturnPage_AndLoadOptions()
	{
		var page = CreatePageModel();
		var result = await page.OnGetAsync();

		Assert.IsType<PageResult>(result);
		Assert.NotEmpty(page.CookieSameSiteOptions);
		Assert.NotEmpty(page.CookieSecureOptions);
		Assert.NotEmpty(page.CookieHttpOnlyOptions);
	}

	/// <summary>
	/// Tests the OnPostAsync method to ensure it redirects when the installation state is invalid.
	/// </summary>
	/// <returns>Returns a RedirectToPageResult that redirects to the home page.</returns>
	[Fact]
	public async Task OnPostAsync_ShouldRedirect_WhenInstallStateInvalid()
	{
		var status = Services.GetRequiredService<IInstallationStatusService>();
		status.ProperInstallState(InstalationSteps.CookieSettings).Returns(false);

		var page = CreatePageModel();
		var result = await page.OnPostAsync();

		var redirect = Assert.IsType<RedirectToPageResult>(result);
		Assert.Equal("/", redirect.PageName);
	}

	/// <summary>
	/// Tests the OnPostAsync method to ensure it returns a PageResult when the model state is invalid due to a missing
	/// cookie name.
	/// </summary>
	/// <returns>Returns a PageResult indicating the page should be rendered again with validation errors.</returns>
	[Fact]
	public async Task OnPostAsync_ShouldReturnPage_WhenModelInvalid()
	{
		var page = CreatePageModel();
		page.ModelState.AddModelError("CookieName", "Required");

		var result = await page.OnPostAsync();

		Assert.IsType<PageResult>(result);
	}

	/// <summary>
	/// Handles the asynchronous posting of settings, saving cookie configurations and redirecting to a specified page.
	/// </summary>
	/// <returns>Returns a redirect result to the '/Install/JwtSettings' page.</returns>
	[Fact]
	public async Task OnPostAsync_ShouldSaveSettings_AndRedirect()
	{
		var page = CreatePageModel();
		page.Input = new CookieSettingsModel.InputModel
		{
			CookieName = ".MyAuth",
			CookieDomain = "localhost",
			CookiePath = "/auth",
			CookieSecure = true,
			CookieSameSite = SameSiteMode.Strict,
			CookieHttpOnly = true,
			CookieExpiration = 120
		};

		page.ModelState.Clear(); // valid

		var result = await page.OnPostAsync();

		var redirect = Assert.IsType<RedirectToPageResult>(result);
		Assert.Equal("/Install/JwtSettings", redirect.PageName);

		var service = Services.GetRequiredService<ISettingsService>();
		await service.Received(1).SetSettingsAsync(Arg.Is<IdentificatorSettings>(s =>
			s.CookieSettings.IsConfigured &&
			s.CookieSettings.Name == ".MyAuth" &&
			s.CookieSettings.Domain == "localhost" &&
			s.CookieSettings.Path == "/auth" &&
			s.CookieSettings.Secure &&
			s.CookieSettings.HttpOnly &&
			s.CookieSettings.Expiration == 120 &&
			s.CookieSettings.SameSite == SameSiteMode.Strict
		));
	}
}