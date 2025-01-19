namespace Id.Services
{
	public class UiTranslatorHostedService(ILogger<UiTranslatorHostedService> logger, IUiTranslatorService translator) : IHostedService, IDisposable
	{
		private readonly IUiTranslatorService _translator = translator;
		private readonly ILogger<UiTranslatorHostedService> _logger = logger;
		private Timer? _timer;

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("UiTranslatorHostedService is starting.");
			_timer = new Timer(DoWork, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(15));
			return Task.CompletedTask;
		}

		private void DoWork(object? state)
		{
			_logger.LogInformation("UiTranslatorHostedService is working.");
			_translator.RunTranslationsAsync().GetAwaiter().GetResult();
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("UiTranslatorHostedService is stopping.");
			_timer?.Change(Timeout.Infinite, 0);
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_timer?.Dispose();
		}
	}
}