/* Program.cs */

using Id.I18n;
using Id.Middlewares;
using Microsoft.AspNetCore.HttpOverrides;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
.CreateLogger();

IConfiguration configuration = builder.Configuration;

builder.Services.AddHttpContextAccessor();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
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
builder.Services.AddSerilog((serviceProvider, loggerConfiguration) => loggerConfiguration
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

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
	// Set property naming policy to camelCase
	options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

	// Allow complex object types like Lists<T> or other nested members
	options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;

	// Add support for preserving references if needed (useful for circular references)
	options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

	// Customize any other settings as needed (e.g., number or date handling)
});

builder.Services
	 .AddLocalization();

builder.Services.AddOpenApi("AfrowaveId");

/*
builder.Services.AddSwaggerGen(options =>
{
	string xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
	string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	options.IncludeXmlComments(xmlPath);
});
*/

// Add services to the container.
builder.Services.AddRazorPages()
	.AddViewLocalization();

builder.Services.AddControllers()
	 .AddJsonOptions(options =>
	 {
		 // Set property naming policy to camelCase
		 options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

		 // Allow Lists and nested objects
		 options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never;

		 // Handle circular references if applicable
		 options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
	 });

// Middleware
builder.Services.AddTransient<I18nMiddleware>();
// builder.Services.AddTransient<ErrorMiddleware>();

// Scoped slu�by (HTTP request-based)
builder.Services.AddScoped<IApplicationLoader, ApplicationLoader>();
builder.Services.AddScoped<IApplicationsManager, ApplicationsManager>();
builder.Services.AddScoped<IApplicationUsersManager, ApplicationUsersManager>();
builder.Services.AddScoped<IBrandsManager, BrandsManager>();
builder.Services.AddScoped<ICookieService, CookieService>();
builder.Services.AddScoped<IInstallationStatusService, InstallationStatusService>();
builder.Services.AddScoped<IRolesManager, RolesManager>();
builder.Services.AddScoped<ISelectOptionsServices, SelectOptionsServices>();
builder.Services.AddScoped<ITermsService, TermsService>();
builder.Services.AddScoped<ITextToHtmlService, TextToHtmlService>();
builder.Services.AddScoped<ITextTranslationService, TextTranslationService>();
builder.Services.AddScoped<ITranslatorService, TranslatorService>();
builder.Services.AddScoped<IUsersManager, UsersManager>();
builder.Services.AddScoped<IEmailManager, EmailManager>();

// Transient slu�by (stateless)
builder.Services.AddTransient<IStringLocalizerFactory, JsonStringLocalizerFactory>();
builder.Services.AddTransient<IEncryptionService, EncryptionService>();
builder.Services.AddTransient<IImageService, ImageService>();
builder.Services.AddTransient<IThemeService, ThemeService>();
builder.Services.AddTransient<IUiTranslatorService, UiTranslatorService>();

// Singleton slu�by (glob�ln�, thread-safe)
builder.Services.AddSingleton<ISettingsService, SettingsService>();
ISettingsService settingsService = builder.Services.BuildServiceProvider().GetRequiredService<ISettingsService>();
Id.Models.SettingsModels.IdentificatorSettings Settings = await settingsService.GetSettingsAsync();
// Hosted services
builder.Services.AddHostedService<ScssCompilerService>();
builder.Services.AddHostedService<UiTranslatorHostedService>();

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

app.UseMiddleware<I18nMiddleware>();

app.UseRequestLocalization(options =>
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
app.UseHttpsRedirection();

app.UseForwardedHeaders();
app.MapOpenApi()
	.CacheOutput();

app.MapScalarApiReference(options =>
{
	_ = options
		 .WithTitle("Afrowave Id")
		 .WithTheme(ScalarTheme.Mars)
		 .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
});

app.UseRouting();
app.UseMiddleware<CustomErrorHandlingMiddleware>();
app.UseAuthorization();

app.MapControllers();
app.MapStaticAssets();
app.MapRazorPages()
	.WithStaticAssets();
app.Use(async (context, next) =>
{
	await next();

	if(context.Response.StatusCode == 404) // Detects 404 responses
	{
		ILogger<Program> logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
		logger.LogWarning("404 Not Found: {Path}", context.Request.Path);

		// Clear the response and reprocess it through the custom middleware
		context.Response.Clear();
		context.Response.StatusCode = 404;

		CustomErrorHandlingMiddleware middleware = new CustomErrorHandlingMiddleware(_ => Task.CompletedTask,
	 context.RequestServices.GetRequiredService<IWebHostEnvironment>(),
	 context.RequestServices.GetRequiredService<ILogger<CustomErrorHandlingMiddleware>>());

		await middleware.InvokeAsync(context);
	}
});
app.Run();