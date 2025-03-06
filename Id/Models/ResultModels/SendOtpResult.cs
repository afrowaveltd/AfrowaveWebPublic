namespace Id.Models.ResultModels
{
	/// <summary>
	/// Send OTP result.
	/// </summary>
	public class SendOtpResult
	{
		/// <summary>
		/// Gets or sets a value indicating whether success.
		/// </summary>
		public bool Success { get; set; }

		/// <summary>
		/// Gets or sets the OTP code.
		/// </summary>
		public string OtpCode { get; set; } = string.Empty;
	}
}