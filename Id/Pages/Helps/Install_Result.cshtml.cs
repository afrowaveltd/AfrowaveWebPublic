using Id.Models.SettingsModels;

namespace Id.Pages.Helps
{
	/// <summary>
	/// Represents the model for the installation result page.
	/// </summary>
	/// <param name="settings"></param>
	/// <param name="t"></param>
	public class Install_ResultModel(ISettingsService settings,
		 IStringLocalizer<Install_ResultModel> t) : PageModel
	{
		private readonly ISettingsService _settings = settings;

		/// <summary>
		/// Represents the localizer for the installation result page.
		/// </summary>
		public readonly IStringLocalizer<Install_ResultModel> _t = t;

		/// <summary>
		/// Gets or sets the list of text blocks.
		/// </summary>
		public List<TextBlock> ElementLines { get; set; } = [];

		/// <summary>
		/// Get method for the installation result page.
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> OnGetAsync()
		{
			IdentificatorSettings data = await _settings.GetSettingsAsync() ?? new();

			TextBlock loginRules = new()
			{
				Title = _t["Login settings"],
				LiElements = []
			};
			loginRules.LiElements.Add("1. " + _t["Require confirmed email"],
				 data.LoginRules.RequireConfirmedEmail ? _t["Yes"] : _t["No"]);
			loginRules.LiElements.Add("2. " + _t["Lockout time"],
				$"{data.LoginRules.LockoutTime} {_t["minutes"]}");
			loginRules.LiElements.Add("3. " + _t["Maximal failed login attempts"],
				$"{data.LoginRules.MaxFailedLoginAttempts}");
			loginRules.LiElements.Add("4. " + _t["Password reset token expiration"],
				$"{data.LoginRules.PasswordResetTokenExpiration} {_t["minutes"]}");
			loginRules.LiElements.Add("5. " + _t["OTP token expiration"],
				$"{data.LoginRules.OTPTokenExpiration} {_t["minutes"]}");
			ElementLines.Add(loginRules);

			TextBlock passwordRules = new()
			{
				Title = _t["Password settings"],
				LiElements = []
			};
			passwordRules.LiElements.Add("1. " + _t["Minimal password length"],
				data.PasswordRules.MinimumLength.ToString());
			passwordRules.LiElements.Add("2. " + _t["Maximal password length"],
				data.PasswordRules.MaximumLength.ToString());
			passwordRules.LiElements.Add("3. " + _t["Require the special character"],
				data.PasswordRules.RequireNonAlphanumeric ? _t["Yes"] : _t["No"]);
			passwordRules.LiElements.Add("4. " + _t["Require the numeric character"],
				data.PasswordRules.RequireDigit ? _t["Yes"] : _t["No"]);
			passwordRules.LiElements.Add("5. " + _t["Require the lowercase character"],
				data.PasswordRules.RequireLowercase ? _t["Yes"] : _t["No"]);
			passwordRules.LiElements.Add("6. " + _t["Require the upercase character"],
				data.PasswordRules.RequireUppercase ? _t["Yes"] : _t["No"]);
			ElementLines.Add(passwordRules);

			TextBlock cookieSettings = new()
			{
				Title = _t["Cookie settings"],
				LiElements = []
			};
			cookieSettings.LiElements.Add("1. " + _t["Cookie name"],
				data.CookieSettings.Name);
			cookieSettings.LiElements.Add("2. " + _t["Cookie domain"],
				data.CookieSettings.Domain);
			cookieSettings.LiElements.Add("3. " + _t["Cookie path"],
				data.CookieSettings.Path);
			cookieSettings.LiElements.Add("4. " + _t["Secure cookie"],
				data.CookieSettings.Secure ? _t["Yes"] : _t["No"]);
			cookieSettings.LiElements.Add("5. " + _t["HttpOnly cookie"],
				data.CookieSettings.HttpOnly ? _t["Yes"] : _t["No"]);
			cookieSettings.LiElements.Add("6. " + _t["SameSite cookie"],
				data.CookieSettings.SameSite.ToString());
			cookieSettings.LiElements.Add("7. " + _t["Cookie expiration"],
				$"{data.CookieSettings.Expiration} {_t["minutes"]}");
			ElementLines.Add(cookieSettings);

			TextBlock jwtSettings = new()
			{
				Title = _t["JWT settings"],
				LiElements = []
			};
			jwtSettings.LiElements.Add("1. " + _t["Issuer"],
				data.JwtSettings.Issuer);
			jwtSettings.LiElements.Add("2. " + _t["Audience"],
				data.JwtSettings.Audience);
			jwtSettings.LiElements.Add("3. " + _t["Access token expiration"],
				$"{data.JwtSettings.AccessTokenExpiration} {_t["minutes"]}");
			jwtSettings.LiElements.Add("4. " + _t["Refresh token expiration"],
				$"{data.JwtSettings.RefreshTokenExpiration} {_t["days"]}");
			ElementLines.Add(jwtSettings);

			TextBlock corsSettings = new()
			{
				Title = _t["CORS settings"],
				LiElements = []
			};
			corsSettings.LiElements.Add("1. " + _t["CORS Policy mode"],
				data.CorsSettings.PolicyMode.ToString());
			corsSettings.LiElements.Add("2. " + _t["Allowed Origins"],
				string.Join(", ", data.CorsSettings.AllowedOrigins));
			corsSettings.LiElements.Add("3. " + _t["Allow any headers"],
				data.CorsSettings.AllowAnyHeader ? _t["Yes"] : _t["No"]);
			if(!data.CorsSettings.AllowAnyHeader)
			{
				corsSettings.LiElements.Add("4. " + _t["Allowed Headers"], string.Join(", ", data.CorsSettings.AllowedHeaders));
			}
			corsSettings.LiElements.Add("5. " + _t["Allow any method"],
				data.CorsSettings.AllowAnyMethod ? _t["Yes"] : _t["No"]);
			if(!data.CorsSettings.AllowAnyMethod)
			{
				corsSettings.LiElements.Add("6. " + _t["Allowed Methods"], string.Join(", ", data.CorsSettings.AllowedMethods));
			}
			corsSettings.LiElements.Add("7. " + _t["Allow Credentials"],
				data.CorsSettings.AllowCredentials ? _t["Yes"] : _t["No"]);
			ElementLines.Add(corsSettings);

			return Page();
		}

		/// <summary>
		/// Represents a text block with a title and a list of elements.
		/// </summary>
		public class TextBlock
		{
			/// <summary>
			/// Gets or sets the title of the text block.
			/// </summary>
			public string Title { get; set; } = string.Empty;

			/// <summary>
			/// Gets or sets the list of elements in the text block.
			/// </summary>
			public Dictionary<string, string> LiElements { get; set; } = [];
		}
	}
}