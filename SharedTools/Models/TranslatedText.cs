namespace SharedTools.Models
{
	public class TranslateResponse
	{
		public string TranslatedText { get; set; } = string.Empty;
		public DetectedLanguage? DetectedLanguage { get; set; }
		public List<string>? Alternatives { get; set; }
	}

	public class DetectedLanguage
	{
		//public int Confidence { get; set; } = 0;
		public string Language { get; set; } = string.Empty;
	}
}