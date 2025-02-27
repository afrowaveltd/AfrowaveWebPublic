namespace Id.Models.DatabaseModels
{
	/// <summary>
	/// Represents a UI translator log entity with its details.
	/// </summary>
	public class UiTranslatorLog
	{
		/// <summary>
		/// Gets or sets the unique identifier of the UI translator log.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the start time of the translation process.
		/// </summary>
		public DateTime StartTime { get; set; } = DateTime.UtcNow;

		/// <summary>
		/// Gets or sets the end time of the translation process.
		/// </summary>
		public DateTime EndTime { get; set; }

		/// <summary>
		/// Gets or sets the identifier of the user who started the translation process.
		/// </summary>
		public bool DefaultLanguageFound { get; set; } = true;

		/// <summary>
		/// Gets or sets the amount of languages into which translations will be made.
		/// </summary>
		public int TargetLanguagesCount { get; set; } = 0;

		/// <summary>
		/// Gets or sets the amount of phrazes to translate.
		/// </summary>
		public int PhrazesCount { get; set; } = 0;

		/// <summary>
		/// Gets or sets a value indicating whether old translations were found.
		/// </summary>
		public bool OldTranslationsFound { get; set; } = false;

		/// <summary>
		/// Gets or sets the amount of phrazes to translate.
		/// </summary>
		public int PhrazesToTranslateCount { get; set; } = 0;

		/// <summary>
		/// Gets or sets the amount of phrazes to remove.
		/// </summary>
		public int PhrazesToRemoveCount { get; set; } = 0;

		/// <summary>
		/// Gets or sets the amount of phrazes to add.
		/// </summary>
		public int TranslatedPhrazesCount { get; set; } = 0;

		/// <summary>
		/// Gets or sets the amount of phrazes to add.
		/// </summary>
		public int TranslationErrorCount { get; set; } = 0;

		/// <summary>
		/// Gets or sets the total time of the translation process.
		/// </summary>
		public TimeSpan TotalTime { get; set; } = TimeSpan.Zero;
	}
}