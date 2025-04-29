namespace Id.ConsoleToolkit.Views
{
	/// <summary>
	/// Interface for the main menu view.
	/// </summary>
	public interface IMainMenuView
	{
		/// <summary>
		/// Runs the console, renders view <see cref="ConsoleViewBase.ShowAsync"/>.
		/// </summary>
		/// <returns>The console view</returns>
		Task RunAsync();
	}
}