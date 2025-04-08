namespace SharedTools.Models.MDModels
{
	/// <summary>
	/// Represents a translatable segment in a Markdown document.
	/// </summary>
	public class TranslatableSegment
	{
		/// <summary>
		/// The index of the segment in the line.
		/// </summary>
		public int Index { get; set; }

		/// <summary>
		/// The text of the segment.
		/// </summary>
		public string Text { get; set; } = string.Empty;
	}
}