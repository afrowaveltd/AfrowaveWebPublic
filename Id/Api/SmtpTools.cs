using Id.Models.CommunicationModels;
using Id.Models.InputModels;
using Id.Models.ResultModels;
using Microsoft.AspNetCore.Authorization;

namespace Id.Api
{
	/// <summary>
	/// API controller for handling SMTP-related operations.
	/// </summary>
	[Route("api/smtp")]
	[ApiController]
	public class SmtpTools(IStringLocalizer<SmtpTools> _t, IEmailManager email) : ControllerBase
	{
		private readonly IStringLocalizer<SmtpTools> t = _t;
		private readonly IEmailManager _email = email;

		/// <summary>
		/// Automatically detects SMTP settings based on input parameters.
		/// </summary>
		/// <param name="model">The input model containing SMTP details.</param>
		/// <returns>An API response containing the detected SMTP settings.</returns>
		/// <example>
		/// Example request:
		/// POST /api/smtp/autodetect
		/// {
		///     "Host": "smtp.example.com",
		///     "SmtpUsername": "user@example.com",
		///     "SmtpPassword": "password",
		/// }
		///
		/// Example response:
		/// {
		///     "successful": true,
		///     "port": 587,
		///     "requiresAuthentication": true,
		///     "secure": 2,
		///     }
		/// }
		/// </example>
		[AllowAnonymous]
		[HttpPost]
		[Route("autodetect")]
		public async Task<SmtpDetectionResult> AutodetectSettings([FromBody] DetectSmtpSettingsInput model)
		{
			if(string.IsNullOrWhiteSpace(model.Host))
			{
				return new() { Successful = false, Message = "Host is required" };
			}
			return await _email.AutodetectSmtpSettingsAsync(new DetectSmtpSettingsInput
			{
				Host = model.Host,
				Username = model.Username,
				Password = model.Password,
			});
		}

		/// <summary>
		/// Tests SMTP settings by sending a test email.
		/// </summary>
		/// <param name="model">The SMTP sender model containing the settings to be tested.</param>
		/// <returns>A result object containing test details.</returns>
		/// <example>
		/// Example request:
		/// POST /api/smtp/test
		/// {
		///     "Host": "smtp.example.com",
		///     "Port": 587,
		///     "Username": "user@example.com",
		///     "Password": "password",
		///     "SenderEmail": "sender@example.com"
		/// }
		///
		/// Example response:
		/// {
		///     "Success": true,
		///     "Message": "SMTP settings are valid."
		/// }
		/// </example>
		[AllowAnonymous]
		[HttpPost]
		[Route("test")]
		public async Task<SmtpTestResult> TestSettings([FromBody] SmtpSenderModel model)
		{
			return await _email.TestSmtpSettingsAsync(model);
		}
	}
}