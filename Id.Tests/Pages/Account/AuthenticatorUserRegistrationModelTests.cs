using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Id.Tests.Pages.Account
{
	/// <summary>
	/// Tests the behavior of the AuthenticatorUserRegistrationModel during user registration. It checks redirection on
	/// missing IDs and page return for existing users.
	/// </summary>
	public class AuthenticatorUserRegistrationModelTests
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly Mock<IStringLocalizer<AuthenticatorUserRegistrationModel>> _mockLocalizer;
		private readonly Mock<ILogger<AuthenticatorUserRegistrationModel>> _mockLogger;
		private readonly AuthenticatorUserRegistrationModel _pageModel;

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthenticatorUserRegistrationModelTests"/> class.
		/// </summary>
		public AuthenticatorUserRegistrationModelTests()
		{
			DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
				 .UseInMemoryDatabase(databaseName: "TestDb")
				 .Options;

			_dbContext = new ApplicationDbContext(options);
			_mockLocalizer = new Mock<IStringLocalizer<AuthenticatorUserRegistrationModel>>();
			_mockLogger = new Mock<ILogger<AuthenticatorUserRegistrationModel>>();

			_pageModel = new AuthenticatorUserRegistrationModel(
				 _mockLocalizer.Object,
				 _dbContext,
				 _mockLogger.Object
			);
		}

		/// <summary>
		/// Handles the GET request and checks for missing AuthenticatorId or UserId. Redirects to a 404 error page if either is
		/// absent.
		/// </summary>
		/// <returns>Returns a RedirectToPageResult that points to the 404 error page.</returns>
		[Fact]
		public async Task OnGetAsync_ShouldRedirectToError_WhenAuthenticatorIdOrUserIdIsMissing()
		{
			// Arrange
			_pageModel.AuthenticatorId = "";
			_pageModel.UserId = "";

			// Act
			IActionResult result = await _pageModel.OnGetAsync();

			// Assert
			RedirectToPageResult redirectResult = Assert.IsType<RedirectToPageResult>(result);
			Assert.Equal("/Error/404", redirectResult.PageName);
		}

		/// <summary>
		/// Tests the OnGetAsync method to ensure it returns a page when a user exists in the database.
		/// </summary>
		/// <returns>Returns a PageResult indicating the page was successfully retrieved.</returns>
		[Fact]
		public async Task OnGetAsync_ShouldReturnPage_WhenUserExists()
		{
			// Arrange
			_pageModel.AuthenticatorId = "123";
			_pageModel.UserId = "User1";

			_ = _dbContext.Users.Add(new User { Id = "User1" });
			_ = await _dbContext.SaveChangesAsync();

			// Act
			IActionResult result = await _pageModel.OnGetAsync();

			// Assert
			_ = Assert.IsType<PageResult>(result);
		}
	}
}