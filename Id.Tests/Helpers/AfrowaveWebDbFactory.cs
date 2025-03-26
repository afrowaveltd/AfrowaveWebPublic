namespace Id.Tests.Helpers;

/// <summary>
/// Factory helper for creating isolated in-memory ApplicationDbContext instances for testing.
/// Automatically assigns unique database names to ensure clean state.
/// </summary>
public static class AfrowaveTestDbFactory
{
	/// <summary>
	/// Creates a new in-memory ApplicationDbContext with a unique database name.
	/// You can optionally seed it with initial test data using the provided setup action.
	/// </summary>
	/// <param name="contextLabel">Optional label to identify the test context (for debugging)</param>
	/// <param name="seedAction">Optional action to seed test data into the context</param>
	/// <returns>Instance of <see cref="ApplicationDbContext"/> ready for use</returns>
	public static ApplicationDbContext Create(string? contextLabel = null, Action<ApplicationDbContext>? seedAction = null)
	{
		string dbName = $"{contextLabel ?? "TestDb"}_{Guid.NewGuid()}";

		DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(dbName)
			.Options;

		ApplicationDbContext db = new ApplicationDbContext(options);

		// Optional seeding
		seedAction?.Invoke(db);
		_ = db.SaveChanges();

		return db;
	}
}