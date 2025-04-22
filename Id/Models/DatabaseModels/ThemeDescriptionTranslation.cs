using System.ComponentModel.DataAnnotations;

namespace Id.Models.DatabaseModels
{
	/// <summary>
	/// Represents a translation of a theme description, including details about the language,  translation status, and the
	/// content of the translation.
	/// </summary>
	/// <remarks>This class is used to store and manage translations for theme descriptions in multiple languages.
	/// It includes properties for the original description, the translated description, and any community-provided
	/// translations.</remarks>
	public class ThemeDescriptionTranslation
	{
		/// <summary>
		/// Gets or sets the unique identifier for the entity.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the unique identifier for the theme.
		/// </summary>
		public int ThemeId { get; set; }

		/// <summary>
		/// Gets or sets the current theme applied to the application.
		/// </summary>
		public Theme Theme { get; set; } = null!;

		/// <summary>
		/// Gets or sets the identifier for the language.
		/// </summary>

		[MaxLength(10)]
		public string LanguageId { get; set; } = null!;

		/// <summary>
		/// Gets or sets the content of the previous body text, limited to a maximum of 300 characters.
		/// </summary>

		[MaxLength(300)]
		public string? PreviousBody { get; set; }

		/// <summary>
		/// Gets or sets the body content of the message.
		/// </summary>
		[MaxLength(300)]
		public string Body { get; set; } = null!;

		/// <summary>
		/// Gets or sets the community-provided translation for the associated content.
		/// </summary>
		[MaxLength(300)]
		public string? CommunityTranslation { get; set; }

		/// <summary>
		/// Gets or sets the current status of the translation process.
		/// </summary>
		public TranslationStatus TranslationStatus { get; set; } = TranslationStatus.Translating;
	}
}