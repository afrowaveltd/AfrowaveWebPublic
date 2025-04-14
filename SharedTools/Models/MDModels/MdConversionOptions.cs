namespace SharedTools.Models.MdModels
{
	/// <summary>
	/// Options for converting Markdown to HTML.
	/// </summary>
	public class MdConversionOptions
	{
		/// <summary>
		/// Map of HTML tags with classes added ie: { "code" => "code-style" }
		/// </summary>
		public Dictionary<string, string> CustomClasses { get; set; } = [];

		/// <summary>
		/// If true, the output will be streamed as it is generated.
		/// </summary>
		public bool EnableStreaming { get; set; } = false;

		/// <summary>
		/// If true the output will be formatted with indentation and new lines.
		/// </summary>
		public bool PrettyPrint { get; set; } = true;
	}
}