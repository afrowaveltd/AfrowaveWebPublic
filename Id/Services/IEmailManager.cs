using Id.Models.CommunicationModels;
using Id.Models.InputModels;
using Id.Models.ResultModels;

namespace Id.Services
{
	public interface IEmailManager
	{
		Task<ApiResponse<SmtpSenderModel>> AutodetectSmtpSettingsAsync(DetectSmtpSettingsInput input);
		Task<EmailResult> SendEmailAsync(string targetEmail, string subject, string body);
		Task<EmailResult> SendEmailAsync(string targetEmail, string subject, string body, string applicationId);
		Task<EmailResult> SendEmailFromTemplateAsync(string targetEmail, string templateName, object model);
		Task<EmailResult> SendEmailFromTemplateAsync(string targetEmail, string templateName, object model, string applicationId);
		Task<SendEmailsResult> SendGroupEmailAsync(List<string> targetEmails, string subject, string body);
		Task<SendEmailsResult> SendGroupEmailAsync(List<string> targetEmails, string subject, string body, string applicationId);
		Task<SendEmailsResult> SendGroupEmailFromTemplateAsync(List<string> targetEmails, string templateName, object model);
		Task<SendEmailsResult> SendGroupEmailFromTemplateAsync(List<string> targetEmails, string templateName, object model, string applicationId);
		Task<SmtpTestResult> TestSmtpSettingsAsync(SmtpSenderModel input);
	}
}