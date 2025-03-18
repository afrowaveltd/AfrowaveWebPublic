using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Id.Tests.Pages.Account
{
	public class AuthenticatorUserRegistrationModelTests
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly Mock<IStringLocalizer<AuthenticatorUserRegistrationModel>> _mockLocalizer;
		private readonly Mock<ILogger<AuthenticatorUserRegistrationModel>> _mockLogger;
		private readonly AuthenticatorUserRegistrationModel _pageModel;

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