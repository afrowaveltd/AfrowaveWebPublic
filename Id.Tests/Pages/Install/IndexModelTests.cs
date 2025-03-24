using Id.Models;
using Id.Tests.Helpers;
using IndexModel = Id.Pages.Install.IndexModel;

namespace Id.Tests.Pages.Install;

/// <summary>
/// Tests for the initial install page - administrator account setup.
/// </summary>
public class IndexModelTests : RazorPageTestBase<IndexModel>
{
	/// <summary>
	/// Configures the services for the test.
	/// </summary>
	/// <param name="services">Service collection for the test</param>
	protected override void ConfigureServices(IServiceCollection services)
	{
		// Mock: Installation status is valid by default
		IInstallationStatusService install = Substitute.For<IInstallationStatusService>();
		_ = install.ProperInstallState(InstalationSteps.Administrator).Returns(true);
		_ = services.AddSingleton(install);

		// Mock: Encryption service returns hashed password
		IEncryptionService encryption = Substitute.For<IEncryptionService>();
		_ = encryption.HashPasswordAsync(Arg.Any<string>()).Returns("hashed");
		_ = services.AddSingleton(encryption);

		// In-memory EF Core
		DbContextOptions<ApplicationDbContext> dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase("Afrowave_Install_TestDb").Options;
		_ = services.AddSingleton(new ApplicationDbContext(dbOptions));
	}

	/// <summary>
	/// Tests the OnGetAsync method.
	/// </summary>
	/// <returns>The page</returns>
	[Fact]
	public async Task OnGetAsync_ShouldReturnPage_WhenInstallIsReady()
	{
		// Arrange
		IndexModel page = CreatePageModel();

		// Act
		IActionResult result = await page.OnGetAsync();

		// Assert
		_ = Assert.IsType<PageResult>(result);
	}

	/// <summary>
	/// Tests the OnGetAsync method.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task OnGetAsync_ShouldRedirect_WhenInstallStateInvalid()
	{
		// Arrange
		IndexModel page = CreatePageModel();
		IInstallationStatusService installMock = Services.GetRequiredService<IInstallationStatusService>();
		_ = installMock.ProperInstallState(InstalationSteps.Administrator).Returns(false);

		// Act
		IActionResult result = await page.OnGetAsync();

		// Assert
		RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
		Assert.Equal("/Index", redirect.PageName);
	}

	/// <summary>
	/// Tests the OnPostAsync method.
	/// </summary>
	/// <returns>Redirects to Brand registration when input is valid</returns>
	[Fact]
	public async Task OnPostAsync_ShouldRedirectToBrand_WhenInputIsValid()
	{
		// Arrange
		IndexModel page = CreatePageModel();
		page.Input.Email = "admin@afrowave.io";
		page.Input.Password = "Secret123!";
		page.Input.PasswordConfirm = "Secret123!";
		page.ModelState.Clear();

		// Act
		IActionResult result = await page.OnPostAsync();

		// Assert
		RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
		Assert.Equal("/Install/Brand", redirect.PageName);

		// Assert (bonus: database check)
		ApplicationDbContext db = Services.GetRequiredService<ApplicationDbContext>();
		User? user = await db.Users.FirstOrDefaultAsync();
		Assert.NotNull(user);
		Assert.Equal("admin@afrowave.io", user.Email);
	}

	/// <summary>
	/// Tests the OnPostAsync method.
	/// </summary>
	/// <returns>Returns Page when model is invalid</returns>
	[Fact]
	public async Task OnPostAsync_ShouldReturnPage_WhenModelIsInvalid()
	{
		// Arrange
		IndexModel page = CreatePageModel();
		page.ModelState.AddModelError("Password", "Required");

		// Act
		IActionResult result = await page.OnPostAsync();

		// Assert
		_ = Assert.IsType<PageResult>(result);
	}
}