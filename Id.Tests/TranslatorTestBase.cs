// TranslatorTestBase.cs
public class TranslatorTestBase
{
	protected readonly ITranslatorService TranslatorService;
	protected readonly Mock<HttpMessageHandler> MockHttpMessageHandler;
	protected readonly HttpClient MockHttpClient;

	public TranslatorTestBase()
	{
		MockHttpMessageHandler = new Mock<HttpMessageHandler>();
		MockHttpClient = new HttpClient(MockHttpMessageHandler.Object);

		ServiceCollection services = new ServiceCollection();
		IConfigurationRoot config = new ConfigurationBuilder()
			 .AddInMemoryCollection(new Dictionary<string, string?>
			 {
					 {"Translator:Host", "http://fake-translator-service"}
			 })
			 .Build();

		_ = services.AddSingleton<IConfiguration>(config);
		_ = services.AddLogging(builder => builder.AddConsole());
		_ = services.AddSingleton(MockHttpClient); // ✅ Register mock client
		_ = services.AddTransient<ITranslatorService, TranslatorService>();

		ServiceProvider serviceProvider = services.BuildServiceProvider();
		TranslatorService = serviceProvider.GetRequiredService<ITranslatorService>();
	}
}