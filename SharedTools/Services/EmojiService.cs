using System.Text.Json;

namespace SharedTools.Services.I18n;

public interface IEmojiService
{
	string Get(string input);

	string GetByUnicode(string unicodeHex);

	string GetByName(string name);
}

public class EmojiEntry
{
	public string? Name { get; set; }
	public string? UnicodeHex { get; set; }
	public string? Utf8String { get; set; }
	public string? CSharpString { get; set; }
}

public class EmojiService : IEmojiService
{
	private readonly Dictionary<string, EmojiEntry> _emojiByName;
	private readonly Dictionary<string, EmojiEntry> _emojiByUnicode;

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

	public string GetByUnicode(string unicodeHex)
	{
		return _emojiByUnicode.TryGetValue(unicodeHex, out EmojiEntry? emoji)
			 ? emoji.Utf8String ?? string.Empty
			 : string.Empty;
	}

	public string GetByName(string name)
	{
		return _emojiByName.TryGetValue(name, out EmojiEntry? emoji)
			 ? emoji.Utf8String ?? string.Empty
			 : string.Empty;
	}
}