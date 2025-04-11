using System.ComponentModel.DataAnnotations;

namespace SharedTools.Models.BaseEntities;

/// <summary>
/// This is the base class for all translations.
/// </summary>
public abstract class TranslationBase
{
	/// <summary>
	/// The unique identifier for the translation.
	/// </summary>
	[Key]
	public int Id { get; set; }

	/// <summary>
	/// The unique identifier for the object being translated.
	/// </summary>
	public string ObjectId { get; set; } = string.Empty;

	/// <summary>
	/// Two letter language code (ISO 639-1).
	/// </summary>
	[MaxLength(2)]
	public string Language { get; set; } = string.Empty;

	/// <summary>
	/// The boolean value indicating if the translation is the original from which the other translations are derived.
	/// </summary>
	public bool IsOriginal { get; set; }

	/// <summary>
	/// The boolean value indicating if the translation was automatically translated.
	/// </summary>

	public bool AutoTranslated { get; set; }

	/// <summary>
	/// Translated text to be used in the application.
	/// </summary>
	public string Body { get; set; } = string.Empty;

	/// <summary>
	/// The previous version of the translated text. It is used until the new translation is accepted.
	/// </summary>
	public string? PreviousBody { get; set; }

	/// <summary>
	/// The date and time when the translation was created.
	/// </summary>
	public DateTime LastChange { get; set; } = DateTime.UtcNow;

	/// <summary>
	/// User ID of the person who created the translation.
	/// </summary>
	public string? EditorId { get; set; }

	/// <summary>
	/// The boolean value indicating if the changes made to the translation have been accepted.
	/// </summary>
	public bool ChangesAccepted { get; set; }
}