using Id.Models.ResultModels;
using MailKit.Net.Smtp;
using MimeKit;
using RazorLight;
using System.ComponentModel.DataAnnotations;

namespace Id.Services
{
	public class EmailManager(IStringLocalizer<EmailManager> t,
		ILogger<EmailManager> logger,
		ISettingsService settings,
		IApplicationsManager applicationManager)
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
			message.From.Add(new MailboxAddress(smtpSettings.SenderName, smtpSettings.SenderEmail));
			message.To.Add(new MailboxAddress("", targetEmail));
			message.Subject = subject;
			message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
			{
				Text = body
			};
			return await SendEmailAsync(message, smtpSettings);
		}

		public async Task<EmailResult> SendEmailAsync(string targetEmail, string subject, string body)
		{
			return await SendEmailAsync(targetEmail, subject, body, _authenticatorId);
		}

		public async Task<EmailResult> SendEmailFromTemplateAsync(string targetEmail, string templateName, object model, string applicationId)
		{
			ApplicationSmtpSettings smtpSettings = await _applicationManager.GetApplicationSmtpSettingsAsync(applicationId);
			if(smtpSettings == null)
			{
				_logger.LogError("SMTP settings not found for application {applicationId}", applicationId);
				return new EmailResult
				{
					TargetEmail = targetEmail,
					Subject = templateName,
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
					Subject = templateName,
					Success = false,
					ErrorMessage = _t["Target email is empty"]
				};
			}
			if(string.IsNullOrEmpty(templateName))
			{
				_logger.LogError("Template name is empty");
				return new EmailResult
				{
					TargetEmail = targetEmail,
					Subject = templateName,
					Success = false,
					ErrorMessage = _t["Template name is empty"]
				};
			}
			string body = await _razorEngine.CompileRenderAsync($"Templates.{templateName}", model);
			string subject = model.GetType().GetProperty("Subject")?.GetValue(model)?.ToString() ?? _t["Notification"];
			return await SendEmailAsync(targetEmail, subject, body, applicationId);
		}

		public async Task<EmailResult> SendEmailFromTemplateAsync(string targetEmail, string templateName, object model)
		{
			return await SendEmailFromTemplateAsync(targetEmail, templateName, model, _authenticatorId);
		}

		public async Task<SendEmailsResult> SendGroupEmailFromTemplateAsync(List<string> targetEmails, string templateName, object model, string applicationId)
		{
			SendEmailsResult result = new();
			result.Subject = model.GetType().GetProperty("Subject")?.GetValue(model)?.ToString() ?? _t["Notification"];
			// do checks if address list is not empty, and in each loop check if the recepient address is valid
			if(targetEmails != null && targetEmails.Count > 0)
			{
				foreach(string recepient in targetEmails)
				{
					if(!string.IsNullOrEmpty(recepient) && new EmailAddressAttribute().IsValid(recepient))
					{
						EmailResult emailResult = await SendEmailFromTemplateAsync(recepient, templateName, model, applicationId);
						result.Results.Add(emailResult);
					}
					else
					{
						_logger.LogError("Invalid email address: {recepient}", recepient);
						result.Results.Add(new EmailResult
						{
							TargetEmail = recepient,
							Subject = model.GetType().GetProperty("Subject")?.GetValue(model)?.ToString() ?? _t["Notification"],
							Success = false,
							ErrorMessage = _t["Invalid email address"]
						});
					}
				}
				return result;
			}
			else
				if(targetEmails == null)
			{
				_logger.LogError("Target email list is null");
				result.Results.Add(new EmailResult
				{
					TargetEmail = "",
					Subject = model.GetType().GetProperty("Subject")?.GetValue(model)?.ToString() ?? _t["Notification"],
					Success = false,
					ErrorMessage = _t["Target email list is null"]
				});
				return result;
			}
			else
			{
				_logger.LogError("Target email list is empty");
				result.Results.Add(new EmailResult
				{
					TargetEmail = "",
					Subject = model.GetType().GetProperty("Subject")?.GetValue(model)?.ToString() ?? _t["Notification"],
					Success = false,
					ErrorMessage = _t["Target email list is empty"]
				});
				return result;
			}
		}

		public async Task<SendEmailsResult> SendGroupEmailFromTemplateAsync(List<string> targetEmails, string templateName, object model)
		{
			return await SendGroupEmailFromTemplateAsync(targetEmails, templateName, model, _authenticatorId);
		}

		public async Task<SendEmailsResult> SendGroupEmailAsync(List<string> targetEmails, string subject, string body, string applicationId)
		{
			SendEmailsResult result = new();
			result.Subject = subject;
			// do checks if address list is not empty, and in each loop check if the recepient address is valid
			if(targetEmails != null && targetEmails.Count > 0)
			{
				foreach(string recepient in targetEmails)
				{
					if(!string.IsNullOrEmpty(recepient) && new EmailAddressAttribute().IsValid(recepient))
					{
						EmailResult emailResult = await SendEmailAsync(recepient, subject, body, applicationId);
						result.Results.Add(emailResult);
					}
					else
					{
						_logger.LogError("Invalid email address: {recepient}", recepient);
						result.Results.Add(new EmailResult
						{
							TargetEmail = recepient,
							Subject = subject,
							Success = false,
							ErrorMessage = _t["Invalid email address"]
						});
					}
				}
				return result;
			}
			else
				if(targetEmails == null)
			{
				_logger.LogError("Target email list is null");
				result.Results.Add(new EmailResult
				{
					TargetEmail = "",
					Subject = subject,
					Success = false,
					ErrorMessage = _t["Target email list is null"]
				});
				return result;
			}
			else
			{
				_logger.LogError("Target email list is empty");
				result.Results.Add(new EmailResult
				{
					TargetEmail = "",
					Subject = subject,
					Success = false,
					ErrorMessage = _t["Target email list is empty"]
				});
				return result;
			}
		}

		public async Task<SendEmailsResult> SendGroupEmailAsync(List<string> targetEmails, string subject, string body)
		{
			return await SendGroupEmailAsync(targetEmails, subject, body, _authenticatorId);
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
				_ = await client.SendAsync(message);
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