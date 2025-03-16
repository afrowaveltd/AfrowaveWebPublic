
using Id.Tests.Data;
public class TestBase
{
    protected readonly ServiceProvider ServiceProvider;

    public TestBase()
    {
        var services = new ServiceCollection();

        // ✅ Load Configuration from appsettings.json
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Test.json", optional: true, reloadOnChange: true)
            .Build();

        services.AddSingleton<IConfiguration>(config);

        // ✅ Mocking Logger
        services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

        // ✅ Register Localization Services
        services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
        services.AddSingleton(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));

        // ✅ Register the service we are testing
        services.AddTransient<ITranslatorService, TranslatorService>();

        // ✅ Register SQLite in-memory test database
        services.AddDbContext<ApplicationDbContextTesting>(options =>
          options.UseSqlite("Data Source=:memory:"));

        ServiceProvider = services.BuildServiceProvider();

        // Ensure database is created
        using var scope = ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContextTesting>();
        dbContext.EnsureDatabaseCreated();
    }

    protected T GetService<T>() => ServiceProvider.GetRequiredService<T>();
}
