namespace Id.ConsoleToolkit
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			string toolkitRoot = Directory.GetCurrentDirectory();
			string mainAppSettingsPath = Path.Combine(toolkitRoot, "../Id/appsettings.json");

			if(!File.Exists(mainAppSettingsPath))
			{
				Console.WriteLine($"[x] Nepodařilo se najít appsettings.json hlavní aplikace na cestě: {mainAppSettingsPath}");
				return;
			}

			IConfigurationRoot mainAppConfig = new ConfigurationBuilder()
				 .SetBasePath(toolkitRoot)
				 .AddJsonFile(mainAppSettingsPath, optional: false)
				 .Build();
			/*
			var proxyConfig = mainAppConfig.Get<AppConfigurationProxy>();
			if(proxyConfig is null || string.IsNullOrWhiteSpace(proxyConfig.DatabaseProvider))
			{
				Console.WriteLine("[x] Nelze načíst konfiguraci hlavní aplikace nebo chybí DatabaseProvider.");
				return;
			}
			var connectionString = proxyConfig.GetSelectedConnectionString();
			if(connectionString is null)
			{
				Console.WriteLine($"[x] Připojovací řetězec pro poskytovatele '{proxyConfig.DatabaseProvider}' nebyl nalezen.");
				return;
			}

			*/
			IHostBuilder builder = Host.CreateDefaultBuilder(args)

				 .ConfigureServices((_, services) =>
				 {
					 /*
					 _ = services.AddLogging();

					 services.AddSingleton(proxyConfig);

					 switch(proxyConfig.DatabaseProvider?.ToLower())
					 {
						 case "sqlite":
							 _ = services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));
							 break;

						 case "sqlserver":
							 _ = services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
							 break;

						 case "postgresql":
							 _ = services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
							 break;

						 default:
							 throw new Exception($"Nepodporovaný provider databáze: {proxyConfig.DatabaseProvider}");
					 }

				//	 _ = services.AddScoped<MainMenu>();
				//	 _ = services.AddScoped<ThemeImportService>();
					 */
				 });

			IHost host = builder.Build();

			using IServiceScope scope = host.Services.CreateScope();
			//var menu = scope.ServiceProvider.GetRequiredService<MainMenu>();
			//await menu.RunAsync();
		}
	}
}