using Microsoft.AspNetCore.Mvc.Rendering;

namespace Id.Tests.Pages.Install;

/// <summary>
/// Tests for the CORS settings page model
/// </summary>
public class CorsSettingsModelTests : RazorPageTestBase<CorsSettingsModel>
{
	/// <summary>
	/// Configures services for dependency injection, setting up mock implementations for various services.
	/// </summary>
	/// <param name="services"></param>
	protected override void ConfigureServices(IServiceCollection services)
	{
		ISettingsService settingsService = Substitute.For<ISettingsService>();
		_ = settingsService.GetSettingsAsync().Returns(new IdentificatorSettings());

		IInstallationStatusService status = Substitute.For<IInstallationStatusService>();
		_ = status.ProperInstallState(InstalationSteps.CorsSettings).Returns(true);

		ISelectOptionsServices selectOptions = Substitute.For<ISelectOptionsServices>();
		_ = selectOptions.GetBinaryOptionsAsync(true).Returns(Task.FromResult(new List<SelectListItem>
		{
			new() { Value = "true", Text = "Yes" },
			new() { Value = "false", Text = "No" }
		}));
		_ = selectOptions.GetHttpMethodsAsync(Arg.Any<List<string>>()).Returns(Task.FromResult(new List<SelectListItem>
		{
			new() { Value = "GET", Text = "GET" },
			new() { Value = "POST", Text = "POST" }
		}));
		_ = selectOptions.GetHttpHeadersAsync(Arg.Any<List<string>>()).Returns(Task.FromResult(new List<SelectListItem>
		{
			new() { Value = "Authorization", Text = "Authorization" },
			new() { Value = "Content-Type", Text = "Content-Type" }
		}));

		_ = services.AddSingleton(settingsService);
		_ = services.AddSingleton(status);
		_ = services.AddSingleton(selectOptions);
		_ = services.AddSingleton(Substitute.For<IStringLocalizer<CorsSettingsModel>>());
		_ = services.AddSingleton(Substitute.For<ILogger<CorsSettingsModel>>());
	}

	/// <summary>
	/// Tests the OnGetAsync method to ensure it redirects when the installation state is invalid.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task OnGetAsync_ShouldRedirect_WhenInvalidInstallState()
	{
		IInstallationStatusService status = Services.GetRequiredService<IInstallationStatusService>();
		_ = status.ProperInstallState(InstalationSteps.CorsSettings).Returns(false);

		CorsSettingsModel page = CreatePageModel();
		IActionResult result = await page.OnGetAsync();

		RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
		Assert.Equal("/", redirect.PageName);
	}

	/// <summary>
	/// Tests the OnGetAsync method to ensure it returns the page and loads options.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task OnGetAsync_ShouldReturnPage_AndLoadOptions()
	{
		CorsSettingsModel page = CreatePageModel();
		IActionResult result = await page.OnGetAsync();

		_ = Assert.IsType<PageResult>(result);
		Assert.NotEmpty(page.HttpMethodsOptions);
		Assert.NotEmpty(page.HttpHeadersOptions);
		Assert.NotEmpty(page.AllowAnyHeaderOptions);
		Assert.NotEmpty(page.AllowAnyMethodOptions);
		Assert.NotEmpty(page.AllowAnyOriginOptions);
		Assert.NotEmpty(page.AllowCredentialsOptions);
		Assert.NotNull(page.Input);
	}

	/// <summary>
	/// Tests the OnPostAsync method to ensure it redirects when the installation state is invalid.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task OnPostAsync_ShouldRedirect_WhenInvalidInstallState()
	{
		IInstallationStatusService status = Services.GetRequiredService<IInstallationStatusService>();
		_ = status.ProperInstallState(InstalationSteps.CorsSettings).Returns(false);

		CorsSettingsModel page = CreatePageModel();
		IActionResult result = await page.OnPostAsync();

		RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
		Assert.Equal("/", redirect.PageName);
	}

	/// <summary>
	/// Tests the OnPostAsync method to ensure it returns the page when the model is invalid.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task OnPostAsync_ShouldReturnPage_WhenModelInvalid()
	{
		CorsSettingsModel page = CreatePageModel();
		page.ModelState.AddModelError("AllowedOrigins", "Required");

		IActionResult result = await page.OnPostAsync();

		_ = Assert.IsType<PageResult>(result);
	}

	/// <summary>
	/// Tests the OnPostAsync method to ensure it saves the CORS settings and redirects.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task OnPostAsync_ShouldSaveCorsSettings_AndRedirect()
	{
		CorsSettingsModel page = CreatePageModel();
		page.Input = new CorsSettingsModel.InputModel
		{
			PolicyMode = CorsPolicyMode.AllowSpecific,
			AllowedOrigins = new() { "https://example.com" },
			AllowAnyMethod = false,
			AllowedMethods = new() { "GET", "POST" },
			AllowAnyHeader = false,
			AllowedHeaders = new() { "Authorization", "Content-Type" },
			AllowCredentials = true
		};

		page.ModelState.Clear();

		IActionResult result = await page.OnPostAsync();

		RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
		Assert.Equal("/Install/InstallationResult", redirect.PageName);

		ISettingsService service = Services.GetRequiredService<ISettingsService>();
		await service.Received(1).SetSettingsAsync(Arg.Is<IdentificatorSettings>(s =>
			s.CorsSettings.IsConfigured &&
			s.CorsSettings.PolicyMode == CorsPolicyMode.AllowSpecific &&
			s.CorsSettings.AllowedOrigins.Contains("https://example.com") &&
			s.CorsSettings.AllowedMethods.Contains("GET") &&
			s.CorsSettings.AllowCredentials &&
			!s.CorsSettings.AllowAnyMethod &&
			!s.CorsSettings.AllowAnyHeader
		));
	}
}