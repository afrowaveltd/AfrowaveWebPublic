namespace Id.Tests.Pages.Install;

/// <summary>
/// Tests for the Application install page.
/// </summary>
public class ApplicationModelTests : RazorPageTestBase<ApplicationModel>
{
	protected override void ConfigureServices(IServiceCollection services)
	{
		// Mock installation status
		IInstallationStatusService install = Substitute.For<IInstallationStatusService>();
		_ = install.ProperInstallState(InstalationSteps.Application).Returns(true);
		_ = services.AddSingleton(install);

		// Mock applications service
		IApplicationsManager apps = Substitute.For<IApplicationsManager>();
		_ = apps.RegisterApplicationAsync(Arg.Any<RegisterApplicationInput>())
			.Returns(new RegisterApplicationResult
			{
				ApplicationCreated = true,
				ApplicationId = "test-app-id"
			});
		_ = services.AddSingleton(apps);

		// Mock application users
		IApplicationUsersManager users = Substitute.For<IApplicationUsersManager>();
		_ = services.AddSingleton(users);

		// Mock settings
		ISettingsService settings = Substitute.For<ISettingsService>();
		_ = services.AddSingleton(settings);

		// Image service
		IImageService image = Substitute.For<IImageService>();
		_ = services.AddSingleton(image);

		// Mock Encryption service
		IEncryptionService encryption = Substitute.For<IEncryptionService>();
		_ = services.AddSingleton(encryption);

		// In-memory EF Core DB
		ApplicationDbContext db = AfrowaveTestDbFactory.Create("ApplicationInstallTest", db =>
		{
			_ = db.Users.Add(new User
			{
				Id = "owner1",
				Email = "test@email.com",
				Firstname = "Test",
				Lastname = "User",
				Password = "Password01",
				DisplayName = "Test User"
			});

			_ = db.Brands.Add(new Brand
			{
				Id = 1,
				Name = "Test Brand",
				OwnerId = "owner1"
			});
		});
		_ = services.AddSingleton(db);

		// Mock encryption service
		_ = Substitute.For<IEncryptionService>();
	}

	[Fact]
	public async Task OnGetAsync_ShouldReturnPage_WhenStateIsValid()
	{
		ApplicationModel page = CreatePageModel();

		IActionResult result = await page.OnGetAsync();

		_ = Assert.IsType<PageResult>(result);
		Assert.Equal("owner1", page.Input.OwnerId);
		Assert.Equal(1, page.Input.BrandId);
	}

	[Fact]
	public async Task OnGetAsync_ShouldRedirect_WhenInvalidInstallState()
	{
		ApplicationModel page = CreatePageModel();
		IInstallationStatusService install = Services.GetRequiredService<IInstallationStatusService>();
		_ = install.ProperInstallState(InstalationSteps.Application).Returns(false);

		IActionResult result = await page.OnGetAsync();

		RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
		Assert.Equal("/Index", redirect.PageName);
	}

	[Fact]
	public async Task OnPostAsync_ShouldRedirect_WhenSuccessful()
	{
		ApplicationModel page = CreatePageModel();
		page.Input.ApplicationName = "Test App";
		page.Input.ApplicationDescription = "App desc";
		page.Input.ApplicationEmail = "app@example.com";
		page.Input.ApplicationWebsite = "https://app.com";
		page.Input.BrandId = 1;
		page.Input.OwnerId = "owner1";
		page.ModelState.Clear();

		IActionResult result = await page.OnPostAsync();

		RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
		Assert.Equal("/Install/ApplicationRoles", redirect.PageName);
	}

	[Fact]
	public async Task OnPostAsync_ShouldReturnPage_WhenModelInvalid()
	{
		ApplicationModel page = CreatePageModel();
		page.ModelState.AddModelError("ApplicationName", "Required");

		IActionResult result = await page.OnPostAsync();

		_ = Assert.IsType<PageResult>(result);
	}

	[Fact]
	public async Task OnPostAsync_ShouldReturnPage_WhenAppNotCreated()
	{
		ApplicationModel page = CreatePageModel();
		page.Input.ApplicationName = "Test";
		page.Input.OwnerId = "owner1";
		page.Input.BrandId = 1;
		page.ModelState.Clear();

		IApplicationsManager apps = Services.GetRequiredService<IApplicationsManager>();
		_ = apps.RegisterApplicationAsync(Arg.Any<RegisterApplicationInput>())
			.Returns(new RegisterApplicationResult
			{
				ApplicationCreated = false,
				ErrorMessage = "Something went wrong"
			});

		IActionResult result = await page.OnPostAsync();

		_ = Assert.IsType<PageResult>(result);
		Assert.Equal("Something went wrong", page.ErrorMessage);
	}

	[Fact]
	public async Task OnPostAsync_ShouldRedirect_WhenOwnerMismatch()
	{
		ApplicationModel page = CreatePageModel();
		page.Input.ApplicationName = "Test";
		page.Input.OwnerId = "someone-else";
		page.Input.BrandId = 1;
		page.ModelState.Clear();

		IActionResult result = await page.OnPostAsync();

		RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
		Assert.Equal("/Index", redirect.PageName);
	}
}