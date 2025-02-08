using Id.Models.CommunicationModels;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using RazorLight;
using System.Text;

namespace Id.Services
{
	public class EmailService(IStringLocalizer<EmailService> _t,
		ILogger<EmailService> logger,
		ApplicationDbContext context,
		ISettingsService settings,
		IApplicationService applicationService) : IEmailService

	{
		private readonly IStringLocalizer<EmailService> t = _t;
		private readonly ILogger<EmailService> _logger = logger;
		private readonly ApplicationDbContext _context = context;
		private readonly ISettingsService _settings = settings;
		private readonly IApplicationService _applicationService = applicationService;

		private readonly RazorLightEngine _razorEngine = new RazorLightEngineBuilder()
			.UseEmbeddedResourcesProject(typeof(EmailService))
			.UseMemoryCachingProvider()
			.Build();

		public async Task<ApiResponse<string>> SendTemplatedEmailAsync(string targetEmail, string templateName, object model, string applicationId)
		{
			var smtpSettings = await _context.ApplicationSmtpSettings
				.Where(s => s.ApplicationId == applicationId)
				.FirstOrDefaultAsync();

			if(smtpSettings == null)
			{
				return new ApiResponse<string>
				{
					Successful = false,
					Message = "SMTP settings not found for the application."
				};
			}
			string emailBody = await _razorEngine.CompileRenderAsync($"Templates.{templateName}", model);

			var message = new MimeMessage();
			message.From.Add(new MailboxAddress(smtpSettings.SenderName, smtpSettings.SenderEmail));
			message.To.Add(MailboxAddress.Parse(targetEmail));
			message.Subject = model.GetType().GetProperty("Subject")?.GetValue(model)?.ToString() ?? "Notification";
			message.Body = new TextPart("html")
			{
				Text = emailBody
			};

			try
			{
				using var client = new SmtpClient();
				await client.ConnectAsync(smtpSettings.Host, smtpSettings.Port, smtpSettings.Secure);
				if(smtpSettings.AuthorizationRequired)
					await client.AuthenticateAsync(smtpSettings.Username, smtpSettings.Password);

				await client.SendAsync(message);
				await client.DisconnectAsync(true);

				return new ApiResponse<string> { Successful = true, Message = "Email sent successfully." };
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Failed to send email.");
				return new ApiResponse<string> { Successful = false, Message = $"Error sending email: {ex.Message}" };
			}
		}

		public async Task<ApiResponse<string>> SendTemplatedEmailAsync(string targetEmail, string templateName, object model)
		{
			string applicationId = await _applicationService.GetDefaultApplicationId();
			return await SendTemplatedEmailAsync(targetEmail, templateName, model, applicationId);
		}

		public async Task<ApiResponse<SmtpTestResponse>> TestSmtpConnectionAsync(SmtpSenderModel smtpModel)
		{
			string targetEmail = smtpModel.TargetForTesting ?? smtpModel.SenderEmail;
			ApiResponse<SmtpTestResponse> result = new();
			result.Message = "SMTP test completed successfully.";
			SmtpTestResponse response = new SmtpTestResponse();
			StringBuilder logBuilder = new StringBuilder();
			if(targetEmail == null || targetEmail == string.Empty)
			{
				targetEmail = await GetApplicationEmail(await GetAppicationId());
				if(targetEmail == string.Empty)
				{
					result.Successful = false;
					result.Message = "No target email was given or found";
					return result;
				}
			}

			try
			{
				// Use a MemoryStream to capture logs in memory
				using MemoryStream memoryStream = new MemoryStream();
				using SmtpClient client = new SmtpClient(new ProtocolLogger(memoryStream));
				await client.ConnectAsync(
					 smtpModel.Host,
					 smtpModel.Port ?? 25,
					 smtpModel.Secure
				);

				// Authenticate if required
				if(smtpModel.AuthorizationRequired)
				{
					if(string.IsNullOrEmpty(smtpModel.Username) || string.IsNullOrEmpty(smtpModel.Password))
					{
						result.Successful = false;
						result.Message = "Username and password are required for authentication.";
						return result;
					}

					await client.AuthenticateAsync(smtpModel.Username, smtpModel.Password);
				}

				// Prepare a test email message
				MimeMessage message = new MimeMessage();
				message.From.Add(new MailboxAddress(smtpModel.SenderName, smtpModel.SenderEmail));
				message.To.Add(MailboxAddress.Parse(targetEmail));
				message.Subject = "SMTP Test Email";
				message.Body = new TextPart("plain")
				{
					Text = "This is a test email sent by the SMTP testing tool."
				};

				// Send the email
				_ = await client.SendAsync(message);

				// Disconnect from the server
				await client.DisconnectAsync(true);

				// Set success
				result.Successful = true;
				response.ErrorType = SmtpTestErrorType.None;

				// Extract the logs from the MemoryStream
				response.Log = Encoding.UTF8.GetString(memoryStream.ToArray());
			}
			catch(MailKit.Security.AuthenticationException ex)
			{
				result.Successful = false;
				response.ErrorType = SmtpTestErrorType.AuthenticationFailed;
				result.Message = ex.Message;
			}
			catch(SmtpCommandException ex)
			{
				result.Successful = false;
				response.ErrorType = ex.ErrorCode switch
				{
					SmtpErrorCode.RecipientNotAccepted => SmtpTestErrorType.InvalidRecipient,
					SmtpErrorCode.SenderNotAccepted => SmtpTestErrorType.Other,
					_ => SmtpTestErrorType.Other
				};
				result.Message = $"SMTP Command Error";
			}
			catch(SmtpProtocolException)
			{
				result.Successful = false;
				response.ErrorType = SmtpTestErrorType.ConnectionFailed;
				result.Message = $"Protocol Error";
			}
			catch(TimeoutException)
			{
				result.Successful = false;
				response.ErrorType = SmtpTestErrorType.Timeout;
				result.Message = $"Timeout";
			}
			catch(Exception)
			{
				result.Successful = false;
				response.ErrorType = SmtpTestErrorType.Other;
				result.Message = $"Connection error";
			}
			finally
			{
				// Ensure the log is attached even if there's an error
				response.Log ??= logBuilder.ToString();
			}
			result.Data = response;
			return result;
		}

		public async Task<ApiResponse<SmtpSenderModel>> AutodetectSmtpSettingsAsync(SmtpSenderModel smtpModel)
		{
			// Common SMTP ports and security options to test
			List<(int Port, SecureSocketOptions Security)> portSecurityCombinations = new List<(int Port, SecureSocketOptions Security)>
	 {
		  (25, SecureSocketOptions.None),
		  (587, SecureSocketOptions.StartTls),
		  (465, SecureSocketOptions.SslOnConnect),
		  (2525, SecureSocketOptions.StartTls) // Additional fallback port
    };

			ApiResponse<SmtpSenderModel> response = new ApiResponse<SmtpSenderModel>();

			foreach((int port, SecureSocketOptions security) in portSecurityCombinations)
			{
				try
				{
					using(SmtpClient client = new SmtpClient())
					{
						// Attempt to connect with the current combination
						await client.ConnectAsync(smtpModel.Host, port, security);

						// Check if the server requires authentication
						if(client.Capabilities.HasFlag(SmtpCapabilities.Authentication))
						{
							if(string.IsNullOrEmpty(smtpModel.Username) || string.IsNullOrEmpty(smtpModel.Password))
							{
								// Authentication is required, but credentials are missing
								throw new InvalidOperationException("SMTP server requires authentication, but no credentials were provided.");
							}

							// Attempt to authenticate
							await client.AuthenticateAsync(smtpModel.Username, smtpModel.Password);
						}

						// If connection and authentication succeed, update the model
						smtpModel.Port = port;
						smtpModel.Secure = security;

						// Disconnect from the server
						await client.DisconnectAsync(true);

						// Return the successful configuration
						response.Data = smtpModel;
						response.Successful = true;
						response.Message = "SMTP settings successfully detected.";
						return response;
					}
				}
				catch(InvalidOperationException ex)
				{
					// Authentication required but credentials are missing
					response.Message = ex.Message;
					response.Successful = false;
					return response;
				}
				catch
				{
					// Continue to the next combination if this one fails
					continue;
				}
			}

			// If no combination succeeds, return an error
			response.Successful = false;
			response.Message = "Unable to detect SMTP settings. Please check the host, username, and password.";
			return response;
		}

		public async Task<ApiResponse<Dictionary<string, bool>>> SendEmailsAsync(
			 string applicationId,
			 List<string> targetEmails,
			 string subject,
			 string htmlBody,
			 string textBody)
		{
			ApiResponse<Dictionary<string, bool>> response = new ApiResponse<Dictionary<string, bool>>();
			response.Data = new Dictionary<string, bool>();

			// Retrieve the SMTP settings for the application
			ApplicationSmtpSettings? smtpSettings = await _context.ApplicationSmtpSettings
				  .Where(s => s.ApplicationId == applicationId)
				  .FirstOrDefaultAsync();

			if(smtpSettings == null)
			{
				response.Successful = false;
				response.Message = "SMTP settings not found for the application.";
				return response;
			}

			try
			{
				using(SmtpClient client = new SmtpClient())
				{
					// Connect to the SMTP server
					await client.ConnectAsync(smtpSettings.Host, smtpSettings.Port, smtpSettings.Secure);

					// Authenticate if required
					if(smtpSettings.AuthorizationRequired)
					{
						await client.AuthenticateAsync(smtpSettings.Username, smtpSettings.Password);
					}

					// Prepare the email message
					MimeMessage message = new MimeMessage();

					// Use the application email as the primary recipient
					message.From.Add(new MailboxAddress(smtpSettings.SenderName, smtpSettings.SenderEmail));

					// Set the application email as the primary recipient and BCC the target emails
					message.To.Add(new MailboxAddress(smtpSettings.SenderName, smtpSettings.SenderEmail));

					foreach(string targetEmail in targetEmails)
					{
						message.Bcc.Add(MailboxAddress.Parse(targetEmail));
					}

					message.Subject = subject;

					// Set the email body with both plain text and HTML
					message.Body = new Multipart("alternative")
				{
					 new TextPart("plain") { Text = textBody },
					 new TextPart("html") { Text = htmlBody }
				};

					// Send the email and track success/failure for each recipient
					try
					{
						_ = await client.SendAsync(message);

						// Add result to the dictionary for each target email
						foreach(string targetEmail in targetEmails)
						{
							response.Data[targetEmail] = true;
						}
					}
					catch(Exception ex)
					{
						// If an error occurs while sending, mark all emails as failed
						foreach(string email in targetEmails)
						{
							response.Data[email] = false;
						}

						// Handle exception by returning a failure response
						response.Successful = false;
						response.Message = $"Failed to send emails: {ex.Message}";
						return response;
					}

					// Disconnect from the SMTP server
					await client.DisconnectAsync(true);

					// Set success message for the operation
					response.Successful = true;
					response.Message = "Emails sent successfully.";
				}
			}
			catch(Exception ex)
			{
				// Handle errors during the email sending process
				response.Successful = false;
				response.Message = $"Failed to send emails: {ex.Message}";

				// For failed operations, mark all recipients as unsuccessful
				foreach(string email in targetEmails)
				{
					response.Data[email] = false;
				}
			}

			return response;
		}

		public async Task<ApiResponse<string>> SendEmailAsync(string applicationId, string targetEmail, string subject, string htmlBody, string textBody)
		{
			ApiResponse<string> response = new ApiResponse<string>();
			// Retrieve the SMTP settings for the application
			ApplicationSmtpSettings? smtpSettings = await _context.ApplicationSmtpSettings
				.Where(s => s.ApplicationId == applicationId)
				.FirstOrDefaultAsync();
			if(smtpSettings == null)
			{
				response.Successful = false;
				response.Message = "SMTP settings not found for the application.";
				return response;
			}
			try
			{
				using(SmtpClient client = new SmtpClient())
				{
					// Connect to the SMTP server
					await client.ConnectAsync(smtpSettings.Host, smtpSettings.Port, smtpSettings.Secure);
					// Authenticate if required
					if(smtpSettings.AuthorizationRequired)
					{
						await client.AuthenticateAsync(smtpSettings.Username, smtpSettings.Password);
					}
					// Prepare the email message
					MimeMessage message = new MimeMessage();
					message.From.Add(new MailboxAddress(smtpSettings.SenderName, smtpSettings.SenderEmail));
					message.To.Add(MailboxAddress.Parse(targetEmail));
					message.Subject = subject;
					message.Body = new Multipart("alternative")
					{
						new TextPart("plain")
						{
							Text = textBody
						},
						new TextPart("html")
						{
							Text = htmlBody
						}
					};
					// Send the email
					_ = await client.SendAsync(message);
					// Disconnect from the server
					await client.DisconnectAsync(true);
					// Set success
					response.Successful = true;
					response.Message = "Email sent successfully.";
					response.Data = targetEmail;
				}
			}
			catch(Exception ex)
			{
				response.Successful = false;
				response.Message = $"Failed to send email: {ex.Message}";
			}
			return response;
		}

		public async Task<ApiResponse<Dictionary<string, bool>>> SendEmailsAsync(List<string> targetEmails, string subject, string htmlBody, string textBody, SmtpSenderModel smtpModel)
		{
			string applicationId = await GetAppicationId();
			return await SendEmailsAsync(applicationId, targetEmails, subject, htmlBody, textBody);
		}

		public async Task<ApiResponse<string>> SendEmailAsync(string targetEmail, string subject, string htmlBody, string textBody, SmtpSenderModel smtpModel)
		{
			string applicationId = await GetAppicationId();
			return await SendEmailAsync(applicationId, targetEmail, subject, htmlBody, textBody);
		}

		private async Task<string> GetAppicationId()
		{
			Models.SettingsModels.IdentificatorSettings applicationSettings = await _settings.GetSettingsAsync();
			return applicationSettings.ApplicationId;
		}

		private async Task<string> GetApplicationEmail(string applicationId)
		{
			return await _context.Applications.Where(s => s.Id == applicationId).Select(s => s.ApplicationEmail).FirstOrDefaultAsync() ?? string.Empty;
		}
	}
}