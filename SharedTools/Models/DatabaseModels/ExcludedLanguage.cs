using System.ComponentModel.DataAnnotations;

namespace SharedTools.Models.DatabaseModels;

/// <summary>
/// This class represents a language that is excluded from translation.
/// </summary>
public class ExcludedLanguage
{
	/// <summary>
	/// The unique identifier for the excluded language.
	/// </summary>
	[Key]
	public int Id { get; set; }

	/// <summary>
	/// The unique identifier for the object being excluded from translation.
	/// </summary>
	public string ObjectId { get; set; } = string.Empty;

	/// <summary>
	/// The type of the object being excluded from translation.
	/// </summary>

	public string ObjectType { get; set; } = string.Empty; // enum as string

	/// <summary>
	/// The two-letter language code (ISO 639-1) of the excluded language.
	/// </summary>
	public string LanguageCode { get; set; } = string.Empty;
}