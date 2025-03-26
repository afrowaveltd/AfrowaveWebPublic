namespace Id.Tests.Pages.Install
{
	/// <summary>
	/// Tests for the brand registration page.
	/// </summary>
	public class BrandModelTests : RazorPageTestBase<BrandModel>
	{
		/// <summary>
		/// Configures the services for the test.
		/// </summary>
		/// <param name="services">The service collection for the page</param>
		protected override void ConfigureServices(IServiceCollection services)
		{
			// Mock: Installation status is valid by default
			IInstallationStatusService install = Substitute.For<IInstallationStatusService>();
			_ = install.ProperInstallState(InstalationSteps.Brand).Returns(true);
			_ = services.AddSingleton(install);

			// In-memory EF Core
			ApplicationDbContext db = AfrowaveTestDbFactory.Create("BrandTest", db =>
			{
				_ = db.Users.Add(new User
				{
					Id = "Test",
					Email = "Test",
					Firstname = "Test",
					Lastname = "Test",
					Password = "Test",
					DisplayName = "Test",
				});
			});
			_ = services.AddSingleton(db);

			// Mock: Brand manager service
			IBrandsManager brandService = Substitute.For<IBrandsManager>();
			_ = brandService.RegisterBrandAsync(Arg.Any<RegisterBrandInput>()).Returns(new RegisterBrandResult
			{
				BrandCreated = true,
				LogoUploaded = true
			});
			_ = services.AddSingleton(brandService);
		}

		/// <summary>
		/// Tests the OnGetAsync method.
		/// </summary>
		/// <returns>The Page</returns>
		[Fact]
		public async Task OnGetAsync_ShouldReturnPage_WhenInstallIsReady()
		{
			// Arrange

			BrandModel page = CreatePageModel();
			// Act
			IActionResult result = await page.OnGetAsync();
			// Assert
			_ = Assert.IsType<PageResult>(result);
		}

		/// <summary>
		/// Tests the OnGetAsync method.
		/// </summary>
		/// <returns>Redirects to Index page when installation state is incorrect</returns>
		[Fact]
		public async Task OnGetAsync_ShouldRedirect_WhenInstallStateInvalid()
		{
			// Arrange
			BrandModel page = CreatePageModel();
			IInstallationStatusService installMock = Services.GetRequiredService<IInstallationStatusService>();
			_ = installMock.ProperInstallState(InstalationSteps.Brand).Returns(false);
			// Act
			IActionResult result = await page.OnGetAsync();
			// Assert
			_ = Assert.IsType<RedirectToPageResult>(result);
		}

		/// <summary>
		/// Tests the OnPostAsync method.
		/// </summary>
		/// <returns>Returns Page when model is not valid</returns>
		[Fact]
		public async Task OnPostAsync_ShouldReturnPage_WhenModelIsInvalid()
		{
			// Arrange
			BrandModel page = CreatePageModel();
			page.ModelState.AddModelError("test", "test");
			// Act
			IActionResult result = await page.OnPostAsync();
			// Assert
			_ = Assert.IsType<PageResult>(result);
		}

		/// <summary>
		/// Tests the OnPostAsync method.
		/// </summary>
		/// <returns>Should redirect to Application installation page when model is valid.</returns>
		[Fact]
		public async Task OnPostAsync_ShouldRedirect_WhenModelIsValid()
		{
			// Arrange
			BrandModel page = CreatePageModel();
			_ = page.Input = new()
			{
				Name = "Test",
				OwnerId = "Test",
				CompanyLogo = null,
				Website = "https://test.com",
				Description = "Test",
				Email = "Test@email.com"
			};
			// Act
			IActionResult result = await page.OnPostAsync();
			// Assert
			RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
			Assert.Equal("/Install/Application", redirect.PageName);
		}
	}
}