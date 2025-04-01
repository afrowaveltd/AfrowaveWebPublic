namespace Id.Tests.Pages.Install;

/// <summary>
/// Installation result page model tests
/// </summary>
public class InstallationResultModelTests : RazorPageTestBase<InstallationResultModel>
{
	/// <summary>
	/// Configures services for the test
	/// </summary>
	/// <param name="services">Services for the test</param>
	protected override void ConfigureServices(IServiceCollection services)
	{
		// 1. Registrace EF kontextu jako testovací InMemory databáze
		_ = services.AddDbContext<ApplicationDbContext>(options =>
		{
			_ = options.UseInMemoryDatabase("InstallationResultTestDb");
		});

		// 2. Ostatní služby
		IInstallationStatusService status = Substitute.For<IInstallationStatusService>();
		_ = status.ProperInstallState(InstalationSteps.Result).Returns(true);
		_ = services.AddSingleton(status);

		ITranslatorService translator = Substitute.For<ITranslatorService>();
		_ = translator.TranslateAsync("Brand Desc", Arg.Any<string>(), Arg.Any<string>()).Returns(new ApiResponse<string>() { Data = "Překlad Brand" });
		_ = translator.TranslateAsync("App Desc", Arg.Any<string>(), Arg.Any<string>()).Returns(new ApiResponse<string>() { Data = "Překlad App" });
		_ = services.AddSingleton(translator);

		ISettingsService settings = Substitute.For<ISettingsService>();
		_ = settings.GetSettingsAsync().Returns(new IdentificatorSettings());
		_ = services.AddSingleton(settings);

		IBrandsManager brands = Substitute.For<IBrandsManager>();
		_ = brands.GetIconPath(1).Returns("/icons/brand.svg");
		_ = services.AddSingleton(brands);

		IApplicationsManager apps = Substitute.For<IApplicationsManager>();
		_ = apps.GetIconPath("app1").Returns("/icons/app.svg");
		_ = services.AddSingleton(apps);

		_ = services.AddSingleton(Substitute.For<ILogger<InstallationResultModel>>());
		_ = services.AddSingleton(Substitute.For<IStringLocalizer<InstallationResultModel>>());
	}

	/// <summary>
	/// Creates the page model
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task OnGetAsync_ShouldRedirect_WhenInstallStateInvalid()
	{
		IInstallationStatusService status = Services.GetRequiredService<IInstallationStatusService>();
		_ = status.ProperInstallState(InstalationSteps.Result).Returns(false);

		InstallationResultModel page = CreatePageModel();
		IActionResult result = await page.OnGetAsync();

		RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
		Assert.Equal("/", redirect.PageName);
	}

	/// <summary>
	/// Creates the page model
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task OnPostAsync_ShouldMarkAsFinished_AndRedirectToIndex()
	{
		ISettingsService settings = Services.GetRequiredService<ISettingsService>();
		IdentificatorSettings captured = new IdentificatorSettings();

		settings
			.When(s => s.SetSettingsAsync(Arg.Any<IdentificatorSettings>()))
			.Do(call => captured = call.Arg<IdentificatorSettings>());

		InstallationResultModel page = CreatePageModel();
		IActionResult result = await page.OnPostAsync();

		RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
		Assert.Equal("/Index", redirect.PageName);
		Assert.True(captured.InstallationFinished);
	}
}