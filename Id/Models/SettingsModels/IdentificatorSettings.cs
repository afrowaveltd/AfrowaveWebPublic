namespace Id.Models.SettingsModels
{
	/// <summary>
	/// Represents the settings for the Identificator application.
	/// </summary>
	public class IdentificatorSettings
	{
		/// <summary>
		/// Gets or sets the application identifier.
		/// </summary>
		public string ApplicationId { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the login rules for the application.
		/// </summary>
		public LoginRules LoginRules { get; set; } = new();

		/// <summary>
		/// Gets or sets the password rules for the application.
		/// </summary>
		public PasswordRules PasswordRules { get; set; } = new();

		/// <summary>
		/// Gets or sets the cookie settings for the application.
		/// </summary>
		public CookieSettings CookieSettings { get; set; } = new();

		/// <summary>
		/// Gets or sets the JWT settings for the application.
		/// </summary>
		public JwtSettings JwtSettings { get; set; } = new();

		/// <summary>
		/// Gets or sets the CORS settings for the application.
		/// </summary>
		public CorsSettings CorsSettings { get; set; } = new();

		/// <summary>
		/// Gets or sets a value indicating whether the installation process is finished.
		/// </summary>
		public bool InstallationFinished { get; set; } = false;
	}

	/// <summary>
	/// Represents the rules for user login.
	/// </summary>
	public class LoginRules
	{
		/// <summary>
		/// Gets or sets the maximum number of failed login attempts before lockout.
		/// </summary>
		public int MaxFailedLoginAttempts { get; set; } = 5;

		/// <summary>
		/// Gets or sets the lockout time in minutes after exceeding the maximum failed login attempts.
		/// </summary>
		public int LockoutTime { get; set; } = 15; // in minutes

		/// <summary>
		/// Gets or sets the expiration time in minutes for the password reset token.
		/// </summary>
		public int PasswordResetTokenExpiration { get; set; } = 30; // in minutes

		/// <summary>
		/// Gets or sets the expiration time in minutes for the OTP (One-Time Password) token.
		/// </summary>
		public int OTPTokenExpiration { get; set; } = 60; // in minutes

		/// <summary>
		/// Gets or sets a value indicating whether a confirmed email is required for login.
		/// </summary>
		public bool RequireConfirmedEmail { get; set; } = true;

		/// <summary>
		/// Gets or sets a value indicating whether the login rules are configured.
		/// </summary>
		public bool IsConfigured { get; set; } = false;
	}

	/// <summary>
	/// Represents the rules for user passwords.
	/// </summary>
	public class PasswordRules
	{
		/// <summary>
		/// Gets or sets the minimum length required for a password.
		/// </summary>
		public int MinimumLength { get; set; } = 8;

		/// <summary>
		/// Gets or sets the maximum length allowed for a password.
		/// </summary>
		public int MaximumLength { get; set; } = 128;

		/// <summary>
		/// Gets or sets a value indicating whether a password requires at least one non-alphanumeric character.
		/// </summary>
		public bool RequireNonAlphanumeric { get; set; } = false;

		/// <summary>
		/// Gets or sets a value indicating whether a password requires at least one lowercase letter.
		/// </summary>
		public bool RequireLowercase { get; set; } = true;

		/// <summary>
		/// Gets or sets a value indicating whether a password requires at least one uppercase letter.
		/// </summary>
		public bool RequireUppercase { get; set; } = true;

		/// <summary>
		/// Gets or sets a value indicating whether a password requires at least one digit.
		/// </summary>
		public bool RequireDigit { get; set; } = true;

		/// <summary>
		/// Gets or sets a value indicating whether the password rules are configured.
		/// </summary>
		public bool IsConfigured { get; set; } = false;
	}

	/// <summary>
	/// Represents the settings for authentication cookies.
	/// </summary>
	public class CookieSettings
	{
		/// <summary>
		/// Gets or sets the name of the authentication cookie.
		/// </summary>
		public string Name { get; set; } = ".AuthCookie";

		/// <summary>
		/// Gets or sets the domain associated with the cookie.
		/// </summary>
		public string Domain { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the path associated with the cookie.
		/// </summary>
		public string Path { get; set; } = "/";

		/// <summary>
		/// Gets or sets a value indicating whether the cookie should only be transmitted over secure (HTTPS) connections.
		/// </summary>
		public bool Secure { get; set; } = true;

		/// <summary>
		/// Gets or sets the SameSite mode for the cookie.
		/// </summary>
		public SameSiteMode SameSite { get; set; } = SameSiteMode.Lax;

		/// <summary>
		/// Gets or sets a value indicating whether the cookie is accessible only through HTTP requests (not accessible via JavaScript).
		/// </summary>
		public bool HttpOnly { get; set; } = true;

		/// <summary>
		/// Gets or sets the expiration time in minutes for the cookie.
		/// </summary>
		public int Expiration { get; set; } = 60; // in minutes

		/// <summary>
		/// Gets or sets a value indicating whether the cookie settings are configured.
		/// </summary>
		public bool IsConfigured { get; set; } = false;
	}

	/// <summary>
	/// Represents the settings for JWT (JSON Web Token) authentication.
	/// </summary>
	public class JwtSettings
	{
		/// <summary>
		/// Gets or sets the issuer of the JWT.
		/// </summary>
		public string Issuer { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the audience of the JWT.
		/// </summary>
		public string Audience { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the expiration time in minutes for the access token.
		/// </summary>
		public int AccessTokenExpiration { get; set; } = 30; // in minutes

		/// <summary>
		/// Gets or sets the expiration time in days for the refresh token.
		/// </summary>
		public int RefreshTokenExpiration { get; set; } = 7; // in days

		/// <summary>
		/// Gets or sets a value indicating whether the JWT settings are configured.
		/// </summary>
		public bool IsConfigured { get; set; } = false;
	}

	/// <summary>
	/// Represents the settings for CORS (Cross-Origin Resource Sharing).
	/// </summary>
	public class CorsSettings
	{
		/// <summary>
		/// Gets or sets the CORS policy mode.
		/// </summary>
		public CorsPolicyMode PolicyMode { get; set; } = CorsPolicyMode.AllowAll;

		/// <summary>
		/// Gets or sets the list of allowed origins for CORS requests.
		/// </summary>
		public List<string> AllowedOrigins { get; set; } = [];

		/// <summary>
		/// Gets or sets a value indicating whether any HTTP method is allowed for CORS requests.
		/// </summary>
		public bool AllowAnyMethod { get; set; } = false;

		/// <summary>
		/// Gets or sets the list of allowed HTTP methods for CORS requests.
		/// </summary>
		public List<string> AllowedMethods { get; set; } = [];

		/// <summary>
		/// Gets or sets a value indicating whether any HTTP header is allowed for CORS requests.
		/// </summary>
		public bool AllowAnyHeader { get; set; } = false;

		/// <summary>
		/// Gets or sets the list of allowed HTTP headers for CORS requests.
		/// </summary>
		public List<string> AllowedHeaders { get; set; } = [];

		/// <summary>
		/// Gets or sets a value indicating whether credentials are allowed for CORS requests.
		/// </summary>
		public bool AllowCredentials { get; set; } = false;

		/// <summary>
		/// Gets or sets a value indicating whether the CORS settings are configured.
		/// </summary>
		public bool IsConfigured { get; set; } = false;
	}

	/// <summary>
	/// Represents the mode for CORS policy.
	/// </summary>
	public enum CorsPolicyMode
	{
		/// <summary>
		/// Allows requests from all domains.
		/// </summary>
		AllowAll,

		/// <summary>
		/// Allows requests only from selected domains.
		/// </summary>
		AllowSpecific,

		/// <summary>
		/// Denies requests from all domains.
		/// </summary>
		DenyAll
	}
}