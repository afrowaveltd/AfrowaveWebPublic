public class TranslatorTestBase
{
    protected readonly ITranslatorService TranslatorService;

    public TranslatorTestBase()
    {
        var services = new ServiceCollection();

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                {"Translator:Host", "http://fake-translator-service"}
            })
            .Build();

        services.AddSingleton<IConfiguration>(config);
        services.AddLogging(builder => builder.AddConsole());
        services.AddTransient<ITranslatorService, TranslatorService>();

        var serviceProvider = services.BuildServiceProvider();
        TranslatorService = serviceProvider.GetRequiredService<ITranslatorService>();
    }
}