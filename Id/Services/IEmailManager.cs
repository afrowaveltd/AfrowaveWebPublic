using Id.Models.CommunicationModels;
using Id.Models.InputModels;
using Id.Models.ResultModels;

namespace Id.Services
{
	/// <summary>
	/// Interface for managing emails.
	/// </summary>
	public interface IEmailManager
	{
		/// <summary>
		/// Autodetects the SMTP settings.
		/// </summary>
		/// <param name="input">Detect SMTP settings input data for the test</param>
		/// <returns>Resulted settings or an error</returns>
		Task<SmtpDetectionResult> AutodetectSmtpSettingsAsync(DetectSmtpSettingsInput input);

		/// <summary>
		/// Sends an email.
		/// </summary>
		/// <param name="targetEmail">Email address of recipient</param>
		/// <param name="subject">Message subject</param>
		/// <param name="body">Message body HTML</param>
		/// <returns>EmailResult</returns>
		Task<EmailResult> SendEmailAsync(string targetEmail, string subject, string body);

		/// <summary>
		/// Sends an email.
		/// </summary>
		/// <param name="targetEmail">Email address of recipient</param>
		/// <param name="subject">Message subject</param>
		/// <param name="body">Message body HTML</param>
		/// <param name="applicationId">ID of the application from which is email being sent</param>
		/// <returns>EmailResult</returns>
		Task<EmailResult> SendEmailAsync(string targetEmail, string subject, string body, string applicationId);

		/// <summary>
		/// Sends an email from a template.
		/// </summary>
		/// <param name="targetEmail">Email address of recipient</param>
		/// <param name="templateName">Name of the template to be used</param>
		/// <param name="model">Template model</param>
		/// <returns>EmailResult</returns>
		Task<EmailResult> SendEmailFromTemplateAsync(string targetEmail, string templateName, object model);

		/// <summary>
		/// Sends an email from a template.
		/// </summary>
		/// <param name="targetEmail">Email address of recipient</param>
		/// <param name="templateName">Name of the template to be used</param>
		/// <param name="model">Template model</param>
		/// <param name="applicationId">Sender application ID</param>
		/// <returns>EmailResult</returns>
		Task<EmailResult> SendEmailFromTemplateAsync(string targetEmail, string templateName, object model, string applicationId);

		/// <summary>
		/// Sends an email to a group of recipients.
		/// </summary>
		/// <param name="targetEmails">The list of recipient</param>
		/// <param name="subject">Subject of the email</param>
		/// <param name="body">Email body</param>
		/// <returns>SendEmailsResult</returns>
		Task<SendEmailsResult> SendGroupEmailAsync(List<string> targetEmails, string subject, string body);

		/// <summary>
		/// Sends an email to a group of recipients.
		/// </summary>
		/// <param name="targetEmails">The list of recipients</param>
		/// <param name="subject">Subject of the email</param>
		/// <param name="body">Email body</param>
		/// <param name="applicationId">Sender application ID</param>
		/// <returns>SendEmailsResult</returns>
		Task<SendEmailsResult> SendGroupEmailAsync(List<string> targetEmails, string subject, string body, string applicationId);

		/// <summary>
		/// Sends an email to a group of recipients from a template.
		/// </summary>
		/// <param name="targetEmails">The list of recipients</param>
		/// <param name="templateName">Name of the template to be used</param>
		/// <param name="model">Template model</param>
		/// <returns>SendEmailsResult</returns>
		Task<SendEmailsResult> SendGroupEmailFromTemplateAsync(List<string> targetEmails, string templateName, object model);

		/// <summary>
		/// Sends an email to a group of recipients from a template.
		/// </summary>
		/// <param name="targetEmails">The list of recipients</param>
		/// <param name="templateName">Name of the template to be used</param>
		/// <param name="model">Template model</param>
		/// <param name="applicationId">Application ID</param>
		/// <returns>SendEmailsResult</returns>
		Task<SendEmailsResult> SendGroupEmailFromTemplateAsync(List<string> targetEmails, string templateName, object model, string applicationId);

		/// <summary>
		/// Tests the SMTP settings.
		/// </summary>
		/// <param name="input">SmtpSenderModel</param>
		/// <returns>SmtpTestResult</returns>
		Task<SmtpTestResult> TestSmtpSettingsAsync(SmtpSenderModel input);
	}
}