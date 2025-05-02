namespace EmojiViewer.Console.Models
{
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
}