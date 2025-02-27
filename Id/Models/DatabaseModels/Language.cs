using System.ComponentModel.DataAnnotations;

namespace Id.Models.DatabaseModels
{
	/// <summary>
	/// Represents a language entity with its details.
	/// </summary>
	public class Language
	{
		/// <summary>
		/// Gets or sets the unique identifier of the language.
		/// </summary>
		[Key]
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
		/// </summary>
		public int Rtl { get; set; } = 0;

		/// <summary>
		/// Gets or sets the policy translations associated with the language.
		/// </summary>
		public List<PolicyTranslation> PolicyTranslations { get; set; } = new List<PolicyTranslation>();
	}
}