using System.ComponentModel.DataAnnotations;

namespace Id.Models.DatabaseModels
{
	/// <summary>
	/// Represents a localized translation of a theme's name, including metadata about the translation's status and
	/// language.
	/// </summary>
	/// <remarks>This class is used to store and manage translations for a theme's name in different languages.  It
	/// includes information about the language, the translated text, and the status of the translation.</remarks>
	public class ThemeNameTranslation
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
		/// Gets or sets the content of the previous body text, limited to a maximum of 100 characters.
		/// </summary>

		[MaxLength(100)]
		public string? PreviousBody { get; set; }

		/// <summary>
		/// Gets or sets the body content of the message.
		/// </summary>

		[MaxLength(100)]
		public string Body { get; set; } = null!;

		/// <summary>
		/// Gets or sets the community-provided translation for the associated content.
		/// </summary>
		[MaxLength(100)]
		public string? CommunityTranslation { get; set; }

		/// <summary>
		/// Gets or sets the current status of the translation process.
		/// </summary>
		public TranslationStatus TranslationStatus { get; set; } = TranslationStatus.Translating;
	}
}