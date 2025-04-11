namespace SharedTools.Models.MdModels
{
	/// <summary>
	/// Represents a token in a Markdown line.
	/// </summary>
	public class MdToken
	{
		/// <summary>
		/// The MD tag of the token.
		/// </summary>
		public string? Tag { get; set; }

		/// <summary>
		/// The text of the MD token.
		/// </summary>
		public string Text { get; set; } = string.Empty;

		/// <summary>
		/// Boolean information if the text should be translated.
		/// </summary>
		public bool Translate { get; set; } = true;
	}
}