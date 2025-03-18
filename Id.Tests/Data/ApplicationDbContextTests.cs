namespace Id.Tests.Data;

/// <summary>
/// Represents the test cases for the <see cref="ApplicationDbContext"/> class.
/// </summary>
public class ApplicationDbContextTests
{
	private DbContextOptions<ApplicationDbContext> CreateDbContextOptions()
	{
		return new DbContextOptionsBuilder<ApplicationDbContext>()
			 .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Unique DB for each test
			 .Options;
	}

	/// <summary>
	/// Tests that a country can be inserted and retrieved from the database.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task CanInsertAndRetrieveCountry()
	{
		using ApplicationDbContext context = new ApplicationDbContext(CreateDbContextOptions());
		Country country = new Country { Id = 1, Name = "USA", Code = "US", Dial_code = "+1", Emoji = "🇺🇸" };

		_ = context.Countries.Add(country);
		_ = await context.SaveChangesAsync();

		Country? savedCountry = await context.Countries.FindAsync(1);
		Assert.NotNull(savedCountry);
		Assert.Equal("USA", savedCountry.Name);
	}

	/// <summary>
	/// Tests that a language can be inserted and retrieved from the database.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task CanInsertAndRetrieveLanguage()
	{
		using ApplicationDbContext context = new ApplicationDbContext(CreateDbContextOptions());
		Language language = new Language { Id = 1, Code = "en", Name = "English", Native = "English", Rtl = 0 };

		_ = context.Languages.Add(language);
		_ = await context.SaveChangesAsync();

		Language? savedLanguage = await context.Languages.FindAsync(1);
		Assert.NotNull(savedLanguage);
		Assert.Equal("English", savedLanguage.Name);
	}

	/// <summary>
	/// Tests that a policy translation can be inserted and retrieved from the database.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task CanInsertAndRetrievePolicyTranslation()
	{
		using ApplicationDbContext context = new ApplicationDbContext(CreateDbContextOptions());
		PolicyTranslation policyTranslation = new PolicyTranslation
		{
			Id = 1,
			PolicyId = 1,
			LanguageId = 1,
			Content = "Test content"
		};

		_ = context.PolicyTranslations.Add(policyTranslation);
		_ = await context.SaveChangesAsync();

		PolicyTranslation? savedTranslation = await context.PolicyTranslations.FindAsync(1);
		Assert.NotNull(savedTranslation);
		Assert.Equal("Test content", savedTranslation.Content);
	}

	/// <summary>
	/// Tests that a refresh token can be inserted and retrieved from the database.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task CanInsertAndRetrieveRefreshToken()
	{
		using ApplicationDbContext context = new ApplicationDbContext(CreateDbContextOptions());
		RefreshToken token = new RefreshToken { Id = 1, UserId = "user123", Token = "abc123", Expires = DateTime.UtcNow.AddHours(1) };

		_ = context.RefreshTokens.Add(token);
		_ = await context.SaveChangesAsync();

		RefreshToken? savedToken = await context.RefreshTokens.FindAsync(1);
		Assert.NotNull(savedToken);
		Assert.Equal("abc123", savedToken.Token);
	}

	/// <summary>
	/// Tests that a user can be inserted and retrieved from the database.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task CanInsertAndRetrieveApplication()
	{
		using ApplicationDbContext context = new ApplicationDbContext(CreateDbContextOptions());
		Application application = new Application { Id = "app1", Name = "Test App", Description = "Test Description" };

		_ = context.Applications.Add(application);
		_ = await context.SaveChangesAsync();

		Application? savedApp = await context.Applications.FindAsync("app1");
		Assert.NotNull(savedApp);
		Assert.Equal("Test App", savedApp.Name);
	}

	/// <summary>
	/// Tests that an application policy can be inserted and retrieved from the database.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task CanInsertAndRetrieveApplicationPolicy()
	{
		using ApplicationDbContext context = new ApplicationDbContext(CreateDbContextOptions());
		ApplicationPolicy policy = new ApplicationPolicy { Id = 1, ApplicationId = "app1", OriginalLanguage = "en" };

		_ = context.ApplicationPolicies.Add(policy);
		_ = await context.SaveChangesAsync();

		ApplicationPolicy? savedPolicy = await context.ApplicationPolicies.FindAsync(1);
		Assert.NotNull(savedPolicy);
		Assert.Equal("en", savedPolicy.OriginalLanguage);
	}

	/// <summary>
	/// Tests that an application role can be inserted and retrieved from the database.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task CanInsertAndRetrieveApplicationRole()
	{
		using ApplicationDbContext context = new ApplicationDbContext(CreateDbContextOptions());
		ApplicationRole role = new ApplicationRole { Id = 1, ApplicationId = "app1", Name = "Admin" };

		_ = context.ApplicationRoles.Add(role);
		_ = await context.SaveChangesAsync();

		ApplicationRole? savedRole = await context.ApplicationRoles.FindAsync(1);
		Assert.NotNull(savedRole);
		Assert.Equal("Admin", savedRole.Name);
	}

	/// <summary>
	/// Tests that application SMTP settings can be inserted and retrieved from the database.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task CanInsertAndRetrieveApplicationSmtpSettings()
	{
		using ApplicationDbContext context = new ApplicationDbContext(CreateDbContextOptions());
		ApplicationSmtpSettings smtpSettings = new ApplicationSmtpSettings { Id = 1, ApplicationId = "app1", Host = "smtp.example.com", Port = 587 };

		_ = context.ApplicationSmtpSettings.Add(smtpSettings);
		_ = await context.SaveChangesAsync();

		ApplicationSmtpSettings? savedSettings = await context.ApplicationSmtpSettings.FindAsync(1);
		Assert.NotNull(savedSettings);
		Assert.Equal("smtp.example.com", savedSettings.Host);
	}

	/// <summary>
	/// Tests that an application user can be inserted and retrieved from the database.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task CanInsertAndRetrieveApplicationUser()
	{
		using ApplicationDbContext context = new ApplicationDbContext(CreateDbContextOptions());
		ApplicationUser appUser = new ApplicationUser { Id = 1, ApplicationId = "app1", UserId = "user1" };

		_ = context.ApplicationUsers.Add(appUser);
		_ = await context.SaveChangesAsync();

		ApplicationUser? savedUser = await context.ApplicationUsers.FindAsync(1);
		Assert.NotNull(savedUser);
		Assert.Equal("user1", savedUser.UserId);
	}

	/// <summary>
	/// Tests that a brand can be inserted and retrieved from the database.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task CanInsertAndRetrieveBrand()
	{
		using ApplicationDbContext context = new ApplicationDbContext(CreateDbContextOptions());
		Brand brand = new Brand { Id = 1, Name = "TestBrand", OwnerId = "owner1" };

		_ = context.Brands.Add(brand);
		_ = await context.SaveChangesAsync();

		Brand? savedBrand = await context.Brands.FindAsync(1);
		Assert.NotNull(savedBrand);
		Assert.Equal("TestBrand", savedBrand.Name);
	}

	/// <summary>
	/// Tests that a suspended application can be inserted and retrieved from the database.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task CanInsertAndRetrieveSuspendedApplication()
	{
		using ApplicationDbContext context = new ApplicationDbContext(CreateDbContextOptions());
		SuspendedApplication suspendedApp = new SuspendedApplication
		{
			Id = 1,
			ApplicationId = "app1",
			SuspenderId = "admin1",
			Reason = SuspensionReason.Piracy
		};

		_ = context.SuspendedApplications.Add(suspendedApp);
		_ = await context.SaveChangesAsync();

		SuspendedApplication? savedApp = await context.SuspendedApplications.FindAsync(1);
		Assert.NotNull(savedApp);
		Assert.Equal(SuspensionReason.Piracy, savedApp.Reason);
	}

	/// <summary>
	/// Tests that a suspended user can be inserted and retrieved from the database.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task CanInsertAndRetrieveSuspendedUser()
	{
		using ApplicationDbContext context = new ApplicationDbContext(CreateDbContextOptions());
		SuspendedUser suspendedUser = new SuspendedUser
		{
			Id = 1,
			ApplicationId = "app1",
			ApplicationUserId = 100,
			SuspenderId = "admin1",
			Reason = SuspensionReason.AbusiveBehavior
		};

		_ = context.SuspendedUsers.Add(suspendedUser);
		_ = await context.SaveChangesAsync();

		SuspendedUser? savedUser = await context.SuspendedUsers.FindAsync(1);
		Assert.NotNull(savedUser);
		Assert.Equal(SuspensionReason.AbusiveBehavior, savedUser.Reason);
	}

	/// <summary>
	/// Tests that a user address can be inserted and retrieved from the database.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task CanInsertAndRetrieveUiTranslatorLog()
	{
		using ApplicationDbContext context = new ApplicationDbContext(CreateDbContextOptions());
		UiTranslatorLog log = new UiTranslatorLog
		{
			Id = 1,
			TargetLanguagesCount = 5,
			PhrazesCount = 100,
			TranslationErrorCount = 2
		};

		_ = context.UiTranslatorLogs.Add(log);
		_ = await context.SaveChangesAsync();

		UiTranslatorLog? savedLog = await context.UiTranslatorLogs.FindAsync(1);
		Assert.NotNull(savedLog);
		Assert.Equal(5, savedLog.TargetLanguagesCount);
	}

	/// <summary>
	/// Tests that a user address can be inserted and retrieved from the database.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task CanInsertAndRetrieveUser()
	{
		using ApplicationDbContext context = new ApplicationDbContext(CreateDbContextOptions());
		User user = new User
		{
			Id = "user1",
			Email = "user@example.com",
			DisplayName = "Test User",
			Firstname = "John",
			Lastname = "Doe"
		};

		_ = context.Users.Add(user);
		_ = await context.SaveChangesAsync();

		User? savedUser = await context.Users.FindAsync("user1");
		Assert.NotNull(savedUser);
		Assert.Equal("Test User", savedUser.DisplayName);
	}

	/// <summary>
	/// Tests that a user address can be inserted and retrieved from the database.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task CanInsertAndRetrieveUserAddress()
	{
		using ApplicationDbContext context = new ApplicationDbContext(CreateDbContextOptions());
		UserAddress address = new UserAddress
		{
			Id = 1,
			UserId = "user1",
			StreetAddress = "123 Test St",
			City = "Test City",
			PostCode = "12345"
		};

		_ = context.UserAddresses.Add(address);
		_ = await context.SaveChangesAsync();

		UserAddress? savedAddress = await context.UserAddresses.FindAsync(1);
		Assert.NotNull(savedAddress);
		Assert.Equal("123 Test St", savedAddress.StreetAddress);
	}

	/// <summary>
	/// Tests that a user role can be inserted and retrieved from the database.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task CanInsertAndRetrieveUserRole()
	{
		using ApplicationDbContext context = new ApplicationDbContext(CreateDbContextOptions());
		UserRole role = new UserRole
		{
			Id = 1,
			ApplicationRoleId = 10,
			ApplicationUserId = 20
		};

		_ = context.UserRoles.Add(role);
		_ = await context.SaveChangesAsync();

		UserRole? savedRole = await context.UserRoles.FindAsync(1);
		Assert.NotNull(savedRole);
		Assert.Equal(10, savedRole.ApplicationRoleId);
	}
}