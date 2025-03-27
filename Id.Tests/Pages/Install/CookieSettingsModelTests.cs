using Microsoft.AspNetCore.Mvc.Rendering;

namespace Id.Tests.Pages.Install;

public class CookieSettingsModelTests : RazorPageTestBase<CookieSettingsModel>
{
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

	[Fact]
	public async Task OnPostAsync_ShouldReturnPage_WhenModelInvalid()
	{
		var page = CreatePageModel();
		page.ModelState.AddModelError("CookieName", "Required");

		var result = await page.OnPostAsync();

		Assert.IsType<PageResult>(result);
	}

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