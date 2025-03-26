using Microsoft.AspNetCore.Mvc.Rendering;

namespace Id.Tests.Pages.Install;

public class SmtpSettingsModelTests : RazorPageTestBase<SmtpSettingsModel>
{
	protected override void ConfigureServices(IServiceCollection services)
	{
		ApplicationDbContext db = AfrowaveTestDbFactory.Create("SmtpTest", db =>
		{
			_ = db.Users.Add(new User
			{
				Id = "owner1",
				Firstname = "Test",
				Lastname = "User",
				DisplayName = "Test User",
				Password = "Password01",
			});
			_ = db.Brands.Add(new Brand
			{
				Id = 1,
				Name = "Test Brand",
				OwnerId = "owner1"
			});

			_ = db.Applications.Add(new Application
			{
				Id = "app123",
				Name = "Afrowave",
				ApplicationEmail = "noreply@afrowave.io",
				BrandId = 1,
				OwnerId = "owner1"
			});
		});

		_ = services.AddSingleton(db);

		IInstallationStatusService install = Substitute.For<IInstallationStatusService>();
		_ = install.ProperInstallState(InstalationSteps.SmtpSettings).Returns(true);
		_ = services.AddSingleton(install);

		ISettingsService settings = Substitute.For<ISettingsService>();
		_ = settings.GetSettingsAsync().Returns(new Models.SettingsModels.IdentificatorSettings
		{
			ApplicationId = "app123"
		});
		_ = services.AddSingleton(settings);

		ISelectOptionsServices select = Substitute.For<ISelectOptionsServices>();
		_ = select.GetSecureSocketOptionsAsync().Returns(Task.FromResult(new List<SelectListItem>
		{
			new() { Value = "0", Text = "Auto" }
		}));
		_ = services.AddSingleton(select);

		IStringLocalizer<SmtpSettingsModel> localizer = Substitute.For<IStringLocalizer<SmtpSettingsModel>>();
		_ = localizer["Error saving smtp settings"].Returns(new LocalizedString("Error saving smtp settings", "Mocked error message"));
		_ = services.AddSingleton(localizer);

		_ = services.AddSingleton(Substitute.For<ILogger<SmtpSettingsModel>>());
	}

	[Fact]
	public async Task OnGetAsync_ShouldRedirect_WhenInstallStateInvalid()
	{
		IInstallationStatusService install = Services.GetRequiredService<IInstallationStatusService>();
		_ = install.ProperInstallState(InstalationSteps.SmtpSettings).Returns(false);

		SmtpSettingsModel page = CreatePageModel();
		IActionResult result = await page.OnGetAsync();

		RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
		Assert.Equal("/Index", redirect.PageName);
	}

	[Fact]
	public async Task OnGetAsync_ShouldReturnPage_WhenAllValid()
	{
		SmtpSettingsModel page = CreatePageModel();
		IActionResult result = await page.OnGetAsync();

		_ = Assert.IsType<PageResult>(result);
		Assert.Equal("app123", page.Input.ApplicationId);
		Assert.Equal("Afrowave", page.Input.SenderName);
		Assert.Equal("noreply@afrowave.io", page.Input.SenderEmail);
		_ = Assert.Single(page.options);
	}

	[Fact]
	public async Task OnGetAsync_ShouldRedirect_WhenAppNotFound()
	{
		ISettingsService settings = Services.GetRequiredService<ISettingsService>();
		_ = settings.GetSettingsAsync().Returns(new Models.SettingsModels.IdentificatorSettings
		{
			ApplicationId = "missing-app"
		});

		SmtpSettingsModel page = CreatePageModel();
		IActionResult result = await page.OnGetAsync();

		RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
		Assert.Equal("/Error", redirect.PageName);
	}

	[Fact]
	public async Task OnPostAsync_ShouldRedirect_WhenValid()
	{
		SmtpSettingsModel page = CreatePageModel();
		page.Input.ApplicationId = "app123";
		page.Input.Host = "smtp.afrowave.io";
		page.Input.Port = 587;
		page.Input.SmtpUsername = "user";
		page.Input.SmtpPassword = "pass";
		page.Input.SenderEmail = "noreply@afrowave.io";
		page.Input.SenderName = "Afrowave";
		page.Input.Secure = MailKit.Security.SecureSocketOptions.Auto;
		page.Input.AuthorizationRequired = true;

		page.ModelState.Clear();

		IActionResult result = await page.OnPostAsync();

		RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
		Assert.Equal("/Install/LoginRules", redirect.PageName);
	}

	[Fact]
	public async Task OnPostAsync_ShouldReturnPage_WhenModelInvalid()
	{
		SmtpSettingsModel page = CreatePageModel();
		page.ModelState.AddModelError("Host", "Required");

		IActionResult result = await page.OnPostAsync();

		_ = Assert.IsType<PageResult>(result);
	}

	[Fact]
	public async Task OnPostAsync_ShouldRedirect_WhenAppNotFound()
	{
		SmtpSettingsModel page = CreatePageModel();
		page.Input.ApplicationId = "missing";
		page.ModelState.Clear();

		IActionResult result = await page.OnPostAsync();

		RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
		Assert.Equal("/Error", redirect.PageName);
	}

	[Fact]
	public async Task OnPostAsync_ShouldReturnPage_WhenSaveFails()
	{
		var options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase("FailingSave").Options;
		var db = new FailingDbContext(options);

		_ = db.Users.Add(new User
		{
			Id = "owner1",
			Email = "test@test.com",
			Firstname = "Test",
			Lastname = "User",
			DisplayName = "Test User",
			Password = "Password01"
		});
		_ = db.Brands.Add(new Brand
		{
			Id = 1,
			Name = "Test Brand",
			OwnerId = "owner1"
		});
		_ = db.Applications.Add(new Application
		{
			Id = "app123",
			Name = "Afrowave",
			ApplicationEmail = "noreply@afrowave.io",
			BrandId = 1,
			OwnerId = "owner1"
		});
		_ = db.SaveChanges();

		// ✨ Nahradí ApplicationDbContext bez přepsání všech služeb
		ReplaceService<ApplicationDbContext>(db);

		var page = CreatePageModel();
		page.Input = new SmtpSettingsModel.InputModel
		{
			ApplicationId = "app123",
			Host = "smtp",
			Port = 25,
			SmtpUsername = "user",
			SmtpPassword = "pass",
			SenderEmail = "email@test.com",
			SenderName = "Test",
			Secure = MailKit.Security.SecureSocketOptions.None,
			AuthorizationRequired = true
		};
		page.ModelState.Clear();

		var result = await page.OnPostAsync();

		_ = Assert.IsType<PageResult>(result);
		Assert.Equal("Mocked error message", page.ErrorMessage);
	}
}