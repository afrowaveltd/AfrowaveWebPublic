using Id.Models.SettingsModels;

namespace Id.Pages.Helps
{
	public class Install_ResultModel(ISettingsService settings,
		 IStringLocalizer<Install_ResultModel> _t) : PageModel
	{
		private readonly ISettingsService _settings = settings;
		public readonly IStringLocalizer<Install_ResultModel> t = _t;
		public List<TextBlock> ElementLines { get; set; } = new();

		public async Task<IActionResult> OnGetAsync()
		{
			IdentificatorSettings data = await _settings.GetSettingsAsync() ?? new();

			TextBlock loginRules = new();
			loginRules.Title = t["Login settings"];
			loginRules.LiElements = new Dictionary<string, string>();
			loginRules.LiElements.Add("1. " + t["Require confirmed email"],
				 data.LoginRules.RequireConfirmedEmail ? t["Yes"] : t["No"]);
			loginRules.LiElements.Add("2. " + t["Lockout time"],
				$"{data.LoginRules.LockoutTime} {t["minutes"]}");
			loginRules.LiElements.Add("3. " + t["Max login attempts"],
				$"{data.LoginRules.MaxFailedLoginAttempts}");
			loginRules.LiElements.Add("4. " + t["Password reset token expiration"],
				$"{data.LoginRules.PasswordResetTokenExpiration} {t["minutes"]}");
			loginRules.LiElements.Add("5. " + t["Email confirmation token expiration"],
				$"{data.LoginRules.EmailConfirmationTokenExpiration} {t["minutes"]}");
			ElementLines.Add(loginRules);

			TextBlock passwordRules = new();
			passwordRules.Title = t["Password settings"];
			passwordRules.LiElements = new Dictionary<string, string>();
			passwordRules.LiElements.Add("1. " + t["Minimal password length"],
				data.PasswordRules.MinimumLength.ToString());
			passwordRules.LiElements.Add("2. " + t["Maximal password length"],
				data.PasswordRules.MaximumLength.ToString());
			passwordRules.LiElements.Add("3. " + t["Require the special character"],
				data.PasswordRules.RequireNonAlphanumeric ? t["Yes"] : t["No"]);
			passwordRules.LiElements.Add("4. " + t["Require the numeric character"],
				data.PasswordRules.RequireDigit ? t["Yes"] : t["No"]);
			passwordRules.LiElements.Add("5. " + t["Require the lowercase character"],
				data.PasswordRules.RequireLowercase ? t["Yes"] : t["No"]);
			passwordRules.LiElements.Add("6. " + t["Require the upercase character"],
				data.PasswordRules.RequireUppercase ? t["Yes"] : t["No"]);
			ElementLines.Add(passwordRules);

			TextBlock cookieSettings = new();
			cookieSettings.Title = t["Cookie settings"];
			cookieSettings.LiElements = new Dictionary<string, string>();
			cookieSettings.LiElements.Add("1. " + t["Cookie name"],
				data.CookieSettings.Name);
			cookieSettings.LiElements.Add("2. " + t["Cookie domain"],
				data.CookieSettings.Domain);
			cookieSettings.LiElements.Add("3. " + t["Cookie path"],
				data.CookieSettings.Path);
			cookieSettings.LiElements.Add("4. " + t["Secure cookie"],
				data.CookieSettings.Secure ? t["Yes"] : t["No"]);
			cookieSettings.LiElements.Add("5. " + t["HttpOnly cookie"],
				data.CookieSettings.HttpOnly ? t["Yes"] : t["No"]);
			cookieSettings.LiElements.Add("6. " + t["SameSite cookie"],
				data.CookieSettings.SameSite.ToString());
			cookieSettings.LiElements.Add("7. " + t["Cookie expiration"],
				$"{data.CookieSettings.Expiration} {t["minutes"]}");
			ElementLines.Add(cookieSettings);

			TextBlock jwtSettings = new();
			jwtSettings.Title = t["JWT settings"];
			jwtSettings.LiElements = new Dictionary<string, string>();
			jwtSettings.LiElements.Add("1. " + t["Issuer"],
				data.JwtSettings.Issuer);
			jwtSettings.LiElements.Add("2. " + t["Audience"],
				data.JwtSettings.Audience);
			jwtSettings.LiElements.Add("3. " + t["Access token expiration"],
				$"{data.JwtSettings.AccessTokenExpiration} {t["minutes"]}");
			jwtSettings.LiElements.Add("4. " + t["Refresh token expiration"],
				$"{data.JwtSettings.RefreshTokenExpiration} {t["days"]}");
			ElementLines.Add(jwtSettings);

			TextBlock corsSettings = new();
			corsSettings.Title = t["CORS settings"];
			corsSettings.LiElements = new Dictionary<string, string>();
			corsSettings.LiElements.Add("1. " + t["CORS Policy mode"],
				data.CorsSettings.PolicyMode.ToString());
			corsSettings.LiElements.Add("2. " + t["Allowed Origins"],
				string.Join(", ", data.CorsSettings.AllowedOrigins));
			corsSettings.LiElements.Add("3. " + t["Allow any headers"],
				data.CorsSettings.AllowAnyHeader ? t["Yes"] : t["No"]);
			if(!data.CorsSettings.AllowAnyHeader)
			{
				corsSettings.LiElements.Add("4. " + t["Allowed Headers"], string.Join(", ", data.CorsSettings.AllowedHeaders));
			}
			corsSettings.LiElements.Add("5. " + t["Allow any method"],
				data.CorsSettings.AllowAnyMethod ? t["Yes"] : t["No"]);
			if(!data.CorsSettings.AllowAnyMethod)
			{
				corsSettings.LiElements.Add("6. " + t["Allowed Methods"], string.Join(", ", data.CorsSettings.AllowedMethods));
			}
			corsSettings.LiElements.Add("7. " + t["Allow Credentials"],
				data.CorsSettings.AllowCredentials ? t["Yes"] : t["No"]);
			ElementLines.Add(corsSettings);

			return Page();
		}

		public class TextBlock
		{
			public string Title { get; set; } = string.Empty;
			public Dictionary<string, string> LiElements { get; set; } = new();
		}
	}
}