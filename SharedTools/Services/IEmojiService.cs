namespace SharedTools.Services;

/// <summary>
/// Service to handle emoji data.
/// </summary>
public interface IEmojiService
{
	/// <summary>
	/// Get the emoji string based on the input name or unicode hex.
	/// </summary>
	/// <param name="input">Can be either unicode hex or emoji name</param>
	/// <returns>CSharp emoji string</returns>
	string Get(string input);

	/// <summary>
	/// Get the emoji string based on the unicode hex.
	/// </summary>
	/// <param name="unicodeHex">Unicode hex of the emoji</param>
	/// <returns>CSharp emoji string</returns>
	string GetByUnicode(string unicodeHex);

	/// <summary>
	/// Get the emoji string based on the name.
	/// </summary>
	/// <param name="name">Emoji name</param>
	/// <returns>CSharp emoji string</returns>
	string GetByName(string name);
}