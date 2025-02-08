namespace Id.Models.SettingsModels
{
	public class IdentificatorSettings
	{
		public string ApplicationId { get; set; } = string.Empty;
		public LoginRules LoginRules { get; set; } = new();
		public PasswordRules PasswordRules { get; set; } = new();
		public CookieSettings CookieSettings { get; set; } = new();
		public JwtSettings JwtSettings { get; set; } = new();
		public CorsSettings CorsSettings { get; set; } = new();
		public bool InstallationFinished { get; set; } = false;
	}

	public class LoginRules
	{
		public int MaxFailedLoginAttempts { get; set; } = 5;
		public int LockoutTime { get; set; } = 15; // in minutes
		public int PasswordResetTokenExpiration { get; set; } = 30; // in minutes
		public int OTPTokenExpiration { get; set; } = 60; // in minutes
		public bool RequireConfirmedEmail { get; set; } = true;
	}

	public class PasswordRules
	{
		public int MinimumLength { get; set; } = 8;
		public int MaximumLength { get; set; } = 128;
		public bool RequireNonAlphanumeric { get; set; } = false;
		public bool RequireLowercase { get; set; } = true;
		public bool RequireUppercase { get; set; } = true;
		public bool RequireDigit { get; set; } = true;
	}

	public class CookieSettings
	{
		public string Name { get; set; } = ".AuthCookie";
		public string Domain { get; set; } = string.Empty;
		public string Path { get; set; } = "/";
		public bool Secure { get; set; } = true;
		public SameSiteMode SameSite { get; set; } = SameSiteMode.Lax;
		public bool HttpOnly { get; set; } = true;
		public int Expiration { get; set; } = 60; // in minutes
	}

	public class JwtSettings
	{
		public string Issuer { get; set; } = string.Empty;
		public string Audience { get; set; } = string.Empty;
		public int AccessTokenExpiration { get; set; } = 30; // in minutes
		public int RefreshTokenExpiration { get; set; } = 7; // in days
	}

	public class CorsSettings
	{
		public CorsPolicyMode PolicyMode { get; set; } = CorsPolicyMode.AllowAll;
		public List<string> AllowedOrigins { get; set; } = [];

		public bool AllowAnyMethod { get; set; } = false;
		public List<string> AllowedMethods { get; set; } = [];
		public bool AllowAnyHeader { get; set; } = false;
		public List<string> AllowedHeaders { get; set; } = [];
		public bool AllowCredentials { get; set; } = false;
		public bool CorsConfigured { get; set; } = false;
	}

	public enum CorsPolicyMode
	{
		AllowAll,      // Povolit všechny domény
		AllowSpecific, // Povolit jen vybrané domény
		DenyAll        // Zakázat všechny domény
	}
}