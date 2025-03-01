namespace SharedTools.Services
{
	/// <summary>
	/// ITextToHtmlService is an interface that is used to convert text to HTML.
	/// It contains a single method that takes a string input and returns a string output.
	/// </summary>
	public interface ITextToHtmlService
	{
		/// <summary>
		/// ConvertTextToHtml is a method that takes a string input and returns a string output.
		/// </summary>
		/// <param name="input">Formated text - spaces at the begining create headings, empty lines are used for diviging to paragraphs, links are created from the context if they are recognized</param>
		/// <returns>HTML file</returns>
		string ConvertTextToHtml(string input);
	}
}