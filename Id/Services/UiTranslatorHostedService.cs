namespace Id.Services
{
	public class UiTranslatorHostedService(ILogger<UiTranslatorHostedService> logger,
		 IServiceProvider serviceProvider) : IHostedService, IDisposable
	{
		private IServiceProvider _serviceProvider = serviceProvider;
		private readonly ILogger<UiTranslatorHostedService> _logger = logger;
		private Timer? _timer;
		private volatile bool _isProcessing; // Vlajka pro sledování běžícího úkolu

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("UiTranslatorHostedService is starting.");

			// Startujeme první spuštění za 20 minut, perioda je nekonečná (one-shot timer)
			_timer = new Timer(async _ => await DoWorkAsync(), null, TimeSpan.FromMinutes(20), Timeout.InfiniteTimeSpan);

			return Task.CompletedTask;
		}

		private async Task DoWorkAsync()
		{
			if(_isProcessing) // Pokud již úkol běží, ukončíme metodu
			{
				_logger.LogInformation("Previous translation check is still running. Skipping.");
				return;
			}

			_isProcessing = true;

			try
			{
				_logger.LogInformation("UiTranslatorHostedService is working.");
				using(IServiceScope scope = _serviceProvider.CreateScope())
				{
					IUiTranslatorService translator = scope.ServiceProvider.GetRequiredService<IUiTranslatorService>();
					await translator.RunTranslationsAsync(); // Asynchronní volání
				}
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "An error occurred while running translations.");
			}
			finally
			{
				_isProcessing = false;

				// Reset timeru po dokončení s novým intervalem (např. 30 minut)
				// Změň TimeSpan.FromMinutes(30) podle potřeby
				_ = (_timer?.Change(TimeSpan.FromMinutes(30), Timeout.InfiniteTimeSpan));
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("UiTranslatorHostedService is stopping.");
			_ = (_timer?.Change(Timeout.Infinite, 0));
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_timer?.Dispose();
		}
	}
}