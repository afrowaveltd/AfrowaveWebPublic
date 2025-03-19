namespace SharedTools.Models
{
	/// <summary>
	/// TranslateResponse class is a class that is used to return a response from the Translate API.
	/// </summary>
	public class TranslateResponse
	{
		/// <summary>
		/// TranslatedText is a string that contains the translated text.
		/// </summary>
		public string TranslatedText { get; set; } = string.Empty;

		/// <summary>
		/// DetectedLanguage is a DetectedLanguage object that contains information about the detected language.
		/// </summary>
		public DetectedLanguage? DetectedLanguage { get; set; }

		/// <summary>
		/// Alternatives is a list of strings that contains alternative translations.
		/// </summary>
		public List<string>? Alternatives { get; set; }
	}

	/// <summary>
	/// Represents a detected language with its corresponding language code. The Language property holds the code as a
	/// string.
	/// </summary>
	public class DetectedLanguage
	{
		//public int Confidence { get; set; } = 0;
		/// <summary>
		/// Language is a string that contains the language code of the detected language.
		/// </summary>
		public string Language { get; set; } = string.Empty;
	}
}