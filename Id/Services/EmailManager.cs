using Id.Models.CommunicationModels;
using Id.Models.InputModels;
using Id.Models.ResultModels;
using Id.Pages.Install;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using RazorLight;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
		public async Task<ApiResponse<SmtpSettingsModel>> AutodetectSmtpSettingsAsync(DetectSmtpSettingsInput input)
		{
			// Common SMTP ports
			List<(int Port, SecureSocketOptions Security)> portSecurityCombinations = new List<(int Port, SecureSocketOptions Security)>()
			{
				(25, SecureSocketOptions.None),
				(587, SecureSocketOptions.StartTls),
				(465, SecureSocketOptions.SslOnConnect),
				(25, SecureSocketOptions.Auto),
				(587, SecureSocketOptions.Auto),
				(465, SecureSocketOptions.Auto),
				(2525, SecureSocketOptions.Auto) // failback
			};

			ApiResponse<SmtpSettingsModel> response = new();
			response.Data = new SmtpSettingsModel();
			foreach((int port, SecureSocketOptions security) in portSecurityCombinations)
			{
				try
				{
					using SmtpClient client = new();
					// attemt to connect with the current combination
					await client.ConnectAsync(input.Host, port, security);

					// check if the server requires authentication
					if(client.Capabilities.HasFlag(SmtpCapabilities.Authentication))
					{
						response.Data.AuthorizationRequired = true;
						if(string.IsNullOrEmpty(input.Username) || string.IsNullOrEmpty(input.Password))
						{
							response.Success = false;
							response.Message = _t["Server requires authentication, but no credentials provided"];
							return response;
						}
						await client.AuthenticateAsync(input.Username, input.Password);
					}
					response.Successful = true;
					response.Data.Host = input.Host;
					response.Data.Port = port;
					response.Data.Secure = security;
					response.Data.Username = input.Username;
					response.Data.Password = input.Password;
					response.Data.SenderEmail = input.SenderEmail;
					response.Data.SenderName = input.SenderEmail;
					await client.DisconnectAsync(true);
					response.Message = _t["SMTP settings successfully detected"];
					return response;
				}
				catch()
				{
					continue;
				}
			}

			// if we reach this point, the autodetection failed
			response.Successful = false;
			response.Message = _t["SMTP settings autodetection failed"];
			return response;
		}

		public async Task<EmailResult> SendEmailAsync(string targetEmail, string subject, string body, string applicationId)
		{
			ApplicationSmtpSettings? smtpSettings = await _applicationManager.GetApplicationSmtpSettingsAsync(applicationId);
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
			ApplicationSmtpSettings? smtpSettings = await _applicationManager.GetApplicationSmtpSettingsAsync(applicationId);
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

		public async Task<SmtpTestResult> TestSmtpSettingsAsync(SmtpSenderModel input)
		{
			string targetEmail = input.TargetAddress ?? input.SenderEmail;
			SmtpTestResult result = new();
			StringBuilder logBuilder = new StringBuilder();
			if(string.IsNullOrEmpty(input.Host))
			{
				result.Error = _t["Host is empty"];
				return result;
			}
			if(input.Port == 0)
			{
				result.Error = _t["Port is empty"];
				return result;
			}
			if(string.IsNullOrEmpty(input.SenderEmail))
			{
				result.Error = _t["Sender email is empty"];
				return result;
			}
			if(string.IsNullOrEmpty(input.SenderName))
			{
				input.SenderName = input.SenderEmail;
			}
			try
			{
				using MemoryStream memoryStream = new();
				using SmtpClient client = new(new ProtocolLogger(memoryStream));
				await client.ConnectAsync(input.Host, input.Port, input.Secure);

				if(input.AuthorizationRequired)
				{
					if(string.IsNullOrEmpty(input.Username) || string.IsNullOrEmpty(input.Password))
					{
						result.Error = _t["Server requires authentication, but no credentials provided"];
						return result;
					}
					await client.AuthenticateAsync(input.Username, input.Password);
				}

				MimeMessage message = new();
				message.From.Add(new MailboxAddress(input.SenderName, input.SenderEmail));
				message.To.Add(new MailboxAddress("", targetEmail));
				message.Subject = _t["SMTP test"];
				message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
				{
					Text = _t["SMTP test email"]
				};
				await client.SendAsync(message);
				await client.DisconnectAsync(true);
				result.Success = true;
				result.Log = memoryStream.ToString();
				return result;
			}
			catch(Exception ex)
			{
				result.Successful = false;
				result.Error = ex.Message;
			}
			finally
			{
				// Ensure the log is attached even if there's an error
				response.Log ??= logBuilder.ToString();
			}
			result.Data = response;
			return result;
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