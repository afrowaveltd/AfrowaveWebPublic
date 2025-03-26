namespace Id.Tests.Helpers;

/// <summary>
/// Specialized test-only version of <see cref="ApplicationDbContext"/> that throws an exception
/// whenever <see cref="SaveChangesAsync(CancellationToken)"/> is called.
/// </summary>
public class FailingDbContext : ApplicationDbContext
{
	/// <summary>
	/// Initializes a new instance of <see cref="FailingDbContext"/> with the given options.
	/// </summary>
	/// <param name="options">EF Core database context options</param>
	public FailingDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{
	}

	/// <summary>
	/// Always throws an exception to simulate a failed database operation.
	/// </summary>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>Never returns — always throws</returns>
	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		throw new Exception("Simulated DB write failure (SaveChangesAsync)");
	}
}