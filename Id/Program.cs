/* Program.cs */

using Id.I18n;
using Id.Middlewares;
using Microsoft.AspNetCore.HttpOverrides;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using SharedTools.Services.MdServices;

/// <summary>
/// The main entry point for the application.
/// </summary>
public class Program
{
	/// <summary>
	/// The main entry point for the application.
	/// </summary>
	/// <param name="args">Starting parameters</param>
	/// <returns>The ID project running</returns>
	/// <exception cref="InvalidOperationException"></exception>
	public static async global::System.Threading.Tasks.Task Main(string[] args)
	{
		WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

		Log.Logger = new LoggerConfiguration()
		.CreateLogger();

		IConfiguration configuration = builder.Configuration;

		_ = builder.Services.AddHttpContextAccessor();
		_ = builder.Services.Configure<ForwardedHeadersOptions>(options =>
		{
			options.ForwardedHeaders =
			ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
		});

		// select database provider and configure the database connection

		string provider = configuration.GetSection("DatabaseProvider").Value ?? "SQLite";
		_ = provider switch
		{
			"SQLite" => builder.Services.AddDbContext<ApplicationDbContext>(options =>
						  options.UseSqlite(configuration.GetConnectionString("SQLite"))),
			"SQLServer" => builder.Services.AddDbContext<ApplicationDbContext>(options =>
						  options.UseSqlServer(configuration.GetConnectionString("SQLServer"))),
			"PostgreSQL" => builder.Services.AddDbContext<ApplicationDbContext>(options =>
						  options.UseNpgsql(configuration.GetConnectionString("PostgreSQL"))),
			_ => throw new InvalidOperationException("Invalid database provider."),
		};
		_ = builder.Services.AddSerilog((serviceProvider, loggerConfiguration) => loggerConfiguration
			 .Enrich.FromLogContext()
			 .Enrich.WithUserName()
			 .Enrich.WithClientIp()
			 .Enrich.WithRequestHeader("user-agent")
			 .WriteTo.Async(x => x.Console(outputTemplate: "{Timestamp:HH:mm:ss.fff} [{Level:u3}] {ClientIp} {Message:lj}{NewLine}{Exception}", theme: AnsiConsoleTheme.Literate, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose))
			 .WriteTo.Async(x => x.File("logs/log.txt",
			 outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {ClientIp} {Message:lj}{NewLine}{Exception}",
			 rollingInterval: RollingInterval.Day))
			 .WriteTo.Async(s => s.SQLite("../../../logs/log.db", "Logs"))
		);

		_ = builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
		{
			// Set property naming policy to camelCase
			options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

			// Allow complex object types like Lists<T> or other nested members
			options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;

			// Add support for preserving references if needed (useful for circular references)
			options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

			// Customize any other settings as needed (e.g., number or date handling)
		});

		_ = builder.Services
			 .AddLocalization();

		_ = builder.Services.AddOpenApi("AfrowaveId");

		/*
        builder.Services.AddSwaggerGen(options =>
        {
            string xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });
        */

		// Add services to the container.
		_ = builder.Services.AddRazorPages()
			.AddViewLocalization();

		_ = builder.Services.AddControllers()
			 .AddJsonOptions(options =>
			 {
				 // Set property naming policy to camelCase
				 options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

				 // Allow Lists and nested objects
				 options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never;

				 // Handle circular references if applicable
				 options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
			 })
			 .AddXmlDataContractSerializerFormatters();

		// Middleware
		_ = builder.Services.AddTransient<I18nMiddleware>();
		// builder.Services.AddTransient<ErrorMiddleware>();

		// Scoped slu�by (HTTP request-based)
		_ = builder.Services.AddScoped<IApplicationLoader, ApplicationLoader>();
		_ = builder.Services.AddScoped<IApplicationsManager, ApplicationsManager>();
		_ = builder.Services.AddScoped<IApplicationUsersManager, ApplicationUsersManager>();
		_ = builder.Services.AddScoped<IBrandsManager, BrandsManager>();
		_ = builder.Services.AddScoped<ICookieService, CookieService>();
		_ = builder.Services.AddScoped<IInstallationStatusService, InstallationStatusService>();
		_ = builder.Services.AddScoped<IRolesManager, RolesManager>();
		_ = builder.Services.AddScoped<ISelectOptionsServices, SelectOptionsServices>();
		_ = builder.Services.AddScoped<ITermsService, TermsService>();
		_ = builder.Services.AddScoped<ITextToHtmlService, TextToHtmlService>();
		_ = builder.Services.AddScoped<ITextTranslationService, TextTranslationService>();
		_ = builder.Services.AddScoped<ITranslatorService, TranslatorService>();
		_ = builder.Services.AddScoped<ITranslationFilesManager, TranslationFilesManager>();
		_ = builder.Services.AddScoped<IUsersManager, UsersManager>();
		_ = builder.Services.AddScoped<IEmailManager, EmailManager>();
		_ = builder.Services.AddScoped<IHttpService, HttpService>();

		// Transient slu�by (stateless)
		_ = builder.Services.AddTransient<IStringLocalizerFactory, JsonStringLocalizerFactory>();
		_ = builder.Services.AddTransient<IEncryptionService, EncryptionService>();
		_ = builder.Services.AddTransient<IImageService, ImageService>();
		_ = builder.Services.AddTransient<IThemeService, ThemeService>();
		_ = builder.Services.AddTransient<IUiTranslatorService, UiTranslatorService>();

		// Singleton slu�by (glob�ln�, thread-safe)
		_ = builder.Services.AddSingleton<ISettingsService, SettingsService>();
		_ = builder.Services.AddSingleton<HttpClient>();
		ISettingsService settingsService = builder.Services.BuildServiceProvider().GetRequiredService<ISettingsService>();
		Id.Models.SettingsModels.IdentificatorSettings Settings = await settingsService.GetSettingsAsync();
		_ = builder.Services.AddSingleton<IMdConfigService>(provider =>
	 new MdConfigService(
		  masterPath: "SharedTools/Settings/MarkdownMappings.master.json",
		  userPath: "SharedTools/Settings/MarkdownMappings.user.json"
	 ));

		// Hosted services
		_ = builder.Services.AddHostedService<ScssCompilerService>();
		_ = builder.Services.AddHostedService<UiTranslatorHostedService>();

		// I need to use the settings service and load the settings to use them for cookies and JWT configuration

		WebApplication app = builder.Build();

		// Run startup tasks

		using IServiceScope scope = app.Services.CreateScope();

		IApplicationLoader loader = scope.ServiceProvider.GetRequiredService<IApplicationLoader>();
		ILogger<Program> logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
		await loader.ApplyMigrations();
		await loader.SeedLanguagesAsync();
		await loader.SeedCountriesAsync();
		await loader.TranslateLanguageNamesAsync();
		List<ApiResponse<List<string>>> assetTranslationResults = await loader.TranslateStaticAssetsAsync();
		foreach(ApiResponse<List<string>> result in assetTranslationResults)
		{
			if(result.Successful)
			{
				logger.LogInformation("Successfully translated {count} assets for {language}", result.Data?.Count, result.Message);
			}
			else
			{
				logger.LogError("Failed to translate assets: {error}", result.Message);
			}
		}

		string[] supportedCultures = loader.GetSupportedCultures();

		_ = app.UseMiddleware<I18nMiddleware>();

		_ = app.UseRequestLocalization(options =>
		{
			_ = options.AddSupportedCultures(supportedCultures)
					 .AddSupportedUICultures(supportedCultures)
					 .SetDefaultCulture(supportedCultures[0])
					 .ApplyCurrentCultureToResponseHeaders = true;
		});

		if(!app.Environment.IsDevelopment())
		{
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			_ = app.UseHsts();
		}
		_ = app.UseHttpsRedirection();

		_ = app.UseForwardedHeaders();
		_ = app.MapOpenApi()
			.CacheOutput();

		_ = app.MapScalarApiReference(options =>
		{
			_ = options
				 .WithTitle("Afrowave Id")
				 .WithTheme(ScalarTheme.Mars)
				 .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
		});

		_ = app.UseRouting();
		_ = app.UseMiddleware<CustomErrorHandlingMiddleware>();
		_ = app.UseAuthorization();

		_ = app.MapControllers();
		_ = app.MapStaticAssets();
		_ = app.MapRazorPages()
			.WithStaticAssets();

		app.Run();
	}
}