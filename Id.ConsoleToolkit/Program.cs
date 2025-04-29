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
		_ = services.AddSingleton<IConsoleViewHelper, ConsoleViewHelper>();
		_ = services.AddSingleton<IMainMenuView, MainMenuView>();

		_ = services.AddDistributedMemoryCache(); // 1
		_ = services.AddLocalization(); // 2
		_ = services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>(); // 3

		_ = services.AddTransient<IBrowser, Browser>();

	})
	.Build();

ISettingsService settingsService = host.Services.GetRequiredService<ISettingsService>();
await settingsService.LoadAsync();
CultureApplier.ApplyCulture(SettingsService.Current.DefaultLanguage);
IStringLocalizer _t = host.Services.GetRequiredService<IStringLocalizer<Id.ConsoleToolkit.Views.MainMenuView>>();
string selection = string.Empty;
while(selection != "exit")
{
	await host.Services.GetRequiredService<IMainMenuView>().RunAsync();
	selection = "exit";
}
Console.Clear();
AnsiConsole.MarkupLine($"[grey]{_t["Toolkit is closing"]}[/]");
await Task.Delay(500);
// krátké pozastavení na efekt