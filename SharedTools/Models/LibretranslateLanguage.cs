namespace SharedTools.Models
{
	/// <summary>
	/// LibretranslateLanguage class is a class that is used to store information about a language that is supported by the Libretranslate API.
	/// </summary>
	public class LibretranslateLanguage
	{
		/// <summary>
		/// Code is a string that contains the code of the language.
		/// </summary>
		public string Code { get; set; } = string.Empty;

		/// <summary>
		/// Name is a string that contains the name of the language.
		/// </summary>
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// Targets is a list of strings that contains the target languages that the language can be translated to.
		/// </summary>
		public List<string> Targets { get; set; } = [];
	}
}