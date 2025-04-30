namespace SharedTools.Models;

/// <summary>
/// Represents an emoji entry with its name, Unicode hex representation, UTF-8 string, and C# string.
/// </summary>
public class EmojiEntry
{
	/// <summary>
	/// Gets or sets the name of the emoji.
	/// </summary>
	public string? Name { get; set; }

	/// <summary>
	/// Gets or sets the Unicode hex representation of the emoji.
	/// </summary>
	public string? UnicodeHex { get; set; }

	/// <summary>
	/// Gets or sets the UTF-8 string representation of the emoji.
	/// </summary>
	public string? Utf8String { get; set; }

	/// <summary>
	/// Gets or sets the C# string representation of the emoji.
	/// </summary>
	public string? CSharpString { get; set; }
}