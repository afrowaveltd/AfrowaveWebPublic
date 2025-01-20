namespace Id.Services
{
	public class UiTranslatorHostedService(ILogger<UiTranslatorHostedService> logger,
	IServiceProvider serviceProvider) : IHostedService, IDisposable
	{
		private IServiceProvider _serviceProvider = serviceProvider;
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
			using(IServiceScope scope = _serviceProvider.CreateScope())
			{
				IUiTranslatorService translator = scope.ServiceProvider.GetRequiredService<IUiTranslatorService>();
				try
				{
					translator.RunTranslationsAsync().GetAwaiter().GetResult();
				}
				catch(Exception ex)
				{
					_logger.LogError(ex, "An error occurred while running translations.");
				}
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