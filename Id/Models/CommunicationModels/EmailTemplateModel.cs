namespace Id.Models.CommunicationModels
{
	/// <summary>
	/// Email template model.
	/// </summary>
	public class EmailTemplateModel
	{
		/// <summary>
		/// Gets or sets the header text.
		/// </summary>
		public string HeaderText { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the body text.
		/// </summary>
		public string ActionUrl { get; set; } = string.Empty; // Pro reset hesla

		/// <summary>
		/// Gets or sets the OTP code.
		/// </summary>
		public string OtpCode { get; set; } = string.Empty;  // Pro OTP

		/// <summary>
		/// Gets or sets the subject text.
		/// </summary>
		public string Subject { get; set; } = string.Empty;
	}
}