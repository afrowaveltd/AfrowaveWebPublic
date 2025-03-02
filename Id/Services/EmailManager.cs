/*
 *  Class: EmailManager implements IEmailManager interface,
 *  is used to manage email sending and SMTP settings autodetection and testing,
 *  Sends emails using the provided SMTP settings and a template.
 */

using Id.Models.CommunicationModels;
using Id.Models.InputModels;
using Id.Models.ResultModels;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using RazorLight;
using System.ComponentModel.DataAnnotations;

namespace Id.Services
{
	/// <summary>
	/// Email manager class
	/// </summary>
	/// <param name="t">Localization service</param>
	/// <param name="logger">Logging service</param>
	/// <param name="settings">Settings service</param>
	/// <param name="applicationManager">ApplicationManager</param>
	public class EmailManager(IStringLocalizer<EmailManager> t,
		ILogger<EmailManager> logger,
		ISettingsService settings,
		IApplicationsManager applicationManager) : IEmailManager
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

		/// <summary>
		/// Autodetects SMTP settings based on the provided input
		/// </summary>
		/// <param name="input">Basic model including sender name, email, host server and its login data</param>
		/// <returns>SmtpSenderModel class with values going from the autodetection</returns>
		/// <example>
		///  Example usage:
		///  await AutodetectSmtpSettingsAsync(new DetectSmtpSettingsInput { Host = "smtp.example.com", SenderEmail = "example@example.com", Username = "example", Password = "password" });
		///  Example response:
		///  {
		///		Successful : true,
		///		Data: {
		///		  Host: "smtp.example.com",
		///		  Port: 587,
		///		  Secure: 2,
		///		  Username: "example",
		///		  Password: "password",
		///		  SenderEmail: "example@example.com",
		///		  SenderName: "example"
		///		  },
		///		Message = "SMTP settings successfully detected"
		///	 }
		///	 </example>
		public async Task<SmtpDetectionResult> AutodetectSmtpSettingsAsync(DetectSmtpSettingsInput input)
		{
			// Common SMTP ports
			List<(int Port, SecureSocketOptions Security)> portSecurityCombinations =
			[
				(25, SecureSocketOptions.None),
				(587, SecureSocketOptions.StartTls),
				(465, SecureSocketOptions.SslOnConnect),
				(25, SecureSocketOptions.Auto),
				(587, SecureSocketOptions.Auto),
				(465, SecureSocketOptions.Auto),
				(2525, SecureSocketOptions.Auto) // failback
			];

			SmtpDetectionResult response = new();

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
						response.RequiresAuthentication = true;
						if(string.IsNullOrEmpty(input.Username) || string.IsNullOrEmpty(input.Password))
						{
							response.Successful = false;
							response.Message = "Server requires authentication, but no credentials provided";
							return response;
						}
						await client.AuthenticateAsync(input.Username, input.Password);
					}
					response.Successful = true;
					response.Port = port;
					response.Secure = security;
					await client.DisconnectAsync(true);
					response.Message = "SMTP settings successfully detected";
					return response;
				}
				catch
				{
					continue;
				}
			}

			// if we reach this point, the autodetection failed
			response.Successful = false;
			response.Message = "SMTP settings not found";
			return response;
		}

		/// <summary>
		/// Sends an email using the provided SMTP settings
		/// </summary>
		/// <param name="targetEmail">Recipient email address</param>
		/// <param name="subject">Email subject</param>
		/// <param name="body">HTML email body</param>
		/// <param name="applicationId">Application ID to load SMTP settings</param>
		/// <returns>EmailResult</returns>
		/// <example>
		/// <!-- Example usage: -->
		/// await SendEmailAsync("test@somedomain.com", "Test email", "This is a test email", "applicationId");
		/// <!-- Example response: -->
		/// {
		///		TargerEmail: "test@somedomain.com",
		///		Sucess: true,
		///		Suject: "Test email",
		///		ErrorMesssage: null
		///	 }
		///	 <!-- Example error response: -->
		///	 {
		///	   TargetEmail: "test@somedomain.com",
		///	   Subject: "Test email",
		///	   Success: false,
		///	   ErrorMessage: "SMTP settings not found"
		///	 {
		///	</example>
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

		/// <summary>
		/// Sends an email using the provided SMTP settings
		/// </summary>
		/// <param name="targetEmail" > Recipient email address</param>
		/// <param name="subject">Email subject</param>
		/// <param name="body">HTML email body</param>
		/// <returns>EmailResult</returns>
		/// <example>
		/// <!-- Example usage: -->
		/// await SendEmailAsync("test@somedomain.com", "Test email", "This is a test email", "applicationId");
		/// <!-- Example response: -->
		/// {
		///		TargerEmail: "test@somedomain.com",
		///		Sucess: true,
		///		Suject: "Test email",
		///		ErrorMesssage: null
		///	 }
		///	 <!-- Example error response: -->
		///	 {
		///	   TargetEmail: "test@somedomain.com",
		///	   Subject: "Test email",
		///	   Success: false,
		///	   ErrorMessage: "SMTP settings not found"
		///	 {
		///	</example>
		public async Task<EmailResult> SendEmailAsync(string targetEmail, string subject, string body)
		{
			return await SendEmailAsync(targetEmail, subject, body, _authenticatorId);
		}

		/// <summary> Sends an email using the provided SMTP settings and a template </summary>
		/// <param name="applicationId">ID aplikace</param>
		/// <param name="model">Model object for the razor template</param>
		/// <param name="targetEmail">Recipient email address</param>
		/// <param name="templateName">Name of the razor template</param>
		/// <permission cref="EmailManager">Requires permission to send emails</permission>
		/// <returns>Result of the email sending</returns>
		/// <example>
		/// <!-- Example usage: -->
		/// await SendEmailFromTemplateAsync("some@somedomain.com", "TemplateName", new { Subject = "Test email", Body = "This is a test email" }, "applicationId");
		/// <!-- Example response: -->
		/// new EmailResult()
		/// {
		///   TargetEmail: "some@somedomain.com",
		///   Subject: "TemplateName",
		///   Success: true,
		///   ErrorMessage: null
		/// }
		/// </example>
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

		/// <summary> Sends an email using the provided SMTP settings and a template </summary>
		/// <param name="model">Model object for the razor template</param>
		/// <param name="targetEmail">Recipient email address</param>
		/// <param name="templateName">Name of the razor template</param>
		/// <permission cref="EmailManager">Requires permission to send emails</permission>
		/// <returns>Result of the email sending</returns>
		/// <example>
		/// <!-- Example usage: -->
		/// await SendEmailFromTemplateAsync("some@somedomain.com", "TemplateName", new { Subject = "Test email", Body = "This is a test email" }, "applicationId");
		/// <!-- Example response: -->
		/// new EmailResult()
		/// {
		///   TargetEmail: "some@somedomain.com",
		///   Subject: "TemplateName",
		///   Success: true,
		///   ErrorMessage: null
		/// }
		/// </example>
		public async Task<EmailResult> SendEmailFromTemplateAsync(string targetEmail, string templateName, object model)
		{
			return await SendEmailFromTemplateAsync(targetEmail, templateName, model, _authenticatorId);
		}

		/// <summary> Sends an email to a group of recipients using the provided SMTP settings </summary>
		/// <permission cref="EmailManager">Requires permission to send emails</permission>
		/// <param name="targetEmails">List of recipient email addresses</param>
		/// <param name="templateName">Name of the razor template</param>
		/// <param name="model">Model object for the razor template</param>
		/// <param name="applicationId">ID of the application</param>
		/// <returns>Result of the email sending</returns>
		/// <example>
		/// <!-- Example usage: -->
		/// await SendGroupEmailFromTemplateAsync(new ()
		///		{
		///			"email1@somedomain.com",
		///			"email2@otherdomain.com"
		///		},
		///		"TemplateName",
		///		new
		///			{
		///				Subject = "Test email",
		///				Body = "This is a test email"
		///			},
		///		"applicationId");
		///	 <!-- Example response: -->
		///	  new SendEmailsResult()
		///	  {
		///			SenderApplicationId: 0,
		///			Subject: "TemplateName",
		///			Result: new () {
		///			  new EmailResult() {
		///					TargerEmail: "email1@somedomain.com",
		///					Sucess: true,
		///					Suject: "TemplateName",
		///					ErrorMesssage: null
		///			  },
		///			  new EmailResult() {
		///					TargerEmail: "email2@otherdomain.com",
		///					Sucess: true,
		///					Suject: "TemplateName",
		///					ErrorMesssage: null
		///				}
		///			}
		///	 }
		/// </example>
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

		/// <summary> Sends an email to a group of recipients using the provided SMTP settings </summary>
		/// <permission cref="EmailManager">Requires permission to send emails</permission>
		/// <param name="targetEmails">List of recipient email addresses</param>
		/// <param name="templateName">Name of the razor template</param>
		/// <param name="model">Model object for the razor template</param>
		/// <returns>Result of the email sending</returns>
		/// <example>
		/// <!-- Example usage: -->
		/// await SendGroupEmailFromTemplateAsync(new ()
		///		{
		///			"email1@somedomain.com",
		///			"email2@otherdomain.com"
		///		},
		///		"TemplateName",
		///		new
		///			{
		///				Subject = "Test email",
		///				Body = "This is a test email"
		///			},
		///		"applicationId");
		///	 <!-- Example response: -->
		///	  new SendEmailsResult()
		///	  {
		///			SenderApplicationId: 0,
		///			Subject: "TemplateName",
		///			Result: new() {
		///			  new EmailResult() {
		///					TargerEmail: "email1@somedomain.com",
		///					Sucess: true,
		///					Suject: "TemplateName",
		///					ErrorMesssage: null
		///			  },
		///			  new EmailResult() {
		///					TargerEmail: "email2@otherdomain.com",
		///					Sucess: true,
		///					Suject: "TemplateName",
		///					ErrorMesssage: null
		///				}
		///			}
		///	 }
		/// </example>
		public async Task<SendEmailsResult> SendGroupEmailFromTemplateAsync(List<string> targetEmails, string templateName, object model)
		{
			return await SendGroupEmailFromTemplateAsync(targetEmails, templateName, model, _authenticatorId);
		}

		/// <summary> Sends an email to a group of recipients using the provided SMTP settings </summary>
		/// <param name="applicationId">ID aplikace</param>
		/// <param name="body">HTML email body</param>
		/// <param name="subject">Email subject</param>
		/// <param name="targetEmails">List of recipient email addresses</param>
		/// <returns>Result of the email sending</returns>
		/// <example>
		/// <!-- Example usage: -->
		/// await SendGroupEmailAsync(new ()
		/// {
		///		"email1@somedomain.com",
		///		"email2@somedomain.com"
		///	 },
		///	 "Test email",
		///	 "This is a test email",
		///	 "applicationId");
		///	 <!-- Example response: -->
		///	 SendEmailsResult()
		///	 {
		///	   SenderApplicationId: 0,
		///	   Subject: "Test email",
		///	   Result: new()
		///	   {
		///			new()
		///			{
		///				TargetEmail: "email1@somedomain.com",
		///				Success: true,
		///				Subject: "Test email",
		///				ErrorMessage: null
		///			},
		///			new()
		///			{
		///				TargerEmail: "email2@somedomain.com,
		///				Sucess: true,
		///				Subject: "Test email",
		///				ErrorMesssage: null
		///			}
		///	   }
		///	 }
		///	 </example>
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

		/// <summary> Sends an email to a group of recipients using the provided SMTP settings </summary>
		/// <param name="body">HTML email body</param>
		/// <param name="subject">Email subject</param>
		/// <param name="targetEmails">List of recipient email addresses</param>
		/// <returns>Result of the email sending</returns>
		/// <example>
		/// <!-- Example usage: -->
		/// await SendGroupEmailAsync(new ()
		/// {
		///		"email1@somedomain.com",
		///		"email2@somedomain.com"
		///	 },
		///	 "Test email",
		///	 "This is a test email",
		///	 "applicationId");
		///	 <!-- Example response: -->
		///	 SendEmailsResult()
		///	 {
		///	   SenderApplicationId: 0,
		///	   Subject: "Test email",
		///	   Result: ()
		///	   {
		///			new()
		///			{
		///				TargetEmail: "email1@somedomain.com",
		///				Success: true,
		///				Subject: "Test email",
		///				ErrorMessage: null
		///			},
		///			new()
		///			{
		///				TargerEmail: "email2@somedomain.com,
		///				Sucess: true,
		///				Subject: "Test email",
		///				ErrorMesssage: null
		///			}
		///	   }
		///	 }
		///	 </example>
		public async Task<SendEmailsResult> SendGroupEmailAsync(List<string> targetEmails, string subject, string body)
		{
			return await SendGroupEmailAsync(targetEmails, subject, body, _authenticatorId);
		}

		/// <summary> Tests the SMTP settings </summary>
		/// <param name="input">SMTP settings to test</param>
		/// <returns>Result of the SMTP settings test</returns>
		/// <example>
		/// <!-- Example usage: -->
		/// await TestSmtpSettingsAsync(new SmtpSenderModel()
		/// {
		///		Host = "smtp.example.com",
		///		Port = 587,
		///		Secure = SecureSocketOptions.StartTls,
		///		SenderEmail = "someone@example.com",
		///		SenderName = "Someone",
		///		AuthorizationRequired = true,
		///		Username = "someone",
		///		Password = "password"
		///	 });
		/// <!-- Example response: -->
		/// SmtpTestResult()
		/// {
		///		Sucess: true,
		///		Error: null,
		///		Log: "SMTP log"
		/// }
		/// </example>
		public async Task<SmtpTestResult> TestSmtpSettingsAsync(SmtpSenderModel input)
		{
			string targetEmail = input.TargetForTesting ?? input.SenderEmail;
			SmtpTestResult result = new();

			if(string.IsNullOrEmpty(input.Host))
			{
				result.Error = "Host is empty";
				return result;
			}
			if(input.Port == 0)
			{
				result.Error = "Port is empty";
				return result;
			}
			if(string.IsNullOrEmpty(input.SenderEmail))
			{
				result.Error = "Sender email is empty";
				return result;
			}
			if(string.IsNullOrEmpty(input.SenderName))
			{
				input.SenderName = input.SenderEmail;
			}

			var logger = new StringProtocolLogger();

			try
			{
				using var client = new SmtpClient(logger);
				await client.ConnectAsync(input.Host, input.Port ?? 25, input.Secure);

				if(input.AuthorizationRequired)
				{
					if(string.IsNullOrEmpty(input.Username) || string.IsNullOrEmpty(input.Password))
					{
						result.Error = "Server requires authentication, but no credentials provided";
						return result;
					}
					await client.AuthenticateAsync(input.Username, input.Password);
				}

				var message = new MimeMessage();
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
			}
			catch(Exception ex)
			{
				result.Success = false;
				result.Error = ex.Message;
			}
			finally
			{
				result.Log = logger.GetLog();
			}

			return result;
		}

		// private methods
		private async Task<EmailResult> SendEmailAsync(MimeMessage message, ApplicationSmtpSettings settings)
		{
			EmailResult result = new()
			{
				Subject = message.Subject,
				TargetEmail = message.To.ToString()
			};
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