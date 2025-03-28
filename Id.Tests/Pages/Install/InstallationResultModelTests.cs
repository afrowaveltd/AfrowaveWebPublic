namespace Id.Tests.Pages.Install;

public class InstallationResultModelTests : RazorPageTestBase<InstallationResultModel>
{
	protected override void ConfigureServices(IServiceCollection services)
	{
		// 1. Registrace EF kontextu jako testovací InMemory databáze
		_ = services.AddDbContext<ApplicationDbContext>(options =>
		{
			_ = options.UseInMemoryDatabase("InstallationResultTestDb");
		});

		// 2. Ostatní služby
		IInstallationStatusService status = Substitute.For<IInstallationStatusService>();
		_ = status.ProperInstallState(InstalationSteps.InstallationResult).Returns(true);
		_ = services.AddSingleton(status);

		ITranslatorService translator = Substitute.For<ITranslatorService>();
		translator.TranslateAsync("Brand Desc", Arg.Any<string>()).Returns("Překlad Brand");
		translator.TranslateAsync("App Desc", Arg.Any<string>()).Returns("Překlad App");
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

	// 3. Naplnění testovací databáze po sestavení DI
	private void SeedTestData()
	{
		using IServiceScope scope = Services.CreateScope();
		ApplicationDbContext db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

		_ = db.Users.Add(new User
		{
			Id = "user1",
			DisplayName = "Admin",
			Email = "admin@example.com",
			Password = "x", // required
			Firstname = "admin", // required
			Lastname = "admin"
		});

		_ = db.Brands.Add(new Brand
		{
			Id = 1,
			Name = "Afrowave Brand",
			Description = "Brand Desc",
			OwnerId = "user1"
		});

		_ = db.Applications.Add(new Application
		{
			Id = "app1",
			Name = "Afrowave App",
			Description = "App Desc",
			OwnerId = "user1",
			BrandId = 1,
			ApplicationEmail = "noreply@afrowave.io" // required
		});

		_ = db.ApplicationSmtpSettings.Add(new ApplicationSmtpSettings
		{
			Id = 1,
			ApplicationId = "app1",
			SenderName = "Afrowave",
			SenderEmail = "noreply@afrowave.io",
			Host = "smtp.test.com",
			AuthorizationRequired = false
		});

		_ = db.SaveChanges();
	}

	[Fact]
	public async Task OnGetAsync_ShouldRedirect_WhenInstallStateInvalid()
	{
		IInstallationStatusService status = Services.GetRequiredService<IInstallationStatusService>();
		_ = status.ProperInstallState(InstalationSteps.InstallationResult).Returns(false);

		InstallationResultModel page = CreatePageModel();
		IActionResult result = await page.OnGetAsync();

		RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
		Assert.Equal("/", redirect.PageName);
	}

	[Fact]
	public async Task OnGetAsync_ShouldLoadModelData()
	{
		SeedTestData();

		InstallationResultModel page = CreatePageModel();
		IActionResult result = await page.OnGetAsync();

		_ = Assert.IsType<PageResult>(result);

		Assert.Equal("Afrowave Brand", page.BrandName);
		Assert.Equal("Afrowave App", page.ApplicationName);
		Assert.Equal("Překlad Brand", page.BrandDescription);
		Assert.Equal("Překlad App", page.ApplicationDescription);
		Assert.Equal("noreply@afrowave.io", page.SmtpSettings.SenderEmail);
		Assert.Equal("/icons/brand.svg", page.BrandLogoLink);
		Assert.Equal("/icons/app.svg", page.ApplicationLogoLink);
	}

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