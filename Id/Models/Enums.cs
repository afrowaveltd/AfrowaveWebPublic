namespace Id.Models
{
	public enum Gender
	{
		Male,
		Female,
		Other
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

	public enum LogoSize
	{
		png16px,
		png32px,
		png76px,
		png152px,
		png120px,
		pngOriginal
	}

	public enum RegistrationStep
	{
		User,
		Application,
		MailVerification,
		Finish
	}

	public enum PolicyType
	{
		Terms,
		Privacy,
		Cookie
	}

	public enum RegistrationResult
	{
		None,
		Success,
		Failed,
		AllreadyRegistered
	}

	public enum TranslationStatus
	{
		Untranslated,
		Translating,
		Translated,
		Ignored,
		Approved,
		Default
	}
}