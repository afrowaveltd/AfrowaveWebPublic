namespace Id.Models.SettingsModels
{
	public class IdentificatorSettings
	{
		public string ApplicationId { get; set; } = string.Empty;
		public LoginRules LoginRules { get; set; } = new();
		public PasswordRules PasswordRules { get; set; } = new();
		public Cookie Cookie { get; set; } = new();
		public JwtSettings JwtSettings { get; set; } = new();
	}

	public class LoginRules
	{
		public int MaxFailedLoginAttempts { get; set; } = 0;
		public int LockoutTime { get; set; } = 0;
		public int PasswordResetTokenExpiration { get; set; } = 0;
		public int EmailConfirmationTokenExpiration { get; set; } = 0;
		public int RefreshTokenExpiration { get; set; } = 0;
		public int AccessTokenExpiration { get; set; } = 0;
		public bool RequireConfirmedEmail { get; set; } = false;
	}

	public class PasswordRules
	{
		public int MinimumLength { get; set; }
		public int MaximumLength { get; set; }
		public bool RequireNonAlphanumeric { get; set; }
		public bool RequireLowercase { get; set; }
		public bool RequireUppercase { get; set; }
		public bool RequireDigit { get; set; }
	}

	public class Cookie
	{
		public string Name { get; set; } = string.Empty;
		public string Domain { get; set; } = string.Empty;
		public string Path { get; set; } = string.Empty;
		public bool Secure { get; set; }
		public bool HttpOnly { get; set; }
		public int Expiration { get; set; }
	}

	public class JwtSettings
	{
		public string Secret { get; set; } = string.Empty;
		public string Issuer { get; set; } = string.Empty;
		public string Audience { get; set; } = string.Empty;
		public int AccessTokenExpiration { get; set; }
		public int RefreshTokenExpiration { get; set; }
	}
}