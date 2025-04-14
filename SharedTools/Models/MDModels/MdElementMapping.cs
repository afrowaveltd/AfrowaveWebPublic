namespace SharedTools.Models.MdModels;

/// <summary>
/// Represents a mapping between a Markdown pattern and its corresponding HTML representation.
/// </summary>
public class MdElementMapping
{
	/// <summary>
	/// Markdown prefix at the beginning of a line (e.g. "# ", "## ", "- ", "> ").
	/// </summary>
	public string MdStart { get; set; } = string.Empty;

	/// <summary>
	/// Optional Markdown end pattern (usually null, as most blocks end with a newline).
	/// </summary>
	public string? MdEnd { get; set; }

	/// <summary>
	/// HTML element to generate (e.g. h1, h2, p, blockquote, hr, li, etc.).
	/// </summary>
	public string HtmlElement { get; set; } = string.Empty;

	/// <summary>
	/// Determines whether the HTML element is self-closing (e.g. hr, br).
	/// </summary>
	public bool SelfClosing { get; set; }

	/// <summary>
	/// Optional CSS class to apply to the HTML element. If null, no class will be added.
	/// </summary>
	public string? CssClass { get; set; }
}