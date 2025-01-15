using LibSassHost;

namespace Id.Services
{
	public class ThemeManagementService : IHostedService, IDisposable
	{
		private readonly ILogger<ThemeManagementService> _logger;

		//private readonly IThemeService _themeService;
		private readonly string _scssDirectory;

		private FileSystemWatcher? _watcher;

		public ThemeManagementService(ILogger<ThemeManagementService> logger, IThemeService themeService)
		{
			_logger = logger;
			//_themeService = themeService;
			_scssDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "css");
			Task.Run(() => OnReferenceThemeChanged(this, new FileSystemEventArgs(WatcherChangeTypes.Changed, _scssDirectory, "light-theme.scss")));
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Starting Theme Management Service...");

			if(!Directory.Exists(_scssDirectory))
			{
				_logger.LogError($"SCSS directory '{_scssDirectory}' not found.");
				return Task.CompletedTask;
			}

			EnsureCssFilesUpToDate();
			StartWatching();

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Stopping Theme Management Service...");
			_watcher?.Dispose();
			return Task.CompletedTask;
		}

		private void StartWatching()
		{
			_watcher = new FileSystemWatcher(_scssDirectory, "*-theme.scss")
			{
				NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName
			};

			_watcher.Changed += OnScssFileChanged;
			_watcher.Created += OnScssFileChanged;

			// Watching only "light-theme.scss" specifically
			string referenceThemePath = Path.Combine(_scssDirectory, "light-theme.scss");
			if(File.Exists(referenceThemePath))
			{
				FileSystemWatcher referenceWatcher = new FileSystemWatcher(_scssDirectory, "light-theme.scss")
				{
					NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName
				};
				referenceWatcher.Changed += OnReferenceThemeChanged;
				referenceWatcher.Created += OnReferenceThemeChanged;
				referenceWatcher.EnableRaisingEvents = true;
			}
			else
			{
				_logger.LogError("Reference theme file not found at startup: {Path}", referenceThemePath);
			}

			_watcher.EnableRaisingEvents = true;
			_logger.LogInformation("Watching SCSS files in {Path}", _scssDirectory);
		}

		private async void OnReferenceThemeChanged(object sender, FileSystemEventArgs e)
		{
			_logger.LogInformation("Reference theme {FileName} changed, updating all themes...", e.Name);

			await Task.Delay(500); // Ensure file write is complete

			try
			{
				string referencePath = Path.Combine(_scssDirectory, "light-theme.scss");
				if(!File.Exists(referencePath))
				{
					_logger.LogError("Reference theme file not found.");
					return;
				}

				string referenceContent = await File.ReadAllTextAsync(referencePath);
				string[] referenceLines = referenceContent.Split('\n');

				// Extract everything BELOW the color definitions
				int colorEndIndex = Array.FindLastIndex(referenceLines, line => line.Trim().StartsWith("$"));
				string commonScssContent = colorEndIndex >= 0
					 ? string.Join("\n", referenceLines.Skip(colorEndIndex + 1))
					 : "";

				// Update all theme files
				string[] themeFiles = Directory.GetFiles(_scssDirectory, "*-theme.scss", SearchOption.TopDirectoryOnly);
				foreach(string themeFile in themeFiles)
				{
					string[] lines = await File.ReadAllLinesAsync(themeFile);
					string fontDefinition = lines.Length > 0 ? lines[0] : "";
					string userColorLines = string.Join("\n", lines.Take(colorEndIndex + 1));

					string newContent = $"{fontDefinition}\n{userColorLines}\n{commonScssContent}";
					await File.WriteAllTextAsync(themeFile, newContent);
				}

				_logger.LogInformation("All themes updated with new SCSS rules.");
				EnsureCssFilesUpToDate(); // Recompile all themes
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Error while updating themes.");
			}
		}

		private void OnScssFileChanged(object sender, FileSystemEventArgs e)
		{
			_logger.LogInformation("Detected change in {FileName}, recompiling...", e.Name);
			CompileScssToCss(e.FullPath);
		}

		private void EnsureCssFilesUpToDate()
		{
			foreach(string scssFile in Directory.GetFiles(_scssDirectory, "*-theme.scss"))
			{
				CompileScssToCss(scssFile);
			}
		}

		private void CompileScssToCss(string scssFilePath)
		{
			try
			{
				string cssFilePath = Path.ChangeExtension(scssFilePath, ".css");
				string scssContent = File.ReadAllText(scssFilePath);
				CompilationResult compiledCss = SassCompiler.Compile(scssContent);

				File.WriteAllText(cssFilePath, compiledCss.CompiledContent);
				_logger.LogInformation("Compiled {ScssFile} -> {CssFile}", scssFilePath, cssFilePath);
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Failed to compile {ScssFile}", scssFilePath);
			}
		}

		public void Dispose()
		{
			_watcher?.Dispose();
		}
	}
}