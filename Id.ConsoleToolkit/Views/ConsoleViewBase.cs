using Spectre.Console;

namespace Id.ConsoleToolkit.Views
{
	/// <summary>
	/// Base class for console views.
	/// </summary>
	/// <remarks>
	/// Constructor for <see cref="ConsoleViewBase"/>.
	/// </remarks>
	/// <param name="viewHelper">Helper for the layout of the view</param>
	public abstract class ConsoleViewBase(IConsoleViewHelper viewHelper)
	{
		private readonly IConsoleViewHelper _viewHelper = viewHelper;

		/// <summary>
		/// The optional Title <see cref="RunAsync"/>.
		/// </summary>
		protected virtual string? Title => null;

		/// <summary>
		/// Runs the console, renders view <see cref="ShowAsync"/>.
		/// </summary>
		public async Task RunAsync()
		{
			Console.Clear();

			if(!string.IsNullOrWhiteSpace(Title))
			{
				_viewHelper.ShowTitle(Title, Color.Yellow2);
			}

			await ShowAsync();
		}

		/// <summary>
		/// Each view must implement its own logic for rendering and interaction.
		/// </summary>
		protected abstract Task ShowAsync();
	}
}