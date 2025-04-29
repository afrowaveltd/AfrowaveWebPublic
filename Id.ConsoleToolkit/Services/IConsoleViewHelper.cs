using Spectre.Console;

namespace Id.ConsoleToolkit.Services
{
	/// <summary>
	/// Helper for console view.
	/// </summary>
	public interface IConsoleViewHelper
	{
		/// <summary>
		/// Select a value from a list of options.
		/// </summary>
		/// <param name="titleKey">The title of selection</param>
		/// <param name="options">List of pairs Value, TextKey</param>
		/// <returns>String of selected option value</returns>
		string Select(string titleKey, params (string Value, string TextKey)[] options);

		/// <summary>
		/// Shows a message in the console.
		/// </summary>
		/// <param name="message">The error message text</param>
		void ShowError(string message);

		/// <summary>
		/// Shows a success message in the console.
		/// </summary>
		/// <param name="message">The success message text</param>
		void ShowSuccess(string message);

		/// <summary>
		/// Shows a title in the console.
		/// </summary>
		/// <param name="text">Text of the title</param>
		/// <param name="color">The color of the title (Yellow by default)</param>
		void ShowTitle(string text, Color? color = null);
	}
}