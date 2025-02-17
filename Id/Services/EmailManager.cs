using Id.Models.ResultModels;
using MimeKit;
using RazorLight;

namespace Id.Services
{
	public class EmailManager(IStringLocalizer<EmailManager> t,
		ILogger<EmailManager> logger,
		ISettingsService settings,
		IApplicationService applicationService)
	{
		// Initialize
		private readonly IStringLocalizer<EmailManager> _t = t;

		private readonly string _authenticatorId = settings.GetApplicationIdAsync().GetAwaiter().GetResult();
		private readonly ILogger<EmailManager> _logger = logger;
		private readonly ISettingsService _settings = settings;
		private readonly IApplicationService _applicationService = applicationService;

		private readonly RazorLightEngine _razorEngine = new RazorLightEngineBuilder()
			.UseEmbeddedResourcesProject(typeof(EmailManager))
			.UseMemoryCachingProvider()
			.Build();

		// public methods

		// private methods
		private async Task<EmailResult> SendEmailAsync(MimeMessage message)
		{
			EmailResult result = new();
			return result;
		}
	}
}