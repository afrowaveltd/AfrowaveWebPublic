using LibSassHost;

namespace Id.Services
{
	/// <summary>
	/// Service to compile SCSS files to CSS.
	/// </summary>
	/// <param name="logger"></param>
	public class ScssCompilerService(ILogger<ScssCompilerService> logger) : IHostedService, IDisposable
	{
		private readonly ILogger<ScssCompilerService> _logger = logger;
		private readonly string _scssDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "css");
		private FileSystemWatcher _watcher;

		/// <summary>
		/// Initializes a new instance of the <see cref="ScssCompilerService"/> class.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Starting SCSS Compiler Service...");
			EnsureCssFilesUpToDate();
			StartWatching();
			return Task.CompletedTask;
		}

		/// <summary>
		/// Stops the SCSS Compiler Service.
		/// </summary>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns></returns>
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

		/// <summary>
		/// Disposes the FileSystemWatcher.
		/// </summary>
		public void Dispose()
		{
			_watcher?.Dispose();
		}
	}
}