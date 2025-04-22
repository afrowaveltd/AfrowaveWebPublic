namespace Id.Models.DataViews
{
	/// <summary>
	/// Represents a view model for a language entity.
	/// </summary>
	public class LanguageView
	{
		/// <summary>
		/// Gets or sets the unique identifier of the language.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the code of the language.
		/// </summary>
		public string Code { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the name of the language.
		/// </summary>
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the native name of the language.
		/// </summary>
		public string Native { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the direction of the language.
		/// 0 for LTR (left to right), 1 for RTL (right to left).
		/// </summary>
		public int Rtl { get; set; } = 0;

		/// <summary>
		/// Checks if the language is presented in the LibreTranslate service.
		/// </summary>
		public bool CanAutotranslate { get; set; } = false;
	}
}