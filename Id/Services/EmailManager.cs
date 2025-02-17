using Id.Models.ResultModels;
using MimeKit;
using RazorLight;

namespace Id.Services
{
	public class EmailManager(IStringLocalizer<EmailManager> t,
		ILogger<EmailManager> logger,
		ISettingsService settings,
		IApplicationsManager applicationService)
	{
		// Initialize
		private readonly IStringLocalizer<EmailManager> _t = t;

		private readonly string _authenticatorId = settings.GetApplicationIdAsync().GetAwaiter().GetResult();
		private readonly ILogger<EmailManager> _logger = logger;
		private readonly ISettingsService _settings = settings;
		private readonly IApplicationsManager _applicationManager = applicationManager;

		private readonly RazorLightEngine _razorEngine = new RazorLightEngineBuilder()
			.UseEmbeddedResourcesProject(typeof(EmailManager))
			.UseMemoryCachingProvider()
			.Build();

		// public methods
		public async Task<EmailResult> SendEmailAsync(string targetEmail, string subject, string body, string applicationId)
		{
			ApplicationSmtpSettings smtpSettings = await _applicationManager.GetApplicationSmtpSettingsAsync(applicationId);
			if(smtpSettings == null)
			{
				_logger.LogError("SMTP settings not found for application {applicationId}", applicationId);
				return new EmailResult
				{
					TargetEmail = targetEmail,
					Subject = subject,
					Success = false,
					ErrorMessage = _t["SMTP settings not found"]
				};
			}
			if(string.IsNullOrEmpty(targetEmail))
			{
				_logger.LogError("Target email is empty");
				return new EmailResult
				{
					TargetEmail = targetEmail,
					Subject = subject,
					Success = false,
					ErrorMessage = _t["Target email is empty"]
				};
			}
			if(string.IsNullOrEmpty(subject))
			{
				_logger.LogError("Subject is empty");
				return new EmailResult
				{
					TargetEmail = targetEmail,
					Subject = subject,
					Success = false,
					ErrorMessage = _t["Subject is empty"]
				};
			}
			if(string.IsNullOrEmpty(body))
			{
				_logger.LogError("Body is empty");
				return new EmailResult
				{
					TargetEmail = targetEmail,
					Subject = subject,
					Success = false,
					ErrorMessage = _t["Body is empty"]
				};
			}
			MimeMessage message = new();
			message.From.Add(new MailboxAddress(smtpSettings.FromName, smtpSettings.FromEmail));
			message.To.Add(new MailboxAddress(targetEmail));
			message.Subject = subject;
			message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
			{
				Text = body
			};
			return await SendEmailAsync(message, smtpSettings);
		}

		// private methods
		private async Task<EmailResult> SendEmailAsync(MimeMessage message, ApplicationSmtpSettings settings)
		{
			EmailResult result = new();
			result.Subject = message.Subject;
			result.TargetEmail = message.To.ToString();
			try
			{
				using SmtpClient client = new();
				await client.ConnectAsync(settings.Host, settings.Port, settings.Secure);
				if(settings.AuthorizationRequired)
				{
					await client.AuthenticateAsync(settings.Username, settings.Password);
				}
				await client.SendAsync(message);
				await client.DisconnectAsync(true);
				result.Success = true;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Error sending email");
				result.ErrorMessage = _t["Error sending email"];
			}
			return result;
		}
	}
}