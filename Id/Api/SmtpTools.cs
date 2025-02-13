using Id.Models.CommunicationModels;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace Id.Api
{
	[Route("api/smtp")]
	[ApiController]
	public class SmtpTools(IStringLocalizer<SmtpTools> _t, IEmailService email) : ControllerBase
	{
		private readonly IStringLocalizer<SmtpTools> t = _t;
		private readonly IEmailService _email = email;

		public class InputModel
		{
			[Required]
			public string Host { get; set; } = string.Empty;

			public string? SmtpUsername { get; set; }
			public string? SmtpPassword { get; set; }
		}

		[AllowAnonymous]
		[HttpPost]
		[Route("autodetect")]
		public async Task<ApiResponse<SmtpSenderModel>> AutodetectSettings([FromBody] InputModel model)
		{
			if(string.IsNullOrWhiteSpace(model.Host))
			{
				return new() { Successful = false, Message = "Host is required" };
			}
			return await _email.AutodetectSmtpSettingsAsync(new SmtpSenderModel { Host = model.Host, Username = model.SmtpUsername, Password = model.SmtpPassword });
			// Autodetect settings
		}

		[AllowAnonymous]
		[HttpPost]
		[Route("test")]
		public async Task<ApiResponse<SmtpTestResponse>> TestSettings([FromBody] SmtpSenderModel model)
		{
			// Test settings
			return await _email.TestSmtpConnectionAsync(model);
		}
	}
}