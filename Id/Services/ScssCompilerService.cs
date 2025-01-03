using LibSassHost;

namespace Id.Services
{
	public class ScssCompilerService : IHostedService, IDisposable
	{
		private readonly ILogger<ScssCompilerService> _logger;
		private readonly string _scssDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "css");
		private FileSystemWatcher _watcher;

		public ScssCompilerService(ILogger<ScssCompilerService> logger)
		{
			_logger = logger;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Starting SCSS Compiler Service...");
			EnsureCssFilesUpToDate();
			StartWatching();
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Stopping SCSS Compiler Service...");
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
			_watcher.EnableRaisingEvents = true;

			_logger.LogInformation("Watching SCSS files in {Path}", _scssDirectory);
		}

		private void OnScssFileChanged(object sender, FileSystemEventArgs e)
		{
			_logger.LogInformation("Detected change in {FileName}, recompiling...", e.Name);
			CompileScssToCss(e.FullPath);
		}

		private void EnsureCssFilesUpToDate()
		{
			foreach(var scssFile in Directory.GetFiles(_scssDirectory, "*-theme.scss"))
			{
				CompileScssToCss(scssFile);
			}
		}

		private void CompileScssToCss(string scssFilePath)
		{
			try
			{
				var cssFilePath = Path.ChangeExtension(scssFilePath, ".css");
				var scssContent = File.ReadAllText(scssFilePath);
				var compiledCss = SassCompiler.Compile(scssContent);

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