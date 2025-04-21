using System.ComponentModel.DataAnnotations;

namespace Id.Models.DatabaseModels
{
	/// <summary>
	/// Represents a customizable theme with associated metadata, translations, and privacy settings.
	/// </summary>
	/// <remarks>A theme is a user-defined or system-defined entity that includes a name, description, and
	/// associated translations. It can be used to represent a specific design, style, or configuration within an
	/// application.</remarks>
	public class Theme
	{
		/// <summary>
		/// Gets or sets the unique identifier for the entity.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the unique identifier for the application user.
		/// </summary>
		public int ApplicationUserId { get; set; }

		/// <summary>
		/// Gets or sets the unique identifier for the user.
		/// </summary>
		public string? UserId { get; set; }

		/// <summary>
		/// Gets or sets the name of the theme.
		/// </summary>
		[Required]
		[MaxLength(100)]
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the description text, limited to a maximum of 300 characters.
		/// </summary>
		[MaxLength(300)]
		public string? Description { get; set; }

		/// <summary>
		/// Gets or sets the source language for translation or processing.
		/// </summary>
		/// <remarks>The value should be a valid language code in ISO 639-1 format. Ensure the language code is
		/// supported by the system or service using this property.</remarks>
		public string SourceLanguage { get; set; } = "en";

		/// <summary>
		/// Gets or sets a value indicating whether the entity is a system-defined entity.
		/// </summary>
		public bool IsSystem { get; set; } = false;

		/// <summary>
		/// Gets or sets the privacy settings for the theme.
		/// </summary>
		public ThemePrivacy Privacy { get; set; }

		/// <summary>
		/// Gets or sets the user associated with the current context.
		/// </summary>
		public User? User { get; set; }

		/// <summary>
		/// Gets or sets the theme definition that specifies the visual and styling properties for the application.
		/// </summary>
		public ThemeDefinition ThemeDefinition { get; set; } = null!;

		/// <summary>
		/// Gets or sets the application user associated with the current context.
		/// </summary>
		public ApplicationUser? ApplicationUser { get; set; }

		/// <summary>
		/// Gets or sets the collection of translations for the theme's name in different languages.
		/// </summary>
		public ICollection<ThemeNameTranslation> NameTranslations { get; set; } = [];

		/// <summary>
		/// Gets or sets the collection of localized translations for the theme's description.
		/// </summary>
		/// <remarks>Use this property to manage or retrieve localized descriptions for the theme in multiple
		/// languages. Each translation in the collection should correspond to a unique language or culture.</remarks>
		public ICollection<ThemeDescriptionTranslation> DescriptionTranslations { get; set; } = [];
	}
}