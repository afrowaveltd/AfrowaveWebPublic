using Id.Models.CommunicationModels;

namespace Id.Services
{
	public interface IEmailService
	{
		Task<ApiResponse<SmtpSenderModel>> AutodetectSmtpSettingsAsync(SmtpSenderModel smtpModel);

		Task<ApiResponse<string>> SendEmailAsync(string targetEmail, string subject, string htmlBody, string textBody, SmtpSenderModel smtpModel);

		Task<ApiResponse<string>> SendEmailAsync(string applicationId, string targetEmail, string subject, string htmlBody, string textBody);

		Task<ApiResponse<Dictionary<string, bool>>> SendEmailsAsync(List<string> targetEmails, string subject, string htmlBody, string textBody, SmtpSenderModel smtpModel);

		Task<ApiResponse<Dictionary<string, bool>>> SendEmailsAsync(string applicationId, List<string> targetEmails, string subject, string htmlBody, string textBody);
		Task<ApiResponse<string>> SendTemplatedEmailAsync(string targetEmail, string templateName, object model);
		Task<ApiResponse<SmtpTestResponse>> TestSmtpConnectionAsync(SmtpSenderModel smtpModel);
	}
}