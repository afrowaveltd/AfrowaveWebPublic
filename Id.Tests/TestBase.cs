/// <summary>
/// Base test class for setting up a service provider with dependency injection.
/// This class provides shared configurations and mock dependencies for unit tests.
/// </summary>
public class TestBase
{
	/// <summary>
	/// Service provider for resolving dependencies in tests.
	/// </summary>
	protected readonly ServiceProvider ServiceProvider;

	/// <summary>
	/// Initializes a new instance of the <see cref="TestBase"/> class.
	/// Sets up dependency injection, configuration, and an in-memory SQLite database.
	/// </summary>
	public TestBase()
	{
		ServiceCollection services = new ServiceCollection();

		// ✅ Load Configuration from appsettings.json
		IConfigurationRoot config = new ConfigurationBuilder()
			 .SetBasePath(Directory.GetCurrentDirectory())
			 .AddJsonFile("appsettings.Test.json", optional: true, reloadOnChange: true)
			 .Build();

		_ = services.AddSingleton<IConfiguration>(config);

		// ✅ Mocking Logger
		_ = services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

		// ✅ Register Localization Services
		_ = services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
		_ = services.AddSingleton(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));

		// ✅ Register the service we are testing
		_ = services.AddTransient<ITranslatorService, TranslatorService>();
	}

	/// <summary>
	/// Resolves a service of the specified type from the service provider.
	/// </summary>
	/// <typeparam name="T">The type of service to resolve.</typeparam>
	/// <returns>The requested service instance.</returns>
	protected T GetService<T>() => ServiceProvider.GetRequiredService<T>();
}