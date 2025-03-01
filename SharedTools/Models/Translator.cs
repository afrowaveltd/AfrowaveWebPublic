namespace SharedTools.Models
{
	/// <summary>
	/// Translator class is a class that is used to store information about a translator.
	/// </summary>
	public class Translator
	{
		/// <summary>
		/// Name is a string that contains the name of the translator.
		/// </summary>
		public string Host { get; set; } = string.Empty;

		/// <summary>
		/// Port is an integer that contains the port of the translator.
		/// </summary>
		public int Port { get; set; } = 0;
	}
}