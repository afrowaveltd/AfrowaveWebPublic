using Id.Models.CommunicationModels;
using Id.Models.InputModels;
using Id.Models.ResultModels;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

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
		/// Represents input data for SMTP settings detection.
		/// </summary>
		public class InputModel
		{
			/// <summary>
			/// The SMTP host.
			/// </summary>
			[Required]
			public string Host { get; set; } = string.Empty;

			/// <summary>
			/// The SMTP username (optional).
			/// </summary>
			public string? SmtpUsername { get; set; }

			/// <summary>
			/// The SMTP password (optional).
			/// </summary>
			public string? SmtpPassword { get; set; }

			/// <summary>
			/// The sender's display name (optional).
			/// </summary>
			public string? SenderName { get; set; }

			/// <summary>
			/// The sender's email address (optional).
			/// </summary>
			public string? SenderEmail { get; set; }
		}

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
		///     "SenderEmail": "sender@example.com"
		/// }
		///
		/// Example response:
		/// {
		///     "Successful": true,
		///     "Data": {
		///         "Host": "smtp.example.com",
		///         "Port": 587,
		///         "RequiresAuthentication": true
		///     }
		/// }
		/// </example>
		[AllowAnonymous]
		[HttpPost]
		[Route("autodetect")]
		public async Task<ApiResponse<SmtpSenderModel>> AutodetectSettings([FromBody] InputModel model)
		{
			if(string.IsNullOrWhiteSpace(model.Host))
			{
				return new() { Successful = false, Message = "Host is required" };
			}
			return await _email.AutodetectSmtpSettingsAsync(new DetectSmtpSettingsInput
			{
				Host = model.Host,
				Username = model.SmtpUsername,
				Password = model.SmtpPassword,
				SenderEmail = model.SenderEmail ?? string.Empty
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