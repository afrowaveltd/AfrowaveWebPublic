namespace Id.Models.DatabaseModels
{
	public class UiTranslatorLog
	{
		public int Id { get; set; }
		public DateTime StartTime { get; set; } = DateTime.UtcNow;
		public DateTime EndTime { get; set; }
		public bool DefaultLanguageFound { get; set; }
		public int TargetLanguagesCount { get; set; } = 0;
		public int PhrazesCount { get; set; } = 0;
		public bool OldTranslationsFound { get; set; } = false;
		public int PhrazesToTranslateCount { get; set; } = 0;
		public int PhrazesToRemoveCount { get; set; } = 0;
		public int TranslatedPhrazesCount { get; set; } = 0;
		public int TranslationErrorCount { get; set; } = 0;
		public TimeSpan TotalTime { get; set; } = TimeSpan.Zero;
	}
}