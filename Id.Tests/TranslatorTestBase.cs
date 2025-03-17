/// <summary>
/// Base test class for TranslatorService tests.
/// This class provides common setup functionality for test cases,
/// including dependency injection and HTTP mocking.
/// </summary>
public class TranslatorTestBase
{
	/// <summary>
	/// The instance of ITranslatorService used in tests.
	/// </summary>
	protected readonly ITranslatorService TranslatorService;

	/// <summary>
	/// Mocked HTTP message handler for simulating API responses.
	/// </summary>
	protected readonly Mock<HttpMessageHandler> MockHttpMessageHandler;

	/// <summary>
	/// Mocked HttpClient that uses the mocked message handler.
	/// </summary>
	protected readonly HttpClient MockHttpClient;

	/// <summary>
	/// Initializes a new instance of the <see cref="TranslatorTestBase"/> class.
	/// Sets up dependency injection and registers a mock HTTP client.
	/// </summary>
	public TranslatorTestBase()
	{
		MockHttpMessageHandler = new Mock<HttpMessageHandler>();
		MockHttpClient = new HttpClient(MockHttpMessageHandler.Object);

		// Setup dependency injection container
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