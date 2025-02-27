namespace Id.Models.ResultModels
{
	/// <summary>
	/// Result of registering a brand
	/// </summary>
	public class RegisterBrandResult
	{
		/// <summary>
		/// Brand created - true if brand created, false if not
		/// </summary>
		public bool BrandCreated { get; set; } = true;

		/// <summary>
		/// Brand ID
		/// </summary>
		public int BrandId { get; set; } = 0;

		/// <summary>
		/// Logo uploaded - true if logo uploaded, false if not
		/// </summary>
		public bool LogoUploaded { get; set; } = true;

		/// <summary>
		/// Error message
		/// </summary>
		public string ErrorMessage { get; set; } = string.Empty;
	}
}