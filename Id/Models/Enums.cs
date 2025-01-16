namespace Id.Models
{
	public enum Gender
	{
		Male,
		Female,
		Other
	}

	public enum LogoSize
	{
		png16px,
		png32px,
		png76px,
		png152px,
		png120px,
		pngOriginal
	}

	public enum InstalationSteps
	{
		Administrator,
		Brand,
		Application,
		ApplicationRoles,
		ApplicationSettings,
		SmtpSettings,
		LoginRules,
		PasswordRules,
		CookieSettings,
		JwtSettings,
		CorsSettings,
		Result,
		Finish
	}
}