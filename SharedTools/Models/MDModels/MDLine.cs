namespace SharedTools.Models.MDModels
{
	/// <summary>
	/// Represents a line in a Markdown document.
	/// </summary>
	public class MDLine
	{
		/// <summary>
		/// The index of the line in the document.
		/// </summary>
		public int LineNumber { get; set; }

		/// <summary>
		/// The text of the line.
		/// </summary>
		public string OriginalLine { get; set; } = string.Empty;

		/// <summary>
		/// List of translatable segments in the line.
		/// </summary>
		public List<MDToken> Tokens { get; set; } = [];
	}
}