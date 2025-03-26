namespace Id.Tests.Pages.Install;

/// <summary>
/// Unit tests for the ApplicationRoles install page.
/// </summary>
public class ApplicationRolesModelTests : RazorPageTestBase<ApplicationRolesModel>
{
	/// <summary>
	/// Configures the services for the test.
	/// </summary>
	/// <param name="services"></param>
	protected override void ConfigureServices(IServiceCollection services)
	{
		// In-memory DB
		ApplicationDbContext db = AfrowaveTestDbFactory.Create("AppRolesTest", db =>
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

			_ = db.Applications.Add(new Application
			{
				Id = "app123",
				OwnerId = "owner1",
				BrandId = 1
			});
		});
		_ = services.AddSingleton(db);

		// Mock installation state
		IInstallationStatusService install = Substitute.For<IInstallationStatusService>();
		_ = install.ProperInstallState(InstalationSteps.ApplicationRoles).Returns(true);
		_ = services.AddSingleton(install);

		// Mock role creation
		IRolesManager roles = Substitute.For<IRolesManager>();
		_ = roles.CreateApplicationRoleAsync(Arg.Any<CreateRoleInput>()).Returns(new CreateRoleResult { Success = true });
		_ = roles.SetAllRolesToOwner("app123", "owner1").Returns(new List<RoleAssignResult>
		{
			new() { Successful = true, RoleName = "Admin" }
		});
		_ = services.AddSingleton(roles);

		// Mock app users
		IApplicationUsersManager appUsers = Substitute.For<IApplicationUsersManager>();
		_ = appUsers.RegisterApplicationUserAsync(Arg.Any<RegisterApplicationUserInput>())
			.Returns(new RegisterApplicationUserResult { Success = true, ApplicationUserId = 42 });
		_ = services.AddSingleton(appUsers);

		// Zbývající služby (stačí dummy instance)
		_ = services.AddSingleton(Substitute.For<IApplicationsManager>());
		_ = services.AddSingleton(Substitute.For<IEncryptionService>());
		_ = services.AddSingleton(Substitute.For<ISettingsService>());
		_ = services.AddSingleton(Substitute.For<IUsersManager>());
	}

	/// <summary>
	/// Tests the OnGetAsync method.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task OnGetAsync_ShouldReturnPage_WhenAllIsValid()
	{
		ApplicationRolesModel page = CreatePageModel();

		IActionResult result = await page.OnGetAsync();

		_ = Assert.IsType<PageResult>(result);
		Assert.True(page.UserIdFound);
		Assert.True(page.ApplicationIdFound);
		Assert.Equal("owner1", page.UserIdData);
		Assert.Equal("app123", page.ApplicationIdData);
		Assert.Contains(page.RoleAssigningResult, r => r.RoleName == "Admin");
	}

	/// <summary>
	/// Tests the OnGetAsync method.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task OnGetAsync_ShouldRedirect_WhenInstallStateInvalid()
	{
		ApplicationRolesModel page = CreatePageModel();
		_ = Services.GetRequiredService<IInstallationStatusService>()
			.ProperInstallState(InstalationSteps.ApplicationRoles)
			.Returns(false);

		IActionResult result = await page.OnGetAsync();

		RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
		Assert.Equal("/Index", redirect.PageName);
	}

	/// <summary>
	/// Tests the OnGetAsync method.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task OnGetAsync_ShouldReturnPage_WhenApplicationUserCreationFails()
	{
		IApplicationUsersManager appUsers = Services.GetRequiredService<IApplicationUsersManager>();
		_ = appUsers.RegisterApplicationUserAsync(Arg.Any<RegisterApplicationUserInput>())
			.Returns(new RegisterApplicationUserResult { Success = false, ErrorMessage = "Failed to create" });

		ApplicationRolesModel page = CreatePageModel();
		IActionResult result = await page.OnGetAsync();

		_ = Assert.IsType<PageResult>(result);
		Assert.Equal("Failed to create", page.ErrorMessage);
	}

	// On Post tests
}