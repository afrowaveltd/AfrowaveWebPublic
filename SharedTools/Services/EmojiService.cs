using SharedTools.Models;
using System.Text.Json;

namespace SharedTools.Services;

/// <summary>
/// Service to handle emoji data.
/// </summary>
public class EmojiService : IEmojiService
{
	private readonly Dictionary<string, EmojiEntry> _emojiByName;
	private readonly Dictionary<string, EmojiEntry> _emojiByUnicode;

	/// <summary>
	/// Constructor for the EmojiService class.
	/// </summary>
	public EmojiService()
	{
		_emojiByName = new(StringComparer.OrdinalIgnoreCase);
		_emojiByUnicode = new(StringComparer.OrdinalIgnoreCase);
		LoadEmojiData();
	}

	private void LoadEmojiData()
	{
		string path = Path.Combine(AppContext.BaseDirectory, "Jsons", "EmojiData.json");

		if(!File.Exists(path))
		{
			return;
		}

		string json = File.ReadAllText(path);
		List<EmojiEntry>? emojis = JsonSerializer.Deserialize<List<EmojiEntry>>(json);

		if(emojis is null)
		{
			return;
		}

		foreach(EmojiEntry emoji in emojis)
		{
			if(!string.IsNullOrWhiteSpace(emoji.Name))
			{
				_emojiByName[emoji.Name] = emoji;
			}

			if(!string.IsNullOrWhiteSpace(emoji.UnicodeHex))
			{
				_emojiByUnicode[emoji.UnicodeHex] = emoji;
			}
		}
	}

	/// <summary>
	/// Get the emoji string based on the input name or unicode hex.
	/// </summary>
	/// <param name="input">Can be either unicode hex or emoji name</param>
	/// <returns>CSharp emoji string</returns>
	public string Get(string input)
	{
		if(string.IsNullOrWhiteSpace(input))
		{
			return string.Empty;
		}

		if(_emojiByName.TryGetValue(input, out EmojiEntry? byName))
		{
			return byName.Utf8String ?? string.Empty;
		}

		if(_emojiByUnicode.TryGetValue(input, out EmojiEntry? byUnicode))
		{
			return byUnicode.Utf8String ?? string.Empty;
		}

		return string.Empty;
	}

	/// <summary>
	/// Get the emoji string based on the unicode hex.
	/// </summary>
	/// <param name="unicodeHex">Unicode hex of the emoji</param>
	/// <returns>CSharp emoji string</returns>
	public string GetByUnicode(string unicodeHex)
	{
		return _emojiByUnicode.TryGetValue(unicodeHex, out EmojiEntry? emoji)
			 ? emoji.Utf8String ?? string.Empty
			 : string.Empty;
	}

	/// <summary>
	/// Get the emoji string based on the name.
	/// </summary>
	/// <param name="name">Emoji name</param>
	/// <returns>CSharp emoji string</returns>
	public string GetByName(string name)
	{
		return _emojiByName.TryGetValue(name, out EmojiEntry? emoji)
			 ? emoji.Utf8String ?? string.Empty
			 : string.Empty;
	}
}