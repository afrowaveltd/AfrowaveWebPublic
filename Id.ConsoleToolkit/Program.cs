IHost host = Host.CreateDefaultBuilder(args)
	.ConfigureServices((hostContext, services) =>
	{
		_ = services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
		{
			options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
			options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
			options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
		});
		_ = services.AddSingleton<ISettingsService, SettingsService>();
		_ = services.AddLocalization();
		_ = services.AddTransient<IStringLocalizerFactory, JsonStringLocalizerFactory>();
		_ = services.AddTransient<IBrowser, Browser>();
	})
	.Build();

ISettingsService settingsService = host.Services.GetRequiredService<ISettingsService>();
await settingsService.LoadAsync();
CultureApplier.ApplyCulture(SettingsService.Current.DefaultLanguage);

await host.RunAsync();