namespace Id.Models
{
	/// <summary>
	/// Represents the gender of a user.
	/// </summary>
	public enum Gender
	{
		/// <summary>
		/// Male gender.
		/// </summary>
		Male,

		/// <summary>
		/// Female gender.
		/// </summary>
		Female,

		/// <summary>
		/// Other gender (non-binary, unspecified, etc.).
		/// </summary>
		Other
	}

	/// <summary>
	/// Represents the steps in the installation process.
	/// </summary>
	public enum InstalationSteps
	{
		/// <summary>
		/// Step for setting up the administrator.
		/// </summary>
		Administrator,

		/// <summary>
		/// Step for configuring the brand settings.
		/// </summary>
		Brand,

		/// <summary>
		/// Step for setting up the application.
		/// </summary>
		Application,

		/// <summary>
		/// Step for configuring application roles.
		/// </summary>
		ApplicationRoles,

		/// <summary>
		/// Step for configuring SMTP (email) settings.
		/// </summary>
		SmtpSettings,

		/// <summary>
		/// Step for configuring login rules.
		/// </summary>
		LoginRules,

		/// <summary>
		/// Step for configuring password rules.
		/// </summary>
		PasswordRules,

		/// <summary>
		/// Step for configuring cookie settings.
		/// </summary>
		CookieSettings,

		/// <summary>
		/// Step for configuring JWT (JSON Web Token) settings.
		/// </summary>
		JwtSettings,

		/// <summary>
		/// Step for configuring CORS (Cross-Origin Resource Sharing) settings.
		/// </summary>
		CorsSettings,

		/// <summary>
		/// Step for displaying the installation result.
		/// </summary>
		Result,

		/// <summary>
		/// Final step to complete the installation process.
		/// </summary>
		Finish
	}

	/// <summary>
	/// Represents the size options for logos.
	/// </summary>
	public enum LogoSize
	{
		/// <summary>
		/// 16x16 pixels PNG logo.
		/// </summary>
		png16px,

		/// <summary>
		/// 32x32 pixels PNG logo.
		/// </summary>
		png32px,

		/// <summary>
		/// 76x76 pixels PNG logo.
		/// </summary>
		png76px,

		/// <summary>
		/// 152x152 pixels PNG logo.
		/// </summary>
		png152px,

		/// <summary>
		/// 120x120 pixels PNG logo.
		/// </summary>
		png120px,

		/// <summary>
		/// Original size of the logo in PNG format.
		/// </summary>
		pngOriginal
	}

	/// <summary>
	/// Represents the size options for profile pictures.
	/// </summary>
	public enum ProfilePictureSize
	{
		/// <summary>
		/// Icon-sized profile picture.
		/// </summary>
		icon,

		/// <summary>
		/// Small-sized profile picture.
		/// </summary>
		small,

		/// <summary>
		/// Big-sized profile picture.
		/// </summary>
		big,

		/// <summary>
		/// Original size of the profile picture.
		/// </summary>
		original
	}

	/// <summary>
	/// Represents the steps in the user registration process.
	/// </summary>
	public enum RegistrationStep
	{
		/// <summary>
		/// Step for entering user information.
		/// </summary>
		User,

		/// <summary>
		/// Step for selecting the application.
		/// </summary>
		Application,

		/// <summary>
		/// Step for verifying the user's email address.
		/// </summary>
		MailVerification,

		/// <summary>
		/// Final step to complete the registration process.
		/// </summary>
		Finish
	}

	/// <summary>
	/// Represents the types of policies.
	/// </summary>
	public enum PolicyType
	{
		/// <summary>
		/// Terms and conditions policy.
		/// </summary>
		Terms,

		/// <summary>
		/// Privacy policy.
		/// </summary>
		Privacy,

		/// <summary>
		/// Cookie policy.
		/// </summary>
		Cookie
	}

	/// <summary>
	/// Represents the status of a translation.
	/// </summary>
	public enum TranslationStatus
	{
		/// <summary>
		/// The text has not been translated yet.
		/// </summary>
		Untranslated,

		/// <summary>
		/// The text is currently being translated.
		/// </summary>
		Translating,

		/// <summary>
		/// The text has been translated.
		/// </summary>
		Translated,

		/// <summary>
		/// The text has been ignored and will not be translated.
		/// </summary>
		Ignored,

		/// <summary>
		/// The translation has been approved.
		/// </summary>
		Approved,

		/// <summary>
		/// The default status for translations.
		/// </summary>
		Default
	}
}