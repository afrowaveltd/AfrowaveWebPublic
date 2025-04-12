namespace Id.Tests.Pages.Install;

/// <summary>
/// Tests for the Application install page.
/// </summary>
public class ApplicationModelTests : RazorPageTestBase<ApplicationModel>
{
	/// <summary>
	/// Configures services for dependency injection, setting up various mocked services and an in-memory database for
	/// testing.
	/// </summary>
	/// <param name="services">Facilitates the registration of services that can be injected into other components of the application.</param>
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

	/// <summary>
	/// Tests the OnGetAsync method to ensure it returns a page when the state is valid.
	/// </summary>
	/// <returns>Returns a PageResult if the state is valid.</returns>
	[Fact]
	public async Task OnGetAsync_ShouldReturnPage_WhenStateIsValid()
	{
		ApplicationModel page = CreatePageModel();

		IActionResult result = await page.OnGetAsync();

		_ = Assert.IsType<PageResult>(result);
		Assert.Equal("owner1", page.Input.OwnerId);
		Assert.Equal(1, page.Input.BrandId);
	}

	/// <summary>
	/// Tests the OnGetAsync method to ensure it redirects to the Index page when the installation state is invalid.
	/// </summary>
	/// <returns>Returns a RedirectToPageResult indicating the page to redirect to.</returns>
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

	/// <summary>
	/// Handles the asynchronous post request for a page model and checks for a successful redirect.
	/// </summary>
	/// <returns>Returns a redirect result to the specified page upon successful processing.</returns>
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

	/// <summary>
	///	Tests the OnPostAsync method to ensure it returns a PageResult when the model state is invalid.
	/// </summary>
	/// <returns>Returns a PageResult indicating the page should be rendered again due to model validation errors.</returns>
	[Fact]
	public async Task OnPostAsync_ShouldReturnPage_WhenModelInvalid()
	{
		ApplicationModel page = CreatePageModel();
		page.ModelState.AddModelError("ApplicationName", "Required");

		IActionResult result = await page.OnPostAsync();

		_ = Assert.IsType<PageResult>(result);
	}

	/// <summary>
	/// Tests the OnPostAsync method to ensure it returns a page when the application is not created due to an error.
	/// </summary>
	/// <returns>Returns a PageResult indicating the outcome of the application registration attempt.</returns>
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

	/// <summary>
	/// Tests the OnPostAsync method to ensure it redirects when the owner ID does not match the expected value. It sets up
	/// a page model with specific input values.
	/// </summary>
	/// <returns>Returns a RedirectToPageResult that indicates a redirection to the '/Index' page.</returns>
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