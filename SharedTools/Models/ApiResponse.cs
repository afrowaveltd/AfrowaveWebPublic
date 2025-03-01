namespace SharedTools.Models
{
	/// <summary>
	/// ApiResponse class is a generic class that is used to return a response from the API.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ApiResponse<T>
	{
		/// <summary>
		/// Successful is a boolean that indicates if the operation was successful.
		/// </summary>
		public bool Successful { get; set; } = true;

		/// <summary>
		/// Message is a string that contains a message that can be used to describe the response.
		/// </summary>
		public string? Message { get; set; }

		/// <summary>
		/// Data is a generic type that can be used to return data from the API.
		/// </summary>
		public T? Data { get; set; }
	}
}