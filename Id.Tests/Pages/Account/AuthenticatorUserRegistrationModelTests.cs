﻿namespace Id.Tests.Pages.Account
{
	/// <summary>
	/// Tests the behavior of the AuthenticatorUserRegistrationModel during user registration. It checks redirection on
	/// missing IDs and page return for existing users.
	/// </summary>
	public class AuthenticatorUserRegistrationModelTests
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IStringLocalizer<AuthenticatorUserRegistrationModel> _mockLocalizer;
		private readonly ILogger<AuthenticatorUserRegistrationModel> _mockLogger;
		private readonly AuthenticatorUserRegistrationModel _pageModel;
		private readonly IApplicationUsersManager _mockApplicationUsersManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthenticatorUserRegistrationModelTests"/> class.
		/// </summary>
		public AuthenticatorUserRegistrationModelTests()
		{
			DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
				 .UseInMemoryDatabase(databaseName: "TestDb")
				 .Options;

			_dbContext = new ApplicationDbContext(options);
			_mockLocalizer = Substitute.For<IStringLocalizer<AuthenticatorUserRegistrationModel>>();
			_mockApplicationUsersManager = Substitute.For<IApplicationUsersManager>();
			_mockLogger = Substitute.For<ILogger<AuthenticatorUserRegistrationModel>>();

			_pageModel = new AuthenticatorUserRegistrationModel(
				 _mockLocalizer,
				 _dbContext,
				 _mockApplicationUsersManager,
				 _mockLogger
			);
		}

		/// <summary>
		/// Handles the GET request and checks for missing AuthenticatorId or UserId. Redirects to a 404 error page if either is
		/// absent.
		/// </summary>
		/// <returns>Returns a RedirectToPageResult that points to the 404 error page.</returns>
		[Fact]
		public async Task OnGetAsync_ShouldRedirectToPage_WhenAuthenticatorIdOrUserIdIsMissing()
		{
			// Arrange
			_pageModel.AuthenticatorId = "";
			_pageModel.UserId = "";

			// Act
			IActionResult result = await _pageModel.OnGetAsync();

			// Assert
			_ = Assert.IsType<PageResult>(result);
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